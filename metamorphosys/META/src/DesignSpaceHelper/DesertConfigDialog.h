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

#pragma once

#include "resource.h"
#include "afxcmn.h"
#include "DesertHelper.h"
#include "afxwin.h"
#include "CheckHeadCtrl.h"
// CDesertConfigDialog dialog

#include "WndResizer.h"
#include "SortListCtrl.h"

class CDesertConfigDialog : public CDialog
{
	DECLARE_DYNAMIC(CDesertConfigDialog)

public:
	CDesertConfigDialog(CWnd* pParent = NULL);   // standard constructor
	CDesertConfigDialog(DesertHelper *deserthelper_ptr, CWnd* pParent = NULL);
	virtual ~CDesertConfigDialog();

// Dialog Data
	enum { IDD = IDD_CONFIG_DIALOG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	virtual BOOL OnInitDialog();
private:
	CSortListCtrl m_cfglist;
	CTreeCtrl m_cfgtree;
	DesertHelper *dhelper_ptr;
	bool useCurrentCyphy;
	map<HTREEITEM, int> cfgTreeMap;
	bool isActiveCfg;
	long cfgSize;

	CWndResizer m_resizer;
public:
	afx_msg void OnBnClickedExportselbtn();
	afx_msg void OnBnClickedExportallbtn();
protected:
	void FillCfgList();
	void FillCfgTree();
	void FillCfgTree(DesertIface::Element &elem, HTREEITEM parent);

public:
	afx_msg void OnBnClickedCancel();
	afx_msg void OnLvnItemchangedCfglist(NMHDR *pNMHDR, LRESULT *pResult);

protected:
	struct Color_Font
	{
		COLORREF color;
		LOGFONT  logfont;
	};
	CMap< void*, void*, Color_Font, Color_Font& > m_mapColorFont ;
public:
	CEdit m_cfgsize;
	afx_msg void OnBnClickedClearallbtn();
	afx_msg void OnNMCustomdrawConstraintlist(NMHDR *pNMHDR, LRESULT *pResult);
private:
	std::string cfgSizeInfo;
	BOOL	m_blInited;
	CImageList	m_checkImgList;
//	CCheckHeadCtrl	m_checkHeadCtrl;
	HTREEITEM m_cfgTreeRootItem;
//	BOOL initHeadListCtr();
	void updateSize(int checkedSize);
	void updateConfigList(HTREEITEM &item, set<int> &cfgIds, int &cnt, bool isAlt=false);
	set<int> computeConfigList(HTREEITEM &item, set<int> cfgIds);
	void updateConfigList(set<int> &cfgIds, bool check=true);
//	void updateConfigList_1();
	void checkItem(HTREEITEM &item);
	void checkSiblings_check(HTREEITEM &item);
	void uncheckItem(HTREEITEM &item);
	bool noneChecked;

public:
	afx_msg void OnNMClickCfgtree(NMHDR *pNMHDR, LRESULT *pResult);
	LRESULT OnTreeViewCheckStateChange(WPARAM wParam, LPARAM lParam);
	afx_msg void OnBnClickedCyPhy2MorphMatrix();
	afx_msg void OnBnClickedMorphMatrix2CyPhy();
	afx_msg void OnNMDblclkCfglist(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnLvnBeginlabeleditCfglist(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnLvnEndlabeleditCfglist(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnNMClkCfgList(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnBnClickedCloseallbtn();
};
