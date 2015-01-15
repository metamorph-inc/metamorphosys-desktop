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

#ifndef ISIS_CREO_STRING_H
#define ISIS_CREO_STRING_H

#include "isis_application_exception.h"
#include <string>
#include <iostream>


namespace isis
{

	const int CREO_STRING_DO_NOT_CHECK_NUM_CHARS = -999999;

	// Description:
	//	This function creates a representation of a string based on passing a constructor one of the following types:
	//		char *
	//		std::string
	//		wchar_t
	//	Conversion operators will return any of the three types.  Internally to this class, all three data types are
	//	persisted. 
	//
	//	Pre-Conditions:
	//		in_charArray (both char and wchar) must be null terminated char arrays.
	//		The input strings can be zero length strings.
	//
	//	Post-Conditions:
	//		Conversion operators return any of the three types.
	//		Assignment operators do a deep copy, the lvalue is independent of the rvalue
	//		The destructor deletes the char/wchar_t arrays stored on the heap.
	//
	//	Thread Safety:
	//		The conversion operators are thread safe.  The assignment operators are NOT thread safe.  If objects from 
	//		this class are going to be used in multiple threads, then instantiate the object with the required 
	//		char/wchar/string data, and after that only use the conversion operators.
	class MultiFormatString
	{	
		public:

			MultiFormatString( 
						int	in_MaxNumberChars = CREO_STRING_DO_NOT_CHECK_NUM_CHARS);
			MultiFormatString( const	std::string &in_String,
						int		in_MaxNumberChars = CREO_STRING_DO_NOT_CHECK_NUM_CHARS);
			MultiFormatString( const char		*in_charArray,
						int		in_MaxNumberChars = CREO_STRING_DO_NOT_CHECK_NUM_CHARS);
			MultiFormatString( const wchar_t	*in_charArray,
						int		in_MaxNumberChars = CREO_STRING_DO_NOT_CHECK_NUM_CHARS);

			// The following line will invoke "operator const char* ()"
			// std::cout << std::endl << MultiFormatString_1;
			// cout favors char* over std::string.
			operator const char* () const;

			// The following line will invoke "operator const wchar_t* ()"
			operator const wchar_t* () const;

			// The following line will invoke "operator const std::string ();"
		    // std::cout << std::endl << (std::string)MultiFormatString_1;
			// The cast (std::string) is necessary because by default cout favors char*.
			// If you know that MultiFormatString was created with a std::string, then use the cast (std::string)
			// so that an internal conversion to a char* would not be necessary.
			operator const std::string& () const; 

			MultiFormatString& operator=( const char* );
			MultiFormatString& operator=( const wchar_t* );
			MultiFormatString& operator=( const std::string& );
			MultiFormatString& operator=( const	MultiFormatString &in_MultiFormatString );	

			MultiFormatString( const MultiFormatString &in_MultiFormatString); 

			// size is the number of characters.  The buffer sizes are size + 1
			unsigned int size() const;

			~MultiFormatString();

		private:
			std::string		narrowString;
			bool			narrowString_Defined;		
			char			*narrowCharArray;
			wchar_t			*wideCharArray;
			int				maxNumberChars;

			void setString(const char* );
			void setString(const wchar_t* );
			void setString(const std::string& );

			void setNarrowString();
			void setNarrowCharArray();
			void setWideCharArray();

			void setAllStrings();

			void deleteAndNullStringValues();
			void nullStringValues();

			// Don't allow copy constructor
			//MultiFormatString( const MultiFormatString &in_MultiFormatString); 
	
			//MultiFormatString(					MultiFormatString &in_MultiFormatString);
			//MultiFormatString( volatile const	MultiFormatString &in_MultiFormatString);
			//MultiFormatString( volatile		MultiFormatString &in_MultiFormatString);

			// Don't allow assignment operator
			//MultiFormatString& operator=( const	MultiFormatString &in_MultiFormatString );
			//MultiFormatString& operator=( 			MultiFormatString &in_MultiFormatString);
			//MultiFormatString& operator=( 			MultiFormatString in_MultiFormatString);

			//std::string maxStringLengthError( const std::string &in_String, int in_MaxNumberofChars) const;
	};


} //END namespace isis

#endif