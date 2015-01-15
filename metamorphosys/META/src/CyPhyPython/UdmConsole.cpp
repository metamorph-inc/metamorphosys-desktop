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
#include "UdmConsole.h"
#include "UdmBase.h"


namespace GMEConsole
{
	CComBSTR BSTRFromUTF8(const std::string& utf8)
	{
		if (utf8.empty())
			return CComBSTR();

		// Fail if an invalid input character is encountered
		const DWORD conversionFlags = MB_ERR_INVALID_CHARS;

		const int utf16Length = ::MultiByteToWideChar(CP_UTF8, conversionFlags, utf8.data(), utf8.length(), NULL, 0);
		if (utf16Length == 0)
		{
			DWORD error = ::GetLastError();

			throw udm_exception(
				(error == ERROR_NO_UNICODE_TRANSLATION) ? 
					"Invalid UTF-8 sequence found in input string." :
					"Can't get length of UTF-16 string (MultiByteToWideChar failed).");
		}

		BSTR utf16 = SysAllocStringByteLen(NULL, utf16Length*2);
		if (utf16 == NULL)
			throw std::bad_alloc();

		if (!::MultiByteToWideChar(CP_UTF8, 0, utf8.data(), utf8.length(), utf16, utf16Length))
		{
			DWORD error = ::GetLastError();
			SysFreeString(utf16);
			throw udm_exception("Can't convert string from UTF-8 to UTF-16 (MultiByteToWideChar failed).");
		}

		CComBSTR ret;
		ret.m_str = utf16;
		return ret;
	}

	CComPtr<IGMEOLEApp> Console::gmeoleapp=0;

	void Console::setupConsole(CComPtr<IMgaProject> project)
	{
		CComPtr<IMgaClient> client;	
		CComQIPtr<IDispatch> pDispatch;
		HRESULT s1 = project->GetClientByName(CComBSTR(L"GME.Application"),&client);

		if ((SUCCEEDED(s1)) && (client != 0))
		{
			HRESULT s2 = client->get_OLEServer(&pDispatch);
			if ((SUCCEEDED(s2)) && (pDispatch != 0))
			{
				// release here in case setupConsole was called twice
				gmeoleapp.Release();
				HRESULT s3 = pDispatch->QueryInterface(&gmeoleapp);
				if ((SUCCEEDED(s3)) && (gmeoleapp != 0))
				{
					// gmeoleapp->ConsoleClear();
					// gmeoleapp->put_ConsoleContents(NULL);
				}
				else
				{
					throw udm_exception("GME Console could not be accessed.");
				}
			}
		}
	}
}