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

/*! \file CommonFunctions.h
    \brief Place holder for future functions that will be shared between Creo CAD applications.

	The functions placed in this file will be used by the Ceo CAD applications (e.g. Create-Assemblies,  
	Compute-Metrics, ...).
*/

#ifndef COMMON_FUNCTIONS_H
#define COMMON_FUNCTIONS_H

#include <isis_application_exception.h>
#include <isis_ptc_toolkit_functions.h>
#include <isis_include_ptc_headers.h>
#include <StringToEnumConversions.h>
#include <MultiFormatString.h>
#include <GraphicsFunctions.h>
#include <CommonStructures.h>
#include <string>

namespace isis
{
	void setCreoWorkingDirectory ( const MultiFormatString &in_MultiFormatString ) throw (isis::application_exception);

	//	Description:
	//		Combine in_ModelName and in_ModelType to from a single string.  Examples of returned strings follows:
	//			bracket.prt
	//			damper.asm
	//
	//		The case of in_ModelName is maitained.  The case of the suffix will always be .prt or .asm.
	//
	//	Pre-Conditions:
	//		p_solid_handle must point to a valide Creo feature
	//
	//	Post-Conditions:
	//
	//		if (in_ModelName.size() == 0)
	//			throw isis::application_exception
	//
	//		if ( in_ModelType Not PRO_PART or PRO_ASSEMBLY
	//			throw isis::application_exception
	//
	//		If no exceptions return the string per the description.
	//
	std::string CombineCreoModelNameAndSuffix ( const std::string &in_ModelName, ProMdlType in_ModelType )
																		throw (isis::application_exception);


	// This function computes the bounding box based via two methods as follows:
	// CAD_PRO_SOLID_OUTLINE_GET - does not exclude sketch geometry
	// CAD_PRO_SOLID_OUTLINE_COMPUTE excluded all geometry except for the solid geometry.  This means that 
	// the datums, coordinate system, and sketch curves/lines would be excluded.

	enum e_ComputeBoundingBoxMethod
	{
		CAD_PRO_SOLID_OUTLINE_GET,
		CAD_PRO_SOLID_OUTLINE_COMPUTE
	};

	void	ComputeBoundingBox( 
							e_ComputeBoundingBoxMethod							in_ComputeBoundingBoxMethod,
							ProSolid		in_p_solid,
							isis_CADCommon::Point_3D							&out_BoundingBox_Point_1,
							isis_CADCommon::Point_3D							&out_BoundingBox_Point_2,
							double												out_Dimensions_xyz[3] )
																		throw (isis::application_exception);											

		
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////
	enum e_CAD_SimplifiedRep_InclusionStatus
	{
		CAD_SIMPLIFIED_REP_INCLUDE,
		CAD_SIMPLIFIED_REP_EXCLUDE
	};


	std::string CAD_SimplifiedRep_InclusionStatus_string(  e_CAD_SimplifiedRep_InclusionStatus in_SimplifiedRep_InclusionStatus_enum)
											throw (isis::application_exception);


	struct CAD_SimplifiedRepData
	{
		int featureID;
		e_CAD_SimplifiedRep_InclusionStatus  inclusionStatus;

		CAD_SimplifiedRepData() : featureID(-1), inclusionStatus(CAD_SIMPLIFIED_REP_INCLUDE) {};
	};


	// Description:	
	//		In Creo if you 
	//			1)	click View Manage-Views 
	//			2)	select a simplified rep 
	//			3)	select Edit dropdown an then click Redefine
	//
	//		You will see a window that displays if a .prt/.asm is include or excluded.
	//
	//		This function returns that information in the form of a feature_ID and an indication if the feature_ID is 
	//		included or excluded.  The feature_ID maps to the .prt/.asm in the display.
	//
	//	Pre-Conditions
	//		in_Owner must be an assembly model containing in_SimplifiedRep
	//		in_Owner must not point to a part or any other type other than assembly
	//		Only one level (Assembly containing a simplified rep) of hierarchy is supported.
	//		in_SimplifiedRep must be a valid simplified rep; otherwise address violation will occur.
	//
	//	Post-Conditions
	//		If no exceptions
	//			return a populated out_FeatureID_to_SimplifiedRepData_map
	//		It is possible that SimplifiedRep would be empty and in that case the map will empty.
	//	
	//		An exception could occur is a call to a Creo SDK function returns a non-zero return code.
	void AssemblySimplifiedRep_RetrieveModelInclusionStatus ( 
								ProMdl		  &in_Owner,
								ProSimprep	  &in_SimplifiedRep,
								std::map<int, CAD_SimplifiedRepData> &out_FeatureID_to_SimplifiedRepData_map ) 
											throw (isis::application_exception);


} // END namespace isis

#endif