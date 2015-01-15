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

#define RAWCOMPONENT

// COM UUID-s, names and ProgID
#define TYPELIB_UUID "EA572AF5-9B0A-4ED0-B5A9-ED814C5E6552"
#define TYPELIB_NAME "MGA Interpreter TypeLibrary CyPhyDSRefiner)"
#define COCLASS_UUID "080C83C0-5144-477F-B763-8E4A8246DF49"
#define COCLASS_NAME "MGA Interpreter CoClass (CyPhyDSRefiner)"
#define COCLASS_PROGID "MGA.Interpreter.CyPhyDSRefiner"


// This name will appear in the popup window for interpreter selection.
#define COMPONENT_NAME "CyPhyDSRefiner"


#define TOOLTIP_TEXT "Design Space Refinement Tool"


// This #define determines the interpreter type:
#define GME_INTERPRETER
// The name of the paradigm(s). The GME will use this component
// for this paradigm. Separate the names of paradigms by commas.
#define PARADIGMS "CyPhyML"


#define BON_ICON_SUPPORT


#define REGISTER_SYSTEMWIDE

// Just to please the whims of those Microsoft jerks:
#define COCLASS_UUID_EXPLODED1 0x080C83C0
#define COCLASS_UUID_EXPLODED2  0x5144
#define COCLASS_UUID_EXPLODED3  0x477F
#define COCLASS_UUID_EXPLODED4  0xB7
#define COCLASS_UUID_EXPLODED5  0x63
#define COCLASS_UUID_EXPLODED6  0x8E
#define COCLASS_UUID_EXPLODED7  0x4A
#define COCLASS_UUID_EXPLODED8  0x82
#define COCLASS_UUID_EXPLODED9  0x46
#define COCLASS_UUID_EXPLODED10  0xDF
#define COCLASS_UUID_EXPLODED11  0x49
