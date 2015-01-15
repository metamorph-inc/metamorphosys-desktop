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

/*
*    Copyright (c) Vanderbilt University,  2011
*    ALL RIGHTS RESERVED
*
*/ 

//################################################################################################
//
// CPM attribute part class (decorator part)
//	CPMAttributePart.cpp
//
//################################################################################################

#include "StdAfx.h"

#include "DecoratorDefs.h"
#include "DecoratorUtil.h"

#include "CPMAttributePart.h"

#include "DecoratorLib.h"

namespace Decor {

//################################################################################################
//
// CLASS : CPMAttributePart
//
//################################################################################################

CPMInternalTextPart::CPMInternalTextPart(PartBase* pPart, CComPtr<IMgaCommonDecoratorEvents> eventSink,
										const CString& strText, CComPtr<IMgaFCO>& pFCO, bool bMultiLine,
										int iFontKey):
	TextPart(pPart, eventSink),
	m_spActualFCO(pFCO)
{
	m_strText = strText;
	m_strText.Replace("\r", NULL);
	m_strText.Replace("\n", "\r\n");
	m_bMultiLine = bMultiLine;
	m_textRelXPosition = 0;
	m_textRelYPosition = 0;
	m_iFontKey = iFontKey;

	if (m_strText.GetLength() > 35)
	{
		m_strText = m_strText.Left(30) + "...";
		m_bTextEditable = false;
	}
}

CPMInternalTextPart::~CPMInternalTextPart()
{
}

void CPMInternalTextPart::Draw(CDC* pDC, Gdiplus::Graphics* gdip)
{
	if (m_bTextEnabled) {
		CRect loc = GetLocation();
		DecoratorSDK::GdipFont* pFont = DecoratorSDK::getFacilities().GetFont(m_iFontKey);
		CSize size = GetTextSize(gdip, pFont);
		if (m_strText.GetLength())
		{
			DecoratorSDK::getFacilities().DrawString(gdip,
									   m_strText,
									   CRect(loc.left + m_textRelXPosition, loc.top + m_textRelYPosition - size.cy,
											 loc.right + m_textRelXPosition, loc.top + m_textRelYPosition),
									   pFont,
									   (m_bActive) ? m_crText : DecoratorSDK::COLOR_GREY,
									   TA_BOTTOM | TA_LEFT,
									   INT_MAX,
									   "",
									   "",
									   false);
		}
		else
		{
			DecoratorSDK::getFacilities().DrawRect(gdip, CRect(loc.left + m_textRelXPosition + 3 , loc.top + m_textRelYPosition - size.cy,
											 loc.left + m_textRelXPosition + size.cx - 3 - 1, loc.top + m_textRelYPosition - 1), DecoratorSDK::COLOR_GREY, 1);
		}
	}
	if (m_spFCO)
		resizeLogic.Draw(pDC, gdip);
}

CRect CPMInternalTextPart::GetTextLocation(CDC* pDC, Gdiplus::Graphics* gdip) const
{
	CRect loc = GetLocation();

	DecoratorSDK::GdipFont* pFont = DecoratorSDK::getFacilities().GetFont(m_iFontKey);
	auto cSize = GetTextSize(gdip, pFont);

	return CRect(loc.left + m_textRelXPosition,
				 loc.top + m_textRelYPosition - cSize.cy,
				 loc.left + m_textRelXPosition + cSize.cx,
				 loc.top + m_textRelYPosition);
}

CPoint CPMInternalTextPart::GetTextPosition(CDC* pDC, Gdiplus::Graphics* gdip) const
{
	return GetTextLocation(pDC, gdip).TopLeft();
}

CSize CPMInternalTextPart::GetRelSize(CDC* pDC, Gdiplus::Graphics* gdip) const
{
	DecoratorSDK::GdipFont* pFont = DecoratorSDK::getFacilities().GetFont(m_iFontKey);
	return GetTextSize(gdip, pFont);
}

void CPMInternalTextPart::InitializeEx(CComPtr<IMgaProject>& pProject, CComPtr<IMgaMetaPart>& pPart,
									   CComPtr<IMgaFCO>& pFCO, HWND parentWnd, DecoratorSDK::PreferenceMap& preferences) 
{
	//int iFontKey = m_iFontKey;
	DecoratorSDK::TextPart::InitializeEx(pProject, pPart, pFCO, parentWnd, preferences);
	//m_iFontKey = iFontKey;
}

}; // namespace Decor
