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

// cl exec_unelevated.cpp

// Not exactly unelevated: the Integrety Level is High. But execed programs can't write to HKLM, so it is good enough

#define STRICT
#define UNICODE
#define _UNICODE

#include <Windows.h>
#include <WinSafer.h>

#include <stdio.h>

#pragma comment(linker, "/subsystem:CONSOLE")
#pragma comment(lib, "Advapi32.lib")

int wmain(int argc, wchar_t *argv[])
{
    SAFER_LEVEL_HANDLE hLevel = NULL;
    if (!SaferCreateLevel(SAFER_SCOPEID_USER, SAFER_LEVELID_NORMALUSER, SAFER_LEVEL_OPEN, &hLevel, NULL))
    {
        return GetLastError();
    }

    HANDLE hRestrictedToken = NULL;
    if (!SaferComputeTokenFromLevel(hLevel, NULL, &hRestrictedToken, 0, NULL))
    {
        SaferCloseLevel(hLevel);
        return GetLastError();
    }

    SaferCloseLevel(hLevel);

    wchar_t commandLine[1024*32];
    wcscpy_s(commandLine, GetCommandLineW());
    // skip our exe name
    wchar_t *cmd = commandLine;
    if (*cmd != L'\"')
    {
        cmd = wcschr(cmd, L' ') + 2;
        // FIXME: could be tab or other whitespace
    }
    else
    {
        cmd = wcschr(cmd + 1, L'"') + 2;
    }

    //Create startup info
    STARTUPINFO si = {0};
    PROCESS_INFORMATION pi = {0};
    //si.lpDesktop = L"winsta0\\default";
    si.cb = sizeof( si );
    si.hStdInput = GetStdHandle(STD_INPUT_HANDLE);
    si.hStdOutput = GetStdHandle(STD_OUTPUT_HANDLE);
    si.hStdError = GetStdHandle(STD_ERROR_HANDLE);
    si.dwFlags = STARTF_USESTDHANDLES;

    // printf("%S\n", cmd);

    // Start the new (non-elevated) restricted process
    if (!CreateProcessAsUser(hRestrictedToken, NULL, cmd, NULL, NULL, TRUE, CREATE_UNICODE_ENVIRONMENT, GetEnvironmentStringsW() /* leaks */, NULL, &si, &pi))
    {
        CloseHandle(hRestrictedToken);
        return GetLastError();
    }

    CloseHandle(hRestrictedToken);
    CloseHandle(pi.hThread);

    DWORD wso = WaitForSingleObject(pi.hProcess, INFINITE);
    if (wso != WAIT_OBJECT_0)
        return wso;

    DWORD exitCode = 2;
    GetExitCodeProcess(pi.hProcess, &exitCode);
    CloseHandle(pi.hProcess);

    return exitCode;
}
