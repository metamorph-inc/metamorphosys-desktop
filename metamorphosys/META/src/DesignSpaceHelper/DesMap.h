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

#ifndef DESMAP_H
#define DESMAP_H
#include <afxtempl.h>
#include "uml.h"
#include "UmlExt.h"
#include "DesertIface.h"
#include "DesertIfaceBack.h"

#include "BackIface.h"
#include "desertdll.h"
using namespace DesertIface;

typedef map<DesertBase, long > UdmDesertMap;
typedef const pair<DesertBase const, long> UdmDesertMapItem;

//we need the inverse map, too
//Desert functions are supposed to generate unique IDs

typedef map<long, DesertBase> DesertUdmMap;
typedef const pair<long const, DesertBase > DesertUdmMapItem;




typedef set<DesertIface::Element> UdmElementSet;
typedef set<DesertIface::Member> UdmMemberSet;

void DoMap(DesertBase &db, UdmDesertMap & _map, DesertUdmMap &inv_des_map, long id);
long GetID(DesertBase &db, UdmDesertMap & _map);
DesertBase &GetObject(long id, DesertUdmMap & _map);

bool CreateDesertSpace(Space &sp, DesertIface::Element &e, UdmDesertMap &des_map, DesertUdmMap &inv_des_map, UdmElementSet &elements, bool root);

bool CreateCustomDomain(
		CustomDomain &cd, 
		DesertIface::CustomMember &mb, 
		UdmDesertMap &des_map, 
		DesertUdmMap &inv_des_map,
		UdmMemberSet &mb_set,
		bool root);

bool CreateDesertConstraintSet(ConstraintSet &cs, UdmDesertMap &des_map, DesertUdmMap &inv_des_map );
bool CreateElementRelations(DesertSystem &ds, UdmDesertMap &des_map, DesertUdmMap &inv_des_map);
bool CreateMemberRelations(DesertSystem &ds, UdmDesertMap &des_map, DesertUdmMap &inv_des_map);

bool CreateConstraints(DesertSystem &ds, UdmDesertMap &des_map, DesertUdmMap &inv_des_map);
bool CreateCustomDomains(DesertSystem &ds, UdmDesertMap &des_map, DesertUdmMap &inv_des_map, UdmMemberSet &mb_set );

bool CreateNaturalDomains(DesertSystem &ds, UdmDesertMap& des_map, DesertUdmMap &inv_des_map);
bool CreateVariableProperties(UdmDesertMap &des_map, DesertUdmMap &inv_des_map, UdmElementSet& elements);
bool CreateConstantProperties(UdmDesertMap &des_map, DesertUdmMap &inv_des_map, UdmElementSet& elements, UdmMemberSet &c_members);
bool CreateAssignments(UdmDesertMap& des_map, DesertUdmMap &inv_des_map, UdmElementSet& elements, UdmMemberSet &c_members);
bool CreateSimpleFormulas(DesertSystem &ds, UdmDesertMap& des_map, DesertUdmMap &inv_des_map);




#endif //DESMAP_H