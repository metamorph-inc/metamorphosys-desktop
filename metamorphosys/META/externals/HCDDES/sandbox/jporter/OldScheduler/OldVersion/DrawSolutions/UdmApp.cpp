// UdmApp.cpp: implementation of the CUdmApp class.
// This file was automatically generated as UdmApp.cpp
// by UDM Interpreter Wizard on Thursday, September 06, 2007 12:34:14



#include "stdafx.h"
#include "UdmApp.h"
#include "UdmConfig.h"
#include "Uml.h"

#include "ConstraintMgr.h"

/*********************************************************************************/
/* Initialization function. The framework calls it before preparing the backend. */
/* Initialize here the settings in the config global object.					  */
/* Return 0 if successful.														  */
/*********************************************************************************/
int CUdmApp::Initialize()
{

	// TODO: Your initialization code comes here...
	return 0;
}



/* 
Remarks to CUdmApp::UdmMain(...):
0.	The p_backend points to an already open backend, and the framework 
	closes it automatically. DO NOT OPEN OR CLOSE IT!
	To commit changes use p_backend->CommitEditSequence().
	To abort changes use p_backend->AbortEditSequence().
	To save changes to a different file use p_backend->SaveAs() or p_backend->CloseAs().

1.	Focus is the currently open model.

2.	The possible values for param (from GME DecoratorLib.h component_startmode_enum):
	GME_MAIN_START			=   0,
	GME_BROWSER_START		=   1,
	GME_CONTEXT_START		=   2,
	GME_EMBEDDED_START		=   3,
	GME_MENU_START			=  16,
	GME_BGCONTEXT_START		=  18,
	GME_ICON_START			=  32,
	METAMODEL_CHECK_SYNTAX	= 101

 3. The framework catches all the exceptions and reports the error in a message box,
	clean up and close the transactions aborting the changes. You can override this 
	behavior by catching udm_exception. Use udm_exception::what() to form an error 
	message.
*/

/***********************************************/
/* Main entry point for Udm-based Interpreter  */
/***********************************************/

void CUdmApp::UdmMain(
					 Udm::DataNetwork* p_backend,		// Backend pointer(already open!)
					 Udm::Object focusObject,			// Focus object
					 set<Udm::Object> selectedObjects,	// Selected objects
					 long param)						// Parameters
{	

	// TODO: Your Code Comes Here...

/************************** Demo Code - Delete It *********************/
	// Welcome
	AfxMessageBox("Hello Udm-based Interpreter World!");

#ifdef _DEBUG
	// Displaying the name of the root object meta
	Udm::Object rootObject=p_backend->GetRootObject();
	string rootObjectName("Root Object's Class Name: ");
	rootObjectName+=ExtractName(rootObject.type());
	AfxMessageBox(rootObjectName.c_str());

	// Displaying the focus object
	string focusObjectName("Focus Object Name: ");
	focusObjectName+=ExtractName(focusObject);
	AfxMessageBox(focusObjectName.c_str());

	set<Udm::Object>::iterator i = selectedObjects.begin();
	//Udm::ObjectImpl oparent( sln.parent() );

	if ( i != selectedObjects.end() )
	{
		Solution sln( i->__impl()->clone() ); //(i->__impl()->clone()) ); // Try to cast
		ConstraintProblem cp( sln.ConstraintProblem_parent() );
		ConstraintMgr cm( cp ); // Build from existing object, with solution update	

		// Displaying selected objects
		string selObjNames("Selected Objects:\r\n");
		// Iterate set
		for(i=selectedObjects.begin(); i!=selectedObjects.end();i++)
		{
			selObjNames+=ExtractName(*i);
			selObjNames+="\r\n";

			Solution s( i->__impl()->clone() ); // Try to cast
			if ( s )
			{
				cm.DrawSchedule( std::string("gmedraw"), s.name() );
			}
		}
		AfxMessageBox(selObjNames.c_str());	
	}

#endif
/****************************** Demo Code End *************************/
													
}



#ifdef _DEBUG
/*****************************************************/
/* Debug time helper function. If the object has an  */
/* attribute called "name", this function retreives  */
/* it to help you to find it in the model during the */
/* application development.	Usualy every GME Object	 */
/* has a "name" attribute. If an object hapens not	 */
/* to have it,function retreives <no name specified>.*/
/*****************************************************/
string CUdmApp::ExtractName(Udm::Object ob)
{
	Uml::Class cls= ob.type();				
	set<Uml::Attribute> attrs=cls.attributes();		
	
	// Adding parent attributes
	set<Uml::Attribute> aattrs=Uml::AncestorAttributes(cls);
	attrs.insert(aattrs.begin(),aattrs.end());

	for(set<Uml::Attribute>::iterator ai = attrs.begin();ai != attrs.end(); ai++) 
	{
		if(string(ai->type())=="String")
		{
			string str=ai->name();
			if(str=="name")
			{
				string value=ob.getStringAttr(*ai);
				if(value.empty())value="<empty string>";
				return value;
			}			
		}				
	}	
	return string("<no name specified>");
}
#endif