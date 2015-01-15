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

// Component configuration file automatically generated as ComponentConfig.h
// by ConfigureComponent on Tue Oct 30 10:10:18 2007


#define RAWCOMPONENT

// COM UUID-s, names and progID
#define TYPELIB_UUID "98025EA9-BBDA-4457-8AE8-9D16A259A4EC"
#define TYPELIB_NAME "MGA AddOn TypeLibrary (CyPhyAddOn)"
#define COCLASS_UUID "C8051EB3-7355-4D96-83A2-37EBDC567DC0"
#define COCLASS_NAME "MGA AddOn CoClass (CyPhyAddOn)"
#define COCLASS_PROGID "MGA.AddOn.CyPhyAddOn"

// This name will appear in the popup window for interpreter selection.
#define COMPONENT_NAME "CyPhyAddOn"


// This text will appear in the toolbar icon tooltip and in the menu.
#define TOOLTIP_TEXT "CyPhy AddOn"


// This #define determines the interpreter type:
#define GME_ADDON
#define ADDON_EVENTMASK (OBJEVENT_CREATED | OBJEVENT_ATTR | OBJEVENT_PRE_DESTROYED | OBJEVENT_CONNECTED | OBJEVENT_DISCONNECTED)
// The name of the paradigm(s). The GME will use this component
// for this paradigm. Separate the names of paradigms by commas.
#define PARADIGMS "CyPhyML"


// not defined: #define BON_ICON_SUPPORT

// not defined: #define BON_CUSTOM_TRANSACTIONS

// not defined: #define REGISTER_SYSTEMWIDE

// Just to please the whims of those Microsoft jerks:
#define COCLASS_UUID_EXPLODED1 0xC8051EB3
#define COCLASS_UUID_EXPLODED2  0x7355
#define COCLASS_UUID_EXPLODED3  0x4D96
#define COCLASS_UUID_EXPLODED4  0x83
#define COCLASS_UUID_EXPLODED5  0xA2
#define COCLASS_UUID_EXPLODED6  0x37
#define COCLASS_UUID_EXPLODED7  0xEB
#define COCLASS_UUID_EXPLODED8  0xDC
#define COCLASS_UUID_EXPLODED9  0x56
#define COCLASS_UUID_EXPLODED10  0x7D
#define COCLASS_UUID_EXPLODED11  0xC0