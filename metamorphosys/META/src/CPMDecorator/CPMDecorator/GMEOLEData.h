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

#ifndef GMEOLEData_h
#define GMEOLEData_h

#include "StdAfx.h"
#include "DecoratorLib.h"
#include <afxole.h>
#include "mfcdual.h"
#include "DecoratorSmart.h"

class CGMEDataDescriptor
{
public:
	CGMEDataDescriptor(): pFile(0) {}
	CGMEDataDescriptor(CTypedPtrList<CPtrList, CRect *> &list, CTypedPtrList<CPtrList, CRect *> &annList, CPoint dragPt,CPoint offs);
	~CGMEDataDescriptor() { Reset(); }
public:
	CTypedPtrList<CPtrList, CRect *> rects;	
	CTypedPtrList<CPtrList, CRect *> annRects;	
	CPoint offset;
	CFile *pFile;
public:
	void SimpleDraw(CDC* pDC,CPoint& pt);
	void GetBoundingRect(CRect& rBoundingRect);
	void Reset();
	void Draw(CDC *pDC,CPoint &pt);
	void Serialize(CArchive& ar);
	bool Load(COleDataObject* pDataObject);
	void Clean();
	int GetCount();
public:
	// called externally (from GMEView.cpp and ActiveBrowserPropertyPage.cpp)
	// to destruct certain lists
	static void destructList( CTypedPtrList<CPtrList, CRect *>& pList);//it is a CRectList
};

struct IMgaProject;

class CGMEDataSource : public COleDataSource
{
public:
	CGMEDataSource(IMgaProject *p)
	{
		EnableAutomation();
		ASSERT( p != NULL ); 
		project = p;
		myData++; 
	}

	~CGMEDataSource() { myData--; }

protected:
	CComPtr<IMgaProject> project;
	CComPtr<IUnknown> data;
	CComPtr<IUnknown> folders; // by Zolmol
	CComPtr<IUnknown> regdata;

public:
	static CLIPFORMAT cfGMEDesc;
	static int myData;

public:
	void SetNodes(IMgaFCOs *fcos) { data = fcos; }
	void SetFolders(IMgaFolders *objs) { folders = objs; } // by Zolmol
	void SetMetaRole(IMgaMetaRole *metaRole) { data = metaRole; }
	void SetAnnotations(IMgaRegNodes *regs) { regdata = regs; }

	void CacheDescriptor(CGMEDataDescriptor* desc);
	void DelayXMLDump();

	static bool IsGmeNativeDataAvailable(COleDataObject *pDataObject, IMgaProject *project);
	static bool IsXMLDataAvailable(COleDataObject *pDataObject);
	static bool ParseXMLData(COleDataObject *pDataObject, IMgaObject *target, bool merge);
	static CComPtr<IGMEOLEApp> get_GME(CComObjPtr<IMgaProject> project);

protected:
	HGLOBAL CreateDescriptor(CGMEDataDescriptor* desc);
	virtual BOOL OnRenderFileData(LPFORMATETC lpFormatEtc, CFile* pFile);

// Interface Maps
protected:
	DECLARE_MESSAGE_MAP()
	// OLE dispatch map functions
	afx_msg LPDISPATCH DispGetData();
	afx_msg void DispSetData(LPDISPATCH newValue);
	afx_msg LPDISPATCH DispGetFolders();
	afx_msg void DispSetFolders(LPDISPATCH newValue);
	afx_msg LPDISPATCH DispGetRegistryData();
	afx_msg void DispSetRegistryData(LPDISPATCH newValue);
	afx_msg LPDISPATCH DispGetProject();
	afx_msg void DispSetProject(LPDISPATCH newValue);

	DECLARE_DISPATCH_MAP()
	DECLARE_INTERFACE_MAP()

	BEGIN_DUAL_INTERFACE_PART(MgaDataSource, IMgaDataSource)
		STDMETHOD(get_Data)(IUnknown **p);
		STDMETHOD(get_Folders)(IUnknown **p);
		STDMETHOD(get_RegistryData)(IUnknown **p);
		STDMETHOD(get_Project)(IUnknown **p);
	END_DUAL_INTERFACE_PART(MgaDataSource)

	//     add declaration of ISupportErrorInfo implementation
	//     to indicate we support the OLE Automation error object
	DECLARE_DUAL_ERRORINFO()
};


class CGMEClosureDataSource : public CGMEDataSource // by Zolmol
{
public:
	CGMEClosureDataSource(IMgaProject *p) : CGMEDataSource( p) {}

	~CGMEClosureDataSource() {}

protected:
	CComPtr<IUnknown>	m_topFcos;
	CComPtr<IUnknown>	m_topFolders;
	long				m_options;
	CComBstrObj			m_absPathPart;
	CComBstrObj			m_acceptingKinds;

public:
	void SetTopNodes(IMgaFCOs *objs) { m_topFcos = objs; }
	void SetTopNodes(IMgaFolders *objs) { m_topFolders = objs; }
	void SetOptions( long l) { m_options = l; }
	void SetAbsPathPart( const CComBstrObj& path) { m_absPathPart = path; }
	void SetAcceptingKinds( const CComBstrObj& acceptingkinds) { m_acceptingKinds = acceptingkinds; }

protected:
	virtual BOOL OnRenderFileData(LPFORMATETC lpFormatEtc, CFile* pFile);
};


#endif // whole file