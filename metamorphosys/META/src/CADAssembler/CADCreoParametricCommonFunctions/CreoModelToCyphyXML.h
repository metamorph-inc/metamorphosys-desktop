// Copyright (C) 2013-2015 MetaMorph Software, Inc

// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this data, including any software or models in source or binary
// form, as well as any drawings, specifications, and documentation
// (collectively "the Data"), to deal in the Data without restriction,
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Data, and to
// permit persons to whom the Data is furnished to do so, subject to the
// following conditions:

// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Data.

// THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

// =======================
// This version of the META tools is a fork of an original version produced
// by Vanderbilt University's Institute for Software Integrated Systems (ISIS).
// Their license statement:

// Copyright (C) 2011-2014 Vanderbilt University

// Developed with the sponsorship of the Defense Advanced Research Projects
// Agency (DARPA) and delivered to the U.S. Government with Unlimited Rights
// as defined in DFARS 252.227-7013.

// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this data, including any software or models in source or binary
// form, as well as any drawings, specifications, and documentation
// (collectively "the Data"), to deal in the Data without restriction,
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Data, and to
// permit persons to whom the Data is furnished to do so, subject to the
// following conditions:

// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Data.

// THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

#ifndef CREOMODELTOCYPHYCML_H
#define CREOMODELTOCYPHYCML_H

#include <sstream>
#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include "ProMdl.h"
#include "ProFeature.h"
using namespace std;

namespace isis
{
	class ResourceDep
	{
	private:
		string id;
		string name;
		string notes;
		string path;
	public:
		ResourceDep(const string &_id, const string &_name, const string &_notes, const string &_path) : id(_id), name(_name), notes(_notes), path(_path)
		{
		}
		void write(ostream &s, int tab = 0) const;
		string getName() const
		{
			return name;
		}
		string getId() const
		{
			return id;
		}
		void setId(string id)
		{
			this->id = id;
		}
	};

	class FixedValue
	{
	private:
		string datatype;
		string id;
		string unit;
		string value;
	public:
		FixedValue(const string &_datatype, const string &_id, const string &_unit, const string &_value) : datatype(_datatype), id(_id), value(_value), unit(_unit)
		{
		}
		void write(ostream &s, int tab) const;
	};

	class Parameter
	{
	private:
		string name;
		string keyw;
		FixedValue value;
	public:
		Parameter(const string &_keyw, const string& _name, const FixedValue& _value) : keyw(_keyw), name(_name), value(_value)
		{
		}
		void write(ostream &s, int tab) const;
	};

	class Datum
	{
	private:
		string ID;
		string datumname;
		string type;
		string name;
	public:
		Datum(const string &_ID, const string &_datumname, const string &_type, const string& _name) : ID(_ID), datumname(_datumname), type(_type), name(_name)
		{
		}
		void write(ostream &s, int tab) const;
	private:

	};

	class CADModel
	{
	public:
		void setNotes(const string &notes)
		{
			this->notes = notes;
		}
		void write(ostream &s, int tab) const;
		void addDatum(const Datum &datum)
		{
			datums.push_back(datum);
		}
		void addParam(const Parameter &param)
		{
			parameters.push_back(param);
		}
		void addMMetric(const Parameter &param)
		{
			modelmetrics.push_back(param);
		}
		void setMainResourceID(const string& id)
		{
			mainresourceid = id;
		}
	private:
		string notes;
		string mainresourceid;
		vector<Datum> datums;
		vector<Parameter> parameters;
		vector<Parameter> modelmetrics;
	private:
		void writebegin(ostream &s) const;
		void writeend(ostream &s) const;
	};

	class Component
	{
	private:
		string name;
		string version;
		string avmid;
		CADModel cadmodel;
	public:
		Component()
		{
		}
		Component (const string &_name) : name(_name)
		{
		}
		void write(ostream &s, int tab = 0) const;
		void setVersion(const string version)
		{
			this->version = version;
		}
		void setCADModel(const CADModel& model)
		{
			this->cadmodel = model;
		}
		void setName(const string &name)
		{
			this->name = name;
		}
		void setAVMId(const string& avmid)
		{
			this->avmid = avmid;
		}
		void addResource(const ResourceDep& res)
		{
			resources.push_back(res);
		}
		CADModel& getModel()
		{
			return cadmodel;
		}
		int numResources()
		{
			return static_cast<int>(resources.size());
		}
		ResourceDep& getResource(int i)
		{
			return resources[i];
		}
		bool resourceExists(const string &name)
		{
			for (vector<ResourceDep>::const_iterator it = resources.begin(); it != resources.end(); ++it)
			{
				if (it->getName()==name) return true;
			}
			return false;
		}

	private:
		vector<ResourceDep> resources;
		void writebegin(ostream &s) const;
		void writeend(ostream &s) const;
	};

	int FillComponentStructure(ProMdl mdl, Component &comp);
	std::string CreoModelToCyphyXML(ProMdl mdl);
	bool IsDatumType(ProFeattype type);
}

#endif