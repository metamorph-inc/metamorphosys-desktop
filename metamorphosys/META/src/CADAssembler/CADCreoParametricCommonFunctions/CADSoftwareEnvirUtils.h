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

#ifndef CAD_SOFTWARE_ENVIR_UTILS_H
#define CAD_SOFTWARE_ENVIR_UTILS_H

#pragma warning( disable : 4290 )

#include "isis_application_exception.h"
#include <string>
#include <vector>

namespace isis
{

	//////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Description:
	//		This function either retrieves system settings information from the registry or from system 
	//		environment variables.   
	//
	//		If  (environment  variable CREO_PARAMETRIC_USE_ENVIR_VARS == TRUE)
	//			then
	//				settings are retrieved from system environment variables.
	//				See  "0Readme - CreateAssembly.txt" for environment variable setup instructions.
	//				This file is typical located at "C:\Program Files\META\Proe ISIS Extensions\"
	//
	//			else
	//				settings are retrieved from the registry
	//
	//	Note –	This function also sets the system environment variable PRO_COMM_MSG_EXE.  This setting is 
	//			required to run the Creo SDK.
	//	
	//	Pre-Conditions:
	//		None
	//
	//	Post-Conditions:
	//		If  ( environment  variable CREO_PARAMETRIC_USE_ENVIR_VARS == TRUE)
	//			then
	//				if CREO_PARAMETRIC_INSTALL_PATH or CREO_PARAMETRIC_COMM_MSG_EXE system environment variable not defined
	//					then
	//						throw isis::application_exception
	//			else
	//				if Creo install information not in the windows registry
	//					then
	//						throw isis::application_exception
	//
	//		If no exceptions
	//			then
	//				set PRO_COMM_MSG_EXE system environment variable
	//				return out_CreoStartCommand		
	void SetupCreoEnvironmentVariables( bool			in_graphicsModeOn,
										bool			in_CreoExceptInputFromThisProgramAndCreoUI,
										std::string &out_CreoStartCommand ) throw (isis::application_exception);

}
#endif
