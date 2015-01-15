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

/*! \file CommonUtilities.h
    \brief  Common utilities used by Creo CAD applications.

	Common utilities that support operations such as copying, deleting... files/models.
*/

#ifndef COMMON_UTILITIES_H
#define COMMON_UTILITIES_H

#include <isis_ptc_toolkit_functions.h>
#include <isis_application_exception.h>
#include <string>


namespace isis
{
		std::string ConvertToUpperCase(const std::string &in_String);
		std::string ConvertToLowerCase(const std::string &in_String);

		// Number with at least one digit and possibly "+", "-", and/or "."
		bool IsANumber(const std::string &in_String);

		// throw exception if in_String cannot be coverted to a double.
		double ConvertToDouble(const std::string &in_String) throw (isis::application_exception);

		void DeleteModel_IfItExists( ProFamilyName name, 
								 ProMdlType    type ) throw (isis::application_exception);


		// in_PathAndFileName_or_FileName must not contain double quotes (i.e. “);
		// if in_PathAndFileName_or_FileName will be enclosed in double quotes. This is 
		//	necessary because an error would occur if the string contained spaces, dashes, commas...
		void isis_DeleteFile(const std::string &in_FileName);


		// in_PathAndFileName_or_FileName must not contain double quotes (i.e. “);
		// if in_PathAndFileName_or_FileName will be enclosed in double quotes.  This is 
		//	necessary because an error would occur if the string contained spaces, dashes, commas...
		void IfFileExists_DeleteFile(const std::string &in_PathAndFileName_or_FileName);

		void ExecuteSystemCommand(const std::string &in_Instruction);


		// Description:
		//		Copy in_From_PathAndFileName to in_To_Path_or_PathAndFileName
		//
		// Pre-Conditoins
		//		The input strings must not contain double quotes (i.e. “);
		//		in_From_PathAndFileName must exist.
		//		in_To_Path_or_PathAndFileName must contain a valid path and optionally a file name.
		//
		// Post-Conditions
		//		The string(s) will be enclosed in double quotes.  This is necessary because an error
		//		would occur if the string contained spaces, dashes, commas...
		//		If the files cannot be copied a message will be printed to sysout.  
		//		No exceptions will be returned regardless of success/failure.
		void CopyFileIsis(	const std::string &in_From_PathAndFileName, 
					const std::string &in_To_PathAndFileName );



		//	This program returns true if the file identified by in_PathAndFilename can be 
		//	opened in read mode.  Typically, the file would not exist if it could not be 
		//	opened in read mode; however, it is possible that it could not be opened in 
		//	read mode because the process does not have privileges to read it.  In either 
		//	case, it would be an error condition for assemble_ptc.exe. 
		bool FileExists(const char * in_PathAndFilename) ;



} // end namespace isis

#endif