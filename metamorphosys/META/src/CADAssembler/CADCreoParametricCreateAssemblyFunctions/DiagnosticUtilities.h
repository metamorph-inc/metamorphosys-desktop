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

#ifndef DIAGNOSTIC_UTILITIES_H
#define DIAGNOSTIC_UTILITIES_H

#include <isis_ptc_toolkit_functions.h>
#include <isis_application_exception.h>
#include <isis_include_ptc_headers.h>
#include <CommonStructures.h>
#include <list>
#include <iostream>
#include <map>
//#include "AssemblyInterface.hxx"

namespace isis
{

void stream_PopulateOneConstraintInConstraintStructure(
	ProSolid	 *in_assembly_model,			// typedef struct sld_part* ProSolid;
	const std::list<int>	 &in_base_model_path_list,
	const std::list<int>	 &in_added_model_path_list,
	ProType		 in_pro_datum_type,				// enum PRO_SURFACE, PRO_AXIS
	const ProName		 in_base_model_datum_name,		// ASM_RIGHT, A_1..
	ProDatumside in_base_model_datum_side,		// enum PRO_DATUM_SIDE_YELLOW (SIDE_A), PRO_DATUM_SIDE_RED (SIDE_B), PRO_DATUM_SIDE_NONE
	const ProName		 in_added_model_datum_name,		// RIGHT, A23 ..
	ProDatumside in_added_model_datum_side,		// enum PRO_DATUM_SIDE_YELLOW (SIDE_A), PRO_DATUM_SIDE_RED (SIDE_B), PRO_DATUM_SIDE_NONE
	ProAsmcompConstrType	in_constraint_type, // enum PRO_ASM_ALIGN, PRO_ASM_ALIGN_OFF...
	double		 in_offset_between_datums,		// This is only used if in_constraint_type == PRO_ASM_ALIGN_OFF or PRO_ASM_MATE_OFF
	
	ostream &out_Stream);

void printf_assembled_info( wchar_t in_part_1[], 
						    wchar_t in_part_2[], 
							int in_num_datums, 
							ProName in_datums[3][2] );

void printf_assembled_orientation_info( int in_num_datums,  ProType in_datum_type[],
									    ProDatumside in_datum_side[3][2],
										ProAsmcompConstrType  in_constraint_type[],
										double in_offset[3]);


int ComputeNumberOfComponents(  std::map<std::string, CADComponentData> &in_ComponentAssembledInfo );


void stream_CADComponentData_map( const std::map<std::string, isis::CADComponentData> &in_CADComponentData_map,
									    ostream &clog );


void stream_AssemblyCADComponentData( const std::string					&in_AssemblyComponentID, 
					std::map<string, isis::CADComponentData>			&in_CADComponentData_map,
					 ostream											&out_Stream, 
					 bool												in_OutputChildren = true,
					 int												in_AssemblyLevel = 1  );


void stream_AnalysisInputData( const CADAnalyses &in_CADAnalyses, ostream &out_Stream )
																			throw (isis::application_exception);


void stream_MaterialsData(const std::map<std::string, Material>	&in_Materials, ostream &out_Stream )
																			throw (isis::application_exception);


std::set<std::string> NonSizeToFitComponents_ReferencedBy_SizeToFitConstraints( 
								std::map<string, isis::CADComponentData>	&in_CADComponentData_map )
																			throw (isis::application_exception);

} // END namespace isis

#endif // DIAGNOSTIDIAGNOSTIC_UTILITIES_HC_UTILITIES