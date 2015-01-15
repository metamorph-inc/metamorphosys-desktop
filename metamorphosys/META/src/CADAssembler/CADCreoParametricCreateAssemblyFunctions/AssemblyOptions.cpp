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

=======================
This version of the META tools is a fork of an original version produced
by Vanderbilt University's Institute for Software Integrated Systems (ISIS).
Their license statement:

Copyright (C) 2011-2014 Vanderbilt University

Developed with the sponsorship of the Defense Advanced Research Projects
Agency (DARPA) and delivered to the U.S. Government with Unlimited Rights
as defined in DFARS 252.227-7013.

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

#include "AssemblyOptions.h"
#include "boost\algorithm\string.hpp"
#include <boost\lexical_cast.hpp>
#include <log4cpp/Category.hh>
#include <log4cpp/OstreamAppender.hh>
#include <vector>
#include "CommonDefinitions.h"

namespace isis{

AssemblyOptions AssemblyOptions::Instance;

// Format of input string: optionname=value[;optionname=value][;optionname=value]...
// Option names:
// - stopassembly=<ComponentID>:<0|1> stops assembling after the component added. 0 means stop after the guide, 1 means stop after the full constraint
// - faillevel=<0|1|2> 0 means fail if feature redefine fails, 1 means fail if regen failed, 2 means don't fail during assembly
// - fullregen=<0|1> 1 means full assembly regen after adding each part during the assembly
// - applyparamsafter=<0|1> 1 means apply the cyphy parameters after the assembly has been completed
AssemblyOptions& AssemblyOptions::Create(const string &input)
{
	log4cpp::Category& logcat_fileonly = log4cpp::Category::getInstance(LOGCAT_LOGFILEONLY);
	vector<string> tokens;
	boost::split(tokens, input, boost::is_any_of(";"));
	for (vector<string>::const_iterator it = tokens.begin(); it != tokens.end(); ++it)
	{
		string keyword;
		string value;
		int eq = it->find("=");
		if (eq == string::npos)
		{
			keyword = it->substr(0);
		} else {
			keyword = it->substr(0, eq);
			value = it->substr(keyword.size()+1);
		}
		if (value.size()==0)
		{
			logcat_fileonly.warnStream() << "Invalid assembly option, no value specified. keyword=" << keyword;
			continue;
		}
		if (keyword=="applyparamsafter")
		{
			Instance.ApplyParamsAfter = value=="1";
		} else if (keyword=="stopassembly")
		{
			vector<string> tokens;
			boost::split(tokens, value, boost::is_any_of(":"));
			if (tokens.size()==0)
			{
				logcat_fileonly.warnStream() << "Invalid assembly option, wrong value for stopassembly. value=" << value;
				continue;
			}
			Instance.StopAssemblyComponentID = tokens[0];
			try{
				Instance.StopAssemblyAt = (eStopAssembly)(::boost::lexical_cast<int>(tokens[1])+1);
			} catch (...)
			{
				// conversion from string to int failed
				logcat_fileonly.warnStream() << "Invalid assembly option, wrong value for stopassembly. value=" << value;
			}
		} else if (keyword=="faillevel")
		{
			Instance.FailLevel = (eFailLevel)::boost::lexical_cast<int>(value);
		} else if (keyword=="fullregen")
		{
			Instance.FullRegen = value=="1";
		} else {
			logcat_fileonly.warnStream() << "Invalid assembly option, wrong keyword. keyword=" << keyword;
		}
	}
	return Instance;
}

}; // namespace isis