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

#ifndef DATUM_EDITOR_ROUTINES_H
#define DATUM_EDITOR_ROUTINES_H
#include <isis_application_exception.h>
#include "UdmBase.h"
#include <CADDatumEditor.h>
#include <string>

namespace isis
{
	struct ErrorStatus
	{
		bool warningsOccurred;
		bool errorsOccurred;

		ErrorStatus(): warningsOccurred(false), errorsOccurred(false){};
	};

	std::string ErrorStatusMessage( const ErrorStatus &in_ErrorStatus);
	//int ErrorStatusCode( const ErrorStatus &in_ErrorStatus);

	enum e_DatumEditorFunction
	{
		CAD_ADD_DATUM,
		CAD_DELETE_DATUM,
		CAD_ADD_COORDINATE_SYSTEM,
		CAD_PARTS_LIBRARY
	};


//	enum e_OperationType
//	{
//		INTERNAL_OPERATION_POPULATE_LIB_MAP,
//		INTERNAL_OPERATION_ADD_DATUM,
//		INTERNAL_OPERATION_DELETE_DATUM
//	};

	class DatumEditorDiagram_functor
	{
		public:
			virtual void operator() ( e_DatumEditorFunction in_DatumEditorFunction, 
									  Udm::Object &in_UdmObject_function,
									  Udm::Object *in_UdmObject_component = NULL)=0;
	};


	class Log_DatumEditorDiagram_functor: public DatumEditorDiagram_functor
	{
		public:

			virtual void operator() ( e_DatumEditorFunction in_DatumEditorFunction, 
									  Udm::Object &in_UdmObject_function,
									  Udm::Object *in_UdmObject_component = NULL)
									  throw (isis::application_exception );
	};

	class  CheckDataValidity_DatumEditorDiagram_functor: public DatumEditorDiagram_functor
	{
		public:

			virtual void operator() ( e_DatumEditorFunction in_DatumEditorFunction, 
									  Udm::Object &in_UdmObject_function,
									  Udm::Object *in_UdmObject_component = NULL)
									  throw (isis::application_exception );
	};

	class  PopulateLibraryMap_DatumEditorDiagram_functor: public DatumEditorDiagram_functor
	{

		public:
			std::map<std::string, std::string> libID_to_DirectoryPath_map;
			//e_OperationType operationType; 

			virtual void operator() ( e_DatumEditorFunction in_DatumEditorFunction, 
									  Udm::Object &in_UdmObject_function,
									  Udm::Object *in_UdmObject_component = NULL)
									  throw (isis::application_exception );
	};

	//void TraverseXMLDocument(const std::string &in_DatumXML_PathAndFileName);
   void TraverseXMLDocument(Udm::SmartDataNetwork &in_parsedXMLDiagram,
							 DatumEditorDiagram_functor &in_DatumEditorDiagram_functor ) throw (isis::application_exception );

   void DeleteAndCreateCSysAndDatums(	Udm::SmartDataNetwork	&in_parsedXMLDiagram,
								const std::map<std::string, std::string> &in_LibID_to_DirectoryPath_map,
								const std::string		&in_StartingDirectory,
								ErrorStatus				&out_ErrorStatus)
								throw (isis::application_exception );

} // End namespace isis

#endif