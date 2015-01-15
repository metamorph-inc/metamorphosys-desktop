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

/*
*    Copyright (c) Vanderbilt University,  2011
*    ALL RIGHTS RESERVED
*
*/ 



#ifndef __CPMDECORATORSTYLE_H_
#define __CPMDECORATORSTYLE_H_

#include "StdAfx.h"
#include "PartInterface.h"
#include "Decorator.h"
#include "CPMBasePart.h"

namespace Decor {

	class CPMDecoratorStyle
	{
		CComPtr<IMgaCommonDecoratorEvents> m_eventSink;
		CComPtr<IMgaFCO> m_pFCO;
		CPMBasePart* m_pBasePart;
		Type::cpm_type m_cpmType;
		DecoratorSDK::PreferenceMap m_preferences;
		HWND m_parentWnd;
		CComPtr<IMgaMetaPart> m_pPart;
		CComPtr<IMgaProject> m_pProject;

		CString GetAttribute(const CString& attrName) const;
		CPMNamePart* NewCPMNamePart(int iFontKey = FONT_LABEL,  COLORREF cColorKey = COLOR_BLACK);
		void AddCPMUnitsPart(int iFontKey = FONT_LABEL,  COLORREF cColorKey = COLOR_BLACK);
		CPMAttributePart* NewAttributePart(const CString& attrName, bool multiline = false, int iFontKey = FONT_LABEL,  COLORREF cColorKey = COLOR_BLACK);
		CPMIntegerAttributePart* NewIntegerAttributePart(const CString& attrName, int iFontKey = FONT_LABEL,  COLORREF cColorKey = COLOR_BLACK);
		void AddInternalPart(CPMInternalTextPart* attribute);
		void AddAttributePart(const CString& attrName, bool multiline = false, int iFontKey = CPM_FONT_BOLD_INDEX,  COLORREF cColorKey = COLOR_BLUE);
		void AddLabel(const CString& label, int iFontKey = FONT_LABEL,  COLORREF cColorKey = COLOR_BLACK);
		void AddSpacer(long x, long y = 0);
		void RenderParameter();
		void RenderValueUnit();
		
	public:
		CPMDecoratorStyle(CComPtr<IMgaCommonDecoratorEvents> eventSink, CComPtr<IMgaFCO> pFCO,
			CPMBasePart* pBasePart, Type::cpm_type cpmType, HWND parentWnd, CComPtr<IMgaMetaPart>& pPart,
			CComPtr<IMgaProject>& pProject) :
		m_eventSink(eventSink), m_pFCO(pFCO), m_pBasePart(pBasePart), m_cpmType(cpmType),
		m_parentWnd(parentWnd), m_pPart(pPart), m_pProject(pProject) { }


		void AddInternalParts();
	};

};

#endif // __CPMDECORATORSTYLE_H_
