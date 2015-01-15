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

#ifndef ASSEMBLE_UTILS_H
#define ASSEMBLE_UTILS_H
//#include <CommonStructures.h>
#include <isis_application_exception.h>
#include <fstream>
#include <map>
#include <AssembleUtils.h>

#include <boost/filesystem.hpp>

namespace isis
{

	// This function hase a side effect, it changed the current working directory, and the input parameter.
	::boost::filesystem::path SetupWorkingDirectory( std::string & inout_workingDirectory );

	const std::string manufacturingManifestJson_PathAndFileName = ".\\manufacturing.manifest.json";



	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//const std::string	CouldNotFindManufacturingManifestError =
	//		"\nDid not update manufacturing manifest file (" +
	//		isis::manufacturingManifestJson_PathAndFileName + ") with " + 
	//		"\nthe mapping of component-instance-ID to STEP-file-name because the manifest file " +
	//		"\ncould not be found.  The manufacturing manifest file would only exist if the " +
	//		"\nCyPhyPrepareIFab interpreter had been invoked. Typically, CyPhyPrepareIFab " +
	//		"\nwould not have been invoked."; 

	//const std::string	NotUpdatingManufacturingManifest_SeparateSTEPFilesNotRequested =
	//		"\nDid not update manufacturing manifest file (" +
	//		isis::manufacturingManifestJson_PathAndFileName + ") for the" +
	//		"\nassembled parts because separate STEP part files were not requested." +
	//		"\nAP203_E2_SEPARATE_PART_FILES or AP214_SEPARATE_PART_FILES must be requested" +
	//		"\nin order for the manufacturing manifest to be updated for assembled parts.  The " +
	//		"\nmanifest will be updated for any unassembled parts.";


	std::string GetDayMonthTimeYear();

}

#endif