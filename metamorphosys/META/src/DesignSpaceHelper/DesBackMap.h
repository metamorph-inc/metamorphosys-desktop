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

#ifndef DES_BACK_MAP_H
#define DES_BACK_MAP_H
#include "DesMap.h"


typedef map<DesertIface::Element , DesertIfaceBack::Element>  UDMForwardMap;
typedef map<DesertIface::Element , DesertIfaceBack::Element>  UDMBackMap;

typedef map<long, DesertIfaceBack::Element> DesertUdmBackElemMap;
typedef pair<long const, DesertIfaceBack::Element> DesertUdmBackElemItem;

typedef map<long, DesertIfaceBack::VariableProperty> DesertUdmBackVpMap;
typedef pair<long const, DesertIfaceBack::VariableProperty> DesertUdmBackVpItem;

typedef map<long, DesertIfaceBack::CustomMember> DesertUdmBackCmMap;
typedef pair<long const, DesertIfaceBack::CustomMember> DesertUdmBackCmItem;

typedef map<long, DesertIfaceBack::NaturalMember> DesertUdmBackNmMap;
typedef pair<long const, DesertIfaceBack::NaturalMember> DesertUdmBackNmItem;


namespace BackIfaceFunctions {

	/*
		Creates an element in the DesertBackIface datanetwork
		from a DesertUdmMap and a desertID of the Element.

	*/
	static DesertUdmBackElemMap back_elements_map;
	static DesertUdmBackCmMap back_cm_map;
	static DesertUdmBackNmMap back_nm_map;
	static DesertUdmBackVpMap back_properties_map;

	void ClearMap();

	bool GetElement(DesertIfaceBack::DesertBackSystem &dbs,
					DesertUdmMap & _map,
					long desertID,
					DesertIfaceBack::Element &ret,
					std::string &element_path);
	

	bool GetCustomMember(
					DesertIfaceBack::DesertBackSystem &dbs,
					DesertUdmMap & _map,
					long desertID,
					DesertIfaceBack::CustomMember &ret);


	bool GetNaturalMember(
					DesertIfaceBack::DesertBackSystem &dbs,
					long value,
					DesertIfaceBack::NaturalMember &ret);

	bool GetElementProperty(
		DesertIfaceBack::DesertBackSystem &dbs,
		DesertUdmMap & _map,
		UdmDesertMap &_map_dir,
		long desertID,
		DesertIfaceBack::VariableProperty &ret);

	void CreateAlternativeAssignment(
		DesertIfaceBack::DesertBackSystem &dbs,
		DesertIfaceBack::Configuration &conf,
		DesertUdmMap &_map,
		long alt_of_desertID,
		long alt_desertID,
		bool create_ass);			//or just check only the requirements
	
	void CreatePropertyAssignment(
		DesertIfaceBack::DesertBackSystem &dbs,
		DesertIfaceBack::Configuration &conf,
		DesertUdmMap &_map,
		UdmDesertMap &_map_1,
		long assignment_desertID,
		bool create_ass
		);

};


#endif DES_BACK_MAP_H