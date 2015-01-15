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

#pragma once
#include "stdafx.h"
#include "ComHelp.h"
#include "GMECOM.h"
#include "UdmBase.h"
#include <string>
#include "GMEVersion.h"

#if GME_VERSION_MAJOR >= 11
#include "Gme.h"
#endif

namespace GMEConsole
{
	CComBSTR BSTRFromUTF8(const std::string & utf8);

	class Console
	{
	friend class RawComponent;

#if GME_VERSION_MAJOR <= 10 && GME_VERSION_MINOR <= 2
	private:
#else
	public:
#endif
		static CComPtr<IGMEOLEApp> gmeoleapp;
	public:
		static void setupConsole(CComPtr<IMgaProject> project);
		static void freeConsole()
		{
			if (gmeoleapp != 0)
			{
				gmeoleapp.Release();
				gmeoleapp = 0;
			}
		}
		static void SetupConsole(CComPtr<IMgaProject> project) { setupConsole(project); }

		static void writeLine(const std::string& message, msgtype_enum type)
		{
			if (gmeoleapp == 0) {
				switch (type) {
				case MSG_NORMAL:
				case MSG_INFO:
				case MSG_WARNING:
					printf("%s", message.c_str());
					break;
				case MSG_ERROR:
					fprintf(stderr, "%s", message.c_str());
					break;
				}
			} else {
				if(S_OK != gmeoleapp->ConsoleMessage(CComBSTR(message.length(), message.c_str()), type))
					throw udm_exception("Could not write to GME console.");
			}
		}

		static void writeLine(const std::wstring& message, msgtype_enum type)
		{
			if (gmeoleapp == 0) {
				switch (type) {
				case MSG_NORMAL:
				case MSG_INFO:
				case MSG_WARNING:
					wprintf(L"%s", message.c_str());
					break;
				case MSG_ERROR:
					fwprintf(stderr, L"%s", message.c_str());
					break;
				}
			} else {
				if(S_OK != gmeoleapp->ConsoleMessage(CComBSTR(message.length(), message.c_str()), type))
					throw udm_exception("Could not write to GME console.");
			}
		}

		static void clear()
		{
			if(gmeoleapp!=0)
				gmeoleapp->put_ConsoleContents(NULL);
		}

		static void setContents(const std::string& contents)
		{
			if (gmeoleapp != 0)
				if(S_OK != gmeoleapp->put_ConsoleContents( CComBSTR(contents.length(),contents.c_str()) ))
					throw udm_exception("Could not set the contents of GME console.");
		}
		static void setContents(const std::wstring& contents)
		{
			if (gmeoleapp != 0)
				if(S_OK != gmeoleapp->put_ConsoleContents( CComBSTR(contents.length(),contents.c_str()) ))
					throw udm_exception("Could not set the contents of GME console.");
		}

		class Error
		{
		public:
			static void writeLine(const std::string& message)
			{
				Console::writeLine(message,MSG_ERROR);
			}
			static void writeLine(const std::wstring& message)
			{
				Console::writeLine(message,MSG_ERROR);
			}
		};
		class Out
		{
		public:
			static void writeLine(const std::string& message)
			{
				Console::writeLine(message, MSG_NORMAL);
			}
			static void writeLine(const std::wstring& message)
			{
				Console::writeLine(message, MSG_NORMAL);
			}
		};
		class Warning
		{
		public:
			static void writeLine(const std::string& message)
			{
				Console::writeLine(message, MSG_WARNING);
			}
			static void writeLine(const std::wstring& message)
			{
				Console::writeLine(message, MSG_WARNING);
			}
		};
		class Info
		{
		public:
			static void writeLine(const std::string& message)
			{
				Console::writeLine(message,MSG_INFO);
			}
			static void writeLine(const std::wstring& message)
			{
				Console::writeLine(message,MSG_INFO);
			}
		};
	};
}