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

/*--------------------------------------------------------------------*\
Pro/Toolkit includes
\*--------------------------------------------------------------------*/
#include "stdafx.h"
#include <ProUITree.h>
#include <ProUICmd.h>
#include <ProMdl.h>
#include <ProArray.h>
#include <ProModelitem.h>
#include <ProUIDialog.h>
#include <ProUIPushbutton.h>

#include <stdlib.h>
#include <memory.h>
#include <ProSelbuffer.h>
#include <ProPopupmenu.h>
#include "CreoExport.h"
#include "ProRibbon.h"
#include "CommonDefinitions.h"
#include "CommonStructures.h"
#include "CreoExport.h"

// the windows.h include needs to come after the boost/asio.hpp 
#include <windows.h>

/*--------------------------------------------------------------------*\
Application includes
\*--------------------------------------------------------------------*/
#include "TestError.h"

/*--------------------------------------------------------------------*\
Application global/external data
\*--------------------------------------------------------------------*/
static wchar_t *MSGFIL = L"msg_user.txt";
static char revcode[PRO_LINE_SIZE];

ProError DoUpdateComponent();
ProError AddDialogCreate();
ProMdl getSelectedMdl();
bool CheckMode(isis::MetaLinkMode mode);

static uiCmdAccessState TestAccessAlways(uiCmdAccessMode access_mode)
{
	return ACCESS_AVAILABLE;
}


/*====================================================================*\
FUNCTION : user_initialize()
PURPOSE  : Pro/DEVELOP standard initialize - define menu button
\*====================================================================*/
ProError SetupCreoPlugins()
{
    ProError status;
    uiCmdCmdId	cmd_id_update;
	uiCmdCmdId	cmd_id_refresh;
	uiCmdCmdId	cmd_id_add;
	uiCmdCmdId	cmd_id_create;
	uiCmdCmdId	cmd_id_connect;
	uiCmdCmdId	cmd_id_remove;
	uiCmdCmdId	cmd_id_createconn;
	uiCmdCmdId	cmd_id_createanalysispoints;
	uiCmdCmdId	cmd_id_dumpposition;
	uiCmdCmdId	cmd_id_datumresolve;

    //status = ProUITranslationFilesEnable();
	//if (status != PRO_TK_NO_ERROR) return status;

/*---------------------------------------------------------------------*\
    Add new button to the menu bar
\*---------------------------------------------------------------------*/
    //ProTKSprintf(revcode, "%s - %s", version, build);

	// Update (Export)
    status = ProCmdActionAdd("ISISExportComponent",(uiCmdCmdActFn)DoUpdateComponent,uiProe2ndImmediate, TestAccessAlways,	PRO_B_TRUE, PRO_B_TRUE, &cmd_id_update);
	status = ProCmdDesignate(cmd_id_update, "-ISIS Export Cyphy Component", "-ISIS Export Cyphy Component Label", "-ISIS Export Cyphy Component Description", MSGFIL);
	ProCmdIconSet(cmd_id_update, "isis_update_component.png");
	if (status != PRO_TK_NO_ERROR) return status;

	// Refresh
	status = ProCmdActionAdd("ISISRefresh",(uiCmdCmdActFn)DoResync,uiProe2ndImmediate, TestAccessAlways,	PRO_B_TRUE, PRO_B_TRUE, &cmd_id_refresh);
	status = ProCmdDesignate(cmd_id_refresh, "-ISIS Refresh", "-ISIS Refresh Label", "-ISIS Refresh Description", MSGFIL);
	ProCmdIconSet(cmd_id_refresh, "isis_refresh.png");
	if (status != PRO_TK_NO_ERROR) return status;

	// Add Component
	status = ProCmdActionAdd("ISISAddComponent", (uiCmdCmdActFn)DoAddComponent, uiProe2ndImmediate, TestAccessAlways, PRO_B_TRUE, PRO_B_TRUE, &cmd_id_add);
	status = ProCmdDesignate(cmd_id_add, "-ISIS Add Cyphy Component", "-ISIS Add Cyphy Component Label", "-ISIS Add Cyphy Component Description", MSGFIL);
	ProCmdIconSet(cmd_id_add, "isis_add_component.png");
	if (status != PRO_TK_NO_ERROR) return status;

	// Create Component
	status = ProCmdActionAdd("ISISCreateComponent", (uiCmdCmdActFn)DoCreateComponent, uiProe2ndImmediate, TestAccessAlways, PRO_B_TRUE, PRO_B_TRUE, &cmd_id_create);
	status = ProCmdDesignate(cmd_id_create, "-ISIS Create New Component in Cyphy", "-ISIS Create New Component in Cyphy Label", "-ISIS Create New Component in Cyphy Description", MSGFIL);
	ProCmdIconSet(cmd_id_create, "isis_create_component.png");
	if (status != PRO_TK_NO_ERROR) return status;

	// Connect Components
	status = ProCmdActionAdd("ISISConnectComponent", (uiCmdCmdActFn)DoConnectComponents, uiProe2ndImmediate, TestAccessAlways, PRO_B_TRUE, PRO_B_TRUE, &cmd_id_connect);
	status = ProCmdDesignate(cmd_id_connect, "-ISIS Connect Cyphy Component", "-ISIS Connect Cyphy Component Label", "-ISIS Connect Cyphy Component Description", MSGFIL);
	ProCmdIconSet(cmd_id_connect, "isis_connect.png");
	if (status != PRO_TK_NO_ERROR) return status;

	// Remove Component(s)
	status = ProCmdActionAdd("ISISRemoveComponent", (uiCmdCmdActFn)DoRemoveComponent, uiProe2ndImmediate, TestAccessAlways, PRO_B_TRUE, PRO_B_TRUE, &cmd_id_remove);
	status = ProCmdDesignate(cmd_id_remove, "-ISIS Remove Cyphy Component", "-ISIS Remove Cyphy Component Label", "-ISIS Remove Cyphy Component Description", MSGFIL);
	ProCmdIconSet(cmd_id_remove, "isis_remove_component.png");
	if (status != PRO_TK_NO_ERROR) return status;

	// Create Connector
	status = ProCmdActionAdd("ISISCreateConnector", (uiCmdCmdActFn)DoCreateConnector, uiProe2ndImmediate, TestAccessAlways, PRO_B_TRUE, PRO_B_TRUE, &cmd_id_createconn);
	status = ProCmdDesignate(cmd_id_createconn, "-ISIS Create Cyphy Connector", "-ISIS Create Cyphy Connector Label", "-ISIS Create Cyphy Connector Description", MSGFIL);
	ProCmdIconSet(cmd_id_createconn, "isis_create_connector.png");
	if (status != PRO_TK_NO_ERROR) return status;

	// Create Analysis Points
	status = ProCmdActionAdd("ISISCreateAnalysisPoints", (uiCmdCmdActFn)DoCreateAnalysisPoints, uiProe2ndImmediate, TestAccessAlways, PRO_B_TRUE, PRO_B_TRUE, &cmd_id_createanalysispoints);
	status = ProCmdDesignate(cmd_id_createanalysispoints, "-ISIS Create Analysis Point", "-ISIS Create Analysis Point Label", "-ISIS Create Analysis Point Description", MSGFIL);
	ProCmdIconSet(cmd_id_createanalysispoints, "isis_analysispoint.png");
	if (status != PRO_TK_NO_ERROR) return status;

	// Dump Position Matrices
	status = ProCmdActionAdd("ISISDumpPosition", (uiCmdCmdActFn)DoDumpPosition, uiProe2ndImmediate, TestAccessAlways, PRO_B_TRUE, PRO_B_TRUE, &cmd_id_dumpposition);
	status = ProCmdDesignate(cmd_id_dumpposition, "-ISIS Dump Position", "-ISIS Dump Position Label", "-ISIS Dump Position Description", MSGFIL);
	if (status != PRO_TK_NO_ERROR) return status;

	// Test datum resolution
	status = ProCmdActionAdd("ISISDatumResolve", (uiCmdCmdActFn)DoDatumResolve, uiProe2ndImmediate, TestAccessAlways, PRO_B_TRUE, PRO_B_TRUE, &cmd_id_datumresolve);
	status = ProCmdDesignate(cmd_id_datumresolve, "-ISIS Datum Resolve", "-ISIS Datum Resolve Label", "-ISIS Datum Resolve Description", MSGFIL);
	if (status != PRO_TK_NO_ERROR) return status;

	// Load ribbon
	status = ProRibbonDefinitionfileLoad(L"toolkitribbonui_asm.rbn");
	if (status != PRO_TK_NO_ERROR) return status;
	status = ProRibbonDefinitionfileLoad(L"toolkitribbonui_part.rbn");
	if (status != PRO_TK_NO_ERROR) return status;
	status = ProRibbonDefinitionfileLoad(L"toolkitribbonui_sheetm.rbn");
	if (status != PRO_TK_NO_ERROR) return status;

    /* Upon success */
    return PRO_TK_NO_ERROR;
}

/**
Create a new AVM component in Creo (Creo Create).

Pre-conditions:
 * MetaLink is running and CyPhy and CAD-assebler are connected
 * Creo is opened in AVM component editing mode.
   - CyPhy switches Creo to AVM component editing mode.
   - CyPhy starts CAD-assembler in component editing mode.
 * There is no model (assembly or part) loaded in Creo.
Action:
 * Create a Creo assembly or part.
 * Select the assembly or part you want to "create", make into an AVM component.
 * Press the Create Component button.
 * Enter the AVM component name.
Post-condition:
 * The updates are reflected in the CyPhy component.
   - CyPhy will show the added component in its tree.
*/
ProError DoCreateComponent()
{
	CreateNewComponent(getSelectedMdl());
	return PRO_TK_NO_ERROR;
}

/**
Refresh component instance names in Creo (Creo Refresh).

Pre-conditions:
 * MetaLink is running and CyPhy and CAD-assembler are connected
 * Creo is opened in assembly design editing mode.
   - CyPhy switches Creo to assembly design editing mode.
   - CyPhy starts CAD-assembler in assembly design editing mode.
 * There is an assembly design loaded in Creo.
Action:
 * Press the component name Refresh button.
Post-condition:
 * The names are displayed in the tree browser to the right side of the components.
*/
ProError DoResync()
{
	ProMdl mainMdl;
	ProMdlCurrentGet(&mainMdl);
	Resync(mainMdl);
	return PRO_TK_NO_ERROR;
}

/**
Update Component modifications from Creo to CyPhy (Creo Update (Export)).

Pre-conditions:
 * MetaLink is running and CyPhy and CAD-assebler are connected
 * Creo is opened in AVM component editing mode.
   - CyPhy switches Creo to AVM component editing mode.
   - CyPhy starts CAD-assembler in component editing mode.
Action:
 * Perform AVM component edits in Creo
   - add a datum
   - modify a parameter
 * Press the Update Cyphy Component button.
Post-condition:
 * The updates are reflected in the CyPhy component.

*/
ProError DoUpdateComponent()
{
	ProError status = PRO_TK_NO_ERROR; 
	ProMdlType type;
	wchar_t msg[1000];
	ProMdl mdl;
	*msg = 0;

	if (isis::GlobalModelData::Instance.mode == isis::COMPONENTEDIT)
	{
		ProMdlCurrentGet(&mdl);
	} else {
		mdl = getSelectedMdl();
	}

	if (!mdl)
	{
		ErrorDialog(L"No model has been selected.");
		return status;
	}
	ProMdlTypeGet(mdl, &type);
	if (type != PRO_MDL_PART && type != PRO_MDL_ASSEMBLY)
	{
		ErrorDialog(L"Selected model has to be either part or assembly.");
		return status;
	}

	UpdateComponent(mdl);

	return status;
}




