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

// CGProgressDialog.cpp : implementation file
//

#include "stdafx.h"
#include "CGProgressDialog.h"
#include "afxdialogex.h"


// CCGProgressDialog dialog

IMPLEMENT_DYNAMIC(CCGProgressDialog, CDialog)

CCGProgressDialog::CCGProgressDialog(CWnd* pParent /*=NULL*/)
	: CDialog(CCGProgressDialog::IDD, pParent),
	  processPos(0)
{
	m_maxPrg  = 100;
	m_cancel = false;
}

CCGProgressDialog::~CCGProgressDialog()
{
}

void CCGProgressDialog::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_PROGRESS1, m_progress);
	DDX_Control(pDX, IDC_STATUS_EDIT, m_status);
}


BEGIN_MESSAGE_MAP(CCGProgressDialog, CDialog)

END_MESSAGE_MAP()


// CCGProgressDialog message handlers
void CCGProgressDialog::PostNcDestroy() 
{
	CDialog::PostNcDestroy();
	delete this; 
}

void CCGProgressDialog::OnCancel()
{
	// TODO: Add your specialized code here and/or call the base class
	m_cancel = true;
	CDialog::OnCancel();
}

void CCGProgressDialog::SetProgress(const CString &status)
{
	if( processPos > int(m_maxPrg)) processPos = 0;
	m_progress.SetRange( 0, m_maxPrg);
	m_progress.SetPos(processPos);

	processPos += 10;

	m_status.SetWindowText(status);

	this->UpdateData();
	this->UpdateWindow();
	this->RedrawWindow();
	this->ShowWindow(SW_SHOW);
}

void CCGProgressDialog::OnFinished()
{
	m_progress.SetRange( 0, (short) m_maxPrg);
	while(processPos < int(m_maxPrg))
	{
		Sleep(20);
		m_progress.StepIt();
		processPos += 5;
	}
	CDialog::OnCancel();
}

void CCGProgressDialog::SetRange(int range)
{
	if(range > int(m_maxPrg))
		m_maxPrg = range;
};

CCGProgressDialog * GetCGProgressDlg(CCGProgressDialog * set_dlg)
{
	static CCGProgressDialog * progressdlg;
	
	if (set_dlg) progressdlg = set_dlg;
	ASSERT(progressdlg != NULL);
	return progressdlg;
};
