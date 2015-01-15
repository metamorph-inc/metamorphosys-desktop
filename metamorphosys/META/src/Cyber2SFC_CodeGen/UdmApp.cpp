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

#include "stdafx.h"
#include "UdmApp.h"
#include "UdmConfig.h"
#include "Uml.h"
#include "UdmUtil.h"

#include "runSF_CodeGen.h"

using namespace std;

CString CUdmApp::mgaPath = "";

void showUsage()
{
	CString usage("Cyber2SFC_CodeGen interpreter cannot be invoked. Please launch the interpreter inside a SimulinkWrapper model.\r\n");
	AfxMessageBox(usage,MB_ICONINFORMATION);				
}

/*********************************************************************************/
/* Initialization function. The framework calls it before preparing the backend. */
/* Initialize here the settings in the config global object.					 */
/* Return 0 if successful.														 */
/*********************************************************************************/
int CUdmApp::Initialize()
{


	// TODO: Your initialization code comes here...
	return 0;
}

void CUdmApp::UdmMain(
					 Udm::DataNetwork* p_backend,		// Backend pointer(already open!)
					 Udm::Object focusObject,			// Focus object
					 std::set<Udm::Object> selectedObjects,	// Selected objects
					 long param)						// Parameters
{	
	if(focusObject==Udm::null)
	{
		showUsage();	
		return;
	}

	if(!selectedObjects.empty())
	{
		for(std::set<Udm::Object>::iterator it=selectedObjects.begin();it!=selectedObjects.end();++it)
		{
			Udm::Object selectObj = *it;
			if(Uml::IsDerivedFrom(selectObj.type(), CyberComposition::Simulink::Subsystem::meta))
			{
				CyberComposition::Simulink::Subsystem sys = CyberComposition::Simulink::Subsystem::Cast(selectObj);
				runCyberSF_CodeGen(sys, (LPCTSTR)mgaPath, p_backend);
			}
			//else if(Uml::IsDerivedFrom(selectObj.type(), CyberComposition::SimulinkWrapper::meta))
			//{
			//	CyberComposition::SimulinkWrapper slmodel = CyberComposition::SimulinkWrapper::Cast(selectObj);
			//	runCyberSF_CodeGen(slmodel, (LPCTSTR)mgaPath, p_backend);
			//}
		}
	}
	else if(Uml::IsDerivedFrom(focusObject.type(), CyberComposition::SimulinkWrapper::meta))
	{
		CyberComposition::SimulinkWrapper slmodel = CyberComposition::SimulinkWrapper::Cast(focusObject);
		runCyberSF_CodeGen(slmodel, (LPCTSTR)mgaPath, p_backend);
	}
	else
	{
		showUsage();	
		return;
	}

	//AfxMessageBox("SF_Code has been generated for the SimulinkWrapper model. ",MB_ICONINFORMATION);
	//GMEConsole::Console::Info::writeLine("SF_Code has been generated for the SimulinkWrapper model.");
}

