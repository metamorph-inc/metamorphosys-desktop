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

#ifndef TOOL_KIT_PASS_THROUGH_FUNCTIONS_H
#define TOOL_KIT_PASS_THROUGH_FUNCTIONS_H

#include <isis_ptc_toolkit_functions.h>
#include <isis_application_exception.h>
#include <isis_include_ptc_headers.h>
#include <CommonStructures.h>
#include <string>
#include <map>

namespace isis
{
	// This function first trys to retrieve the model (part/assembly) based on the simplified  rep (in_Representation); and
	// if that fails, then the model is opened without a simplified representation.  Often, models will not have 
	// simplified reps, but if they exists, and specified via in_Representation, they will be used.  A use case would be 
	// for FEA, you would prefer the
	// simplified rep.  The input xml to this program specifies the representation (i.e. simplified rep).  In some
	// cases. it would specify Featured_Rep and in other cased DeFeatured_Rep.  Other reps would be added in the 
	// future.
	//
	//	Model Type	Reps Supported
	//	----------  --------------
	//	Part		MASTER REP
	//	Part		<user defined (e.g. Featured_Rep, Defeatured_Rep, My_rep...)>
	//	Assembly	DEFAULT ENVELOPE REP
	//  Assembly	<user defined (e.g. Featured_Rep, Defeatured_Rep, My_rep...)>
	//
	// Notes:
	//		1) in_Representation is case insensitive.  Creo supports storing case senstive simplified reps, but
	//		   the SDK is treating those in a case insensitive fashion.
	//		2) this insensitive will retrieve "Master_Rep" for parts but will NOT retrieve Master_Rep for assemblies.

    void isis_ProMdlRetrieve_WithDescriptiveErrorMsg( 
										// Added Arguments
										const std::string &in_ComponentID,
										const std::string &in_Model_Name,
										const isis::MultiFormatString &in_Representation, // Simplified representation
										// Original arguments
										const ProFamilyName name, 
										ProMdlType    type,
										ProMdl       *p_handle)
												throw (isis::application_exception); 

	void isis_ProModelitemByNameInit_WithDescriptiveErrorMsg( 
										 // Added Arguments
										const std::string &in_ComponentID,
										const std::string &in_Model_Name,
										ProMdlType     mdltype,
										 // Original arguments
										ProMdl         mdl, 
                                        ProType        type, 
                                        const ProName        name, 
                                        ProModelitem*  p_item ) 
												throw(isis::application_exception); 

	void isis_ProSolidMassPropertyGet_WithDescriptiveErrorMsg( 
						const std::string									&in_ComponentID,
						std::map<std::string, isis::CADComponentData>		&in_CADComponentData_map,
						ProMassProperty*									mass_prop )
										throw(isis::application_exception); 
} // end namespace isis

#endif