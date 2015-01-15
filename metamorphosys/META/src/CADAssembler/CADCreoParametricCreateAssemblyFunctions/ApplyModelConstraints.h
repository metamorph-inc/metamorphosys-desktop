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

#ifndef APPLY_MODEL_CONSTRAINTS_H
#define APPLY_MODEL_CONSTRAINTS_H
#include <isis_application_exception.h>
#include <isis_ptc_toolkit_functions.h>
#include <isis_include_ptc_headers.h>
#include <string>
#include <iostream>
#include <StringToEnumConversions.h>
#include <BuildAssembly.h>
#include <AssembleComponents.h>
#include <CommonStructures.h>
#include "CadFactoryAbstract.h"

using namespace std;

namespace isis
{

	/*! \file ApplyModelConstraints.h 
    \brief This file declares a function (ApplyModelConstraints) that constrains (i.e. positions) Creo assemblies/parts.

	Detailed Description:

	The function ApplyModelConstraints constrains/positions the parts/assemblies (identified by component IDs) 
	listed in in_ComponentIDsToBeConstrained.  in_CADComponentData_map contains the constraint information 
	for each component.  Constraining a part/assembly consists of invoking Creo Toolkit funtions that align 
	datums on the part/assembly with datums on another part/assembly.  This alignment process fixes the 
	part/assembly in 3D space.  

	Pre-Conditions:

-	None

	Post-Conditions:

-	If in_assembly_model does not point to an in-memory Creo model, isis::application_exception would be thrown.

-	If in_ComponentIDsToBeConstrained.size() == 0, then this function returns without taking any action.

-	If in_CADComponentData_map does not contain constraint information for a particular component defined in 
	in_ComponentIDsToBeConstrained, then no action would be taken for that component. 

-	If an error occurs when applying the constraints (usually caused by erroneous constraint data), then 
	isis::application_exception would be thrown. 

	*/
bool ApplyModelConstraints( 
			cad::CadFactoryAbstract				&	in_factory,
			ProSolid							*in_assembly_model,
			const std::list<std::string>		&in_ComponentIDsToBeConstrained,  
			bool								in_AllowUnconstrainedModels,
			std::map<string, isis::CADComponentData>	&in_CADComponentData_map,
														// Provide for the case where the first assembled part does not have
														// the datums front, top, and right defined. 
			bool										in_IgnoreGuides,
			bool										in_FirstComponentToBePositionedAsIntiiallyPlaced_IfDatumsCannotBeFound )
										throw (isis::application_exception);

	
bool ComponentIDChildOf(	
	const string &in_ChildID,
	const string &in_ParentID,
	std::map<string, isis::CADComponentData> &in_CADComponentData_map );



} // end namespace isis

#endif