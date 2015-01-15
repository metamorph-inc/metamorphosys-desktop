/*
Copyright (C) 2013-2015 MetaMorph Software, Inc

Permission is hereby granted, free of charge, to any person obtaining a
copy of this data, including any software or models in source or binary
form, as well as any drawings, specifications, and documentation
(collectively "the Data"), to deal in the Data without restriction,
including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Data, and to
permit persons to whom the Data is furnished to do so, subject to the
following conditions:

The above copyright notice and this permission notice shall be included
in all copies or substantial portions of the Data.

THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

=======================
This version of the META tools is a fork of an original version produced
by Vanderbilt University's Institute for Software Integrated Systems (ISIS).
Their license statement:

Copyright (C) 2011-2014 Vanderbilt University

Developed with the sponsorship of the Defense Advanced Research Projects
Agency (DARPA) and delivered to the U.S. Government with Unlimited Rights
as defined in DFARS 252.227-7013.

Permission is hereby granted, free of charge, to any person obtaining a
copy of this data, including any software or models in source or binary
form, as well as any drawings, specifications, and documentation
(collectively "the Data"), to deal in the Data without restriction,
including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Data, and to
permit persons to whom the Data is furnished to do so, subject to the
following conditions:

The above copyright notice and this permission notice shall be included
in all copies or substantial portions of the Data.

THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  
*/

#include <AssemblyEditingViaLink.h>
#include <fstream>

namespace isis
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void Test_CreateAssembly(   std::map<string, isis::CADComponentData>	&in_out_CADComponentData_map,
								isis::MetaLinkAssemblyEditor &in_out_MetaLinkAssemblyEditor,
								std::string &out_ParentAssemblyInstanceID	)
																			throw (isis::application_exception)
	{

		std::ifstream t("TwoPlates_Cad.xml");
		std::stringstream buffer;
		buffer << t.rdbuf();

		in_out_MetaLinkAssemblyEditor.CreateAssembly( buffer.str() );

		out_ParentAssemblyInstanceID="3_1";

	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void Test_AddComponentToAssembly(   const std::string &in_ParentAssemblyInstanceID,	
										std::map<string, isis::CADComponentData>	&in_out_CADComponentData_map,
										isis::MetaLinkAssemblyEditor &in_MetaLinkAssemblyEditor)
																			throw (isis::application_exception)
	{
	
		//MetaLinkAssemblyEditor  metaLinkAssemblyEditor( 
		//									in_ParentAssemblyInstanceID,
		//									in_out_CADComponentData_map);

		std::vector<isis::CADParameter>	cADParameters;

		isis::CADParameter cADParameter;

			cADParameter.name	= "Length";
			cADParameter.value	= "300";
			cADParameter.type	=  isis::CAD_FLOAT;
			cADParameters.push_back(cADParameter);

			cADParameter.name	= "Width";
			cADParameter.value	= "50";
			cADParameter.type	=  isis::CAD_FLOAT;

			cADParameters.push_back(cADParameter);

			// This is intended to work with C:\Temp\scratch\2013_06_27_Two_Plates
			std::vector<isis::CADCreateAssemblyError> errorList;
			in_MetaLinkAssemblyEditor.AddComponentToAssembly (	
								 "3_1",							// in_ParentAssemblyInstanceID
								 "new_comp_1", 					// in_ComponentInstanceID,
								 "Plate_02",					// in_CreoModelName,
								  PRO_MDL_PART,					// ProMdlType in_CreoModelType,
								  "Steel",						// in_MaterialID,
								  isis::CAD_SPECIAL_INSTRUCTION_NONE, // in_SpecialInstruction,
								  cADParameters,
								  errorList );   //	&in_CADParameters) 
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void Test_ModifyParameters(  const std::string &in_ParentAssemblyInstanceID,	
								std::map<string, isis::CADComponentData>	&in_out_CADComponentData_map,
								isis::MetaLinkAssemblyEditor &in_MetaLinkAssemblyEditor)
																			throw (isis::application_exception)
	{
	
		//MetaLinkAssemblyEditor  metaLinkAssemblyEditor( 
		//									in_ParentAssemblyInstanceID,
		//									in_out_CADComponentData_map);

		std::vector<isis::CADParameter>	cADParameters;

		isis::CADParameter cADParameter;

			cADParameter.name	= "Length";
			cADParameter.value	= "500";
			cADParameter.type	=  isis::CAD_FLOAT;
			cADParameters.push_back(cADParameter);

			cADParameter.name	= "Width";
			cADParameter.value	= "20";
			cADParameter.type	=  isis::CAD_FLOAT;

			cADParameters.push_back(cADParameter);

			// This is intended to work with C:\Temp\scratch\2013_06_27_Two_Plates

			in_MetaLinkAssemblyEditor.ModifyParameters ( "6",cADParameters);
												

	}


	void Test_ConstrainComponent(  const std::string &in_ParentAssemblyInstanceID,	
									std::map<string, isis::CADComponentData>	&in_out_CADComponentData_map,
									isis::MetaLinkAssemblyEditor &in_MetaLinkAssemblyEditor)
																			throw (isis::application_exception)
	{
	

		//MetaLinkAssemblyEditor  metaLinkAssemblyEditor( 
		//									in_ParentAssemblyInstanceID,
		//									in_out_CADComponentData_map);


		std::vector< isis::ConstraintPair>  creoConstraintPairs;

		

			isis::ConstraintPair  creoConstraintPair_1;

			creoConstraintPair_1.featureAlignmentType = isis::ProAsmcompConstrType_enum("MATE");
			creoConstraintPair_1.featureGeometryType  = isis::FeatureGeometryType_enum("SURFACE");
			creoConstraintPair_1.featureInterfaceType = isis::FeatureInterfaceType_enum("CAD_DATUM");

			isis::ConstraintFeature  creoConstraintFeature_A;
			isis::ConstraintFeature  creoConstraintFeature_B;

			creoConstraintFeature_A.componentInstanceID	 =  "new_comp_1";
			creoConstraintFeature_A.featureName			 =  "Front";
			creoConstraintFeature_A.featureOrientationType =  isis::ProDatumside_enum("SIDE_A");
			creoConstraintPair_1.constraintFeatures.push_back(creoConstraintFeature_A);	

			creoConstraintFeature_B.componentInstanceID	 =  "7";
			creoConstraintFeature_B.featureName			 =  "Plane_Front";
			creoConstraintFeature_B.featureOrientationType =   isis::ProDatumside_enum("SIDE_A");
			creoConstraintPair_1.constraintFeatures.push_back(creoConstraintFeature_B);

			creoConstraintPairs.push_back(creoConstraintPair_1);

			/*  // ZZZZ this code is not working can't find datums
			isis::ConstraintPair  creoConstraintPair_2;

			creoConstraintPair_1.featureAlignmentType = isis::ProAsmcompConstrType_enum("ALIGN");
			creoConstraintPair_1.featureGeometryType  = isis::FeatureGeometryType_enum("AXIS");
			creoConstraintPair_1.featureInterfaceType = isis::FeatureInterfaceType_enum("CAD_DATUM");
			creoConstraintFeature_A.componentInstanceID	 =  "new_comp_1";

			creoConstraintFeature_A.featureName			 =  "Axis_1";
			creoConstraintFeature_A.featureOrientationType =  isis::ProDatumside_enum("NONE");
			creoConstraintPair_2.constraintFeatures.push_back(creoConstraintFeature_A);	

			creoConstraintFeature_B.componentInstanceID	 =  "7";
			creoConstraintFeature_B.featureName			 =  "Axis_1";
			creoConstraintFeature_B.featureOrientationType =  isis::ProDatumside_enum("NONE");
			creoConstraintPair_2.constraintFeatures.push_back(creoConstraintFeature_B);
		
			creoConstraintPairs.push_back(creoConstraintPair_2);
			*/
		in_MetaLinkAssemblyEditor.ConstrainComponent("new_comp_1",creoConstraintPairs);

	}

	void Test_AddSearchPaths(const std::list<std::string> &in_SearchPaths,
							isis::MetaLinkAssemblyEditor &in_MetaLinkAssemblyEditor ) throw (isis::application_exception)
	{
		in_MetaLinkAssemblyEditor.AddSearchPaths(in_SearchPaths);
	}

}  // END namespace isis