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

/******************************************/
/* General purpose string routines.       */
/* Tihamer Levendovszky 2009			  */
/*										  */
/******************************************/
#include <iostream>
#include <string>
#include <algorithm>
#include <functional>
#include <fstream>
#include <sstream>
#include <iomanip>
#include <locale>



template<class charT>
void tolower(std::basic_string<charT>& str,
             const std::locale& loc = std::locale())
{
  typedef std::basic_string<charT>  string_type;
  for(string_type::iterator it = str.begin();
      it != str.end(); ++it)
  {
    *it = std::tolower(*it, loc);
  }
}


template<class charT>
void toupper(std::basic_string<charT>& str,
             const std::locale& loc = std::locale())
{
  typedef std::basic_string<charT>  string_type;
  for(string_type::iterator it = str.begin();
      it != str.end(); ++it)
  {
    *it = std::toupper(*it, loc);
  }
}


template<class charT>
int case_insensitive_compare(std::basic_string<charT> lhs, 
							 std::basic_string<charT> rhs,
							 const std::locale& loc = std::locale())
{
  tolower(lhs, loc);
  tolower(rhs,loc);
  return loc(lhs, rhs);
}



template <
	class CharType, 
	class Traits, 
	class Allocator
>
bool starts_with(const std::basic_string <CharType, Traits, Allocator>& str, 
				 const std::basic_string <CharType, Traits, Allocator>& value)
{
	return (str.compare(0,value.length(), value)==0);
}


template <
	class CharType, 
	class Traits, 
	class Allocator
>
bool ends_with(const std::basic_string <CharType, Traits, Allocator>& str, 
				 const std::basic_string <CharType, Traits, Allocator>& value)
{
	std::basic_string <CharType, Traits, Allocator>::size_type value_size=value.length();
	return (str.compare(str.length()-value_size,value_size, value)==0);
}


template <
	class CharType, 
	class Traits, 
	class Allocator,
	class SourceType
>
void to_string (std::basic_string <CharType, Traits, Allocator>& dst, SourceType src)
{
	std::basic_stringstream <CharType, Traits, Allocator> sstream;
	sstream << src;
	if(!sstream)
		throw std::bad_cast();

	dst=sstream.str();
}


template <
	class CharType, 
	class Traits, 
	class Allocator,
	class TargetType
>
void convert_to (TargetType & dst, const std::basic_string <CharType, Traits, Allocator>& src)
{
	std::basic_istringstream <CharType, Traits, Allocator> sstream (src);

	sstream >> dst;

	// Error in stream
	if(!sstream)
		throw std::bad_cast();

	// We could not convert everything, 
	// there are characters remaining in the string
	sstream.get();
	if(!sstream.eof())
		throw std::bad_cast();
	
}


template <
	class CharType, 
	class Traits, 
	class Allocator,
	class TargetType,
	class FormatType
>
void convert_to (TargetType & dst, const std::basic_string <CharType, 
				 Traits, Allocator>& src,  FormatType  format)
{
	std::basic_istringstream <CharType, Traits, Allocator> sstream (src);

	sstream >> format;
	sstream >> dst;

	// Error in stream
	if(!sstream)
		throw std::bad_cast();

	// We could not convert everything
	// there are characters reemaining in the string
	sstream.get();
	if(!sstream.eof())
		throw std::bad_cast();
	
}

template<class charT>
void trim_left(std::basic_string<charT>& str,
               const std::locale& loc =std::locale())
{
	for(std::basic_string<charT>::iterator it = str.begin(); 
                                         it != str.end(); ++it)
  {
    if(!isspace(*it, loc))
    {
      str.erase(str.begin(),it);
      return;
    }
  }
  // If it was entirely whitespace
  str.clear();
}


template<class charT>
void trim_right(std::basic_string<charT>& str,
                const std::locale& loc=std::locale())
{
	for(std::basic_string<charT>::reverse_iterator rit = str.rbegin();
                                   rit != str.rend(); ++rit)
  {
    if(!isspace(*rit, loc))
    {
      str.erase(rit.base(), str.end());
      return;
    }
  }
    // If it was entirely whitespace
  str.clear();
}


// Sorts the iterator range consisting of strings aphabetically according to the locale.
template<class RandomAccessIterator>
void string_sort(RandomAccessIterator begin, RandomAccessIterator end, const std::locale& loc=std::locale())
{
	sort(begin, end, loc);
}
