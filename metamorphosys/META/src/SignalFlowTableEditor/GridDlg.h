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

#if !defined(AFX_GRIDDLG_H__CEF65935_5737_49A0_AF82_8854513FA90F__INCLUDED_)
#define AFX_GRIDDLG_H__CEF65935_5737_49A0_AF82_8854513FA90F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// GridDlg.h : header file
//

#include "resource.h"
#include "GridCtrl_src/GridCtrl.h"
#include "ComHelp.h"
#include "GMECOM.h"
#include "Mga.h"

/////////////////////////////////////////////////////////////////////////////
// CGridDlg dialog

class CGridDlg : public CDialog
{
// Construction
public:
	CGridDlg(CWnd* pParent = NULL);   // standard constructor

	CGridDlg(IMgaFCOs* selectedObjs, CWnd* pParent = NULL);


	void SetProject(IMgaProject *proj) {m_Project = proj;}

	void SetFilter(IMgaFilter *filter);

// Dialog Data
	//{{AFX_DATA(CGridDlg)
	enum { IDD = IDD_GRIDDLG };
	CButton	m_btnImport;
	CStatic	m_stcFilters;
	CStatic	m_stcSelect;
	CButton	m_btnAllTypes;
	CButton	m_btnAllKinds;
	CStatic	m_stcType;
	CStatic	m_stcKind;
	CStatic	m_stcHelp;
	CButton	m_btnDisp;
	CButton	m_btnSet;
	CButton	m_btnRef;
	CButton	m_btnModel;
	CButton	m_btnCon;
	CButton	m_btnAtom;
	CListBox	m_lstKind;
	CButton	m_btnExport;
	CButton	m_btnCANCEL;
	CButton	m_btnOK;
	BOOL	m_chkAllKinds;
	BOOL	m_chkAllTypes;
	BOOL	m_chkAtom;
	BOOL	m_chkCon;
	BOOL	m_chkModel;
	BOOL	m_chkRef;
	BOOL	m_chkSet;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CGridDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	//void Trace(LPCTSTR szFmt, ...);

// Implementation
protected:

	CGridCtrl m_Grid;
	CComPtr<IMgaProject> m_Project;
	CComPtr<IMgaFCOs> m_FCOs;
	CComPtr<IMgaFilter> m_Filter;
	CComPtr<IMgaMetaFolder> m_rootMetaFolder;

	CSize m_OldSize;

	void GetMetaObjectNames(IMgaMetaBase *metaBase);

	void BuildExtendedName(IMgaFCO *named, CString &extName);
	void BuildExtendedName(IMgaFolder *named, CString &extName);

	void MoveWndDown(CWnd *wnd, int offset);

	BOOL GetMultiLine(CComPtr<IMgaMetaAttribute> p_Meta);
	void InitGrid();
	HRESULT ShowItemsRecursively();

	// Generated message map functions
	//{{AFX_MSG(CGridDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnRecursivelyShowItems();
	afx_msg void OnButtonDisplay();
	afx_msg void OnCheckAllKinds();
	afx_msg void OnChkAllTypes();
	afx_msg void OnChkAtom();
	afx_msg void OnChkCon();
	afx_msg void OnChkModel();
	afx_msg void OnChkRef();
	afx_msg void OnChkSet();
	//}}AFX_MSG
	afx_msg void OnGridDblClick(NMHDR *pNotifyStruct, LRESULT* pResult);
    afx_msg void OnGridClick(NMHDR *pNotifyStruct, LRESULT* pResult);
    afx_msg void OnGridRClick(NMHDR *pNotifyStruct, LRESULT* pResult);
    afx_msg void OnGridStartEdit(NMHDR *pNotifyStruct, LRESULT* pResult);
    afx_msg void OnGridEndEdit(NMHDR *pNotifyStruct, LRESULT* pResult);
    afx_msg void OnGridStartSelChange(NMHDR *pNotifyStruct, LRESULT* pResult);
    afx_msg void OnGridEndSelChange(NMHDR *pNotifyStruct, LRESULT* pResult);
    afx_msg void OnGridBeginDrag(NMHDR *pNotifyStruct, LRESULT* pResult);
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_GRIDDLG_H__CEF65935_5737_49A0_AF82_8854513FA90F__INCLUDED_)
