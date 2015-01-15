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
//#include <boost\unordered_map.hpp>
#include "Observer.h"
#include "Notifier.h"
#include "UdmConfig.h"

using namespace META_NAMESPACE;

//class PortObjInfo 
//{
//public:
//	Udm::Object	portObj;
//	enum	portType
//	{
//		INPUT_PORT,
//		OUTPUT_PORT
//	};
//};
//
//#define	PortNameObjectMap		boost::unordered_map<CString, PortObjInfo> 
//#define	PortNameObjectMapIter	boost::unordered_map<CString, PortObjInfo>::iterator

class ModelHandler 
	: public Notifier/*, public Observer*/
{
public:
	ModelHandler(void);
	~ModelHandler(void);
	void attachComponent(ModelicaComponent component);
	BOOL generateCompReport();							// Generates the attached component's contained port pairs and fills
														// the private Collection members of the class.
	void attachUI(CWnd* pUI)	{ this->pUI = pUI; }
private:
	CWnd				*pUI;							// pointer to UI Windows Object
	ModelicaComponent	component;
	//PortNameObjectMap	portNameObjectMapSet;			// used to locate port for modifying port in the GME model
	//void modifyPortInfo(LPCTSTR portName);			// to modify port info in the GME model
	//void observe(IMSG* pIMsg);
	BOOL processComponent();
	void notifyNewPortInfo(LPCTSTR name, LPCTSTR type);
	//void notifyModifyPortinfo(LPCTSTR oldPortName, LPCTSTR newPortName, LPCTSTR newPortType);
	void notifyNewLinkInfo(LPCTSTR srcPortName, LPCTSTR dstPortName);
	CString extractPortType(Udm::Object portObj);
};

