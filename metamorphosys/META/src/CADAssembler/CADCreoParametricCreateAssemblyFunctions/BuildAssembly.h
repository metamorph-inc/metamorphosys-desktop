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

#ifndef BUILD_ASSEMBLY_H
#define BUILD_ASSEMBLY_H
//#include <AssemblyInterface.hxx>
#include <isis_application_exception.h>
#include <CommonStructures.h>
#include <AssembleUtils.h>
#include <map>
#include "CadFactoryAbstract.h"

//using namespace std;

namespace isis
{


void BuildAssembly( 
		cad::CadFactoryAbstract								&in_factory,
		const std::string									&in_AssemblyComponentID, 
		const std::string									&in_WORKING_DIR,
		bool												in_SaveAssembly,
		std::map<std::string, isis::CADComponentData>		&in_CADComponentData_map,
		bool												&out_RegenerationSucceeded,
		std::vector<isis::CADCreateAssemblyError>			&out_ErrorList,
		bool												in_AllowUnconstrainedModels = false)
					throw (isis::application_exception);

//void CopyModels(const std::map<std::string, std::string>	  &in_ToPartName_FromPartName )
//																	throw (isis::application_exception);
void CopyModels(const std::vector<CopyModelDefinition>	&in_FromModel_ToModel )
																	throw (isis::application_exception);

void ReadInitialPositions(std::map<string, double*> &out_positions, const std::string &in_filename);


void Add_Subassemblies_and_Parts( 
		cad::CadFactoryAbstract				&	in_factory,
		ProMdl								in_p_asm,
		const std::string					&	in_ParentName,
		const std::list<std::string>		&	in_Components,
		std::map<string, isis::CADComponentData>	&in_CADComponentData_map,
		int									&in_out_addedToAssemblyOrdinal)
					throw (isis::application_exception);

void	RegenerateModel( ProSolid in_p_asm,
						 const std::string in_ParentName,
						 const std::string in_ParentComponentID,
						 bool  &out_RegenerationSucceeded,
						 bool  in_PresentDetailedErrorMessage = false)
												throw (isis::application_exception);

} // end namespace isis


#endif // ASSEMBLY_TREE_PARSER_H