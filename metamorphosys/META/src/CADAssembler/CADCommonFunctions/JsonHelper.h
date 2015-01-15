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

#ifndef JSONHELPER_H
#define JSONHELPER_H

#include "isis_application_exception.h"
#include <string>
#include <list>
#include <map>
#include <set>
#include <boost\filesystem.hpp>

#pragma warning( disable : 4290) 

namespace isis_CADCommon
{
	/* Parses a json file specified by path. Populates the "STEPModel" tag based on "id" using key and value of the input stepFiles parameter.
	* @param	in_ManifestJson_PathAndFileName path/name to the json file (e.g. .\manufacturing.manifest.json) 
	* @param	in_ComponentInstanceId_to_StepFile_map map of component instance ID to step file name
	* @param	out_ComponentInstanceIds_AddedToManifest component instance IDs that were updated with the STEP file name in the manifest.
	*/
	void AddStepFileMappingToManufacturingManifest(
									const std::string &in_ManifestJson_PathAndFileName,
									const std::map<std::string, std::string> &in_ComponentInstanceId_to_StepFile_map,
									std::set<std::string> &out_ComponentInstanceIds_AddedToManifest ) 
															throw (isis::application_exception);

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/*  hydrostatics.json example
			{
				"NumCount": 1,
				"Density": ,
				   "DensityUnits": "kg/mm^3",
				"CG_X": ,
				"CG_Y": ,
				"CG_Z": ,
				   "CG_Units": "mm",
				"DisplacedVolume": ,
				   "DisplacedVolumeUnits": "mm^3",
				"HydrostaticVolume": ,
				   "HydrostaticVolumeUnits": "mm^3",
				"Data": [
					{
						"Description": "1", 
						"Roll": 0, 
						"Pitch": 0,
						"Yaw": 0,
						"RollPitchYawUnits": "degrees",					       
						"WaterLine": "",
						   "WaterLineUnits": "mm",
						"ReferenceArea": "",
						   "ReferenceAreaUnits": "mm^2",
						"RightingMomentArm": "",
						   "RightingMomentArmUnits": "mm",
						"CB_X": ,
						"CB_Y": ,
						"CB_Z": ,
						   "CB_Units": "mm",
						"XSections": [
							{
								"Offset": 0.0,
								"Area": 0.0,
							}
							{
								"Offset": 0.0,
								"Area": 0.0,
							}
							{
								"Offset": 0.0,
								"Area": 0.0,
							}
						]
					}
				], 
				"TestBench": "Hydrostatics Testbench"
			}
	*/

	//	See json file example above.
	//	UpdateHydrostaticsJsonFile assumes that there is only one entry under "Data".  If there are more than one entry, all entries will 
	//	be filled in with identical data.
	//
	//	in_RightingMomentArm, in_CB_x, in_CB_y, and in_CB_z are not currently written to the json file.  This functio will be updated later
	//	to write those entries.

	void UpdateHydrostaticsJsonFile( const boost::filesystem::path	in_HydrostaticsFile_PathAndFileName,
									double				in_FluidDensity,
									double				in_WaterLine_Height_zAxis,
									double				in_ReferenceArea,  // Wetted surface area
									double				in_DisplacedVolume,
									double				in_HydrostaticVolume,
									double				in_RightingMomentArm, 
									double				in_CG_x, // Center of Gravity
									double				in_CG_y,
									double				in_CG_z,
									double				in_CB_x,  // Center of Bouncy
									double				in_CB_y,
									double				in_CB_z,
									std::vector< std::pair<double,double> > in_XSect)
														throw (isis::application_exception);

}

#endif