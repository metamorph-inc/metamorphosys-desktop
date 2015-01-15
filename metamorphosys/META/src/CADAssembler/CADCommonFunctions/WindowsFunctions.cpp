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

#include "WindowsFunctions.h"
#include "Windows.h"
#include <sstream>

#include <iostream>
#include <fstream>
#include <sstream>
#include <string> 
#include <Shlobj.h>



namespace isis_CADCommon
{
	// the ability to get the current working directory is provided by 
	//  ::boost::filesystem::current_path();

	/*
	std::string isis_GetCurrentDirectoryA()
	{
		char buffer[MAX_PATH+1];

		DWORD errorCode;

		errorCode = GetCurrentDirectoryA(MAX_PATH, buffer);

		if ( errorCode == 0 )
		{
				std::stringstream errorString;
				errorString <<
					"Function - GetCurrentDirectoryA, returned a zero error code, which indicates that the function failed.";
				throw isis::application_exception(errorString.str().c_str());			
		}
		
		return std::string(buffer);
	}
	*/


	void GetPublicDocuments(std::wstring& out_PublicDocumentsPath)
			throw (isis::application_exception)
	{
		//need a wchar_t* because that's what SHGetKnownFolderPath takes
		wchar_t* path = 0;
		SHGetKnownFolderPath(FOLDERID_PublicDocuments, 0, NULL, &path);
		//need wstringstream to convert to wstring
		std::wstringstream stream;
		stream << path;
		//need this to clean up memory for wchar_t*
		::CoTaskMemFree(static_cast<void*>(path));
		//populate output parameter
		out_PublicDocumentsPath = stream.str();

		if ( out_PublicDocumentsPath.size() == 0 )
		{
			std::stringstream errorString;
			errorString <<
				"Function - GetPublicDocuments, did not find the public document folder.";
			throw isis::application_exception(errorString.str().c_str());		
		}
	}


	///////////////////////////////////////////////////////////////////////////////////////////////////	
	bool DirectoryExists(const std::string& in_DirectoryName)
	{
		// http://stackoverflow.com/questions/8233842/how-to-check-if-directory-exist-using-c-and-winapi

		DWORD ftyp = GetFileAttributesA(in_DirectoryName.c_str());
		if (ftyp == INVALID_FILE_ATTRIBUTES) return false;  //something is wrong with your path!
		if (ftyp & FILE_ATTRIBUTE_DIRECTORY) return true;   // this is a directory!

	  return false;    // this is not a directory!
	}
	///////////////////////////////////////////////////////////////////////////////////////////////////
	
	void isis_CreateDirectory(const std::string dirname)
	{
		CreateDirectoryA(dirname.c_str(), 0);
	}

} // END namespace isis_CADCommon

