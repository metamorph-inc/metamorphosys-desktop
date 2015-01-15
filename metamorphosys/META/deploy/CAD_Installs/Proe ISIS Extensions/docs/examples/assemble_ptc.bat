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

REM	The following system environment variables must be defined:
REM	PROE_ISIS_EXTENSIONS	// typically set to	C:\Program Files\Proe ISIS Extensions
REM	PROE_INSTALL_PATH	// typically set to	C:\Progra~1\proeWi~1.0\
REM	PRO_COMM_MSG_EXE	// typically set to C:\Program Files\proeWildfire 5.0\x86e_win64\obj\pro_comm_msg
REM	See "Meta SVN\sandbox\CAD\Installs\Proe ISIS Extensions\0ReadMe.txt" for the complete setup instructions.

set EXE_FILE_NAME=assemble_ptc.exe
set EXE="%PROE_ISIS_EXTENSIONS%\bin\%EXE_FILE_NAME%"
set PROE_START_CMD="%PROE_INSTALL_PATH%/bin/proe.exe -g:no_graphics -i:rpc_input"
set WORKING_DIR="C:\Users\rowens\Documents\Meta SVN\sandbox\seisele\Parts\CAD_TEST_3"


if exist %EXE% goto  :EXE_FOUND
@echo off
echo Error: Could not find %EXE_FILE_NAME%.
echo    Your system is not properly configured to run %EXE_FILE_NAME%.
echo    Please see "Meta SVN\sandbox\CAD\Installs\Proe ISIS Extensions\0ReadMe.txt"
echo    for instructions on how to configure your system.
pause
exit
:EXE_FOUND


set EXIT_PROMPT="YES"

set ASSEMBLY_XML_FILE="IFV_cfg37_hierarchical_Cad.xml"
set LOG_FILE=%ASSEMBLY_XML_FILE%.log

%EXE%     %PROE_START_CMD%    "%PROE_ISIS_EXTENSIONS%"     %WORKING_DIR%      %ASSEMBLY_XML_FILE%     %LOG_FILE%     %EXIT_PROMPT%

