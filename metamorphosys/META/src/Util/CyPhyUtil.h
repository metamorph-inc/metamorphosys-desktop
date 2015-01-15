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

#ifndef CYPHYUTIL_H
#define CYPHYUTIL_H

#include "Uml.h"
#include "UdmUtil.h"
#include <objbase.h>
#include "CyPhyML.h"

#define DOEVENTS() \
{\
    MSG msg;\
    long sts;\
    do {\
    if (sts = PeekMessage(&msg, (HWND) NULL, 0, 0, PM_REMOVE)) {\
            TranslateMessage(&msg);\
            DispatchMessage(&msg);\
        }\
    } while (sts);\
};

namespace CyPhyUtil
{
	struct ComponentPort{
		Udm::Object port;
		Udm::Object port_ref_parent;
		
		bool operator<(const ComponentPort& rhs) const
		{
			if(!port_ref_parent && !rhs.port_ref_parent)
				return port.uniqueId() < rhs.port.uniqueId();
			else if(port_ref_parent && rhs.port_ref_parent)
			{
				if(port_ref_parent.uniqueId() < rhs.port_ref_parent.uniqueId())
					return true;
				else if(port_ref_parent.uniqueId() > rhs.port_ref_parent.uniqueId())
					return false;
				else //==
					return port.uniqueId() < rhs.port.uniqueId();
			}
			else if(!port_ref_parent && rhs.port_ref_parent)
				return true;
			else if(port_ref_parent && !rhs.port_ref_parent)
				return false;
			return port.uniqueId() < rhs.port.uniqueId(); // should be unreachable
		}

		bool operator==(const ComponentPort& rhs) const
		{
			return (port==rhs.port && port_ref_parent==rhs.port_ref_parent);
		}
	};

	typedef struct {
		ComponentPort src;
		std::string srcRoleName;
		ComponentPort dst;
		std::string dstRoleName;
	} ComponentPortPair;

	ComponentPortPair getConnectionEnds(const Uml::Class &type, const Udm::Object &conn);
	ComponentPortPair getCompositionEnds(const Uml::Class &type, const Udm::Object &conn);
	bool isSamePort(ComponentPort &port1, ComponentPort &port2);

	Udm::Object reconstructConnection(const Uml::Class &type,  const Udm::Object& parent, 
							   const Udm::Object &end1, const CyPhyML::ComponentRef &end1_comref, 
							   const Udm::Object &end2, const CyPhyML::ComponentRef &end2_comref, const Udm::Object& origAssocObj);
	/*Udm::Object createComposition(const Uml::Class &type,const CyPhyML::Port& src,const CyPhyML::Port& dst,
						   const Udm::Object& parent,const Udm::Object& srcRefParent,const Udm::Object& dstRefParent);*/
	Udm::Object createComposition(const Uml::Class &type,const Udm::Object& src,const Udm::Object& dst,
						   const Udm::Object& parent,const Udm::Object& srcRefParent,const Udm::Object& dstRefParent);
	Udm::Object createSignalConnectionType(const Uml::Class &type,const Udm::Object& src,const Udm::Object& dst,
									const Udm::Object& parent, const Udm::Object& srcRefParent,const Udm::Object& dstRefParent);
};

#endif