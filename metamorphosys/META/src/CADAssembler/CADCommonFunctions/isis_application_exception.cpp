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

#include "isis_application_exception.h"
#include <string.h>
#include <sstream>

namespace isis
{
	application_exception::application_exception(const char *in_Message) throw() 
		: exception(in_Message) 
	{ strncpy_s(error_code, sizeof(error_code)-1, "C000000", _TRUNCATE ); }

	application_exception::application_exception(const std::string & in_Message) throw() 
		: exception(in_Message.c_str())  
	{ strncpy_s(error_code, sizeof(error_code)-1, "C000000", _TRUNCATE ); }

	application_exception::application_exception(const std::stringstream & in_Message) throw() 
		: exception(in_Message.str().c_str())  
	{ strncpy_s(error_code, sizeof(error_code)-1, "C000000", _TRUNCATE ); }


	application_exception::application_exception(const char *in_code, const char *in_Message) throw() 
		: exception(in_Message)  
	{ strncpy_s(error_code, sizeof(error_code)-1, "C000000", _TRUNCATE ); }

	application_exception::application_exception(const char *in_code, const std::string & in_Message) throw() 
		: exception(in_Message.c_str())  
	{ strncpy_s(error_code, sizeof(error_code)-1, "C000000", _TRUNCATE ); }

	application_exception::application_exception(const char *in_code, const std::stringstream & in_Message) throw() 
		: exception(in_Message.str().c_str())  
	{ strncpy_s(error_code, sizeof(error_code)-1, "C000000", _TRUNCATE ); }


	application_exception::~application_exception() throw(){};

	std::string application_exception::tostring() const {
		std::ostringstream str;
		str << this->what();
		if (componentinfo.size()>0)
			str << " " << "COMPONENT: " << componentinfo;
		if (datuminfo.size() > 0)
			str << " " << "DATUM: " << datuminfo;
		if (parameterinfo.size() > 0)
			str << " " << "PARAM: " << parameterinfo;
		return str.str();
	}

	std::ostream& operator <<(std::ostream & out, const application_exception & applEx) {
		out << (applEx.tostring());
		return out;
	}

}