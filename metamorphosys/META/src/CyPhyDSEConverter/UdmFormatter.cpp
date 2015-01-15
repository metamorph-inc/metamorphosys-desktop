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

#include "stdafx.h"
#include "UdmFormatter.h"
#include <sstream>
#include <iomanip>
#include "UdmGme.h"

using namespace std;

namespace GMEConsole
{
	std::string Formatter::MakeObjectHyperlink(const std::string & text, const Udm::Object& object)
	{
		// Number magic, see uniqueId_type GmeObject::uniqueId() const 
		unsigned long id = object.uniqueId(), c, p;
		p=id%100000000;
		c=id/100000000;
		c+=100;
		ostringstream ostr; ostr <<"<a href=\"mga:id-" << hex << setfill('0')<<setw(4) << c <<'-'<< setw(8)<< p << "\">" << text << "</a>";
		return ostr.str();
	}

	std::string Formatter::MakeColored(const std::string & text, COLORREF color)
	{
		ostringstream ostr; ostr << "<font color=\"#" << hex << setfill('0') << setw(2) << (int)GetRValue(color) 
			<<setw(2) << (int)GetGValue(color) <<setw(2) << (int)GetBValue(color) << "\">" << text <<"</font>";
		
		return ostr.str();
	}


	std::string Formatter::MakeObjectHyperlink2(const Udm::Object& object)
	{
		std::string id = UdmGme::UdmId2GmeId(object.uniqueId());
		ostringstream ostr; ostr <<"<a href=\"mga:" << id << "\">" << id << "</a>";
		return ostr.str();
	}
}

/*
<font color="#ffffff">
My very first html page RGB
</font>
*/