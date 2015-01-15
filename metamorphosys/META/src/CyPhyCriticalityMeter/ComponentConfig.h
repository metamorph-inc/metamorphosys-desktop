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
#define TYPELIB_UUID "0268F5DC-6F7B-46B8-9503-703E4F4075BD"
#define TYPELIB_NAME "MGA Interpreter TypeLibrary CyPhyCriticalityMeter)"
#define COCLASS_UUID "B59C028E-D9FF-4E2E-9D85-D1FBE9E1F6E5"
#define COCLASS_NAME "MGA Interpreter CoClass (CyPhyCriticalityMeter)"
#define COCLASS_PROGID "MGA.Interpreter.CyPhyCriticalityMeter"


// This name will appear in the popup window for interpreter selection.
#define COMPONENT_NAME "CyPhyCriticalityMeter"


#define TOOLTIP_TEXT "Design Space Criticality/Complexity Meter"


// This #define determines the interpreter type:
#define GME_INTERPRETER
// The name of the paradigm(s). The GME will use this component
// for this paradigm. Separate the names of paradigms by commas.
#define PARADIGMS "CyPhyML"


#define BON_ICON_SUPPORT


#define REGISTER_SYSTEMWIDE

// Just to please the whims of those Microsoft jerks:
#define COCLASS_UUID_EXPLODED1 0xB59C028E
#define COCLASS_UUID_EXPLODED2  0xD9FF
#define COCLASS_UUID_EXPLODED3  0x4E2E
#define COCLASS_UUID_EXPLODED4  0x9D
#define COCLASS_UUID_EXPLODED5  0x85
#define COCLASS_UUID_EXPLODED6  0xD1
#define COCLASS_UUID_EXPLODED7  0xFB
#define COCLASS_UUID_EXPLODED8  0xE9
#define COCLASS_UUID_EXPLODED9  0xE1
#define COCLASS_UUID_EXPLODED10  0xF6
#define COCLASS_UUID_EXPLODED11  0xE5
