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

REM	This file was created by CyPhy2CAD.dll 1. 2. 0. 5
REM 	Date:Tue June 20 14:59:28 2013

REM
REM	The following system environment variable must be set:
REM	    PROE_ISIS_EXTENSIONS	// typically set to C:\Program Files\META\Proe ISIS Extensions
REM
REM	See "C:\Program Files\META\Proe ISIS Extensions\0Readme - CreateAssembly.txt" for the complete setup instructions.

set PARTS_DIR=".\Cad_Components_Directory"
set WORKING_DIR="C:\Temp\scratch\2012_08_14\A_Arm"


Rem ****************************
REM Create Creo Assembly
Rem ****************************

set EXE_FILE_NAME=CADCreoParametricCreateAssembly.exe
set EXE="%PROE_ISIS_EXTENSIONS%\bin\%EXE_FILE_NAME%"

set ASSEMBLY_XML_FILE="A_Arm_TestBench_Cad.xml"
set LOG_FILE=%ASSEMBLY_XML_FILE%.log
set EXIT_PROMPT="YES"

if exist %EXE% goto  :EXE_FOUND
@echo off
echo		Error: Could not find %EXE_FILE_NAME%.
echo		Your system is not properly configured to run %EXE_FILE_NAME%.
echo		Please see For instructions on how to configure your system, please see "0Readme - CreateAssembly.txt"
echo		which is typically located at "C:\Program Files\META\Proe ISIS Extensions"
pause
exit
:EXE_FOUND

%EXE%     -w %WORKING_DIR%  -a %PARTS_DIR% -i %ASSEMBLY_XML_FILE%    -l %LOG_FILE%  -p
REM Other examples
REM %EXE%     -w %WORKING_DIR%  -i %ASSEMBLY_XML_FILE%    -l %LOG_FILE%  -p
REM %EXE%     -w %WORKING_DIR%  -i %ASSEMBLY_XML_FILE%    -l %LOG_FILE%  
