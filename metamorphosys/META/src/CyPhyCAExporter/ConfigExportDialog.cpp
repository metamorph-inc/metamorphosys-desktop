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

// ConfigExportDialog.cpp : implementation file
//

#include "stdafx.h"
#include "CyPhyCAExporter.h"
#include "ConfigExportDialog.h"
#include "afxdialogex.h"


// CConfigExportDialog dialog

IMPLEMENT_DYNAMIC(CConfigExportDialog, CDialog)

CConfigExportDialog::CConfigExportDialog(CWnd* pParent /*=NULL*/)
	: CDialog(CConfigExportDialog::IDD, pParent)
	, m_outputfdr(_T(""))
{

}

CConfigExportDialog::~CConfigExportDialog()
{
}

void CConfigExportDialog::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Text(pDX, IDC_OUTPUTEDIT, m_outputfdr);
}


BEGIN_MESSAGE_MAP(CConfigExportDialog, CDialog)
	ON_BN_CLICKED(IDC_CURRENTRADIO, &CConfigExportDialog::OnBnClickedCurrentradio)
	ON_BN_CLICKED(IDC_NEWRADIO, &CConfigExportDialog::OnBnClickedNewradio)
	ON_BN_CLICKED(IDC_OUTPUTBTN, &CConfigExportDialog::OnBnClickedOutputbtn)
	ON_EN_CHANGE(IDC_OUTPUTEDIT, &CConfigExportDialog::OnEnChangeOutputedit)
	ON_BN_CLICKED(IDC_HIERARCHICAL, &CConfigExportDialog::OnBnClickedHierarchical)
	ON_BN_CLICKED(IDC_FLATTEN, &CConfigExportDialog::OnBnClickedFlatten)
END_MESSAGE_MAP()


// CConfigExportDialog message handlers


void CConfigExportDialog::OnBnClickedCurrentradio()
{
	// TODO: Add your control notification handler code here
	m_useCurrent = true;
}


void CConfigExportDialog::OnBnClickedNewradio()
{
	// TODO: Add your control notification handler code here
	m_useCurrent = false;
}

int CALLBACK SetSelProc (HWND hWnd,
                         UINT uMsg,
                         LPARAM lParam,
                         LPARAM lpData)
{
   if (uMsg==BFFM_INITIALIZED)
   {
      ::SendMessage(hWnd, BFFM_SETSELECTION, TRUE, lpData );
   }
   return 0;
}

void CConfigExportDialog::OnBnClickedOutputbtn()
{
	CString m_strFile = m_outputfdr;
	CFolderPickerDialog dlg(m_strFile, 0, NULL, 0); 
	if (dlg.DoModal()==IDOK) 
	{
		m_outputfdr = dlg.GetFolderPath();	
		UpdateData(FALSE);
	} 
}


void CConfigExportDialog::OnEnChangeOutputedit()
{
	// TODO:  If this is a RICHEDIT control, the control will not
	// send this notification unless you override the CDialog::OnInitDialog()
	// function and call CRichEditCtrl().SetEventMask()
	// with the ENM_CHANGE flag ORed into the mask.

	// TODO:  Add your control notification handler code here
	GetDlgItem(IDC_OUTPUTEDIT)->GetWindowText(m_outputfdr);
}


BOOL CConfigExportDialog::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here
	CButton *pRbtn1 = (CButton*)this->GetDlgItem(IDC_CURRENTRADIO);
	pRbtn1->SetFocus();      
    pRbtn1->SetCheck(true); 
	m_useCurrent = true;
	CButton *pRbtn2 = (CButton*)this->GetDlgItem(IDC_HIERARCHICAL);
	pRbtn2->SetFocus();      
    pRbtn2->SetCheck(true); 
	m_flatten = false;

	TCHAR szDirectory[MAX_PATH] = "";
	::GetCurrentDirectory(sizeof(szDirectory) - 1, szDirectory);
	m_outputfdr = (CString)szDirectory;
	this->GetDlgItem(IDC_OUTPUTEDIT)->SetWindowText(szDirectory);
	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}


void CConfigExportDialog::OnBnClickedHierarchical()
{
	// TODO: Add your control notification handler code here
	m_flatten = false;
}


void CConfigExportDialog::OnBnClickedFlatten()
{
	// TODO: Add your control notification handler code here
	m_flatten = true;
}
