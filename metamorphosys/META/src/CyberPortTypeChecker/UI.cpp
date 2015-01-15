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

// UI.cpp : implementation file
//

#include "stdafx.h"
#include "afxdialogex.h"
#include "UI.h"
#include "MyColors.h"
#include <safeint.h>

// UI dialog

IMPLEMENT_DYNAMIC(UI, CDialog)

UI::UI(CWnd* pParent /*=NULL*/)
	: CDialog(UI::IDD, pParent)
{

}

UI::~UI()
{
}

void UI::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDOK, m_btnOK);
}


BEGIN_MESSAGE_MAP(UI, CDialog)
	ON_WM_CTLCOLOR()
	ON_MESSAGE(DM_GETDEFID, OnGetDefID)
END_MESSAGE_MAP()


// UI message handlers


BOOL UI::OnInitDialog()
{
	CDialog::OnInitDialog();

	this->ShowWindow(SW_MAXIMIZE);

	// TODO:  Add extra initialization here
	CRect	dlgRect;
	CString capText;
	// Get the maximized dlg windows area
	GetClientRect(&dlgRect);

	// Set footer, header
	this->HEADER_END = dlgRect.top + 0.05 * dlgRect.Height();
	this->FOOTER_START = dlgRect.bottom - 0.2 * dlgRect.Height();

	//Move the OK button in the maximized window
	CRect btnRect(this->MARGIN_SIZE + 0.95 * dlgRect.Width(), this->FOOTER_START + this->MARGIN_SIZE, dlgRect.right - this->MARGIN_SIZE, dlgRect.bottom - this->MARGIN_SIZE);
	m_btnOK.MoveWindow(&btnRect);
	
	// Create output Console Caption
	capText.LoadString(CONSOLE_CAP_TEXT);
	// Create output console 
	CRect outConsoleRect(dlgRect.left + MARGIN_SIZE, FOOTER_START + MARGIN_SIZE, btnRect.left - MARGIN_SIZE, dlgRect.bottom - MARGIN_SIZE);
	if(!outputConsole.initialize(capText, OUTPUT_CONSOLE_CAPTION, outConsoleRect, this, LB_CONSOLE)) 
	{	
		this->EndDialog(IDOK);
		return TRUE;
	}

	// Create the Matrix Grid view
	CRect gridRect(dlgRect.left + MARGIN_SIZE, HEADER_END + MARGIN_SIZE, dlgRect.right - MARGIN_SIZE, HEADER_END + (FOOTER_START - HEADER_END) * 0.7);
	if(!matrixView.Create(gridRect, this, MATRIX_GRID_VIEW))
	{
		AfxMessageBox(_T("Failed to Create Grid Matrix View! Closing."));
		this->EndDialog(IDOK);
		return TRUE;
	}

	// Create the property viewer
	CRect propViewRect(dlgRect.left + MARGIN_SIZE, gridRect.bottom + MARGIN_SIZE , dlgRect.right - MARGIN_SIZE, FOOTER_START - MARGIN_SIZE);
	if(!propViewer.initialize(this, propViewRect))
	{	
		this->EndDialog(IDOK);
		return TRUE;
	}

	// Attach relevant observers to relevant objects
	matrixView.attachObserver(&propViewer);		// to show the clicked/selected matrix cell's info in the prop editor
	modelHandler.attachObserver(&matrixView);	// to fill in the port info in the grid
	modelHandler.attachObserver(&propViewer);	// to insert new port data type for the drop down lists in editor bars

	// if the modelHandler is able to traverse the info about the comp successfully then display info in the grid
	if(modelHandler.generateCompReport())
		matrixView.showMatrix();
	else
	{
		AfxMessageBox(_T("Error occurred while processing the selected GME Component!"));
		this->EndDialog(IDOK);
		return TRUE;
	}

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

// for CListBox
void UI::writeConsole(CString str, UINT itemType)
{
	INT				errCode;						// err code for CListBox::AddString
	CString			outStr = _T("");				// final concatenated string
	CString			errPrefix;						// Error Statement Prefix
	CString			norPrefix;
	CString			warPrefix;
	CString			newDate = CTime::GetCurrentTime().Format("%#x");
	CString			curTime = CTime::GetCurrentTime().Format("%I:%M:%S  ");

	// if date has changed then print it
	if(curDate.Compare(newDate))
	{
		outputConsole.listBox.AddString(curTime + "------------- " + newDate + " -------------");
		curDate = newDate;
	}

	// Load prefixes from the String table
	errPrefix.LoadString(LOG_ERR_PREF);
	norPrefix.LoadString(LOG_NOR_PREF);
	warPrefix.LoadString(LOG_WAR_PREF);

	// Attach error code to the log statement
	if(itemType == UIConsole::ERROR_ITEM_CONSOLE) outStr += curTime + errPrefix + str;
	else if(itemType == UIConsole::NORMAL_ITEM_CONSOLE) outStr += curTime + norPrefix + str;
	else if(itemType == UIConsole::WARNING_ITEM_CONSOLE) outStr += curTime + warPrefix + str;
	
	if(this->outputConsole.listBox.GetSafeHwnd() == NULL)
	{
		AfxMessageBox(_T("Could not print the message: \"") + outStr + _T("\" on the Output Console!"));
		return;
	}

	// Add string to the console
	errCode = outputConsole.listBox.AddString(outStr);

	// Handle error from above call
	if(errCode == LB_ERR || errCode == LB_ERRSPACE)
		AfxMessageBox(_T("Could not print the message: \"") + outStr + _T("\" on the Output Console!"));
}

void UI::printErrorLog(CString str)
{
	this->writeConsole(str, UIConsole::ERROR_ITEM_CONSOLE);
}

void UI::printNormalLog(CString str)
{
	this->writeConsole(str, UIConsole::NORMAL_ITEM_CONSOLE);
}

void UI::printWarningLog(CString str)
{
	this->writeConsole(str, UIConsole::WARNING_ITEM_CONSOLE);
}

vector<CString> UI::getCompleteLog()
{
	vector<CString> strArray;
	for(INT i=0; i < this->outputConsole.listBox.GetCount(); ++i)
	{
		CString itemText = _T("");
		this->outputConsole.listBox.GetText(i, itemText);
		strArray.push_back(itemText);
	}

	return strArray;
}


HBRUSH UI::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	HBRUSH hbr = CDialog::OnCtlColor(pDC, pWnd, nCtlColor);

	// TODO:  Change any attributes of the DC here
	if(nCtlColor == CTLCOLOR_STATIC)
	{
		pDC->SetTextColor(MY_BLACK);
		pDC->SetBkColor(MY_GRAY);

		return CreateSolidBrush(MY_GRAY);
	}
	// TODO:  Return a different brush if the default is not desired
	return hbr;
}

// Refer to http://msdn.microsoft.com/en-us/magazine/cc301409.aspx
LRESULT UI::OnGetDefID(WPARAM wp, LPARAM lp) 
{
    return MAKELONG(0,DC_HASDEFID); 
} 
 
void UI::attachComponent(ModelicaComponent component)
{
	// Attch UI to modelhandler for it to be able to print on the UIConsole
	this->modelHandler.attachUI(this);

	this->modelHandler.attachComponent(component);
}


BOOL UI::DestroyWindow()
{
	// TODO: Add your specialized code here and/or call the base class
	CString toolPrefix;
	toolPrefix.LoadString(LOG_TOOL_PREF);

	Console::Info::writeLine(string(toolPrefix + _T("Last run's console log:")));
	vector<CString> msgArray = this->getCompleteLog();
	for each(CString msg in msgArray)
		Console::Out::writeLine(string(msg));
	Console::Info::writeLine(string(toolPrefix + _T("Completed Execution.")));

	return CDialog::DestroyWindow();
}
