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

#ifndef TEST_METALINK_H
#define TEST_METALINK_H

#include <string>
#include <AssemblyCreationViaInputFile.h>
#include <map>

// The following should only be defined for testing.  Do not put this in SVN with this macro defined.
//#define TEST_META_LINK

namespace isis
{
	void Test_CreateAssembly(   std::map<string, isis::CADComponentData>	&in_out_CADComponentData_map,
								isis::MetaLinkAssemblyEditor &in_out_MetaLinkAssemblyEditor,
								std::string &out_ParentAssemblyInstanceID	)
																			throw (isis::application_exception);

	void Test_AddComponentToAssembly(   const std::string &in_ParentAssemblyInstanceID,	
										std::map<string, isis::CADComponentData>	&in_out_CADComponentData_map,
									isis::MetaLinkAssemblyEditor &in_out_MetaLinkAssemblyEditor)
																			throw (isis::application_exception);

	void Test_ModifyParameters(  const std::string &in_ParentAssemblyInstanceID,	
								std::map<string, isis::CADComponentData>	&in_out_CADComponentData_map,
									isis::MetaLinkAssemblyEditor &in_MetaLinkAssemblyEditor)
																			throw (isis::application_exception);

	// Before calling Test_ConstrainComponent, you must first call Test_AddComponentToAssembly
	void Test_ConstrainComponent(  const std::string &in_ParentAssemblyInstanceID,	
									std::map<string, isis::CADComponentData>	&in_out_CADComponentData_map,
									isis::MetaLinkAssemblyEditor &in_MetaLinkAssemblyEditor)
																			throw (isis::application_exception);

	void Test_AddSerachPaths(const std::list<std::string> &in_SearchPaths,
							isis::MetaLinkAssemblyEditor &in_MetaLinkAssemblyEditor ) throw (isis::application_exception);
	

}  // END namespace isis


#endif

