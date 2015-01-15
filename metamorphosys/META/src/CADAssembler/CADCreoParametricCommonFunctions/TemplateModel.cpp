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


#include <TemplateModel.h>
#include <log4cpp/Category.hh>
#include "CommonDefinitions.h"
#include <ProMdlUnits.h>
#include <ProUtil.h>
#include <ProWstring.h>

namespace isis {
namespace creo {
namespace model {

bool make_solid_templated( ProSolid& in_original, ProSolid& out_template )
{
	ProError rc;
	char pro_str[128];
	log4cpp::Category& log_cf = log4cpp::Category::getInstance(LOGCAT_CONSOLEANDLOGFILE);
	
	ProUnitsystem original_system;
	switch( rc = ProMdlPrincipalunitsystemGet(in_original, &original_system) ) {
	case PRO_TK_NO_ERROR: break;
	case PRO_TK_BAD_INPUTS:
		log_cf.errorStream() 
			<< "failed getting the principal unit-system.";
		break;
	default:
		log_cf.errorStream() 
			<< "could not aquire the unit system = " << rc;
		return false;
	}
	/*
	ProUnititem* original_mass_unit;
	switch( rc = ProMdlUnitsCollect( in_original, 
		PRO_UNITTYPE_MASS, &original_mass_unit) ) {
	case PRO_TK_NO_ERROR: break;
	default:
		log_cf.errorStream() << "could not collect unit systems = " << rc;
		return false;
	}
	*/

	ProUnitsystem* template_systems;
	//ProUnititem template_units;
	switch( rc = ProMdlUnitsystemsCollect( out_template, &template_systems) ) {
	case PRO_TK_NO_ERROR: break;
	default:
		log_cf.errorStream() << "could not collect unit systems = " << rc;
		return false;
	}
	ProUnitsystem template_system;
	int size_template_systems;
	ProArraySizeGet( template_systems, &size_template_systems );
	for( int ix=0; ix < size_template_systems; ++ix ) {
		int names_matched;
		ProWstringCompare(template_systems[ix].name,
			original_system.name, PRO_VALUE_UNUSED, &names_matched);
		if (names_matched != 0) continue;
		template_system = template_systems[ix];
		break;
	}

	switch( rc = ProMdlPrincipalunitsystemSet(out_template, 
		&template_system, PRO_UNITCONVERT_SAME_SIZE, PRO_B_TRUE,
		PRO_VALUE_UNUSED) ) {
	case PRO_TK_NO_ERROR: break;
	case PRO_TK_BAD_INPUTS:
		log_cf.errorStream() 
			<< "could not set the units in the shrinkwrap : "
			<< ProWstringToString(pro_str, template_system.name);
		break;
	default:
		log_cf.errorStream() 
			<< "could not set units in shinkwrap = "
			<< rc;
		return false;
	}
	return true;
}

} // model
} // creo
} // isis