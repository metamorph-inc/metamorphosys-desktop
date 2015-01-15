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

///////////////////////////////////////////////////////////////////////////
// ComponentConfig.h, component configuration parameters.
//
// Initially generated by the component wizard. Feel free to manually edit.
///////////////////////////////////////////////////////////////////////////

// Component framework type

// COM UUID-s, names and progID
#define TYPELIB_UUID "d46a752b-671a-4824-84c8-041981d3a1ce"
#define TYPELIB_NAME "MGA Interpreter TypeLibrary (Cyber2SFC_CodeGen)"
#define COCLASS_UUID "945c49e4-39ce-4bfb-9a6e-9b01b57b459a"
#define COCLASS_NAME "MGA Interpreter CoClass (Cyber2SFC_CodeGen)"
#define COCLASS_PROGID "MGA.Interpreter.CyberCodeGen"

#define COCLASS_UUID_EXPLODED1  0x945c49e4
#define COCLASS_UUID_EXPLODED2   0x39ce
#define COCLASS_UUID_EXPLODED3   0x4bfb
#define COCLASS_UUID_EXPLODED4   0x9a
#define COCLASS_UUID_EXPLODED5   0x6e
#define COCLASS_UUID_EXPLODED6   0x9b
#define COCLASS_UUID_EXPLODED7   0x01
#define COCLASS_UUID_EXPLODED8   0xb5
#define COCLASS_UUID_EXPLODED9   0x7b
#define COCLASS_UUID_EXPLODED10  0x45
#define COCLASS_UUID_EXPLODED11  0x9a


// This name will appear in the popup window for interpreter selection.
#define COMPONENT_NAME "Cyber2SFC_CodeGen"


// This text will appear in the toolbar icon tooltip and in the menu.
#define TOOLTIP_TEXT "Cyber2SFC_Code Interpreter"

// This macro determines the component type (addon vs. interpreter):
#define GME_INTERPRETER

// The name of the paradigm(s). The GME will use this component
// for this paradigm. Separate the names of paradigms by commas.
#define PARADIGMS "CyberComposition"


#define BON_ICON_SUPPORT

#define BON_CUSTOM_TRANSACTIONS

#define REGISTER_SYSTEMWIDE
