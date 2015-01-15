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

echo off
set INSTALL_DIR=install

REM for /f "tokens=1-5 delims=/ " %%d in ("%date%") do set INSTALL_DIR=%INSTALL_DIR%_%%g_%%e_%%f_%%d
REM for /f "tokens=1-5 delims=:" %%d in ("%time%") do set INSTALL_DIR=%INSTALL_DIR%_%%d_%%e_%%f

echo on
mkdir %INSTALL_DIR%
cd %INSTALL_DIR%

REM install GME
wget -c http://build.isis.vanderbilt.edu/job/GME_x64_msi/label=build-slave6/lastSuccessfulBuild/artifact/trunk/Install/GME_x64.msi
start /wait msiexec /I GME_x64.msi /qn /Lv* GME_x64.install.log ALLUSERS=1

REM install META
wget -c http://build.isis.vanderbilt.edu/job/META_core/lastSuccessfulBuild/artifact/deploy/META_x64_Core.msi
start /wait msiexec /I META_x64_Core.msi /qn /Lv* META_x64_Core.install.log ALLUSERS=1

cd ..