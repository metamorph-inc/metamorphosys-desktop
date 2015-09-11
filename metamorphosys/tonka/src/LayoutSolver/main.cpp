/*
*  Authors:
*    Sandeep Neema <neemask@metamorphsoftware.com>
*
* This file includes the main functoin
* The code here parses the command line arguments, and sets up the problem for the layout solver
*   and invokes Gecode Search engines to solve the defined problem
*/

#include "layout-solver.h"
#include "optionparser.h"


// argument categories:
// 1 - problem definition (chipgap, edgegap, input output files) - should be in Json
// 2 - search tree configuration (center oriented, corner oriented, mix with threshold)
// 3 - solver configuration (threads, maxtries)
// 4 - diagnostics (-nouser, -nooverlap, -noedge)
// 5 - advanced debugging (-debug)
 enum  optionIndex { 
	/*Usage*/				Unknown, Help,
	/*Problem Definition*/	Chipgap, Edgegap,
	/*Search Engine Opts*/	Threads, Maxtries,
	/*Search Tree*/			CenterPlace, CornerPlace, // more to come
	/*Search Order*/		SearchOrder, /*StaticSort_BCF=0, DynamicSort_MCF=1,*/ // more to come
	/*Diagnostics*/			Incremental, //Nosolve, Nouser, Nooverlap, Noedge,
	/*Advanced Debugging*/	Debug
 };
 const option::Descriptor usage[] =
 {
  {Unknown,		0,"" , ""    ,    option::Arg::None,     "USAGE: LayoutSolver [options] <layout.json> [output.json]\n\n"
                                                     "Options:" },
  {Help,		0,"h", "help",    option::Arg::None,     "  -h,\v  --help \tPrint usage and exit." },
  {Chipgap,		0,"i", "chipgap", option::Arg::Required, "  -i,\v  --chipgap \tSpecify the minimium space between chips (default=0.2mm" },
  {Edgegap,		0,"e", "edgegap", option::Arg::Required, "  -e,\v  --edgegap \tSpecify the minimium space from the board edge (default=0.2mm" },
  {Threads,		0,"t", "threads", option::Arg::Required, "  -t,\v  --threads \tNumber of threads that search engine should use (0=max-available-cores, n.)" },
  {Maxtries,	0,"s", "maxtries",option::Arg::Required, "  -s,\v  --maxtries \tNumber of search tree nodes that search engine should explore (default=10000)." },
  {SearchOrder,	0,"o", "searchorder",option::Arg::Required, "  -o,\v  --searchorder \tSorting order for placing chips: 0=StaticSort-BigChipFirst, 1=DynamicSort-MostConstrainedFirst (default=0)." },
  {Incremental,	0,"", "incremental",option::Arg::None, "  ,\v  --incremental \tIn case solution fails try to apply constraint incrementally till the first one fails" },
  {Debug,		0,"g", "debug",   option::Arg::None,     "  -g,\v  --debug \tLaunch Search Tree explorer." },
  {0,0,0,0,0,0}
 };

// global
LayoutOptions solverOptions;

using namespace Gecode;

int GistDebug(LayoutSolver *);

int main(int argc, char* argv[]) 
{
   argc-=(argc>0); argv+=(argc>0); // skip program name argv[0] if present
   std::vector<char *> preOpts;
   while(argc>0)
   {
	   std::cout << argv[0] << std::endl;
	   if (argv[0][0] ==  '-')
		   break;
	   preOpts.push_back(argv[0]);
	   argc--;
	   argv++;
   }

   option::Stats  stats(usage, argc, argv);
   option::Option *options = new option::Option[stats.options_max];
   option::Option *buffer = new option::Option[stats.buffer_max];
   option::Parser parse(usage, argc, argv, options, buffer);

   if (parse.error())
     return 1;

   if (options[Help] || (argc == 0 && preOpts.size() < 1))
   {
     option::printUsage(std::cout, usage);
     return 0;
   }

   if (parse.nonOptionsCount() < 1 && preOpts.size() < 1)
   {
	   option::printUsage(std::cout, usage);
	   return 0;
   }

	double inchipgap = options[Chipgap].arg ? atof(options[Chipgap].arg) : 0.2;
	double optionEdgeGap = options[Edgegap].arg ? atof(options[Edgegap].arg) : 0.2;

	double nthreads = options[Threads].arg ? atoi(options[Threads].arg) : 0;
	int failstop = options[Maxtries].arg ? atol(options[Maxtries].arg) : 100000;

	int searchorder = options[SearchOrder].arg ? atoi(options[SearchOrder].arg) : 0;

	bool debug = options[Debug];

	const char *infn = (parse.nonOptionsCount() > 0) ? parse.nonOption(0) : preOpts[0];
	// default output name
	std::string outfn = infn;
	if (parse.nonOptionsCount() > 1)
		outfn = parse.nonOption(1);
	else if (preOpts.size() > 1)
		outfn = preOpts[1];
	else if (outfn.find(".json") != std::string::npos)
		outfn.insert(outfn.find(".json"), "-output");
	else
		outfn = outfn + "-output";

	std::cout << "Input: " << infn << std::endl;
	std::cout << "Output: " << outfn << std::endl;

	std::cout << "Inter chip gap: " << inchipgap << std::endl;
	std::cout << "Board edge gap: " << optionEdgeGap << std::endl;

	// search engine
	try 
	{
		// parse model
		Json::Reader reader;
		std::ifstream input;
		input.open(infn, std::ios_base::in);
		Json::Value root;
		bool ret = reader.parse(input, root);
		if (!ret)
		{
			std::cout << "Failed to Parse Layout Json: " << infn << std::endl;
			return -1;
		}
		{
			root["chipGap"] = inchipgap;
			root["boardEdgeGap"] = optionEdgeGap;
		}

		solverOptions.searchOrder = searchorder;

		//  construct and initialize layout solver with problem definition
		LayoutSolver* m = new LayoutSolver(root);

		m->CalculateDensity();

		m->DeclareVariables();
		m->PostOverlapConstraints();
		m->PostEdgeConstraints();
		m->PostUserChipConstraints();
		m->PostUserGlobalConstraints();
		m->PostBrancher();

		// advanced debugging with Gist visualizer
		if (debug)
		{
			GistDebug(m);
			delete m;
			return 0;
		}

		Search::Options so;
		so.threads = nthreads;		// 0 = use as many, 1-n fixed 
		so.stop = new Search::FailStop(failstop);

		DFS<LayoutSolver> e(m,so);

		// search and print all solutions
		struct _timeb begin;
		struct _timeb end;
		_ftime_s( &begin );

		std::cout << "Starting Search @" << ctime(&(begin.time));

		LayoutSolver* s = e.next();
		if (s != 0) 
		{
			_ftime_s( &end );
			std::cout << "Search successful" << std::endl;

			float dmsec = (end.time - begin.time)*1000.0 + (end.millitm - begin.millitm)*1.0;

			s->print(); 
			std::cout << "Total Time : " << dmsec << std::endl;

			// write json
			std::ofstream output;
			output.open(outfn, std::ios_base::out);
			s->printLayout(output, root);
			output.close();

			delete s;
			delete m;

			return 0;
		}

		if (e.stopped() )
		{
			std::cout << "Search failed after too many retries (try increasing the maxRetries parameter in your testbench)" << std::endl;
		}
		else
		{
			std::cout << "Search failed with no solution " << std::endl;
		}
		delete m;

		// a) test user constraints incrementally
		if (options[Incremental])
		{
			LayoutSolver *sol = 0;
			LayoutSolver *ns = new LayoutSolver(root);
			ns->DeclareVariables();
			ns->PostOverlapConstraints();
			ns->PostEdgeConstraints();
			ns->PostBrancher();

			// first solve with overlap and edge constraints only
			so.stop = new Search::FailStop(1000);
			DFS<LayoutSolver> testSolution(ns, so);
			LayoutSolver *s = testSolution.next();
			if (s == 0)		// conflict 
			{
				if (testSolution.stopped())
					std::cout << "Critically constrained problem - search fails with too many tries even without applying user-defined constraints" << std::endl;
				else
					std::cout << "Overconstrained problem - no solution found even without applying user-defined constraints" << std::endl;
			}
			else
				sol = s;

			std::cout << "Trying to solve with incrementally applying package constraints" << std::endl;
			int i = 0; 
			for(; i<ns->numChips; i++) 
			{
				ns->PostUserChipConstraints(i); // apply ith package constraint
				ns->PostUserGlobalConstraints(i); // apply ith package constraint
				so.stop = new Search::FailStop(1000);
				DFS<LayoutSolver> testSolution(ns, so);
				LayoutSolver *s = testSolution.next();
				if (s == 0)		// conflict 
				{
					if (testSolution.stopped())
						std::cout << "Constraints of package " << root["packages"][i]["name"].asString() << " causes search to fail with too many tries after applying the constraints of this package " << std::endl;
					else
						std::cout << "Constraints of package " << root["packages"][i]["name"].asString() << " causes conflict with overlap or board-edge constraint or constraints of prior packages [0.." << i-1 << "]" << std::endl;

					break;
				}
				else if (s != 0)
					sol = s;
			}

			if (sol != 0)
			{
				std::cout << "Generating Layout with last non-conflicting solution obtained by applying overlap and board-edge constraints";
				if (i>0)
					std::cout << " and constraints of packages [0.." << i-1 << "]" << std::endl;
				else
					std::cout << std::endl;

				std::ofstream output;
				output.open(outfn, std::ios_base::out);
				sol->printLayout(output, root);
				output.close();
			}
			else
			{
				std::cout << "Unsolvable problem - can not find any solution that respects package overlap as well as board edge constraints" << std::endl;
			}
		}

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

// for debugging layout problem with GIST
class GistInspector : public Gist::Inspector {
public:
	GistInspector(){}
	void inspect(const Space& node) {
		static_cast<const LayoutSolver&>(node).print();
	}
private:
	GistInspector(const GistInspector& a) {}

};

int GistDebug(LayoutSolver *m)
{
#ifndef _DEBUG
	// we /DELAYLOAD gecode gist under Release configuration
	if (LoadLibraryA("GECODEGIST-4-2-1-R-X86.DLL") == nullptr)
	{
		std::cerr << "Could not load '""GECODEGIST-4-2-1-R-X86.DLL""'. Check that it and its dependencies are on the %PATH% or in the application directory" << std::endl;
		return ERROR_MOD_NOT_FOUND;
	}
#endif
	GistInspector inspector;
	Gist::Options opts;
	opts.inspect.click(&inspector);
	Gist::dfs(m, opts);
	return 0;
}