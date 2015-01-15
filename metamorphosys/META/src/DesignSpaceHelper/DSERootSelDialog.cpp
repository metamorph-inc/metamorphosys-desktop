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

// DSERootSelDialog.cpp : implementation file
//

#include "stdafx.h"
#include "DSERootSelDialog.h"
#include "afxdialogex.h"
#include "Uml.h"
#include "UmlExt.h"
#include "UdmUtil.h"
// CDSERootSelDialog dialog

IMPLEMENT_DYNAMIC(CDSERootSelDialog, CDialog)

CDSERootSelDialog::CDSERootSelDialog(CWnd* pParent /*=NULL*/)
	: CDialog(CDSERootSelDialog::IDD, pParent),silent_mode(false)
{

}

CDSERootSelDialog::CDSERootSelDialog(CyPhyML::RootFolder &rf, bool runSilent, CWnd* pParent /*=NULL*/)
	: CDialog(CDSERootSelDialog::IDD, pParent), cyphy_rf(rf), silent_mode(runSilent)
{

}

CDSERootSelDialog::~CDSERootSelDialog()
{
}

void CDSERootSelDialog::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_DSTREE, m_dstree);
}


BEGIN_MESSAGE_MAP(CDSERootSelDialog, CDialog)
	ON_NOTIFY(TVN_SELCHANGED, IDC_DSTREE, &CDSERootSelDialog::OnTvnSelchangedDstree)
END_MESSAGE_MAP()


BOOL CDSERootSelDialog::OnInitDialog()
{
	CDialog::OnInitDialog();

	// TODO:  Add extra initialization here
	m_dstree.DeleteAllItems();
	set<CyPhyML::DesignSpace> dsfdrs = cyphy_rf.DesignSpace_kind_children();
	for(set<CyPhyML::DesignSpace>::iterator i=dsfdrs.begin();i!=dsfdrs.end();++i)
	{
		CyPhyML::DesignSpace dsfdr = *i;
		FillDSETree(dsfdr, NULL);
	}
	if(!silent_mode)
	{
		CWnd* ok = GetDlgItem( IDOK );
		ok->EnableWindow(FALSE);
	}
	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CDSERootSelDialog::FillDSETree(CyPhyML::DesignSpace &cyphy_ds, HTREEITEM parent)
{
	std::string iname = (std::string)cyphy_ds.name()+" [Folder]";

	HTREEITEM dse_node = m_dstree.InsertItem((LPSTR)iname.c_str(), parent, TVI_LAST);
	//m_dstree.SetItemState( dse_node, TVIS_BOLD, TVIS_BOLD ); //bold
	dseTreeMap[dse_node] = cyphy_ds;

	set<CyPhyML::DesignSpace> c_dsfdrs = cyphy_ds.DesignSpace_kind_children();
	for(set<CyPhyML::DesignSpace>::iterator ci=c_dsfdrs.begin();ci!=c_dsfdrs.end();++ci)
	{		
		FillDSETree((CyPhyML::DesignSpace)*ci, dse_node);
	}
	set<CyPhyML::DesignContainer> c_dscons = cyphy_ds.DesignContainer_kind_children();
	for(set<CyPhyML::DesignContainer>::iterator con_it=c_dscons.begin();con_it!=c_dscons.end();++con_it)
	{
		FillDSETree((CyPhyML::DesignContainer)*con_it, dse_node);
		root_dcs.insert((CyPhyML::DesignContainer)*con_it);
	}
//	m_dstree.SortChildren(dse_node);
	m_dstree.Expand(dse_node,TVE_EXPAND);
}

void CDSERootSelDialog::FillDSETree(CyPhyML::DesignContainer &cyphy_con, HTREEITEM parent)
{
	std::string iname = (std::string)cyphy_con.name() + " [" + (std::string)cyphy_con.ContainerType()+"]";
	HTREEITEM dse_node = m_dstree.InsertItem((LPSTR)iname.c_str(), parent, TVI_LAST);
	dseTreeMap[dse_node] = cyphy_con;

	set<CyPhyML::DesignEntity> entities = cyphy_con.DesignEntity_kind_children();
	for(set<CyPhyML::DesignEntity>::iterator i=entities.begin();i!=entities.end();++i)
	{
		CyPhyML::DesignEntity entity = *i;
		if(!Uml::IsDerivedFrom(entity.type(), CyPhyML::DesignContainer::meta))
			continue;
		FillDSETree((CyPhyML::DesignContainer::Cast(entity)), dse_node);
	}
	m_dstree.SortChildren(dse_node);
	m_dstree.Expand(dse_node,TVE_EXPAND);
}

// CDSERootSelDialog message handlers
void CDSERootSelDialog::OnTvnSelchangedDstree(NMHDR *pNMHDR, LRESULT *pResult)
{
	LPNMTREEVIEW pNMTreeView = reinterpret_cast<LPNMTREEVIEW>(pNMHDR);
	// TODO: Add your control notification handler code here
	
	HTREEITEM hItem =m_dstree.GetSelectedItem(); 
	dseroot_sel = dseTreeMap[hItem];
	CWnd* ok = GetDlgItem( IDOK );
	if(!silent_mode && dseroot_sel.type()!=CyPhyML::DesignContainer::meta)
		ok->EnableWindow(FALSE);
	else
		ok->EnableWindow(TRUE);

	*pResult = 0;
}

Udm::Object CDSERootSelDialog::GetSelectedDSERoot()
{
	return dseroot_sel;
}

set<CyPhyML::DesignContainer> CDSERootSelDialog::GetAllRootDesignContainers()
{
	return root_dcs;
}