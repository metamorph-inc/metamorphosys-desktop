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
#include "GridCtrl.h"
#include "GridCellBase.h"
#include "Notifier.h"
#include <boost\unordered_map.hpp>
#include <boost\unordered_set.hpp>
#include <set>
#include <vector>
// UIGridCtrl command target
class PortInfo
{
public:
	CString name;
	CString type;
	PortInfo(){};
	PortInfo(CString portName, CString portType)
		: name(portName), type(portType)
	{}
	void operator=(const PortInfo pi)
	{
		this->name = pi.name;
		this->type = pi.type;
	}
};

#define PortNameIndexMap				boost::unordered_map<CString /*PortName*/, UINT/*Grid Row/Col Index*/>
#define PortNameIndexMapIter			boost::unordered_map<CString /*PortName*/, UINT/*Grid Row/Col Index*/>::iterator
#define IndexPortInfoMap				boost::unordered_map<UINT/*Grid Row/Col Index*/, PortInfo>
#define IndexPortInfoMapIter			boost::unordered_map<UINT/*Grid Row/Col Index*/, PortInfo>::iterator
#define GridCellStatusInfoVector		vector<pair<UINT /*rowIndex*/, UINT /*colIndex*/>>
#define GridCellStatusInfoVectorIter	vector<pair<UINT /*rowIndex*/, UINT /*colIndex*/>>::iterator

class UIGridCtrl : public CGridCtrl, public Notifier, public Observer
{
	DECLARE_DYNAMIC(UIGridCtrl)

public:
	UIGridCtrl();
	virtual ~UIGridCtrl();

protected:
	DECLARE_MESSAGE_MAP()

public:
	static const BOOL ROW_INDEX			=	FALSE;
	static const BOOL COL_INDEX			=	TRUE;

	void showMatrix();
	void observe(IMSG* pIMSG);

private:
	UINT								numPorts;
	PortNameIndexMap					portNameIndexMapSet;	// collection for port info
	IndexPortInfoMap					indexPortInfoMapSet;
	GridCellStatusInfoVector			gridCellStatusInfoVector;
	boost::unordered_set<CString>		strictlyCompatibleDataTypes;
	boost::unordered_set<CString>		weaklyCompatibleDataTypes;

	void insertPortInfo(CString portName, CString portType);
	//void modifyPortInfo(CString oldPortName, PortInfo* newPortInfo);
	void addConnectionInfo(CString srcPortName, CString dstPortName);	// adds the connection info pair in the cellstatusinfo vector
	void addConnectionInfo(UINT rowIndex, UINT colIndex);
	void setConnection(LPCTSTR srcPortName, LPCTSTR dstPortName);
	void setConnection(UINT rowIndex, UINT colIndex);
	void setConnectionClr(LPCTSTR srcPortName, LPCTSTR dstPortName);
	void setConnectionClr(UINT rowIndex, UINT colIndex);
	void setPortClr(UINT index, const BOOL rowIndex);
	UINT findPortIndex(LPCTSTR portName);		// Returns row/col index/num in grid OR -1
	BOOL setPortInfo(UINT portIndex, PortInfo* portInfo);
	BOOL getPortInfo(UINT portIndex, PortInfo &portInfo);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
	void notifyCellInfo(CCellID cell);
	void highlightCell(CCellID cell);
};

