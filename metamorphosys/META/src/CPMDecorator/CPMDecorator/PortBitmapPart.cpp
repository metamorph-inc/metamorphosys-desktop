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

//################################################################################################
//
// Port bitmap part decorator class (decorator part)
//	PortBitmapPart.cpp
//
//################################################################################################

#include "StdAfx.h"
#include "PortBitmapPart.h"
#include "Resource.h"


namespace Decor {

//################################################################################################
//
// CLASS : PortBitmapPart
//
//################################################################################################

PortBitmapPart::PortBitmapPart(PartBase* pPart, CComPtr<IMgaCommonDecoratorEvents>& eventSink, const CPoint& ptInner, _bstr_t icon):
	TypeableBitmapPart(pPart, eventSink),
	m_ptInner(ptInner),
	m_icon(icon)
{
}

PortBitmapPart::~PortBitmapPart()
{
	
}

CSize PortBitmapPart::GetPreferredSize(void) const
{
	CSize size = ResizablePart::GetPreferredSize();
	if (size.cx * size.cy != 0)
		return size;

	return CSize(WIDTH_PORT, HEIGHT_PORT);
}

// New functions
void PortBitmapPart::InitializeEx(CComPtr<IMgaProject>& pProject, CComPtr<IMgaMetaPart>& pPart, CComPtr<IMgaFCO>& pFCO,
									HWND parentWnd, PreferenceMap& preferences)
{
	CString strIcon;
	if (getFacilities().getPreference(pFCO, PREF_PORTICON, strIcon)) {
		if (!strIcon.IsEmpty())
		{
			preferences[PREF_ICON] = PreferenceVariant(strIcon);
			preferences[PREF_PORTICON] = PreferenceVariant(strIcon);
		}
		preferences[PREF_TILES] = PreferenceVariant(getFacilities().getTileVector(TILE_PORTDEFAULT));
	}
	
	//new code
	if (m_icon.length() != 0)
	{
		preferences[PREF_ICON] = PreferenceVariant(CString(static_cast<const TCHAR*>(m_icon)));
		preferences[PREF_PORTICON] = PreferenceVariant(CString(static_cast<const TCHAR*>(m_icon)));
	}

	COMTHROW(pFCO->get_ObjType(&m_eType));
	switch (m_eType) {
		case OBJTYPE_MODEL :
			strIcon = createResString(IDB_MODELPORT);
			break;
		case OBJTYPE_SET :
			strIcon = createResString(IDB_SETPORT);
			break;
		case OBJTYPE_REFERENCE :
			strIcon = createResString(IDB_REFERENCEPORT);
			break;
		default :
			strIcon = createResString(IDB_ATOMPORT);
			break;
	}
	preferences[PREF_ICONDEFAULT]	= PreferenceVariant(strIcon);
	preferences[PREF_TILESDEFAULT]	= PreferenceVariant(getFacilities().getTileVector(TILE_PORTDEFAULT));
	preferences[PREF_TILESUNDEF]	= PreferenceVariant(getFacilities().getTileVector(TILE_PORTDEFAULT));
	preferences[PREF_ITEMSHADOWCAST]= PreferenceVariant(false);

	TypeableBitmapPart::InitializeEx(pProject, pPart, pFCO, parentWnd, preferences);
}

void PortBitmapPart::DrawBackground(CDC* pDC, Gdiplus::Graphics* gdip)
{
	CRect cRect = GetBoxLocation(false);
	if (m_bActive) {
		m_pBitmap->draw(gdip, pDC, cRect, *m_pTileVector);
	} else {
		cRect.right--;
		cRect.bottom--;
		getFacilities().DrawRect(gdip, cRect, COLOR_GRAY, 1);
	}
}

}; // namespace Decor
