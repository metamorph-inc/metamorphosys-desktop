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

#include "CADEnvironmentSettings.h"
#include "CADSoftwareEnvirUtils.h"
#include <CommonUtilities.h>
#include <windows.h>
#include <malloc.h>
//#include <stdio.h>
#include <iostream>
#include <vector>
//#include <list>
#include <tchar.h>
#include <algorithm>
#include <sstream>
#include <ISISConstants.h>
// #define BUFFER 8192

namespace isis
{		

	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Description:	
	//		Based on the input arguments (i.e. in_argv), this function sets the PRO_COMM_MSG_EXE system 
	//		environment variable and retrieves the system settings.  See the function 
	//		SetupCreoEnvironmentVariables (above in this file) for more information about setting the evirmoment 
	//		variable and the source of the retrieved data.
	//
	//		If the data cannot be retrieved then isis::application_exception will be thrown.
	void SetCreoEnvirVariable_RetrieveSystemSettings( int   in_argc, 
													char *in_argv[],
													std::string		&out_CreoStartCommand,
													std::string		&out_XmlInputFile_PathAndFileName,  // may not contain path, if so reads from the same directory as the bat file location
													std::string		&out_LogFile_PathAndFileName )		// may not contain path, if so is written to the same directory as the bat file location
																	throw (isis::application_exception)
	{
		///////////////////////////////////////////////////////////////
		// Log: Use environment  variables or look values in registry
		//////////////////////////////////////////////////////////////
		char *envVariable_CREO_PARAMETRIC_USE_ENVIR_VARS;
		envVariable_CREO_PARAMETRIC_USE_ENVIR_VARS = getenv ("CREO_PARAMETRIC_USE_ENVIR_VARS");
		std::clog << std::endl;
		if ( envVariable_CREO_PARAMETRIC_USE_ENVIR_VARS == NULL )
			std::clog << std::endl << "Environment Variable CREO_PARAMETRIC_USE_ENVIR_VARS: Not Defined";
		else 
			std::clog << std::endl << "Environment Variable CREO_PARAMETRIC_USE_ENVIR_VARS: " << envVariable_CREO_PARAMETRIC_USE_ENVIR_VARS;


		int xmlFileArg;
		int logFileArg;

		//New Case: %WORKING_DIR%   %PARTS_DIR%    %ASSEMBLY_XML_FILE%     %LOG_FILE%     %EXIT_PROMPT%


		xmlFileArg = 2;
		logFileArg = 3;

		//////////////////////////
		// Log Arguments
		//////////////////////////
		std::clog << std::endl << "arg 0, EXE:                     "  << in_argv[0];				// 0 EXE				e.g. C:\Program Files\Proe ISIS Extensions\bin\CADCreoParametricDatumEditor.exe
		std::clog << std::endl << "arg 1, Switch                   "  << in_argv[1];				// 1 Switch				e.g. -check/-edit
		std::clog << std::endl << "arg 2, ASSEMBLY_XML_FILE:       "  << in_argv[xmlFileArg];		// 3 ASSEMBLY_XML_FILE	e.g. RLC_Assembly_5_CAD.xml
		std::clog << std::endl << "arg 3, LOG_FILE:                "  << in_argv[logFileArg];		// 4 LOG_FILE			e.g. RLC_Assembly_5_CAD.xml.log

		//////////////////////////
		// Prompt Before Exiting
		//////////////////////////

		/////////////////
		// Directories
		/////////////////

		out_XmlInputFile_PathAndFileName = in_argv[xmlFileArg];
		out_LogFile_PathAndFileName = in_argv[logFileArg];

		/////////////////////////////
		// Creo Start Command
		/////////////////////////////
		out_CreoStartCommand = ""; 
		isis::SetupCreoEnvironmentVariables(out_CreoStartCommand);

		/////////////////////////////
		// Log Resulting Settings
		/////////////////////////////
		std::clog << std::endl << std::endl << "************** Begin Environment Variables and System Settings *****************";
		std::clog << std::endl << "CreoStartCommand:              "	<< out_CreoStartCommand; 
		std::clog << std::endl << "PRO_COMM_MSG_EXE:              "	<<  getenv ("PRO_COMM_MSG_EXE"); 
		std::clog << std::endl << "XmlInputFile_PathAndFileName:  "	<< out_XmlInputFile_PathAndFileName; 
		std::clog << std::endl << "LogFile:                       "	<< out_LogFile_PathAndFileName;  
		std::clog << std::endl << "************** End Environment Variables and System Settings *****************";

	}

} // end namespace isis