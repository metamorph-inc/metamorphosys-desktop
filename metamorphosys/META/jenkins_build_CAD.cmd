@rem Copyright (C) 2013-2015 MetaMorph Software, Inc

@rem Permission is hereby granted, free of charge, to any person obtaining a
@rem copy of this data, including any software or models in source or binary
@rem form, as well as any drawings, specifications, and documentation
@rem (collectively "the Data"), to deal in the Data without restriction,
@rem including without limitation the rights to use, copy, modify, merge,
@rem publish, distribute, sublicense, and/or sell copies of the Data, and to
@rem permit persons to whom the Data is furnished to do so, subject to the
@rem following conditions:

@rem The above copyright notice and this permission notice shall be included
@rem in all copies or substantial portions of the Data.

@rem THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
@rem IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
@rem FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
@rem THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
@rem LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
@rem OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
@rem WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

@rem =======================
@rem This version of the META tools is a fork of an original version produced
@rem by Vanderbilt University's Institute for Software Integrated Systems (ISIS).
@rem Their license statement:

@rem Copyright (C) 2011-2014 Vanderbilt University

@rem Developed with the sponsorship of the Defense Advanced Research Projects
@rem Agency (DARPA) and delivered to the U.S. Government with Unlimited Rights
@rem as defined in DFARS 252.227-7013.

@rem Permission is hereby granted, free of charge, to any person obtaining a
@rem copy of this data, including any software or models in source or binary
@rem form, as well as any drawings, specifications, and documentation
@rem (collectively "the Data"), to deal in the Data without restriction,
@rem including without limitation the rights to use, copy, modify, merge,
@rem publish, distribute, sublicense, and/or sell copies of the Data, and to
@rem permit persons to whom the Data is furnished to do so, subject to the
@rem following conditions:

@rem The above copyright notice and this permission notice shall be included
@rem in all copies or substantial portions of the Data.

@rem THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
@rem IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
@rem FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
@rem THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
@rem LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
@rem OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
@rem WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

rem set JAVA_HOME=C:\Program Files (x86)\Java\jdk1.7.0_07\

pushd %~dp0

where UdmDll_3_2_VS10.dll

c:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild make_CAD.msbuild /t:All;Push_All_NuGet /fl /flp:diag;PerformanceSummary /m /nodeReuse:false
IF %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

set DBGTOOLS=%ProgramFiles%\Debugging Tools for Windows (x86)
IF "%PROCESSOR_ARCHITECTURE%" == "AMD64" set DBGTOOLS=%ProgramFiles%\Debugging Tools for Windows (x64)
IF "%PROCESSOR_ARCHITEW6432%" == "AMD64" set DBGTOOLS=%ProgramW6432%\Debugging Tools for Windows (x64)
IF NOT EXIST "%DBGTOOLS%" set DBGTOOLS=%ProgramFiles(x86)%\Debugging Tools for Windows (x86)

echo %TIME%
call "%DBGTOOLS%\srcsrv\svnindex.cmd" /debug /Ini="externals\common-scripts\srcsrv.ini" /source="%CD%" /symbols="%CD%"
IF %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%
echo %TIME%
copy_pdbs.py
IF %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%
echo %TIME%
