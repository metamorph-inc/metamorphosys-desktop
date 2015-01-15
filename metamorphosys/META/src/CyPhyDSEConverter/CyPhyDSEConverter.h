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

#ifndef CYPHYDSECONVERTER_H
#define CYPHYDSECONVERTER_H

#include "CyPhyML.h"
#include "UdmUtil.h"
#include "StatusDialog.h"
#include "resource.h"
#include "CyPhyUtil.h"

class CyPhyDSEConverter
{
public:
	CyPhyDSEConverter():genCA(false){};
	CyPhyDSEConverter(CyPhyML::RootFolder &_root);
	void initCopyMap();
	void initCopyMap(const Udm::Object &rootObj);
	void createDSFromCA(const CyPhyML::DesignElement &cyphy_ca);
	void create_CA_DC_copy(const CyPhyML::DesignEntity &cyphy_ca, Udm::Object &to_parent);
	void gatherConnections_tobe_reconstruce(const Udm::Object &obj, const Udm::Object &focusObj);
	void reconstructExtractAssociations(Udm::Object &focusObj);
	CyPhyML::DesignContainer createNewDesignContainer(CyPhyML::DesignSpace &dsFolder, const std::string &name);
	CyPhyML::DesignContainer createNewDesignContainer(CyPhyML::DesignContainer &dc, const std::string &name, bool isAlt=false);
	void updateDesignContainerTypeAndIcon(CyPhyML::DesignContainer &new_dc, bool isAlt=false);
	CyPhyML::ComponentAssembly createNewCA(Udm::Object &parentObj, const std::string &name);
	void createNewObject(const Udm::Object &from_obj, bool delObj=true);
	void createNewObject(const Udm::Object &from_obj, Udm::Object &to_parent);

	void setNewDesignContainer(CyPhyML::DesignContainer &dc);
	void setNewCA(CyPhyML::ComponentAssembly &ca);

	void addCopyMapInput(Udm::Object &from, Udm::Object &to);
	void reconstructAssociations(Udm::Object &parentObj);
	void reconstructReference(CyPhyML::DesignContainer &dc);
	void reconstructReference(CyPhyML::DesignElement &delem);

	void deleteConvertedObjs();
	void startProgressBar();
	void endProgressBar();
private:
	CyPhyML::DesignContainer new_dc;
	CyPhyML::ComponentAssembly new_ca;
	CyPhyML::RootFolder rootFolder;
	bool genCA;
	UdmUtil::copy_assoc_map copyMap;
	set<Udm::Object> tobeDelObjs;
	set<Udm::Object> tobeReconnecteLinks;
	CStatusDialog prgDlg;
	map<CyPhyUtil::ComponentPort, Udm::Object> componentPortMap;
	
	void copyUdmObjAttributes(const Udm::Object &from, Udm::Object &to, map<int, Udm::Object> &fromArchId2FromObj, bool makemap=false);
	void createAssociation(Udm::Object &assocParent, Uml::Class &assocType,
						   std::string &roleName1, Udm::Object &end1,
						   std::string &roleName2, Udm::Object &end2);
	void copyAssociation(Udm::Object &fromAssoc, Udm::Object &toParent);
	void makeArch2InstMap(const Udm::Object &obj, map<int, Udm::Object> &fromArchId2FromObj, int arch_level);
	Udm::Object createNewPort(const CyPhyUtil::ComponentPort &from, Udm::Object &to_parent, std::string port_name="");
	Udm::Object getMappingObject(const Udm::Object &obj);
	CyPhyUtil::ComponentPort getMappingComponentPort(CyPhyUtil::ComponentPort &from_port);
	void updateUdmCopySubtype(const Udm::Object &from, Udm::Object &to);
	void updateVisualConstraintRefs(CyPhyML::DesignContainer &dc);
};

#endif