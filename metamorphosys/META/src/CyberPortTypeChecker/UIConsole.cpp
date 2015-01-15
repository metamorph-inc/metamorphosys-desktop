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

#include "StdAfx.h"
#include "UIConsole.h"
#include "UI.h"


UIConsole::UIConsole(void)
{
}


UIConsole::~UIConsole(void)
{
}

BOOL UIConsole::initialize(CString captionText, UINT captionBoxCtrlId, const CRect rect, CWnd * parentWnd, UINT dispBoxCtrlID)
{
	CRect captionRect(rect.left, rect.top, rect.right, rect.top + UI::CAPTION_HEIGHT);	// Caption Box Area
	CRect displayRect(rect.left, captionRect.bottom, rect.right, rect.bottom);		// Display Box Area

	// Create Caption as the CStatic
	if(!this->caption.Create(captionText, 
							WS_CHILD | WS_VISIBLE | WS_BORDER, 
							captionRect,
							parentWnd,
							captionBoxCtrlId))
	{
		AfxMessageBox(_T("Failed to Create \"") + captionText + _T("\"'s Caption Box! Closing."));
		return FALSE;
	}

	// Create console /*for CListBox*/
	if(!this->listBox.Create(WS_CHILD | WS_VISIBLE | WS_BORDER | WS_VSCROLL | WS_HSCROLL | LBS_DISABLENOSCROLL | LBS_EXTENDEDSEL | LBS_MULTIPLESEL | LBS_HASSTRINGS | LBS_NOTIFY | LBS_OWNERDRAWFIXED | LBS_WANTKEYBOARDINPUT,
							displayRect,
							parentWnd, 
							dispBoxCtrlID)) 
	{	
		AfxMessageBox(_T("Failed to Create \"") + captionText + _T("\"'s Display Box! Closing."));
		return FALSE;
	}

	return TRUE;
}
