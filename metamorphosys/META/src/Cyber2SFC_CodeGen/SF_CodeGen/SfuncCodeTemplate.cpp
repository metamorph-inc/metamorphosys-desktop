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

#include "SfuncCodeTemplate.hpp"

SfuncCodeTemplate::TemplateString::TemplateString( void ) {

	std::ostringstream templateStringStream;

	templateStringStream <<
"#define S_FUNCTION_NAME  %s"                                  << std::endl <<
"#define S_FUNCTION_LEVEL 2"                                   << std::endl <<
                                                                  std::endl <<
"#include \"simstruc.h\""                                      << std::endl <<
"#include \"%s\""                                              << std::endl <<
                                                                  std::endl <<
"static void mdlInitializeSizes(SimStruct *S)"                 << std::endl <<
"{"                                                            << std::endl <<
"    ssSetNumSFcnParams(S, 0);"                                << std::endl <<
"    if (ssGetNumSFcnParams(S) != ssGetSFcnParamsCount(S)) {"  << std::endl <<
"        return;"                                              << std::endl <<
"    }"                                                        << std::endl <<
"    if (!ssSetNumInputPorts(S, 1)) return;"                   << std::endl <<
"    ssSetInputPortWidth(S, 0, %d);"                           << std::endl <<
"    ssSetInputPortDirectFeedThrough(S, 0, 1);"                << std::endl <<
"    if (!ssSetNumOutputPorts(S, 1)) return;"                  << std::endl <<
"    ssSetOutputPortWidth(S, 0, %d);"                          << std::endl <<
"    ssSetNumSampleTimes(S, 1);"                               << std::endl <<
"    ssSetOptions(S, SS_OPTION_EXCEPTION_FREE_CODE |"          << std::endl <<
"                 SS_OPTION_USE_TLC_WITH_ACCELERATOR);"        << std::endl <<
"    %s"                                                       << std::endl <<
"}"                                                            << std::endl <<
                                                                  std::endl <<
"static void mdlInitializeSampleTimes(SimStruct *S)"           << std::endl <<
"{"                                                            << std::endl <<
"    ssSetSampleTime(S, 0, INHERITED_SAMPLE_TIME);"            << std::endl <<
"    ssSetOffsetTime(S, 0, 0.0);"                              << std::endl <<
"}"                                                            << std::endl <<
                                                                  std::endl <<
"static void mdlOutputs(SimStruct *S, int_T tid)"              << std::endl <<
"{"                                                            << std::endl <<
"    %s"                                                       << std::endl <<
"}"                                                            << std::endl <<
                                                                  std::endl <<
"static void mdlTerminate(SimStruct *S)"                       << std::endl <<
"{"                                                            << std::endl <<
"}"                                                            << std::endl <<
                                                                  std::endl <<
                                                                  std::endl <<
                                                                  std::endl <<
"#ifdef  MATLAB_MEX_FILE"                                      << std::endl <<
"#include \"simulink.c\""                                      << std::endl <<
"#else"                                                        << std::endl <<
"#include \"cg_sfun.h\""                                       << std::endl <<
"#endif"                                                       << std::endl <<
                                                                  std::endl;

	_templateString = templateStringStream.str();
}
