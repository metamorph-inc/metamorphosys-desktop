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

// CyPhy2MorphMatrixFileDlg.cpp : implementation file
//

#include "stdafx.h"
#include "CyPhy2MorphMatrixFileDlg.h"
#include "afxdialogex.h"


// CCyPhy2MorphMatrixFileDlg dialog

IMPLEMENT_DYNAMIC(CCyPhy2MorphMatrixFileDlg, CDialog)

CCyPhy2MorphMatrixFileDlg::CCyPhy2MorphMatrixFileDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CCyPhy2MorphMatrixFileDlg::IDD, pParent)
{

}

CCyPhy2MorphMatrixFileDlg::~CCyPhy2MorphMatrixFileDlg()
{
}

void CCyPhy2MorphMatrixFileDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CCyPhy2MorphMatrixFileDlg, CDialog)
	ON_BN_CLICKED(IDC_OUTPUTBTN, &CCyPhy2MorphMatrixFileDlg::OnBnClickedOutputbtn)
	ON_EN_CHANGE(IDC_FILEPATHEDIT, &CCyPhy2MorphMatrixFileDlg::OnEnChangeFilepathedit)
END_MESSAGE_MAP()


// CCyPhy2MorphMatrixFileDlg message handlers


void CCyPhy2MorphMatrixFileDlg::OnBnClickedOutputbtn()
{
	// TODO: Add your control notification handler code here
	CString m_strFile = m_filepath;
	CFileDialog fileDlg( TRUE, NULL, NULL, OFN_FILEMUSTEXIST, "Excel Files (*.xlsm)|*.xlsm|All Files (*.*)|*.*||", this);
	// Call DoModal
	if ( fileDlg.DoModal() == IDOK)
	{
		m_filepath = fileDlg.GetPathName(); // This is your selected file name with path
		GetDlgItem(IDC_FILEPATHEDIT_C2M)->SetWindowText(m_filepath);
	}
}


void CCyPhy2MorphMatrixFileDlg::OnEnChangeFilepathedit()
{
	// TODO:  If this is a RICHEDIT control, the control will not
	// send this notification unless you override the CDialog::OnInitDialog()
	// function and call CRichEditCtrl().SetEventMask()
	// with the ENM_CHANGE flag ORed into the mask.

	// TODO:  Add your control notification handler code here
	GetDlgItem(IDC_FILEPATHEDIT_C2M)->GetWindowText(m_filepath);
}
