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

#include <CommonUtilities.h>
#include <fstream>
#include <locale>

//#include <iostream>

namespace isis
{

	std::string	EncloseStringInDoubleQuotes( const std::string &in_String )
	{
		std::string OutString;	

			OutString = "\"" + in_String + "\"";

		return OutString;
	}
	///////////////////////////////////////////////////////////////////////////////////////////////////
	std::string ConvertToUpperCase(const std::string &in_String)
	{
		std::string temp_string(in_String);
		std::locale loc;
		for (std::string::iterator p = temp_string.begin(); temp_string.end() != p; ++p)    *p = toupper(*p, loc);
		return temp_string;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////
	std::string ConvertToLowerCase(const std::string &in_String)
	{
		std::string temp_string(in_String);
		std::locale loc;
		for (std::string::iterator p = temp_string.begin(); temp_string.end() != p; ++p)    *p = tolower(*p, loc);
		return temp_string;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////

	bool IsANumber(const std::string &in_String)
	{
		if ( in_String.size() == 0 ) return false;

		for ( std::string::const_iterator i( in_String.begin()); i != in_String.end(); ++i)
		{
			if ( isdigit(*i) == 0 ) // not a digit
			{
				if ( !( *i == '.' ||  *i == '+' || *i == '-' || *i == ' ' ))  return false;
			}
		}

		return true;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////

	double ConvertToDouble(const std::string &in_String) throw (isis::application_exception)
	{

		if ( !IsANumber(in_String) )
		{
			std::string TempError = "Function ConvertToDouble, Could not convert string \"" + in_String + "\" to a double.";
			throw isis::application_exception(TempError.c_str());
		}

		return atof(in_String.c_str() );
	}
	///////////////////////////////////////////////////////////////////////////////////////////////////
	void DeleteModel_IfItExists( ProFamilyName name, 
								 ProMdlType    type ) throw (isis::application_exception)
	{
		bool ModelExists = true;
		ProMdl p_handle;
		try
		{
			isis_ProMdlRetrieve( name, type, &p_handle);
		}
		catch (...)
		{
			ModelExists = false;
		}

		if ( ModelExists )
		{
			isis_ProMdlDelete( p_handle );
		}

	}

	///////////////////////////////////////////////////////////////////////////////////////////////////
	void isis_DeleteFile(const std::string &in_PathAndFileName_or_FileName)
	{

		std::string DeleteFileName = EncloseStringInDoubleQuotes( in_PathAndFileName_or_FileName );

		// std::cout << std::endl << "DeleteFile ----> File: " << DeleteFileName;
		char  DeleteInstruction[1024];
		strcpy( DeleteInstruction, "del ");
		strcat( DeleteInstruction, (char *)DeleteFileName.c_str());
		system(DeleteInstruction);
	}


	///////////////////////////////////////////////////////////////////////////////////////////////////
	void IfFileExists_DeleteFile(const std::string &in_PathAndFileName_or_FileName)
	{

		std::string DeleteFileName = EncloseStringInDoubleQuotes( in_PathAndFileName_or_FileName );

		//std::cout << std::endl << "DeleteFile ----> File: " << DeleteFileName;
		char  DeleteInstruction[1024];
		strcpy( DeleteInstruction, "IF EXIST ");
		strcat( DeleteInstruction, (char *)DeleteFileName.c_str());
		strcat( DeleteInstruction, " DEL ");
		strcat( DeleteInstruction, (char *)DeleteFileName.c_str());
		//std::cout << std::endl << "IfFileExists_DeleteFile: " << DeleteInstruction;
		system(DeleteInstruction);
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////
	void ExecuteSystemCommand(const std::string &in_Instruction)
	{
		char  Instruction[1024];
		strcpy( Instruction, in_Instruction.c_str());
		system(Instruction);
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////

	void CopyFileIsis(	const std::string &in_From_PathAndFileName, 
					const std::string &in_To_Path_or_PathAndFileName )
	{
		char  CopyInstruction[1024];

		std::string From;
		std::string To;

		From = EncloseStringInDoubleQuotes( in_From_PathAndFileName );

		To = EncloseStringInDoubleQuotes(in_To_Path_or_PathAndFileName);

		//std::cout << std::endl << "CopyFile ----> From: " << From;
		//std::cout << std::endl << "CopyFile ----> To:   " << To;

		strcpy( CopyInstruction, "copy /y ");
		//strcpy( CopyInstruction, "xcopy ");	xcopy with /q did not suppress the file copy message.
		strcat( CopyInstruction, (char *)From.c_str());
		strcat( CopyInstruction, " ");
		strcat( CopyInstruction, (char *)To.c_str());
		// strcat( CopyInstruction, " /y /q"); 
		system(CopyInstruction);
	}


	///////////////////////////////////////////////////////////////////////////////////////////////////
	bool FileExists(const char * in_PathAndFilename) 
	{ 
		std::ofstream TestFile;
		
		// Try to open the file in read mode.
		TestFile.open(in_PathAndFilename, std::ios::in );
		
		if (TestFile.is_open())
		{
			TestFile.close();
			return true;
		}
		else
		{
			return false;
		}
	} 
	

} // end namespace isis