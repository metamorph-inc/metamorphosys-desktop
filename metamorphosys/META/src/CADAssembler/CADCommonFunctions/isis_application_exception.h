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

/*! \file isis_application_exception.h
    \brief Wrapper class for std::exception.

	isis::application_exception wraps std::exception so that exceptions can be caught based on 
	isis::application_exception.  This provides for segregating catches of   
	isis::application_exception and std::exception.
*/
#ifndef ISIS_APPLICATION_EXCEPTION_H
#define ISIS_APPLICATION_EXCEPTION_H

#include <iostream>
#include <sstream>
#include <exception>

namespace isis
{
	//!  application_exception class. 
	/*!
	A wrapper class for std::exception.  application_exception would normally be thrown by
	CAD routines developed by ISIS. 
	*/

	class application_exception : public std::exception
	{
		public:
			application_exception(const char *in_Message)  throw() ;
			application_exception(const std::string & in_Message) throw() ;
			application_exception(const std::stringstream & in_Message) throw() ;

			application_exception(const char *in_code, const char *in_Message) throw();
			application_exception(const char *in_code, const std::string & in_Message) throw() ;
			application_exception(const char *in_code, const std::stringstream & in_Message) throw() ;

			char *get_error_code() { return error_code; }
			virtual ~application_exception() throw();
			std::string tostring() const;
			void setComponentInfo(const std::string &s)
			{
				componentinfo = s;
			}
			std::string getComponentInfo()
			{
				return componentinfo;
			}
			void setDatumInfo(const std::string &s)
			{
				datuminfo = s;
			}
			std::string getDatumInfo()
			{
				return datuminfo;
			}
			void setParameterInfo(const std::string &s)
			{
				parameterinfo = s;
			}
			std::string getParameterInfo()
			{
				return parameterinfo;
			}
	private:
		char error_code[8];
		std::string componentinfo; // possible information about the component
		std::string parameterinfo; // possible information about the parameter
		std::string datuminfo; // possible information about the datum
	};

	std::ostream& operator <<(std::ostream & out, const application_exception & applEx);
	
}

#endif  /* APPLICATION_EXCEPTION_H */