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

#ifndef SURVIVABILITY_ANALYSIS_H
#define SURVIVABILITY_ANALYSIS_H

#include <GraphicsFunctions.h>

namespace isis
{



	// If at lease one of the assemblies in in_CADAssemblies contains analysesBallistic, then return true.    
	bool IsABallisticAnalysisRun( const CADAssemblies &in_CADAssemblies );
	// If at lease one of the assemblies in in_CADAssemblies contains analysesBlast, then return true.    
	bool IsABlastAnlysisRun( const CADAssemblies &in_CADAssemblies );

	void PopulateBallisticFiles( 
					const TopLevelAssemblyData							&in_TopLevelAssemblyData,
					const std::string									&in_WORKING_DIR,
					std::map<std::string, isis::CADComponentData>		&in_CADComponentData_map )
																	throw (isis::application_exception);

	void PopulateBlastFiles( 
					const TopLevelAssemblyData							&in_TopLevelAssemblyData,
					const std::string									&in_WORKING_DIR,
					std::map<std::string, isis::CADComponentData>		&in_CADComponentData_map )
																	throw (isis::application_exception);

	// This function assumes:
	//	1. The assembly is of a vehicle
	//	2. The coordinate system of the vehicle is as follows:
	//		z axis pointing in the direction of backward motion of the vehicle
	//		y axis pointing upward
	//		x axis in accordance to the right-hand rule
	//	3.	Pt_0  x, y, z values
	//		Pt_1  x, y, z values
	//		Pt_2  x, y, z values
	//		Where
    //           Vector ( Pt_0 to Pt_1 )   X  Vector ( Pt_0 to Pt_2 )   would define the upward direction for a vehicle.
    //           X represents the cross product
	//	4.  For tracked vehicles, the tracks are parallel to the z-axis
	//  5.  For wheeled vehicles, the portion of the wheels touching the ground form a 
	//		plane. 
	void ComputeVehicleGroundPlane( const std::string								&in_AssemblyComponentID,
									std::map<std::string, isis::CADComponentData>	&in_CADComponentData_map,
									std::vector<isis_CADCommon::Point_3D>			&out_GroundPlanePoints )
																			throw (isis::application_exception);




} // END namespace isis

#endif