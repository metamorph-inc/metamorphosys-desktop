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

#ifndef SET_CAD_MODEL_PARAMTERS_H
#define SET_CAD_MODEL_PARAMTERS_H

#include <isis_application_exception.h>
#include <isis_ptc_toolkit_functions.h>
#include <isis_include_ptc_headers.h>
//#include "AssemblyInterface.hxx"
#include <CommonStructures.h>
#include <string>
#include <list>
#include <map>


namespace isis
{


///////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	Description: 
//
//		The following function modifies Pro/E parameters within a model or assembly.  These are 
//		the parameters that are displayed in the Pro/E parameters table (displayed via 
//		“Tools” “Parameters” menu option).  
//
//		Example XML follows:
//
//		<CADComponent ComponentID="100000001" Name="bracket_plate" Type="ASSEMBLY">
//			<CADComponent ComponentID="100000002" Name="bracket_01" Type="Part">
//				<ParametricParameters>
//					<CADParameter Name="Height" Type="Float" Value="3000"> 
//						<Units Value="MM"/>  
//					</CADParameter>
//					<CADParameter Name="Count" Type="Integer" Value="555"/>
//					<CADParameter Name="Show_Wheels" Type="Boolean" Value="False"/>
//				</ParametricParameters>
//				<Constraint>
//					<Pair Fe...
//
//		The supported types are:
//			FLOAT	
//			INTEGER
//			BOOLEAN
//
//		Note – 1)	Type and Name are case insensitive (e.g. Name="HeigHT" Type="FloAT")
//			   2)	Units are optional and are not used by this function.  If they are set, 
//					they would be ignored.  The units will always be in the units of the Pro/E model.
//
//	Pre-Conditions:
//
//		in_model_name must be populated with the name of the model containing the parameters.
//
//		in_p_model must point to an active Pro/E model
//
//		in_ParametricParameters must point to a xml tree defined with the contents of 
//		<ParametricParameters> … </ParametricParameters>
//
//		The type in the xml file and the type in the Pro/E parameters table must align as follows:
//
//			XML File		Pro/E Model
//			-----------		------------
//			FLOAT			Real Number
//			BOOLEAN			Yes No
//			INTEGER			Integer
//
//	Post-Condtions:
//
//		If the parameter does not already exist in the Pro/E model, then 
//		isis::application_exception would be thrown.
//
//		If the parameter types do not align as described in the pre-conditions, 
//		then in some cases isis::application_exception  would be thrown, and in 
//		other cases the type in Pro/E would be modified.  In general, the XML 
//		file should be constructed so this does not happen.
//
//		Otherwise, the parameter would be modified in the Pro/E Model.

void ApplyParametricParameters( std::list<std::string>                          &in_ComponentIDs, 
							   std::map<std::string, isis::CADComponentData>	&in_CADComponentData_map,
							   std::vector<isis::CADCreateAssemblyError>		&out_ErrorList)
																		throw (isis::application_exception);

} // End namespace isis

#endif // PARAMETRIC_PARAMETERS_H