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

#ifndef CFD_ANALYSIS_H
#define CFD_ANALYSIS_H

namespace isis
{
	// If at lease one of the assemblies in in_CADAssemblies contains CFD analysis, then return true.    
	bool IsACFDAnalysisRun( const CADAssemblies &in_CADAssemblies );

	enum CFD_Fidelity {
		V0 = 0,
		V1 = 1,
	};
		


	// Description: 
	//		Creates in_PathAndFileName (typically ComputedValues.xml ). An example contents follows: 
	//
	//			<?xml version="1.0" encoding="UTF-8" standalone="no" ?>
	//			<Components ConfigurationID="|1" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="CADPostProcessingParameters.xsd">
	//				<Component ComponentID="|1" FEAElementID="" _id="id17">
	//					<Metrics _id="id18">
	//						<Metric DataFormat="SCALAR" MetricID="id-0067-00000001" Type="CoefficientOfDrag" Units="" _id="id19"/>
	//					</Metrics>
	//				</Component>
	//			</Components>
	//
	// Pre-Conditions: 
	//		in_CADAssemblies must point to a valid sis::CADAssemblies type
	//		in_out_CADComponentData_map must be a valid map
	// Post-Conditions:
	//		If in_CADAssemblies contains not CFD computation request, throw isis::application_exception
	//		if in_PathAndFileName could not be opened, throw isis::application_exception
	//		If no exceptions, in_PathAndFileName would be created.
	void CreateXMLFile_ComputedValues_CFD( 
					const std::string									&in_PathAndFileName,
					const isis::CADAssemblies							&in_CADAssemblies,
					std::map<std::string, isis::CADComponentData>		&in_CADComponentData_map )
																	throw (isis::application_exception);

	void CFD_Driver( 	const CFD_Fidelity								in_fidelity,
					const std::string									&in_ExtensionDirectory,
					const std::string									&in_WorkingDirectory,
					const isis::TopLevelAssemblyData					&in_TopLevelAssemblyData,
					std::map<std::string, isis::CADComponentData>		&in_CADComponentData_map )
																				throw (isis::application_exception);

} // end namespace isis


#endif
