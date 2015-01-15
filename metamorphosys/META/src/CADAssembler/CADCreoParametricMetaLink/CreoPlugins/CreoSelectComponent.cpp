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

/*--------------------------------------------------------------------*\
Pro/Toolkit includes
\*--------------------------------------------------------------------*/
#include <ProUICmd.h>

/*--------------------------------------------------------------------*\
Meta includes
\*--------------------------------------------------------------------*/
#include "CommonDefinitions.h"
#include "GlobalModelData.h"
#include "TestError.h"
#include "CreoPlugins.h"
#include "CreoExport.h"

/*--------------------------------------------------------------------*\
Application global/external data
\*--------------------------------------------------------------------*/
static wchar_t *MSGFIL = L"msg_user.txt";
static char revcode[PRO_LINE_SIZE];

ProMdl getSelectedMdl();  // defined in CreoExport.cpp

static uiCmdAccessState TestAccessAlways(uiCmdAccessMode access_mode)
{
	return ACCESS_AVAILABLE;
}

static bool CheckMode(isis::MetaLinkMode mode)
{
	if (isis::GlobalModelData::Instance.mode != mode) {
		ostringstream msg;
		msg << "This functionality is only available in "
			<< ((mode == isis::DESIGNEDIT) ? "design" : "component")
			<< " editing mode";
		ErrorDialogStr(msg.str());
		return false;
	}
	return true;
}


/**
Select (highlight) an existing component instance in Creo (Creo Select) 
causing the corresponding element to hightlight in CyPhy.

Pre-conditions:
 * MetaLink is running and CyPhy and CAD-assebler are connected
 * Creo is opened in assembly design editing mode.
   - CyPhy switches Creo to assembly design editing mode.
   - CyPhy starts CAD-assembler in assembly design editing mode.
 * There is an assembly design loaded in Creo.
Action:
 * Right mouse button select the component instance you want to "highlight".
   - choose "Highlight"
Post-condition:
 * The selected item is highlighted in the CyPhy component.
*/
ProError DoSelectComponent()
{
	isis::GlobalModelData::Instance.metalink_handler_ptr->
		send_LocateSelectedRequest(
			isis::GlobalModelData::Instance.mode,
			isis::GlobalModelData::Instance.designId);
	return PRO_TK_NO_ERROR;
}

/**
Select (highlight) an existing datum feature in Creo (Creo Select) 
causing the corresponding element to hightlight in CyPhy.

Pre-conditions:
 * MetaLink is running and CyPhy and CAD-assebler are connected
 * Creo is opened in AVM component editing mode.
   - CyPhy switches Creo to AVM component editing mode.
   - CyPhy starts CAD-assembler in component editing mode.
 * There is an AVM component model (assembly or part) loaded in Creo.
Action:
 * Right mouse button select the assembly or part you want to "highlight".
   - choose "Highlight"
Post-condition:
 * The selected item is highlighted in the CyPhy component.
*/
ProError DoSelectDatum()
{
	return PRO_TK_NOT_IMPLEMENTED;
}

ProError DoSelect() 
{
	log4cpp::Category& logcat = ::log4cpp::Category::getInstance(LOGCAT_LOGFILEONLY);
	logcat.warnStream() 
		<< "selection in mode: " << isis::GlobalModelData::Instance.mode;

	switch(isis::GlobalModelData::Instance.mode) {
	case isis::UNDEFINEDMODE:
	    logcat.warn("select/locate is undefined in current mode");
	    return PRO_TK_NOT_IMPLEMENTED;
	case isis::COMPONENTEDIT:
		return DoSelectDatum();
	case isis::DESIGNEDIT:
		return DoSelectComponent();
	default:
	    logcat.warnStream()  
			<< "select/locate is undefined in current mode "
			<< isis::GlobalModelData::Instance.mode;
	    return PRO_TK_NOT_IMPLEMENTED;
	}
}

/*====================================================================*\
FUNCTION : user_initialize()
PURPOSE  : Pro/DEVELOP standard initialize 
   Define an action and menu button for selecting a component.
\*====================================================================*/
ProError SetupCreoSelectPlugin()
{
	log4cpp::Category& logcat = ::log4cpp::Category::getInstance(LOGCAT_LOGFILEONLY);
    ProError status;
    uiCmdCmdId	cmd_id_select;

    // Add new button to the menu bar
    status = ProCmdActionAdd("IsisSelectComponent",(uiCmdCmdActFn)DoSelect, 
		uiProe2ndImmediate, TestAccessAlways,	PRO_B_TRUE, PRO_B_TRUE, &cmd_id_select);

	switch( status = ProCmdDesignate(cmd_id_select, 
		"-ISIS Select Cyphy Component", 
	    "-ISIS Select Cyphy Component Label", 
	    "-ISIS Select Cyphy Component Description", 
	    MSGFIL) ) 
	{
	case PRO_TK_NO_ERROR:
		logcat.warnStream() << "The command was designated. Use the Screen Customization dialog box to place it.";
		break;
	case PRO_TK_BAD_INPUTS:
		logcat.warnStream() << "One or more input arguments was invalid.";
		return PRO_TK_BAD_INPUTS;
	case PRO_TK_E_NOT_FOUND:
		logcat.warnStream() << "The message file was not found.";
		return PRO_TK_E_NOT_FOUND;
	case PRO_TK_MSG_NOT_FOUND:
		logcat.warnStream() << "One or more messages was not found in the message file.";
	    return PRO_TK_MSG_NOT_FOUND;
	}
	switch( status = ProCmdIconSet(cmd_id_select, "isis_select.png") ) {
	 case  PRO_TK_NO_ERROR:
		logcat.info("The icon was assigned.");
		break;
	 case PRO_TK_BAD_INPUTS:
		logcat.warn("The input arguments were invalid.");
	    return PRO_TK_BAD_INPUTS;
     case PRO_TK_E_NOT_FOUND:
		logcat.warn("The icon file was not found.");
        return PRO_TK_E_NOT_FOUND;
     case PRO_TK_INVALID_FILE:
		logcat.warn("The file specified was not a Creo Parametric .BIF file or a custom .GIF file.");
        return PRO_TK_INVALID_FILE;
	 default:
		logcat.warn("The file specified was not of required format.");
        return status;
	}


    return PRO_TK_NO_ERROR;
}
