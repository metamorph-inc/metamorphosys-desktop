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

#ifndef WINDOWS_FUNCTIONS_H
#define	WINDOWS_FUNCTIONS_H

#pragma warning( disable : 4290 )  // a future feature : exception specification, i.e. throw

#include<string>
#include "isis_application_exception.h"

namespace isis_CADCommon
{
	// std::string isis_GetCurrentDirectoryA() throw (isis::application_exception); 


	// Get the location of the public folder.
	// Example, C:\Users\Public\Documents
	// If directory not found, then isis::application_exception would be thrown.
	void GetPublicDocuments(std::wstring& out_PublicDocumentsPath) throw (isis::application_exception);


	///////////////////////////////////////////////////////////////////////////////////////////////////
	// If in_DirectoryName is an actual directory on the system
	//		then
	//			return true
	// If in_DirectoryName points to a file or points to nothing that exists in file system
	//		then
	//			return false
	//
	// If in_DirectoryName contains spaces, do NOT enclose the string in  parentheses.
	// Passing the following string will work:
	//		C:\Users\rowens\Documents\Microsoft Research
	//		e.g string myString =  "C:\\Users\\rowens\\Documents\\Microsoft Research"
	//
	//	Passing the following string will NOT work:
	//		"C:\\Users\\rowens\\Documents\\Microsoft Research\"
	//		e.g string myString = "\"C:\\Users\\rowens\\Documents\\Microsoft Research\""
	bool DirectoryExists(const std::string& in_DirectoryName);

	// Create a directory
	void isis_CreateDirectory(const std::string dirname);

} // END namespace isis_CADCommon

#endif