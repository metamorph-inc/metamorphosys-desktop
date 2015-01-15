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
#include "UIPropEditorBar.h"
#include "resource.h"

UIPropEditorBar::UIPropEditorBar(void)
{
}


UIPropEditorBar::~UIPropEditorBar(void)
{
}


BOOL UIPropEditorBar::intialize(CWnd* pParentWnd, CRect rect, UINT capBoxCtrlId, UINT capTextCtrlId, UINT contentBoxCtrlId, BOOL editable, BOOL dropDown)
{
	// initialize Caption
	CString captionText;
	DWORD	dwStyle;
	CRect captionRect(rect.left, rect.top, rect.left + CAPTION_WIDTH, rect.bottom);	// Caption Box Area
	CRect contentRect(captionRect.right, rect.top, rect.right, rect.bottom);		// Display Box Area

	// Create Caption as the CStatic
	captionText.LoadString(capTextCtrlId);
	if(!this->caption.Create(captionText, 
							WS_CHILD | WS_VISIBLE | WS_BORDER, 
							captionRect,
							pParentWnd,
							contentBoxCtrlId))
	{
		AfxMessageBox(_T("Failed to Create \"") + captionText + _T("\"'s Caption Box! Closing."));
		return FALSE;
	}

	this->dropDown = dropDown;

	if(!dropDown)
	{
		dwStyle = WS_CHILD | WS_VISIBLE /*| WS_DISABLED */| WS_BORDER | ES_AUTOHSCROLL | ES_LEFT;
	
		if(!editable)
			dwStyle |= ES_READONLY;

		if(!editor.Create(dwStyle, contentRect, pParentWnd, contentBoxCtrlId))
		{
			AfxMessageBox(_T("Failed to Create \"") + captionText + _T("\"'s Content Viewer/Editor Box! Closing."));
			return FALSE;
		}


		// Diable the editor bar currently
		disable();
	}
	else
	{
		dwStyle = WS_CHILD | WS_VISIBLE | CBS_AUTOHSCROLL | CBS_HASSTRINGS;

		if(!editable)
			dwStyle |= CBS_DROPDOWNLIST;
		else
			dwStyle |= CBS_DROPDOWN;

		// Adjust the dropdown icon to come within window
		contentRect.right -= DROPDOWN_ICON_WIDTH;

		if(!dropDownViewer.Create(dwStyle, contentRect, pParentWnd, contentBoxCtrlId))
		{
			AfxMessageBox(_T("Failed to Create \"") + captionText + _T("\"'s Content Viewer/Editor Box! Closing."));
			return FALSE;
		}

		// Insert default string
		CString defStr;
		defStr.LoadString(PROPERTY_EDITORBAR_DROPDOWN_DEFAULT_TEXT);
		dropDownViewer.AddString(defStr);

		// Disable the dropdown
		disable();
	}
	return TRUE;
}

void UIPropEditorBar::showText(LPCTSTR str)
{
	if(!dropDown)
		editor.SetWindowText(str);
	else
	{
		if(dropDownViewer.SelectString(-1, str) == CB_ERR)
		{
			// if string not found show NO TYPE
			CString defStr;
			defStr.LoadString(PROPERTY_EDITORBAR_DROPDOWN_DEFAULT_TEXT);
			dropDownViewer.SelectString(-1, defStr);
		}
	}
}

void UIPropEditorBar::clearEditorText()
{
	editor.Clear();
}

void UIPropEditorBar::addDropDownString(LPCTSTR str)
{
	// if string not already present in the drop down list then add it
	if(dropDownViewer.FindStringExact(-1, str) == CB_ERR)
	{
		dropDownViewer.AddString(str);
	}
}


void UIPropEditorBar::disable()
{
	if(!dropDown)
		editor.ModifyStyle(NULL, WS_DISABLED);
	else
		dropDownViewer.ModifyStyle(NULL, WS_DISABLED);
}

void UIPropEditorBar::enable()
{
	if(!dropDown)
		editor.ModifyStyle(WS_DISABLED, NULL);
	else
		dropDownViewer.ModifyStyle(WS_DISABLED, NULL);
}
