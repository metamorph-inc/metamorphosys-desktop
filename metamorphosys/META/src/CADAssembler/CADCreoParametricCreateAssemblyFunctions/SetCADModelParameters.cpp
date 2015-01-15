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

#include <SetCADModelParameters.h>
#include <iostream>
#include <string>
#include <StringToEnumConversions.h>
#include <ParametricParameters.h>
#include "CommonDefinitions.h"
#include <map>
#include <log4cpp/Category.hh>
#include <log4cpp/OstreamAppender.hh>


namespace isis
{

void ApplyParametricParameters( std::list<std::string>                          &in_ComponentIDs, 
							   std::map<std::string, isis::CADComponentData>	&in_CADComponentData_map,
							   std::vector<isis::CADCreateAssemblyError>		&out_ErrorList)
																		throw (isis::application_exception)
{
	log4cpp::Category& logcat_fileonly = log4cpp::Category::getInstance(LOGCAT_LOGFILEONLY);

	for ( std::list<std::string>::const_iterator t = in_ComponentIDs.begin(); t != in_ComponentIDs.end(); ++t )
	{
		// to avoid dereferencing
		CADComponentData *cadata = &in_CADComponentData_map[*t];

		std::string ModelNameWithSuffix = AmalgamateModelNameWithSuffix ( cadata->name, cadata->modelType );
		

		if ( cadata->parametricParametersPresent )
		{
			for( std::list<CADParameter>::const_iterator p( cadata->parametricParameters.begin());
			p != cadata->parametricParameters.end();
			++ p )
			{
				logcat_fileonly.infoStream() << "Set Component Parameter: ";
				logcat_fileonly.infoStream() << "   ModelNameWithSuffix: " << ModelNameWithSuffix;
				logcat_fileonly.infoStream() << "   in_CADComponentData_map[*t].p_model: " << cadata->p_model;
				logcat_fileonly.infoStream() << "    p->name:   " <<	p->name;
				logcat_fileonly.infoStream() << "    p->type:   " <<	CADParameterType_string(p->type);
				logcat_fileonly.infoStream() << "    p->value:  " <<	p->value;
				try 
				{
					SetParametricParameter( ModelNameWithSuffix, cadata->p_model, p->name, p->type, p->value);

				} catch (isis::application_exception &ex)
				{
					out_ErrorList.push_back(CADCreateAssemblyError(ex.what(), CADCreateAssemblyError_Severity_Warning));
				}
			}
		}

		// Regen 5 times. It may not work for the 1st time
		int i = 5;
		while (true)
		{
			try{
				i--;
				isis_ProSolidRegenerate((ProSolid)cadata->modelHandle, PRO_REGEN_NO_RESOLVE_MODE);
				break;
			} catch (isis::application_exception &ex)
			{
				if (i==0)
				{
					throw isis::application_exception("C06051", "Solid regeneration failed after applying parameters: " + ModelNameWithSuffix);
				}
			}
		}
	
	
		/*clog << std::endl << "Set Assembly Parameters: ";
		clog << std::endl << "   CYPHY_COMPONENT_INSTANCE_ID: " << cadata->componentID << std::endl;

		try {
			isis::SetParametricParameter( FORCE_KEY, ModelNameWithSuffix, cadata->p_model, CYPHY_COMPONENT_INSTANCE_ID, CAD_STRING, cadata->componentID);
		} catch (isis::application_exception &ex)
		{
			out_ErrorList.push_back(CADCreateAssemblyError(ex.what(), CADCreateAssemblyError_Severity_Warning));
		}
		*/
		try {
			if (cadata->displayName.size() >= MAX_STRING_PARAMETER_LENGTH )
			{
				cadata->displayName	= cadata->displayName.substr(0, MAX_STRING_PARAMETER_LENGTH-3)+"...";
			}
			isis::SetParametricParameter( FORCE_KEY, ModelNameWithSuffix, cadata->p_model, CYPHY_NAME, CAD_STRING, cadata->displayName);
		} catch (isis::application_exception &ex)
		{
			out_ErrorList.push_back(CADCreateAssemblyError(ex.what(), CADCreateAssemblyError_Severity_Warning));
		}
	}

}


} // end namespace isis