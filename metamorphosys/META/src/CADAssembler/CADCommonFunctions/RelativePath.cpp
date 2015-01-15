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

#include "RelativePath.h"
#include <locale>
#include <iostream>


namespace isis_CADCommon
{

	////////////////////////////////////////////////////////////////////////////////////////////////
	std::string RelativePath_ConvertToUpperCase(const std::string &in_String)
	{
		std::string temp_string(in_String);
		std::locale loc;
		for (std::string::iterator p = temp_string.begin(); temp_string.end() != p; ++p)    *p = toupper(*p, loc);
		return temp_string;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////

	bool FindRelativePath_From_B_to_A ( const std::string	&in_Path_A, 
									    const std::string	&in_Path_B,
											  std::string	&out_RelativePath,
										bool				in_DefineRelativePathWithBackSlashes )
	{
		out_RelativePath = "";

		// Check for null strings
		if ( in_Path_A.size() == 0 || in_Path_B.size() == 0 ) return false;		

		std::string path_A = RelativePath_ConvertToUpperCase(in_Path_A);
		std::string path_B = RelativePath_ConvertToUpperCase(in_Path_B);

		// Replace any forward slashes ('/') with a back slashes ('\')
		for ( std::size_t i = 0; i < path_A.size(); ++i ) if ( path_A[i] == '/') path_A[i] = '\\';
		for ( std::size_t i = 0; i < path_B.size(); ++i ) if ( path_B[i] == '/') path_B[i] = '\\';

		// Replace  with a back slashes ('\')
		for ( std::size_t i = 0; i < path_A.size(); ++i ) if ( path_A[i] == '/') path_A[i] = '\\';
		for ( std::size_t i = 0; i < path_B.size(); ++i ) if ( path_B[i] == '/') path_B[i] = '\\';

		// Remove parentheses
		std::string tempString;
		for ( std::size_t i = 0; i < path_A.size(); ++i ) if ( path_A[i] != '\"') tempString += path_A[i];
		path_A = tempString;
		tempString = "";
		for ( std::size_t i = 0; i < path_B.size(); ++i ) if ( path_B[i] != '\"') tempString += path_B[i];
		path_B = tempString;

		// For consistency, add a closing back slash ('\') if not already there
		if ( path_A[path_A.size() - 1] != '\\' ) path_A += "\\";
		if ( path_B[path_B.size() - 1] != '\\' ) path_B += "\\";

		//std::cout << std::endl << "FindRelativePath_From_B_to_A, path_A " << path_A;
		//std::cout << std::endl << "FindRelativePath_From_B_to_A, path_B " << path_B;

		// See if A is subset of B where A starts at position 0
		if ( path_B.find( path_A) !=  0  ) return false;

		// At this point we know that B defines a path that is under/subordinate to A
		
		// Get the portion of B that is surbordinate to A
		std::string subordinate = path_B.substr(path_A.size() );
		//std::cout << std::endl << "FindRelativePath_From_B_to_A, subordinate: " << subordinate;

		// Count the number of back slashes '\'
		int numBackSlashes = 0;
		for each ( char i in subordinate ) if ( i == '\\' ) ++ numBackSlashes;

		// Delimiter string
		std::string slashDelimiterString;
		if ( in_DefineRelativePathWithBackSlashes ) 
			slashDelimiterString = "\\";
		else
			slashDelimiterString = "/";

		// Build relative path
		if ( numBackSlashes == 0 ) 
			out_RelativePath = "." + slashDelimiterString;
		else
			for ( int i = 0; i < numBackSlashes; ++i )  out_RelativePath += ".." + slashDelimiterString;
		
		return true;
	}

}