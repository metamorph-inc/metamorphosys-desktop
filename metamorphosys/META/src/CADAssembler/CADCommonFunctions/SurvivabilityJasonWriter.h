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

#ifndef SURVIVABILITY_JASON_WRITER_H
#define SURVIVABILITY_JASON_WRITER_H
#include "isis_application_exception.h"
#include "GraphicsFunctions.h"
#include <vector>
#include <string>
namespace isis_CADCommon
{

	
	struct CADCommon_Shotline
	{
		std::string					name;
		std::string					threatRef; 
		isis_CADCommon::Point_3D	startPoint;
		isis_CADCommon::Point_3D	targetPoint;

		CADCommon_Shotline();
		
		CADCommon_Shotline( const std::string &in_Name,
				  const std::string &in_ThreatRef,
				  const isis_CADCommon::Point_3D &in_StartPoint,
				  const isis_CADCommon::Point_3D &in_TargetPoint );


	};
	
	enum e_CADCommon_ReferencePlane
    {
        CADCommon_GROUND,  
		CADCommon_WATERLINE, 
    };

	struct  CADCommon_ReferencePlane
	{
		e_CADCommon_ReferencePlane type;
		std::vector<isis_CADCommon::Point_3D> points;
	};

	// Normally there would be only one reference plane; however this function supports writing
	// multiple reference planes.
	// if in_ReferencePlanes and in_Shotlines are empty, then this program would not change the
	// json file except for deleting the referencePlanes and shotlines from the json file.
	// Deleting the referencePlanes and shotlines assures that the json file would not be used
	// with obsolete data.
	void Write_ReferencePlane_and_ShotLines_to_Json( const std::string &in_BallisticConfig_PathAndFileName,
										  std::vector<CADCommon_ReferencePlane> in_ReferencePlanes,
										  const std::vector<CADCommon_Shotline> in_Shotlines,
										  bool isBallistic = true /*07-29-2013: DY added to distinguish between ballistic and blast configs*/) 
															 throw (isis::application_exception);



}  // END namespace isis

#endif