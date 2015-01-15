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

// mfcdual.cpp: Helpful functions for adding dual interface support to
//              MFC applications

// This is a part of the Microsoft Foundation Classes C++ library.
// Copyright (c) Microsoft Corporation.  All rights reserved.
//
// This source code is only intended as a supplement to the
// Microsoft Foundation Classes Reference and related
// electronic documentation provided with the library.
// See these sources for detailed information regarding the
// Microsoft Foundation Classes product.

#include "stdafx.h"

#include <afxpriv.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// DualHandleException

HRESULT DualHandleException(REFIID riidSource, const CException* pAnyException)
{
	ASSERT_VALID(pAnyException);

	TRACE0("DualHandleException called\n");

	// Set ErrInfo object so that VTLB binding container
	// applications can get rich error information.
	ICreateErrorInfo* pcerrinfo;
	HRESULT hr = CreateErrorInfo(&pcerrinfo);
	if (SUCCEEDED(hr))
	{
		TCHAR   szDescription[256];
		LPCTSTR pszDescription = szDescription;
		GUID    guid = GUID_NULL;
		DWORD   dwHelpContext = 0;
		BSTR    bstrHelpFile = NULL;
		BSTR    bstrSource = NULL;
		if (pAnyException->IsKindOf(RUNTIME_CLASS(COleDispatchException)))
		{
			// specific IDispatch style exception
			COleDispatchException* e = (COleDispatchException*)pAnyException;

			guid = riidSource;
			hr = MAKE_HRESULT(SEVERITY_ERROR, FACILITY_ITF,
							  (e->m_wCode + 0x200));

			pszDescription = e->m_strDescription;
			dwHelpContext = e->m_dwHelpContext;

			// propagate source and help file if present
			// call ::SysAllocString directly so no further exceptions are thrown
			if (!e->m_strHelpFile.IsEmpty()) {
				CT2COLE strHelpFile(e->m_strHelpFile);
				bstrHelpFile = ::SysAllocString(strHelpFile);
			}
			if (!e->m_strSource.IsEmpty()) {
				CT2COLE strSource(e->m_strSource);
				bstrSource = ::SysAllocString(strSource);
			}

		}
		else if (pAnyException->IsKindOf(RUNTIME_CLASS(CMemoryException)))
		{
			// failed memory allocation
			AfxLoadString(AFX_IDP_FAILED_MEMORY_ALLOC, szDescription);
			hr = E_OUTOFMEMORY;
		}
		else
		{
			// other unknown/uncommon error
			AfxLoadString(AFX_IDP_INTERNAL_FAILURE, szDescription);
			hr = E_UNEXPECTED;
		}

		if (bstrHelpFile == NULL && dwHelpContext != 0) {
			CT2COLE strHelpFilePath(AfxGetApp()->m_pszHelpFilePath);
			bstrHelpFile = ::SysAllocString(strHelpFilePath);
		}

		if (bstrSource == NULL) {
			CT2COLE strAppName(AfxGetAppName());
			bstrSource = ::SysAllocString(strAppName);
		}

		// Set up ErrInfo object
		pcerrinfo->SetGUID(guid);
		CT2COLE strDescription(pszDescription);
		pcerrinfo->SetDescription(::SysAllocString(strDescription));
		pcerrinfo->SetHelpContext(dwHelpContext);
		pcerrinfo->SetHelpFile(bstrHelpFile);
		pcerrinfo->SetSource(bstrSource);

		TRACE("\tSource = %ws\n", bstrSource);
		TRACE("\tDescription = %s\n", pszDescription);
		TRACE("\tHelpContext = %lx\n", dwHelpContext);
		TRACE("\tHelpFile = %ws\n", bstrHelpFile);

		// Set the ErrInfo object for the current thread
		IErrorInfo* perrinfo;
		if (SUCCEEDED(pcerrinfo->QueryInterface(IID_IErrorInfo, (LPVOID*)&perrinfo)))
		{
			SetErrorInfo(0, perrinfo);
			perrinfo->Release();
		}

		pcerrinfo->Release();
	}

	TRACE("DualHandleException returning HRESULT %lx\n", hr);

	return hr;
}
