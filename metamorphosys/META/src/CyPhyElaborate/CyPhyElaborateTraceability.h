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

#ifdef _DEBUG
#import "..\CyPhyCOMInterfacesCS\bin\Debug\CyPhyCOMInterfacesCS.tlb"
#else
#import "..\CyPhyCOMInterfacesCS\bin\Release\CyPhyCOMInterfacesCS.tlb"
#endif

#include "UdmBase.h"
#include <algorithm>
#include <iterator>

namespace CyPhyCOMInterfaces { using namespace CyPhyCOMInterfacesCS; }

struct CyPhyElaborateTraceability : public CyPhyCOMInterfaces::IMgaTraceability
{
	std::unique_ptr<std::map<Udm::Object, Udm::Object> > copiedObjectsToOriginals;

	virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID riid, void ** ppvObject)
	{
		if (IsEqualGUID(__uuidof(IUnknown), riid) || IsEqualGUID(__uuidof(CyPhyCOMInterfaces::IMgaTraceability), riid))
		{
			*ppvObject = this;
			refcount++;
			return S_OK;
		}
		return E_NOINTERFACE;
	}
	
	long refcount;
	virtual ULONG STDMETHODCALLTYPE AddRef(void)
	{
		return ++refcount;
	}
	
	virtual ULONG STDMETHODCALLTYPE Release(void)
	{
		long rc = --refcount;
		if (rc == 0)
		{
			delete this;
		}
		return rc;
	}

	CyPhyElaborateTraceability(std::unique_ptr<std::map<Udm::Object, Udm::Object>>&& copiedObjectsToOriginals_)
		: copiedObjectsToOriginals(std::move(copiedObjectsToOriginals_)), refcount(1)
	{
	//	std::set<std::string> mapped;
	//	std::transform(copiedObjectsToOriginals->begin(), copiedObjectsToOriginals->end(), std::inserter(mapped, mapped.end()), [](const std::pair<Udm::Object, Udm::Object>& it)
	//	{
	//		return it.first.getPath("/") + "     " + it.second.getPath("/");
	//	});
	//	std::for_each(mapped.begin(), mapped.end(), [](const std::string str) { OutputDebugStringA(str.c_str()); OutputDebugStringA("\n"); });
	}

	virtual HRESULT __stdcall raw_GetMappedObject(BSTR in_id, BSTR * out_id)
	{
		VARIANT_BOOL has_mapped_obj;
		BSTR out_id_;
		raw_TryGetMappedObject(in_id, &out_id_, &has_mapped_obj);
		if (has_mapped_obj == VARIANT_FALSE)
		{
			return E_FAIL;
		}
		else
		{
			*out_id = out_id_;
			return S_OK;
		}
	}

	virtual HRESULT __stdcall raw_TryGetMappedObject(BSTR in_id, BSTR * out_id, VARIANT_BOOL * has_mapped_obj )
	{
		// set the return values to default
		*out_id = NULL;
		*has_mapped_obj = VARIANT_FALSE;

		std::string strin_id = static_cast<const char*>(_bstr_t(in_id));
		auto it = copiedObjectsToOriginals->begin();
		for (; it != copiedObjectsToOriginals->end(); it++)
		{
			auto id = UdmGme::UdmId2GmeId(it->first.uniqueId());
			if (_stricmp(id.c_str(), strin_id.c_str()) == 0)
			{
				// overwrite the return values if we found it
				auto ret = UdmGme::UdmId2GmeId(it->second.uniqueId());
				std::transform(ret.begin(), ret.end(), ret.begin(), ::tolower);
				*out_id = _bstr_t(ret.c_str()).Detach();
				*has_mapped_obj = VARIANT_TRUE;
				// while it is a map we do not need to iterate through if we have found it
				break;
			}
		}

		return S_OK;
	}

	virtual HRESULT __stdcall raw_CopyTo(IMgaTraceability* copy)
	{
		auto it = copiedObjectsToOriginals->begin();
		for (; it != copiedObjectsToOriginals->end(); it++)
		{
			auto id = UdmGme::UdmId2GmeId(it->first.uniqueId());
			std::transform(id.begin(), id.end(), id.begin(), ::tolower);
			auto original = UdmGme::UdmId2GmeId(it->second.uniqueId());
			std::transform(original.begin(), original.end(), original.begin(), ::tolower);
			HRESULT hr = copy->raw_AddItem(_bstr_t(id.c_str()), _bstr_t(original.c_str()));
			if (FAILED(hr))
				return hr;
		}
		return S_OK;
	}

	virtual HRESULT __stdcall raw_AddItem(BSTR copy, BSTR original)
	{
		return E_NOTIMPL;
	}
};