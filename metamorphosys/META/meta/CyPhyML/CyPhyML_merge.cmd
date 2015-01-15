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

Setlocal EnableDelayedExpansion

set XME=CyPhyML.xme
set LEFT_R=r27825
set RIGHT_R=r27878

if not exist %GME_ROOT%\GME\Parser\xme_id2guid.py (echo Need SVN GME & exit /b 1)

git init . || exit /b !ERRORLEVEL!
c:\Python27\python %GME_ROOT%\GME\Parser\xme_id2guid.py %XME%.merge-left.%LEFT_R% || exit /b !ERRORLEVEL!
move /y %XME%.merge-left_guids.xme %XME%_guids.xme || exit /b !ERRORLEVEL!
git add %XME%_guids.xme || exit /b !ERRORLEVEL!
git commit -am "left" || exit /b !ERRORLEVEL!

git checkout -b working || exit /b !ERRORLEVEL!
c:\Python27\python %GME_ROOT%\GME\Parser\xme_id2guid.py %XME%.working || exit /b !ERRORLEVEL!
git commit -am "working" || exit /b !ERRORLEVEL!

git checkout master || exit /b !ERRORLEVEL!
git checkout -b right || exit /b !ERRORLEVEL!
c:\Python27\python %GME_ROOT%\GME\Parser\xme_id2guid.py %XME%.merge-right.%RIGHT_R% || exit /b !ERRORLEVEL!
move /y %XME%.merge-right_guids.xme %XME%_guids.xme || exit /b !ERRORLEVEL!
git commit -am "right" || exit /b !ERRORLEVEL!

git merge working
echo resolve conflict on %XME%_guids.xme: make new project/@guid
pause
git add %XME%_guids.xme || exit /b !ERRORLEVEL!
git commit -am "merged" || exit /b !ERRORLEVEL!

set PYTHONPATH=%GME_ROOT%\Tests\
c:\python27\python %GME_ROOT%\Tests\GPyUnit\util\gme.py xme2mga %XME%_guids.xme || exit /b !ERRORLEVEL!
c:\python27\python %GME_ROOT%\Tests\GPyUnit\util\gme.py mga2xme %XME%_guids.mga %XME% || exit /b !ERRORLEVEL!
set PYTHONPATH=
svn resolved %XME% || exit /b !ERRORLEVEL!
rd /s/q .git
rem git diff master
rem head -4 CyPhyML.xme
