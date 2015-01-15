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

#ifndef FINITE_ELEMENT_ANALYSIS_H
#define FINITE_ELEMENT_ANALYSIS_H

namespace isis
{

	bool IsAFEAAnlysisDeckBasedRun( const CADAssemblies &in_CADAssemblies );
	bool IsFEAAnalysisAbaqusModelBasedRun( const CADAssemblies &in_CADAssemblies );

	void Create_FEADecks_BatFiles( 
					const TopLevelAssemblyData							&in_TopLevelAssemblyData,
					std::map<std::string, Material>						&in_Materials,
					const std::string									&in_WORKING_DIR,
					const std::string									in_ProgramName_Version_TimeStamp,
					std::map<std::string, isis::CADComponentData>		&in_CADComponentData_map )
																	throw (isis::application_exception);

	void RetrieveDatumPointCoordinates( const std::string							&in_AssemblyComponentID,
									const std::string							&in_PartComponentID,
									std::map<string, isis::CADComponentData>	&in_CADComponentData_map,
									const MultiFormatString						&in_DatumName,
									CADPoint									&out_CADPoint);


	// Pre-Conditions
	//	in_TopLevelAssemblyData.CADAnalyses.list<AnalysisFEA> can contain only one item.  Multiple Analysis
	//  per assembly are not currently supported.
	//
	//	in_NastranMaterialID_to_CompnentID_map can be empty.  If empty, then FEAElementID="", instead of e.g. PSOLID_1
	void CreateXMLFile_FEA_AnalysisMetaData( 
						const std::string									&in_PathAndFileName,
						const TopLevelAssemblyData							&in_TopLevelAssemblyData,
						std::map<std::string, Material>						&in_Materials,
						std::map<std::string, std::string>                  &in_NastranMaterialID_to_CompnentID_map,
						std::map<std::string, isis::CADComponentData>		&in_CADComponentData_map )
																	throw (isis::application_exception);

}
#endif // FINITE_ELEMENT_ANALYSIS_H