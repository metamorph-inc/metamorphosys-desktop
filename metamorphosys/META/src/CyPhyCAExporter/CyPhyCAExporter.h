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

#ifndef CYPHYCAEXPORTER_H
#define CYPHYCAEXPORTER_H

#include "CyPhyML.h"
#include "CyPhyUtil.h"

typedef pair<CyPhyML::Connector, Udm::Object> Connector_Pair;

class CyPhyCAExporter
{
public:
	CyPhyCAExporter(){};
	CyPhyCAExporter(CyPhyML::ComponentAssemblies &cyphy_cas, CyPhyML::CWC &cyphy_cwc, bool cyphy_flatten);
	void createComponentAssembly();
	CyPhyML::ComponentAssembly getComponentAssembly();
	bool showGui;
private:
	CyPhyML::CWC cwc;
	CyPhyML::DesignContainer rootDC;
	CyPhyML::ComponentAssemblies ca_folder;
	CyPhyML::ComponentAssembly ca_model;
	map<CyPhyML::DesignContainer, CyPhyML::ComponentAssembly> container2caMap;
	map<Udm::Object, Udm::Object> copyMap;
	//map< pair<Udm::Object, Udm::Object>, Udm::Object>  newLinks;
	//map< pair<CyPhyUtil::ComponentPort, CyPhyUtil::ComponentPort>, Udm::Object>  newLinks;

	map<int, int> containersMap; //for performance, map<int(uniqueId ofcontainer), int(0:not selected, 1:selected, 2:for optional not null, 3:for optional use null)>

	bool flatten;
	void init(CyPhyML::DesignContainer &container, CyPhyML::ComponentAssembly &ca);
	void postProcessComponentAssembly(CyPhyML::ComponentAssembly &ca);
	void copyDesignElementInstance(CyPhyML::DesignElement &from_elem, Udm::Object &to_parent);

	Udm::Object getMappedInstanceObject(const Udm::Object &arcObj, Udm::Object &instanceParent);
	void copyUdmInstanceObjAttributes(const Udm::Object &from, Udm::Object &to, map<int, Udm::Object> &fromArchId2FromObj, bool makemap=false);
	
//	Udm::Object getorCopyObject(const Udm::Object &obj);
	Udm::Object getMappingObject(const Udm::Object &obj);

	set<Udm::Object> connsTraversed;//for dircted connection
	
	void createConnections(CyPhyML::DesignContainer &dcontainer);	
	void getNearestEndSet_for_ComRef(const Udm::Object::AssociationInfo &assocInfo, CyPhyUtil::ComponentPort &src_end, 
		                             list<CyPhyUtil::ComponentPort> &src_ends, CyPhyUtil::ComponentPort &peer_end, 
									 Udm::Object &currParentObj, 
									 bool isSrc,  bool bidirect);
	void createFlattenedConnections(CyPhyML::DesignContainer &dcontainer);
	void getFlattenedAssociationEnds_for_ComRef(const Udm::Object::AssociationInfo &assocInfo, CyPhyUtil::ComponentPort &src_end, 
									list<CyPhyUtil::ComponentPort> &ends, CyPhyUtil::ComponentPort &peer_end, 
									Udm::Object &currParentObj,
									bool isSrc, bool bidirect);

	void copyDesignContainerElement(Udm::Object &from_obj, CyPhyML::ComponentAssembly &to_parent);
	
	bool isOptionalContainer(const Udm::Object &obj, bool &isNullSelected);
	
	bool isSelected(const CyPhyML::DesignElement &com);
	bool isSelected(const CyPhyML::ComponentRef &comref);
	bool isSelected(const CyPhyML::DesignContainer &container);
	
	void makeArch2InstMap(Udm::Object &obj, map<int, Udm::Object> &fromArchId2FromObj, int arch_level);

	CyPhyML::ComponentRef createComponentRef(CyPhyML::DesignElement &from_com, CyPhyML::DesignContainer &from_parent);
	CyPhyML::ComponentRef createComponentRef(CyPhyML::ComponentRef &from_ref, CyPhyML::DesignContainer &from_parent);
	bool isRefPort(Udm::Object &obj, CyPhyML::ComponentRef &comref);
	void copyPropertyAndParameter(CyPhyML::DesignContainer &container, CyPhyML::ComponentAssembly &ca);


	CyPhyUtil::ComponentPort getMappingComponentPort(CyPhyUtil::ComponentPort &from_port);
	//void reconstructConnections(const Uml::Class &type, CyPhyML::ComponentAssembly &parent_ca, list<CyPhyUtil::ComponentPort> &end1s, list<CyPhyUtil::ComponentPort> &end2s);
	void reconstructConnections(const Udm::Object &from_assoc, CyPhyML::ComponentAssembly &parent_ca, list<CyPhyUtil::ComponentPort> &end1s, list<CyPhyUtil::ComponentPort> &end2s);
	void getConnectors(CyPhyML::Connector &sport, Udm::Object &sport_ref_parent,
							list<Connector_Pair> &ends, set<CyPhyML::ConnectorComposition> &traversedConns, bool flattened=false);

};

#endif