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

#include "resource.h"
// Messages to be communicated between Observer and Notifier
class IMSG
{
public:
	UINT		fromCtrlId;				// Control Id of the Notifier = Resource Id Associated with the class's object
	CString		srcPortName;
	CString		srcPortType;
	CString		dstPortName;
	CString		dstPortType;
	//CString		linkType;
	enum		ActionType
	{									// RELEVANT Msg Attr:
				DEFAULT,
				ADD_PORT,				// srcPortName, srcPorType , fromCtrlId
				//FIND_PORT,				// srcPortName, fromCtrlId
				DELETE_PORT_LINK,		// srcPortName, dstPortName
				MODIFY_PORT_NAME,		// srcPortName, fromCtrlId
				MODIFY_PORT_TYPE,		// srcPortType, fromCtrlId
				MODIFY_PORT,			// srcPortName, srcPortType, fromCtrlId
				ADD_PORT_LINK			// linkType
	};

	ActionType	actionType;
	IMSG()
	{
		fromCtrlId = ID_DUMMY;
		srcPortName = _T("");
		srcPortType = _T("");
		dstPortName = _T("");
		dstPortType = _T("");
		actionType = DEFAULT;
	}
};


/*
MODIFY_PORT_TYPE:
									IMSG: 
									SRCPORTTYPE		new
									DSTPORTNAME		old
MODIFY_PORT_NAME:
									IMSG: 
									SRCPORTNAME		new
									DSTPORTNAME		old
									
ADD_PORT:
									IMSG: 
									SRCPORTNAME		new
									SRCPORTTYPE		new
*/