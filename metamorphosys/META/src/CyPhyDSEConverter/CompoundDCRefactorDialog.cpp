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

// CompoundDCRefactorDialog.cpp : implementation file
//

#include "stdafx.h"
#include "CompoundDCRefactorDialog.h"
#include "afxdialogex.h"


// CCompoundDCRefactorDialog dialog

IMPLEMENT_DYNAMIC(CCompoundDCRefactorDialog, CDialogEx)

CCompoundDCRefactorDialog::CCompoundDCRefactorDialog(CWnd* pParent /*=NULL*/)
	: _disableCA(false), CDialogEx(CCompoundDCRefactorDialog::IDD, pParent)
{

}

CCompoundDCRefactorDialog::CCompoundDCRefactorDialog(bool disableCA, CWnd* pParent /*=NULL*/)
	: _disableCA(disableCA), CDialogEx(CCompoundDCRefactorDialog::IDD, pParent)
{

}

CCompoundDCRefactorDialog::~CCompoundDCRefactorDialog()
{
}

void CCompoundDCRefactorDialog::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CCompoundDCRefactorDialog, CDialogEx)
	ON_BN_CLICKED(IDC_CA_RADIO, &CCompoundDCRefactorDialog::OnBnClickedCaRadio)
	ON_BN_CLICKED(IDC_DCRADIO, &CCompoundDCRefactorDialog::OnBnClickedDcradio)
	ON_BN_CLICKED(IDC_EXRADIO, &CCompoundDCRefactorDialog::OnBnClickedExradio)
END_MESSAGE_MAP()


// CCompoundDCRefactorDialog message handlers


void CCompoundDCRefactorDialog::OnBnClickedCaRadio()
{
	// TODO: Add your control notification handler code here
	rt = genCA;
}


void CCompoundDCRefactorDialog::OnBnClickedDcradio()
{
	// TODO: Add your control notification handler code here
	rt = genDC;
}


void CCompoundDCRefactorDialog::OnBnClickedExradio()
{
	// TODO: Add your control notification handler code here
	rt = extract;
}


BOOL CCompoundDCRefactorDialog::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// TODO:  Add extra initialization here
	if(_disableCA)
		GetDlgItem(IDC_CA_RADIO)->EnableWindow(FALSE); 
	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}
