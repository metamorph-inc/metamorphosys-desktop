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

///////////////////////////////////////////////////////////////////////////
//
// GridCellDateTime.cpp: implementation of the CGridCellDateTime class.
//
// Provides the implementation for a datetime picker cell type of the
// grid control.
//
// Written by Podsypalnikov Eugen 15 Mar 2001
// Modified:
//    31 May 2001  Fixed m_cTime bug (Chris Maunder)
//
// For use with CGridCtrl v2.22+
//
///////////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "../GridCtrl_src/GridCtrl.h"
#include "../GridCtrl_src/GridCell.h"
#include "GridCellDouble.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// CGridCellDateTime

IMPLEMENT_DYNCREATE(CGridCellDouble, CGridCell)

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CGridCellDouble::CGridCellDouble() : CGridCell()
{
	m_dwStyle = 0;
}


CGridCellDouble::~CGridCellDouble()
{
}

BOOL CGridCellDouble::Edit(int nRow, int nCol, CRect rect, CPoint /* point */, UINT nID, UINT nChar)
{
    if ( m_bEditing )
	{      
        if (m_pEditWnd)
		    m_pEditWnd->SendMessage ( WM_CHAR, nChar );    
    }  
	else  
	{   
		DWORD dwStyle = ES_LEFT;
		if (GetFormat() & DT_RIGHT) 
			dwStyle = ES_RIGHT;
		else if (GetFormat() & DT_CENTER) 
			dwStyle = ES_CENTER;

		m_bEditing = TRUE;
		
		// InPlaceEdit auto-deletes itself
		CGridCtrl* pGrid = GetGrid();
		m_pEditWnd = new CInPlaceDoubleEdit(pGrid, rect, dwStyle, nID, nRow, nCol, GetText(), nChar);
    }
    return TRUE;
}


/////////////////////////////////////////////////////////////////////////////
// CInPlaceDoubleEdit


CInPlaceDoubleEdit::CInPlaceDoubleEdit(CWnd* pParent, CRect& rect, DWORD dwStyle, UINT nID,
                           int nRow, int nColumn, CString sInitText, 
                           UINT nFirstChar):CInPlaceEdit(pParent, rect, dwStyle, nID,
                           nRow, nColumn, sInitText, 
                           nFirstChar)
{
 //do nothing, just pass to the default constructor for CInPlaceEdit
	
}


BEGIN_MESSAGE_MAP(CInPlaceDoubleEdit, CInPlaceEdit)
	//{{AFX_MSG_MAP(CInPlaceDateTime)
    ON_WM_CHAR()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()




void CInPlaceDoubleEdit::OnChar(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	//AfxMessageBox(CString(nChar));

	if (nChar == VK_TAB || nChar == VK_RETURN || nChar == VK_ESCAPE || 
		nChar == 127 || nChar == VK_BACK) //VK_DELETE is 46, but that's '.', 127 is the ascii for Delete
	{
		CInPlaceEdit::OnChar(nChar, nRepCnt, nFlags);
		return;
	}
	
	
	if ((nChar >= '0') && (nChar <= '9')) 
	{
		CInPlaceEdit::OnChar(nChar, nRepCnt, nFlags);
		return;
	}


	CString currentText;
	this->GetWindowText(currentText.GetBuffer(100),100);
	
	if (nChar == '.') //can only allow one '.'
	{
		if (currentText.FindOneOf(".") != -1)
		{
			//AfxMessageBox("has '.' ");
			return;
		}
		else
		{
			//AfxMessageBox("no '.'");
			CInPlaceEdit::OnChar(nChar, nRepCnt, nFlags);
			return;
		}
	}

	if (nChar == '-') //can only allow one '-'
	{
		if (currentText.FindOneOf("-") != -1)
		{
			//AfxMessageBox("has '-' ");
			return;
		}
		else
		{
			//AfxMessageBox("no '-'");
			this->SetSel(0,0); //'-' can only appear at the beginning of a double
			CInPlaceEdit::OnChar(nChar, nRepCnt, nFlags);
			return;
		}
	}
	
}



