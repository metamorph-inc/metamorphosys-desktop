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

#ifndef __CGLOG_CPP
#define __CGLOG_CPP

#ifdef _MSC_VER
#pragma warning (disable : 4786)
#pragma warning (disable : 4503) 
#endif


#include "CGLog.h"

void CGLog::print(const std::string& msg)
{
	if ( logFile.is_open())
	{
		logFile<<msg<<std::endl;
	}

}
	
void CGLog::close()
{
	if ( logFile.is_open())
		logFile.close();
	if(instance)
		delete instance, instance = 0;
}

CGLog::CGLog(){};
CGLog::CGLog( const std::string& file_path, bool append)
{
	filePath = file_path;
	if(append)
		logFile.open( filePath.c_str(), std::ios::app);
	else
		logFile.open( filePath.c_str());
}

CGLog::~CGLog()
{
	if ( logFile.is_open() && instance)
	{	
		logFile.close();
		delete instance, instance = 0;
	}
}

std::string CGLog::filePath = "";
CGLog* CGLog::instance = 0;

CGLog* CGLog::Instance()
{
    if( instance == 0)
    {
        instance = new CGLog();
	}
    return instance;
}

CGLog* CGLog::Instance(const std::string& file_path, bool append)
{
    if( instance == 0)
    {
        instance = new CGLog(file_path, append);
	}
    return instance;
}
//Open log file
extern void openLogFile(const std::string& file_path, bool append)
{
	CGLog *log = CGLog::Instance(file_path, append);
}
// Close log file
extern void closeLogFile()
{
	CGLog *log = CGLog::Instance();
	log->close();
}

extern void printLog( const std::string& msg)
{
	CGLog *log = CGLog::Instance();
	log->print(msg);
}

#endif //__CGLOG_CPP
