/*
Copyright (C) 2013-2015 MetaMorph Software, Inc

Permission is hereby granted, free of charge, to any person obtaining a
copy of this data, including any software or models in source or binary
form, as well as any drawings, specifications, and documentation
(collectively "the Data"), to deal in the Data without restriction,
including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Data, and to
permit persons to whom the Data is furnished to do so, subject to the
following conditions:

The above copyright notice and this permission notice shall be included
in all copies or substantial portions of the Data.

THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  
*/

/*
*  Authors:
*    Sandeep Neema <neemask@metamorphsoftware.com>
*
*/

#include <gecode/int.hh>
#include <gecode/search.hh>
#include <gecode/gist.hh>
#include <gecode/minimodel.hh>
#include <json.h>
#include <fstream>
#include <algorithm>
#include <map>
#include <list>
#include <utility>
#include <time.h>
#include <sys/timeb.h>
#include <math.h>

#include <MaxRectsBinPack.h>

/*
* @todo: 
*   apply prior-placement and proximity constraints
*   1. custom brancher rotation and layer 
*   2. test custom brancher with pre-placement constraints
*   3. implement other constraints
*   4. scalability, symmetry breaking
*/

using namespace Gecode;

struct BiggerChip : public std::binary_function<std::pair<double,int>, std::pair<double,int>, bool>
{
	std::map<int, int> IsConstrained;
	BiggerChip(std::map<int, int>& isConstrained) {
		IsConstrained = isConstrained;
	}
	inline bool operator()(const std::pair<double,int>& p1, const std::pair<double, int>& p2)
	{
		int p1Constr = IsConstrained[p1.second];
		int p2Constr = IsConstrained[p2.second];
		// constraint weight dominates area
		if (p1Constr > p2Constr)
			return true;					// higher constraint weight, comes before
		else if (p1Constr == p2Constr)
			return p1.first > p2.first;		// same constraint weight,  bigger chip comes before
		else 
			return false;					// lower constraint weight, does not come before
	}
};

#define MAX_LAYERS 8
#define MAX(a, b) (a)>(b) ? (a) : (b)
#define MIN(a, b) (a)<(b) ? (a) : (b)

class LayoutBrancher : public Brancher
{
protected:
	mutable rbp::MaxRectsBinPack binPacker[MAX_LAYERS];
	mutable std::vector<int> wasPlaced;  // list of what we placed

	ViewArray<Int::IntView> px;
	ViewArray<Int::IntView> py;
	ViewArray<Int::IntView> pz;
	ViewArray<Int::IntView> pr;

	IntSharedArray pw;
	IntSharedArray ph;
	int boardW;
	int boardH;
	int numLayers;
	int chipGap;
	mutable int item;	// next chip to place

	struct RZRect {
		int r;
		int z;
		struct rbp::Rect rect;
	};

	class Choice : public Gecode::Choice {
	public:
		int item;
		RZRect* possible;
		int n_possible;
		Choice(const Brancher& b, unsigned int a, int i, RZRect* p, int n_p)
			: Gecode::Choice(b,a), item(i), 
			possible(heap.alloc<RZRect>(n_p)), n_possible(n_p) {
				for (int k=n_possible; k--; )
					possible[k] = p[k];
		}
		virtual size_t size(void) const {
			return sizeof(Choice) + sizeof(RZRect) * n_possible;
		}
		virtual void archive(Archive& e) const {
			Gecode::Choice::archive(e);
			e << alternatives() << item << n_possible;
			for (int i=n_possible; i--;) 
				e << possible[i].r 
				  << possible[i].z 
				  << possible[i].rect.x << possible[i].rect.y << possible[i].rect.width << possible[i].rect.height;
		}
		virtual ~Choice(void) {
			heap.free<RZRect>(possible, n_possible);
		}
	};

public:
	LayoutBrancher(Home home, 
		ViewArray<Int::IntView>& x, 
		ViewArray<Int::IntView>& y,
		ViewArray<Int::IntView>& z,
		ViewArray<Int::IntView>& r,
		IntSharedArray& w,
		IntSharedArray& h,
		int bw,
		int bh,
		int nl, 
		int cg) 
		: Brancher(home), px(x), py(y), pz(z), pr(r), pw(w), ph(h), 
		boardW(bw), boardH(bh), numLayers(nl), chipGap(cg), item(0) {
			for(int i=0; i<nl; i++)
				binPacker[i].Init(bw, bh);
			wasPlaced.resize(x.size(), 0);
			home.notice(*this,AP_DISPOSE);
	}
	static BrancherHandle post(Home home, 
		ViewArray<Int::IntView>& x, 
		ViewArray<Int::IntView>& y,
		ViewArray<Int::IntView>& z,
		ViewArray<Int::IntView>& r,
		IntSharedArray& w,
		IntSharedArray& h,
		int bw, int bh, int nl, int cg) {
			return *new (home) LayoutBrancher(home, x, y, z, r, w, h, bw, bh, nl, cg);
	}

	LayoutBrancher(Space& home, bool share, LayoutBrancher& LayoutBrancher) 
		: Brancher(home, share, LayoutBrancher), item(LayoutBrancher.item) {
			px.update(home, share, LayoutBrancher.px);
			py.update(home, share, LayoutBrancher.py);
			pz.update(home, share, LayoutBrancher.pz);
			pr.update(home, share, LayoutBrancher.pr);
			pw.update(home, share, LayoutBrancher.pw);
			ph.update(home, share, LayoutBrancher.ph);
			boardW = LayoutBrancher.boardW;
			boardH = LayoutBrancher.boardH;
			numLayers = LayoutBrancher.numLayers;
			chipGap = LayoutBrancher.chipGap;
			for(int i=0; i<numLayers; i++)
				binPacker[i] = LayoutBrancher.binPacker[i];	// check OR implement the copy constructor of binPacker
			wasPlaced = LayoutBrancher.wasPlaced;
	}
	virtual Actor* copy(Space& home, bool share) {
		return new (home) LayoutBrancher(home, share, *this);
	}
	virtual size_t dispose(Space& home) {
		home.ignore(*this,AP_DISPOSE);
		pw.~IntSharedArray();
		ph.~IntSharedArray();
		return sizeof(*this);
	}
	virtual bool status(const Space&) const {
		for (int i = item; i < px.size(); i++)
		{
			bool assigned = px[i].assigned() && py[i].assigned() && pz[i].assigned() && pr[i].assigned();
			bool placed = (wasPlaced[i] == 1);
			if (assigned && !placed) {		// we are assigning all px,py,pz,pr together - so picking one should be adequate
				struct rbp::Rect rect;
				rect.x = px[i].val(); rect.y = py[i].val();
				if (pr[i].val() == 0 || pr[i].val() == 2 ) { rect.width = pw[i] + chipGap; rect.height = ph[i] + chipGap; }
				else if (pr[i].val() == 1 || pr[i].val() == 3 ) { rect.width = ph[i] + chipGap; rect.height = pw[i] + chipGap; }
				binPacker[pz[i].val()].PlaceRect(rect);
				wasPlaced[i] = 1;
			}
		}
		for (int i = item; i < px.size(); i++)
		{
			bool assigned = px[i].assigned() && py[i].assigned() && pz[i].assigned() && pr[i].assigned();
			if (!assigned) {		
				item = i; 
				return true;
			} 
		}
		return false;
	}
	virtual Gecode::Choice* choice(Space& home) {
		int iw = pw[item] + chipGap;
		int ih = ph[item] + chipGap;

		typedef std::multimap<int, RZRect> AreaRects;
		AreaRects dsts; 

		for(int z=pz[item].min(); z<=pz[item].max(); z++)
		{
			rbp::MaxRectsBinPack& bpl = binPacker[z];

			for(int i=0; i<bpl.freeRectangles.size(); i++)
			{
				struct rbp::Rect& freeRect = bpl.freeRectangles[i];
				struct rbp::Rect dest;

				//  (1) CHECK free rect area > chip area
				int areaFit = freeRect.width * freeRect.height - iw * ih;
				if (areaFit < 0)
					continue;

				//  (2) freeRect above the allowable max of domain
				if ((px[item].max() < freeRect.x) || (py[item].max() < freeRect.y))
					continue;

				// bounds on origin of rects induced by domain constraints && free rect
				int xOrgLower = MAX(px[item].min(), freeRect.x);
				int yOrgLower = MAX(py[item].min(), freeRect.y);

				// direct placement
				if ((freeRect.width >= iw) && (freeRect.height >= ih) && (pr[item].in(0) || pr[item].in(2)))
				{
					int xOrgUpper = MIN(px[item].max(), freeRect.x+freeRect.width-iw);
					int yOrgUpper = MIN(py[item].max(), freeRect.y+freeRect.height-ih);
			
					int x[2] = {xOrgLower, xOrgUpper};
					int y[2] = {yOrgLower, yOrgUpper};

					for(int xi=0; xi<2; xi++)
						for(int yi=0; yi<2; yi++)
						{
							dest.x = x[xi];
							dest.y = y[yi];
							if ((dest.x >= xOrgLower && dest.y >= yOrgLower) &&
								((dest.x + iw) <= (freeRect.x + freeRect.width)) &&
								((dest.y + ih) <= (freeRect.x + freeRect.height)))
							{
								dest.width = iw;
								dest.height = ih;
								RZRect rzRect = {0, z, dest};

								dsts.insert(std::pair<int, RZRect>(areaFit, rzRect));
							}
						}
				} // if direct

				// rotated placement
				if ((freeRect.width >= ih) && (freeRect.height >= iw) && (pr[item].in(1) || pr[item].in(3)))
				{
					int xOrgUpper = MIN(px[item].max(), freeRect.x+freeRect.width-ih);
					int yOrgUpper = MIN(py[item].max(), freeRect.y+freeRect.height-iw);
			
					int x[2] = {xOrgLower, xOrgUpper};
					int y[2] = {yOrgLower, yOrgUpper};

					for(int xi=0; xi<2; xi++)
						for(int yi=0; yi<2; yi++)
						{
							dest.x = x[xi];
							dest.y = y[yi];
							if ((dest.x >= xOrgLower && dest.y >= yOrgLower) &&
								((dest.x + ih) <= (freeRect.x + freeRect.width)) &&
								((dest.y + iw) <= (freeRect.x + freeRect.height)))
							{
								dest.width = ih;
								dest.height = iw;
								RZRect rzRect = {0, z, dest};

								dsts.insert(std::pair<int, RZRect>(areaFit, rzRect));
							}
						}
				} // if rotated


			} // loop over freeRect's
		}


		Region region(home);	// local space for allocation
		struct RZRect *possible = region.alloc<RZRect>(
			(dsts.size() > 0 ? dsts.size() : 1)
			* sizeof(RZRect));
		int n_possible = 0;

		for(AreaRects::const_iterator i=dsts.begin(); i!=dsts.end(); i++)
		{
			possible[n_possible++] = (*i).second;
		}
		dsts.clear();

		if (n_possible == 0)
		{
			RZRect invalid = {0, 0, {-1, -1, 10, 10}};
			possible[n_possible++] = invalid;
		}


		// all possible rectangle placements
		return new Choice(*this, n_possible, item, possible, n_possible);

	}
	virtual const Gecode::Choice* choice(const Space& home, Archive& e) {
		int alt, item, n_same;
		e >> alt >> item >> n_same;
		Region re(home);
		struct RZRect* same = re.alloc<RZRect>(n_same);
		for (int i=n_same; i--;) e >> same[i].r >> same[i].z >> same[i].rect.x >> same[i].rect.y >> same[i].rect.width >> same[i].rect.height;
		return new Choice(*this, alt, item, same, n_same);
	}
	virtual ExecStatus commit(Space& home, const Gecode::Choice& _c, unsigned int a) {
		const Choice& c = static_cast<const Choice&>(_c);

		// place the commited rect in binPacker
		rbp::MaxRectsBinPack& bpl = binPacker[c.possible[a].z];
		bpl.PlaceRect(c.possible[a].rect);
		wasPlaced[c.item] = 1;

		// assign the values of px, py, pr
		GECODE_ME_CHECK(pr[c.item].eq(home, c.possible[a].r));
		GECODE_ME_CHECK(pz[c.item].eq(home, c.possible[a].z));
		GECODE_ME_CHECK(px[c.item].eq(home, c.possible[a].rect.x));
		GECODE_ME_CHECK(py[c.item].eq(home, c.possible[a].rect.y));

		return ES_OK;
	}
	virtual void print(const Space&, const Gecode::Choice& _c, 
		unsigned int a,
		std::ostream& o) const {
			const Choice& c = static_cast<const Choice&>(_c);
			if (a == 0) {
				o << "px[" << c.item << "] = " << c.possible[0].rect.x;
				o << "py[" << c.item << "] = " << c.possible[0].rect.y;
			} else {
				o << "px[" << c.item << "] != " << c.possible[0].rect.x;
				o << "py[" << c.item << "] != " << c.possible[0].rect.y;
			}
	}
};

BrancherHandle lobranch(Home home, 
	const IntVarArgs& px, const IntVarArgs& py,
	const IntVarArgs& pz, const IntVarArgs& pr,
	const int *pw, const int *ph,
	const int bw, const int bh, const int nl, const int cg,
	std::list<std::pair<double,int>>& area_index_list) 
{
		ViewArray<Int::IntView> pxv(home, px.size());
		ViewArray<Int::IntView> pyv(home, py.size());
		ViewArray<Int::IntView> pzv(home, pz.size());
		ViewArray<Int::IntView> prv(home, pr.size());
		IntSharedArray pws(px.size());
		IntSharedArray phs(py.size());

		int j=0;
		for(std::list<std::pair<double, int>>::const_iterator i=area_index_list.begin(); i!=area_index_list.end(); i++, j++)
		{
			pxv[j] = Int::IntView(px[ (*i).second ]);
			pyv[j] = Int::IntView(py[ (*i).second ]);
			pzv[j] = Int::IntView(pz[ (*i).second ]);
			prv[j] = Int::IntView(pr[ (*i).second ]);
			pws[j] = pw[ (*i).second ];
			phs[j] = ph[ (*i).second ];
		}

		return LayoutBrancher::post(home, pxv, pyv, pzv, prv, pws, phs, bw, bh, nl, cg);
}


class LayoutSolver : public Space 
{
public:
	static const int resolution = 10;
protected:
	static int *pkg_idx_map;

	int chipgap;		// inflate each package to add margins 
	int edgegap;		// board edge margin
	IntVarArray px;		// x-position of packages
	IntVarArray py;		// y-position of packages
	IntVarArray pz;		// z-position of packages (layers)
	IntVarArray pr;	// rotation of packages (0 - as is, 1 - width, height flipped)
	int *pw;
	int *ph;
	int *pl;
public:
	LayoutSolver(Json::Value& root, double cg, double bg) : 
	  chipgap(cg*resolution), edgegap(bg*resolution) {
		int boardW = resolution*root["boardWidth"].asDouble();
		int boardH = resolution*root["boardHeight"].asDouble();
		int boardL = root["numLayers"].asInt();

		//		const Json::Value packages = root["packages"];
		Json::Value packages(Json::arrayValue);

		pkg_idx_map = new int[root["packages"].size()]; // create a mapping pkg idx mapping table

		unsigned int numChips = 0; 
		for (int i=0; i<root["packages"].size(); i++)
		{
			if (root["packages"][i]["doNotPlace"] == true)
			{
				pkg_idx_map[i] = -1;		// pkg-idx skipped for placement
				continue;
			}
			pkg_idx_map[i] = numChips;
			packages[numChips++] = root["packages"][i];
		}


		pw = new int[numChips];
		ph = new int[numChips];
		pl = new int[numChips];						// chips that span multiple layers e.g. EPM, through-hole, pogo-pins

		std::map<std::string, int> packageMap;		// index of package in variable list

		// package width and height constants
		for(int i=0; i<numChips; i++)
		{
			pw[i] = ceil(packages[i]["width"].asDouble()*resolution);
			ph[i] = ceil(packages[i]["height"].asDouble()*resolution);
			pl[i] = packages[i]["multiLayer"].asBool();
			packageMap[packages[i]["name"].asString()] = i;
		}

		// variable declaration
		px = IntVarArray(*this, numChips, 0, boardW);
		py = IntVarArray(*this, numChips, 0, boardH);
		pz = IntVarArray(*this, numChips, 0, boardL-1);	
		pr = IntVarArray(*this, numChips, 0, 3);	// rotation 0,1,2,3

		// non overlap constraints
		for(int i=0; i<numChips; i++)
		{
			BoolVar pri = expr(*this, (pr[i] == 1 || pr[i] == 3));
			BoolVar npri = expr(*this, (pr[i] == 0 || pr[i] == 2));
			for(int j=0; j<i; j++)
			{
				// no overlap with no rotation
				BoolVar nolnr = expr(*this, (px[i] + pw[i] + chipgap <= px[j]) || (px[j] + pw[j] + chipgap <= px[i]) ||
					(py[i] + ph[i] + chipgap <= py[j]) || (py[j] + ph[j] + chipgap <= py[i]));
				// no overlap with rotation - flip w & h of [i]
				BoolVar nolri = expr(*this, (px[i] + ph[i] + chipgap <= px[j]) || (px[j] + pw[j] + chipgap <= px[i]) ||
					(py[i] + pw[i] + chipgap <= py[j]) || (py[j] + ph[j] + chipgap <= py[i]));
				// no overlap with rotation - flip w & h of [j]
				BoolVar nolrj = expr(*this, (px[i] + pw[i] + chipgap <= px[j]) || (px[j] + ph[j] + chipgap <= px[i]) ||
					(py[i] + ph[i] + chipgap <= py[j]) || (py[j] + pw[j] + chipgap <= py[i]));
				// no overlap with rotation - flip w & h of [i] & [j]
				BoolVar nolrij = expr(*this, (px[i] + ph[i] + chipgap <= px[j]) || (px[j] + ph[j] + chipgap <= px[i]) ||
					(py[i] + pw[i] + chipgap <= py[j]) || (py[j] + pw[j] + chipgap <= py[i]));
				// no overlap with or without rotation
				BoolVar prj = expr(*this, (pr[j] == 1 || pr[j] == 3));
				BoolVar nprj = expr(*this, (pr[j] == 0 || pr[j] == 2));
				BoolVar nol = expr(*this, 
					(pri && nprj && nolri) || 
					(npri && prj && nolrj) || 
					(pri && prj && nolrij) || 
					(npri && nprj && nolnr) );
				// same layers
				BoolVar sl = expr(*this, pz[i] == pz[j]);
				// if chip i OR chip j is multi-layer then there can't be an overlap
				if (pl[i] || pl[j])
					rel(*this, nol);
				else // else if they are on same-layer then there can't be an overalp
					rel(*this, sl >> nol);
			}
			// board bound with edge gap with no rotation
			BoolVar nb = expr(*this, (px[i] + pw[i] + edgegap <= boardW) && (py[i] + ph[i] + edgegap <= boardH));
			// board bound with edge gap with rotation
			BoolVar rb  = expr(*this, (px[i] + ph[i] + edgegap <= boardW) && (py[i] + pw[i] + edgegap <= boardH));
			// reification
			rel(*this, (pri && rb) || (npri && nb));

			// board bound left-bottom edge
			BoolVar lb = expr(*this, (px[i] >= edgegap) && (py[i] >= edgegap));
			rel(*this, lb);

			// don't try rotation for packages with equal height and width
			// this introduces problem with equal size packages that users want to rotate and have a specific rotation for those
			//if (pw[i] == ph[i])
			//{
			//	rel(*this, pr[i] == 0);
			//}
		}

		std::map<int, int> ExactConstraintWeight;
		// enforce specified placement constraints
		std::cout << "Applying User-defined Package Constraints ..." << std::endl;
		for (int i=0; i<numChips; i++)
		{
			const Json::Value& constraints = packages[i]["constraints"];
			int numConstraints = constraints.size();
			for (int j=0; j<numConstraints; j++)
			{
				const Json::Value& constraint = constraints[j];
				processConstraint(constraint, packages, root, ExactConstraintWeight, i);
			}
		}

		std::cout << "Applying User-defined Global/Group Constraints ..." << std::endl;
		{
			const Json::Value& constraints = root["constraints"];
			int numConstraints = constraints.size();
			for (int j=0; j<numConstraints; j++)
			{
				const Json::Value& constraint = constraints[j];
				const Json::Value& cgroup = constraint["group"];
				if (cgroup.isNull())
					for (int i=0; i<numChips; i++)
						processConstraint(constraint, packages, root, ExactConstraintWeight, i);
				else
					for (int i=0; i<cgroup.size(); i++)
						processConstraint(constraint, packages, root, ExactConstraintWeight, cgroup[i].asInt());
			}
		}

		double layer_area = root["boardWidth"].asDouble() * root["boardHeight"].asDouble();
		double board_area = layer_area * root["numLayers"].asInt();

		// branch
		double total_chip_area = 0.0;
		std::list<std::pair<double,int>> area_index_list;
		for(int i=0; i<numChips; i++)
		{
			double width = ceil(packages[i]["width"].asDouble() * resolution)/resolution + chipgap;
			double height = ceil(packages[i]["height"].asDouble() * resolution)/resolution + chipgap;
			double area = width*height;
			total_chip_area += area;
			// Process chips with constraints first 
			area_index_list.push_back(std::pair<double,int>(area, i));
		}
		std::cout << "Board Width: " << root["boardWidth"].asDouble() << " Board Height: " << root["boardHeight"].asDouble()
			<< " Layers: " << root["numLayers"].asInt() << std::endl;
		std::cout << "Number of Chips: " << numChips << " Total Chip Area: " << total_chip_area << std::endl;
		std::cout << "Layout Density = " << total_chip_area / board_area << std::endl;

		area_index_list.sort(BiggerChip(ExactConstraintWeight));

#if 0
		// first pack small components like resistors etc. on one layer
		double small_area = 0.0;
		for(std::list<std::pair<double, int>>::const_iterator i=area_index_list.begin(); i!=area_index_list.end(); i++)
		{
			int index = (*i).second;
			double area = (*i).first;
			// heuristic: try bigger chips on one layer, smaller chips in another layer  
			// assuming two layers
			if (area > 25.0 || small_area > layer_area)
				branch(*this, pz[index], INT_VAL_SPLIT_MIN());
			else
			{
				branch(*this, pz[index], INT_VAL_SPLIT_MAX());
				small_area += area;
			}
		}
		// within a layer try to fit
		// should try INT_VAL MID or RND???
		for(std::list<std::pair<double, int>>::const_iterator i=area_index_list.begin(); i!=area_index_list.end(); i++)
		{
			int index = (*i).second;
			branch(*this, px[index], INT_VAL_MED());
			branch(*this, py[index], INT_VAL_SPLIT_MIN());
		}
		for(std::list<std::pair<double, int>>::const_iterator i=area_index_list.begin(); i!=area_index_list.end(); i++)
		{
			int index = (*i).second;
			branch(*this, pr[index], INT_VAL_SPLIT_MIN());
		}
#else
		// try custom brancher
		lobranch(*this, px, py, pz, pr, pw, ph, boardW, boardH, boardL, chipgap, area_index_list);
#endif

	}
	// search support
	LayoutSolver(bool share, LayoutSolver& s) : Space(share, s) {
		px.update(*this, share, s.px);
		py.update(*this, share, s.py);
		pr.update(*this, share, s.pr);
		pz.update(*this, share, s.pz);
	}
	virtual Space* copy(bool share) {
		return new LayoutSolver(share,*this);
	}
	// print solution
	void print(std::ostream& os = std::cout) const {
		int numChips = px.size();
		os << "X,Y,R,Z" << std::endl;
		for(int i=0; i<numChips; i++)
		{
			os << px[i] << "," << py[i] 
			<< "," << pr[i]
			<< "," <<  pz[i] 
			<< std::endl;
		}
	}

	void printLayout(std::ostream& os, Json::Value& root)
	{
		Json::Value& packages = root["packages"];
		int numChips = packages.size();
		for(int i=0; i<numChips; i++)
		{
			int j = pkg_idx_map[i];
			if (j < 0 || j >= px.size()) 
				continue;
			packages[i]["x"] = px[j].assigned() ? (double)(px[j].val()*1.0)/resolution : 0.0;
			packages[i]["y"] = py[j].assigned() ? (double)(py[j].val()*1.0)/resolution : 0.0;
			packages[i]["rotation"] = pr[j].assigned() ? Json::Value(pr[j].val()) : Json::Value(0);
			packages[i]["layer"] = pz[j].assigned() ? Json::Value(pz[j].val()) : Json::Value(0);
			packages[i]["constraints"] = Json::nullValue;
		}
		root["packages"] = packages;
		Json::StyledStreamWriter writer;
		writer.write(os, root);
	}

private:
	void processConstraint(const Json::Value& constraint, 
		const Json::Value& packages,
		const Json::Value& root,
		std::map<int, int>& ExactConstraintWeight,
		int pkg_idx)
	{
		/*
		* what do the users want?
		* place a part on a specific location on a specific layer
		* place a part (fixed-location) relative to another part
		* place a part (range-location) relative to another part
		* place a part within a region on a layer 
		* place a part on a layer - but outside a region
		* multiple constraints of a type?
		*/
		const std::string& type = constraint["type"].asString();
		if (type.compare("exact") == 0)	// exact constraint - could be on location, layer, or rotation
		{
			processExactConstraint(constraint, packages, ExactConstraintWeight, pkg_idx);
		}
		else if (type.compare("relative-pkg") == 0)
		{
			processRelativeConstraint(constraint, packages, root, ExactConstraintWeight, pkg_idx);
		}
		else if (type.compare("range") == 0)	// ranges must be specified as string
		{
			processRangeConstraint(constraint, packages, root, ExactConstraintWeight, pkg_idx);
		}
		else if (type.compare("in-region") == 0)
		{
			processRegionConstraint(constraint, packages, root, ExactConstraintWeight, pkg_idx);
		}
		else if (type.compare("ex-region") == 0)
		{
			processExRegionConstraint(constraint, packages, root, ExactConstraintWeight, pkg_idx);
		}
		else
			std::cout << "WARNING: Ignoring unsupported constraint type: " << type;
	}


	void processExactConstraint(const Json::Value& constraint,
		const Json::Value& packages,
		std::map<int, int>& ExactConstraintWeight,
		int pkg_idx)
	{
		int exactPlace = 0;
		const Json::Value x = constraint["x"];
		if (!x.isNull())
		{
			rel(*this, px[pkg_idx] == (int)(x.asDouble() * resolution));
			std::cout << "Applying Constraint: " << packages[pkg_idx]["name"].asString() << ".x = " << x.asDouble() << std::endl;
			exactPlace += 10;
		}
		const Json::Value y = constraint["y"];
		if (!y.isNull())
		{
			rel(*this, py[pkg_idx] == (int)(y.asDouble() * resolution));
			std::cout << "Applying Constraint: " << packages[pkg_idx]["name"].asString() << ".y = " << y.asDouble() << std::endl;
			exactPlace += 10;
		}
		const Json::Value r = constraint["rotation"];
		if (!r.isNull())
		{
			rel(*this, pr[pkg_idx] == r.asInt());
			std::cout << "Applying Constraint: " << packages[pkg_idx]["name"].asString() << ".rotation = " << r.asInt() << std::endl;
			exactPlace += 1;
		}
		const Json::Value z = constraint["layer"];
		if (!z.isNull())
		{
			rel(*this, pz[pkg_idx] == z.asInt());
			std::cout << "Applying Constraint: " << packages[pkg_idx]["name"].asString() << ".layer = " << z.asInt() << std::endl;
			exactPlace += 5;
		}

		if (exactPlace > 0)
			ExactConstraintWeight[pkg_idx] = exactPlace;
	}

	void processRelativeConstraint(const Json::Value& constraint, 
		const Json::Value& packages,
		const Json::Value& root,
		std::map<int, int>& ExactConstraintWeight,
		int pkg_idx)
	{
		int rel_pkg = constraint["pkg_idx"].asInt();	// index of other package
		if (rel_pkg < 0 && rel_pkg > root["packages"].size())
		{
			std::cout << "Invalid pkg_idx value: " << rel_pkg << " in constraint applied to pkg: " << pkg_idx << std::endl;
			return;	
		}
		else
		{
			rel_pkg = pkg_idx_map[rel_pkg];		// map 
			if (packages[rel_pkg]["pkg_idx"] != rel_pkg)
			{
				std::cout << "Invalid pkg_idx map: from: " << rel_pkg << "to: " << packages[rel_pkg]["pkg_idx"] << std::endl;
				return;
			}
		}

		const Json::Value x = constraint["x"];
		if (!x.isNull())
		{
			rel(*this, px[pkg_idx] == (px[rel_pkg] + (int)(x.asDouble() * resolution)));
			std::cout << "Applying Constraint: " << packages[pkg_idx]["name"].asString() << ".x = " << packages[rel_pkg]["name"].asString() << ".x + " << x.asDouble() << std::endl;
		}
		const Json::Value y = constraint["y"];
		if (!y.isNull())
		{
			rel(*this, py[pkg_idx] == (py[rel_pkg] + (int)(y.asDouble() * resolution)));
			std::cout << "Applying Constraint: " << packages[pkg_idx]["name"].asString() << ".y = " << packages[rel_pkg]["name"].asString() << ".y + " << y.asDouble() << std::endl;
		}
		const Json::Value r = constraint["rotation"];
		if (!r.isNull())
			rel(*this, pr[pkg_idx] == r.asInt());			// if rotation is specified - force to that
		else
			rel(*this, pr[pkg_idx] == pr[rel_pkg]);		// else orients the chip same as another one

		rel(*this, pz[pkg_idx] == pz[rel_pkg]);		// force same layer - pkg relative constraints are coplanar only
	}

	void processRangeConstraint(const Json::Value& constraint, 
		const Json::Value& packages,
		const Json::Value& root,
		std::map<int, int>& ExactConstraintWeight,
		int pkg_idx)
	{
		const Json::Value x = constraint["x"];
		if (!x.isNull() && x.isString())
		{
			processRangeConstraint(x.asString(), pkg_idx, px, packages[pkg_idx]["name"].asString(), "x");
		}
		const Json::Value y = constraint["y"];
		if (!y.isNull() && y.isString())
		{
			processRangeConstraint(y.asString(), pkg_idx, py, packages[pkg_idx]["name"].asString(), "y");
		}
		const Json::Value z = constraint["layer"];
		if (!z.isNull() && z.isString())
		{
			processRangeConstraint(z.asString(), pkg_idx, pz, packages[pkg_idx]["name"].asString(), "layer");
		}
	}

	void processRangeConstraint(std::string val, int vi, IntVarArray& varArr, std::string name, std::string varName)
	{
		std::pair<double, double> range = extractRange(val);
			BoolVar lowerp = expr( *this, varArr[vi] >= (int)(range.first * resolution));
			BoolVar upperp = expr( *this, varArr[vi] <= (int)(range.second * resolution));

			rel( *this, upperp && lowerp );
			std::cout << "Applying Constraint: " << range.first << " <= " << name << "." << varName << " <= " << range.second << std::endl;
	}

	void processRegionConstraint(const Json::Value& constraint, 
		const Json::Value& packages,
		const Json::Value& root,
		std::map<int, int>& ExactConstraintWeight,
		int pkg_idx)
	{
		std::cout << "Applying In Region Constraint: x[";
		BoolVar prb = expr( *this, pr[pkg_idx] == 0 || pr[pkg_idx] == 2);
		BoolVar xCons;
		{
			const Json::Value& x = constraint["x"];
			std::pair<double, double> xRange = extractRange(x.asString());
			std::cout << xRange.first << "-" << xRange.second << "] y[";
			BoolVar lower = expr( *this, px[pkg_idx] >= (int)(xRange.first * resolution));
			BoolVar upperw = expr( *this, px[pkg_idx] <= (int)(xRange.second * resolution - pw[pkg_idx]));
			BoolVar upperl = expr( *this, px[pkg_idx] <= (int)(xRange.second * resolution - ph[pkg_idx]));
			BoolVar upper = expr( *this, (prb >> upperw) && ((!prb) >> upperl));
			xCons = expr( *this, lower && upper);
		}
		BoolVar yCons;
		{
			const Json::Value& y = constraint["y"];
			std::pair<double, double> yRange = extractRange(y.asString());
			std::cout << yRange.first << "-" << yRange.second << "] layer[";
			BoolVar lower = expr( *this, py[pkg_idx] >= (int)(yRange.first * resolution));
			BoolVar upperw = expr( *this, py[pkg_idx] <= (int)(yRange.second * resolution - pw[pkg_idx]));
			BoolVar upperl = expr( *this, py[pkg_idx] <= (int)(yRange.second * resolution - ph[pkg_idx]));
			BoolVar upper = expr( *this, (prb >> upperl) && ((!prb) >> upperw));
			yCons = expr( *this, lower && upper);
		}
		BoolVar zCons;
		{
			const Json::Value& z = constraint["layer"];
			int zVal = atoi(z.asString().c_str());
			std::cout << zVal << "]" << std::endl;
			zCons = expr( *this, pz[pkg_idx] == zVal );
		}
		rel( *this, xCons && yCons && zCons);
	}


	void processExRegionConstraint(const Json::Value& constraint, 
		const Json::Value& packages,
		const Json::Value& root,
		std::map<int, int>& ExactConstraintWeight,
		int pkg_idx)
	{
		std::cout << "Applying Ex Region Constraint: x[";
		BoolVar prb = expr( *this, pr[pkg_idx] == 0 || pr[pkg_idx] == 2);
		BoolVar xCons;
		{
			const Json::Value& x = constraint["x"];
			std::pair<double, double> xRange = extractRange(x.asString());
			std::cout << xRange.first << "-" << xRange.second << "] y[";
			BoolVar upper = expr( *this, px[pkg_idx] >= (int)(xRange.second * resolution));
			BoolVar lowerw = expr( *this, px[pkg_idx] <= (int)(xRange.first * resolution - pw[pkg_idx]));
			BoolVar lowerh = expr( *this, px[pkg_idx] <= (int)(xRange.first * resolution - ph[pkg_idx]));
			BoolVar lower = expr( *this, (prb && lowerw) || ((!prb) && lowerh));
			xCons = expr( *this, lower || upper);
		}
		BoolVar yCons;
		{
			const Json::Value& y = constraint["y"];
			std::pair<double, double> yRange = extractRange(y.asString());
			std::cout << yRange.first << "-" << yRange.second << "] layer[";
			BoolVar upper = expr( *this, py[pkg_idx] >= (int)(yRange.second * resolution));
			BoolVar lowerw = expr( *this, py[pkg_idx] <= (int)(yRange.first * resolution - pw[pkg_idx]));
			BoolVar lowerh = expr( *this, py[pkg_idx] <= (int)(yRange.first * resolution - ph[pkg_idx]));
			BoolVar lower = expr( *this, (prb && lowerh) || ((!prb) && lowerw));
			yCons = expr( *this, lower || upper);
		}
		BoolVar zCons;
		{
			const Json::Value& z = constraint["layer"];
			int zVal = atoi(z.asString().c_str());
			std::cout << zVal << "]" << std::endl;
			zCons = expr( *this, pz[pkg_idx] != zVal );
		}
		rel( *this, xCons || yCons || zCons);
	}


	std::pair<double, double> extractRange(const std::string& val)
	{
		// range is a-b, or a:b, or a
		int seppos = val.find_first_of("-:");
		double begin=0, end=0;
		if (seppos != std::string::npos)
		{
			std::string bs = val.substr(0, seppos);
			std::string es = val.substr(seppos+1);
			begin = (bs != "") ? atof(bs.c_str()) : 0;
			end = (es !="") ? atof(es.c_str()) : 9999;
		}
		else
			begin = end = atof(val.c_str());

		return std::pair<double,double>(begin, end);
	}

};

int *LayoutSolver::pkg_idx_map = 0;

int main(int argc, char* argv[]) 
{
try
{
	double inchipgap = 0.2;
	double edgegap = 0.2; // keepout at the board edge
	int failstop = 1000;
	if (argc < 2)
	{
		std::cout << "Usage: " << argv[0] << " <layout-input.json> [layout-output.json] [-i <interchip-gap>] [-e <board-edge-gap>" << std::endl;
		return 0;
	}
	// default output name
	std::string outfn = argv[1];
	outfn.insert(outfn.find(".json"), "-output");
	for(int i=2; i<argc; i++)
	{
		if (strncmp(argv[i], "-i", strlen("-i"))==0  && argv[i+1] != 0)
			inchipgap = atof(argv[++i]);
		else if (strncmp(argv[i], "-e", strlen("-e"))==0  && argv[i+1] != 0)
			edgegap = atof(argv[++i]);
		else if (strncmp(argv[i], "-s", strlen("-s"))==0  && argv[i+1] != 0)
			failstop = atoi(argv[++i]);
		else
			outfn = argv[i];
	}
	std::cout << "Input: " << argv[1] << std::endl;
	std::cout << "Output: " << outfn << std::endl;
	std::cout << "Inter chip gap: " << inchipgap << std::endl;
	std::cout << "Board edge gap: " << edgegap << std::endl;

	// parse model
	Json::Reader reader;
	std::ifstream input;
	input.open(argv[1], std::ios_base::in);
	Json::Value root;
	bool ret = reader.parse(input, root);
	if (!ret)
	{
		std::cout << "Failed to Parse" << std::endl;
	}
	LayoutSolver* m = new LayoutSolver(root, inchipgap, edgegap);

	// create model and search engine
	Search::Options so;
	so.threads = 0;		// use as many threads as possible
	so.stop = new Search::FailStop(failstop);
	//so.cutoff = Search::Cutoff::luby(1);


#if 1
	DFS<LayoutSolver> e(m,so);
	//RBS<DFS, LayoutSolver> e(m,so);

	// search and print all solutions
	struct _timeb begin;
	struct _timeb end;
	_ftime_s( &begin );
	LayoutSolver* s = e.next();
	if (s != 0) 
	{
		_ftime_s( &end );
		float dmsec = (end.time - begin.time)*1000.0 + (end.millitm - begin.millitm)*1.0;

		s->print(); 
		std::cout << "Time : " << dmsec << std::endl;

		// write json
		std::ofstream output;
		output.open(outfn, std::ios_base::out);
		s->printLayout(output, root);
		output.close();

		delete s;
		delete m;

		return 0;
	}
	else if (e.stopped() )
	{
		std::cout << "Search stopped with too many failures" << std::endl;
		// write default json
		std::ofstream output;
		output.open(outfn, std::ios_base::out);
		m->printLayout(output, root);
		output.close();
	}
	else
		std::cout << "No solution" << std::endl;

#else
	Gist::dfs(m);
#endif
	delete m;
	return -1;
}
catch (std::bad_alloc& e)
{
	std::cerr << "ERROR: out of memory (" << e.what() << ")" << std::endl;
	return E_OUTOFMEMORY;
}
catch (std::exception& e)
{
	std::cerr << "ERROR: " << e.what() << std::endl;
	return E_FAIL;
}
}


/*
*
* The problem definition generated from CyPhy2Schematic in JSON
* "boardWidth" 
* "boardHeight"
* "packages" : {
*     "width" : 
*     "height" : 
*     "constraint" : {
*		"type" :  exact | relative-pkg | range
*       "x" :        // pre-defined position
*       "y" :
*		"rotation" : 0 | 1 | 2 | 3
*		"layer" : 0 | 1 | ...
*       "pkg-idx" :
*     }
*/