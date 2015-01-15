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

#ifndef CADDatumEditor_xsd_H
#define CADDatumEditor_xsd_H
#include <string>
#pragma warning( disable : 4010)

namespace CADDatumEditor_xsd
{
const std::string& getString()
{
	static std::string str;
	if (str.empty())
	{
		str +="<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
		str +="<?udm interface=\"CADDatumEditor\" version=\"1.00\"?>\n";
		str +="<xsd:schema xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"\n";
		str +=" elementFormDefault=\"qualified\" \n";
		str +=">\n";
//		str +="<!-- generated on Tue Dec 11 11:41:38 2012 -->\n";
		str +="\n";
		str +="\n";
		str +="	<xsd:complexType name=\"CADComponentsType\">\n";
		str +="		<xsd:sequence>\n";
		str +="			<xsd:element name=\"CADComponent\" type=\"CADComponentType\" minOccurs=\"0\" maxOccurs=\"unbounded\"/>\n";
		str +="		</xsd:sequence>\n";
		str +="		<xsd:attribute name=\"_id\" type=\"xsd:ID\"/>\n";
		str +="		<xsd:attribute name=\"_archetype\" type=\"xsd:IDREF\"/>\n";
		str +="		<xsd:attribute name=\"_derived\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_instances\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_desynched_atts\" type=\"xsd:string\"/>\n";
		str +="		<xsd:attribute name=\"_real_archetype\" type=\"xsd:boolean\"/>\n";
		str +="		<xsd:attribute name=\"_subtype\" type=\"xsd:boolean\"/>\n";
		str +="	</xsd:complexType>\n";
		str +="\n";
		str +="	<xsd:complexType name=\"LibrariesType\">\n";
		str +="		<xsd:sequence>\n";
		str +="			<xsd:element name=\"Library\" type=\"LibraryType\" minOccurs=\"0\" maxOccurs=\"unbounded\"/>\n";
		str +="		</xsd:sequence>\n";
		str +="		<xsd:attribute name=\"_id\" type=\"xsd:ID\"/>\n";
		str +="		<xsd:attribute name=\"_archetype\" type=\"xsd:IDREF\"/>\n";
		str +="		<xsd:attribute name=\"_derived\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_instances\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_desynched_atts\" type=\"xsd:string\"/>\n";
		str +="		<xsd:attribute name=\"_real_archetype\" type=\"xsd:boolean\"/>\n";
		str +="		<xsd:attribute name=\"_subtype\" type=\"xsd:boolean\"/>\n";
		str +="	</xsd:complexType>\n";
		str +="\n";
		str +="	<xsd:complexType name=\"CADComponentType\">\n";
		str +="		<xsd:sequence>\n";
		str +="			<xsd:element name=\"Add\" type=\"AddType\" minOccurs=\"0\"/>\n";
		str +="			<xsd:element name=\"Delete\" type=\"DeleteType\" minOccurs=\"0\"/>\n";
		str +="		</xsd:sequence>\n";
		str +="		<xsd:attribute name=\"Name\" type=\"xsd:string\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"Type\" type=\"xsd:string\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"LibraryID\" type=\"xsd:string\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"Format\" type=\"xsd:string\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"File\" type=\"xsd:string\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"_id\" type=\"xsd:ID\"/>\n";
		str +="		<xsd:attribute name=\"_archetype\" type=\"xsd:IDREF\"/>\n";
		str +="		<xsd:attribute name=\"_derived\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_instances\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_desynched_atts\" type=\"xsd:string\"/>\n";
		str +="		<xsd:attribute name=\"_real_archetype\" type=\"xsd:boolean\"/>\n";
		str +="		<xsd:attribute name=\"_subtype\" type=\"xsd:boolean\"/>\n";
		str +="	</xsd:complexType>\n";
		str +="\n";
		str +="	<xsd:complexType name=\"LibraryType\">\n";
		str +="		<xsd:attribute name=\"ID\" type=\"xsd:string\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"DirectoryPath\" type=\"xsd:string\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"_id\" type=\"xsd:ID\"/>\n";
		str +="		<xsd:attribute name=\"_archetype\" type=\"xsd:IDREF\"/>\n";
		str +="		<xsd:attribute name=\"_derived\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_instances\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_desynched_atts\" type=\"xsd:string\"/>\n";
		str +="		<xsd:attribute name=\"_real_archetype\" type=\"xsd:boolean\"/>\n";
		str +="		<xsd:attribute name=\"_subtype\" type=\"xsd:boolean\"/>\n";
		str +="	</xsd:complexType>\n";
		str +="\n";
		str +="	<xsd:complexType name=\"DeleteType\">\n";
		str +="		<xsd:sequence>\n";
		str +="			<xsd:element name=\"DeleteDatums\" type=\"DeleteDatumsType\" minOccurs=\"0\"/>\n";
		str +="		</xsd:sequence>\n";
		str +="		<xsd:attribute name=\"_id\" type=\"xsd:ID\"/>\n";
		str +="		<xsd:attribute name=\"_archetype\" type=\"xsd:IDREF\"/>\n";
		str +="		<xsd:attribute name=\"_derived\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_instances\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_desynched_atts\" type=\"xsd:string\"/>\n";
		str +="		<xsd:attribute name=\"_real_archetype\" type=\"xsd:boolean\"/>\n";
		str +="		<xsd:attribute name=\"_subtype\" type=\"xsd:boolean\"/>\n";
		str +="	</xsd:complexType>\n";
		str +="\n";
		str +="	<xsd:complexType name=\"AddType\">\n";
		str +="		<xsd:sequence>\n";
		str +="			<xsd:element name=\"AddCoordinateSystems\" type=\"AddCoordinateSystemsType\" minOccurs=\"0\"/>\n";
		str +="			<xsd:element name=\"AddDatums\" type=\"AddDatumsType\" minOccurs=\"0\"/>\n";
		str +="		</xsd:sequence>\n";
		str +="		<xsd:attribute name=\"_id\" type=\"xsd:ID\"/>\n";
		str +="		<xsd:attribute name=\"_archetype\" type=\"xsd:IDREF\"/>\n";
		str +="		<xsd:attribute name=\"_derived\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_instances\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_desynched_atts\" type=\"xsd:string\"/>\n";
		str +="		<xsd:attribute name=\"_real_archetype\" type=\"xsd:boolean\"/>\n";
		str +="		<xsd:attribute name=\"_subtype\" type=\"xsd:boolean\"/>\n";
		str +="	</xsd:complexType>\n";
		str +="\n";
		str +="	<xsd:complexType name=\"DeleteDatumType\">\n";
		str +="		<xsd:attribute name=\"DatumName\" type=\"xsd:string\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"DatumType\" type=\"xsd:string\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"_id\" type=\"xsd:ID\"/>\n";
		str +="		<xsd:attribute name=\"_archetype\" type=\"xsd:IDREF\"/>\n";
		str +="		<xsd:attribute name=\"_derived\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_instances\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_desynched_atts\" type=\"xsd:string\"/>\n";
		str +="		<xsd:attribute name=\"_real_archetype\" type=\"xsd:boolean\"/>\n";
		str +="		<xsd:attribute name=\"_subtype\" type=\"xsd:boolean\"/>\n";
		str +="	</xsd:complexType>\n";
		str +="\n";
		str +="	<xsd:complexType name=\"AddDatumType\">\n";
		str +="		<xsd:attribute name=\"DatumName\" type=\"xsd:string\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"DatumType\" type=\"xsd:string\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"ReplaceIfExists\" type=\"xsd:string\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"CoordinateSystemName\" type=\"xsd:string\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"CoordinateAlignment\" type=\"xsd:string\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"FlipDatumPlaneDirection\" type=\"xsd:string\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"_id\" type=\"xsd:ID\"/>\n";
		str +="		<xsd:attribute name=\"_archetype\" type=\"xsd:IDREF\"/>\n";
		str +="		<xsd:attribute name=\"_derived\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_instances\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_desynched_atts\" type=\"xsd:string\"/>\n";
		str +="		<xsd:attribute name=\"_real_archetype\" type=\"xsd:boolean\"/>\n";
		str +="		<xsd:attribute name=\"_subtype\" type=\"xsd:boolean\"/>\n";
		str +="	</xsd:complexType>\n";
		str +="\n";
		str +="	<xsd:complexType name=\"CADDatumEditorType\">\n";
		str +="		<xsd:sequence>\n";
		str +="			<xsd:element name=\"CADComponents\" type=\"CADComponentsType\" maxOccurs=\"unbounded\"/>\n";
		str +="			<xsd:element name=\"Libraries\" type=\"LibrariesType\" maxOccurs=\"unbounded\"/>\n";
		str +="			<xsd:element name=\"CADDatumEditor\" type=\"CADDatumEditorType\" minOccurs=\"0\" maxOccurs=\"unbounded\"/>\n";
		str +="		</xsd:sequence>\n";
		str +="		<xsd:attribute name=\"_id\" type=\"xsd:ID\"/>\n";
		str +="		<xsd:attribute name=\"_archetype\" type=\"xsd:IDREF\"/>\n";
		str +="		<xsd:attribute name=\"_derived\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_instances\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_desynched_atts\" type=\"xsd:string\"/>\n";
		str +="		<xsd:attribute name=\"_real_archetype\" type=\"xsd:boolean\"/>\n";
		str +="		<xsd:attribute name=\"_subtype\" type=\"xsd:boolean\"/>\n";
		str +="		<xsd:attribute name=\"_libname\" type=\"xsd:string\"/>\n";
		str +="	</xsd:complexType>\n";
		str +="\n";
		str +="	<xsd:complexType name=\"OriginType\">\n";
		str +="		<xsd:attribute name=\"X\" type=\"xsd:double\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"Y\" type=\"xsd:double\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"Z\" type=\"xsd:double\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"_id\" type=\"xsd:ID\"/>\n";
		str +="		<xsd:attribute name=\"_archetype\" type=\"xsd:IDREF\"/>\n";
		str +="		<xsd:attribute name=\"_derived\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_instances\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_desynched_atts\" type=\"xsd:string\"/>\n";
		str +="		<xsd:attribute name=\"_real_archetype\" type=\"xsd:boolean\"/>\n";
		str +="		<xsd:attribute name=\"_subtype\" type=\"xsd:boolean\"/>\n";
		str +="	</xsd:complexType>\n";
		str +="\n";
		str +="	<xsd:complexType name=\"AddCoordinateSystemsType\">\n";
		str +="		<xsd:sequence>\n";
		str +="			<xsd:element name=\"AddCoordinateSystem\" type=\"AddCoordinateSystemType\" maxOccurs=\"unbounded\"/>\n";
		str +="		</xsd:sequence>\n";
		str +="		<xsd:attribute name=\"_id\" type=\"xsd:ID\"/>\n";
		str +="		<xsd:attribute name=\"_archetype\" type=\"xsd:IDREF\"/>\n";
		str +="		<xsd:attribute name=\"_derived\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_instances\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_desynched_atts\" type=\"xsd:string\"/>\n";
		str +="		<xsd:attribute name=\"_real_archetype\" type=\"xsd:boolean\"/>\n";
		str +="		<xsd:attribute name=\"_subtype\" type=\"xsd:boolean\"/>\n";
		str +="	</xsd:complexType>\n";
		str +="\n";
		str +="	<xsd:complexType name=\"AddCoordinateSystemType\">\n";
		str +="		<xsd:sequence>\n";
		str +="			<xsd:element name=\"Origin\" type=\"OriginType\"/>\n";
		str +="			<xsd:element name=\"XVector\" type=\"XVectorType\"/>\n";
		str +="			<xsd:element name=\"YVector\" type=\"YVectorType\"/>\n";
		str +="		</xsd:sequence>\n";
		str +="		<xsd:attribute name=\"CoordinateSystemName\" type=\"xsd:string\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"ReplaceIfExists\" type=\"xsd:string\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"_id\" type=\"xsd:ID\"/>\n";
		str +="		<xsd:attribute name=\"_archetype\" type=\"xsd:IDREF\"/>\n";
		str +="		<xsd:attribute name=\"_derived\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_instances\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_desynched_atts\" type=\"xsd:string\"/>\n";
		str +="		<xsd:attribute name=\"_real_archetype\" type=\"xsd:boolean\"/>\n";
		str +="		<xsd:attribute name=\"_subtype\" type=\"xsd:boolean\"/>\n";
		str +="	</xsd:complexType>\n";
		str +="\n";
		str +="	<xsd:complexType name=\"DeleteDatumsType\">\n";
		str +="		<xsd:sequence>\n";
		str +="			<xsd:element name=\"DeleteDatum\" type=\"DeleteDatumType\" maxOccurs=\"unbounded\"/>\n";
		str +="		</xsd:sequence>\n";
		str +="		<xsd:attribute name=\"_id\" type=\"xsd:ID\"/>\n";
		str +="		<xsd:attribute name=\"_archetype\" type=\"xsd:IDREF\"/>\n";
		str +="		<xsd:attribute name=\"_derived\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_instances\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_desynched_atts\" type=\"xsd:string\"/>\n";
		str +="		<xsd:attribute name=\"_real_archetype\" type=\"xsd:boolean\"/>\n";
		str +="		<xsd:attribute name=\"_subtype\" type=\"xsd:boolean\"/>\n";
		str +="	</xsd:complexType>\n";
		str +="\n";
		str +="	<xsd:complexType name=\"AddDatumsType\">\n";
		str +="		<xsd:sequence>\n";
		str +="			<xsd:element name=\"AddDatum\" type=\"AddDatumType\" maxOccurs=\"unbounded\"/>\n";
		str +="		</xsd:sequence>\n";
		str +="		<xsd:attribute name=\"_id\" type=\"xsd:ID\"/>\n";
		str +="		<xsd:attribute name=\"_archetype\" type=\"xsd:IDREF\"/>\n";
		str +="		<xsd:attribute name=\"_derived\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_instances\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_desynched_atts\" type=\"xsd:string\"/>\n";
		str +="		<xsd:attribute name=\"_real_archetype\" type=\"xsd:boolean\"/>\n";
		str +="		<xsd:attribute name=\"_subtype\" type=\"xsd:boolean\"/>\n";
		str +="	</xsd:complexType>\n";
		str +="\n";
		str +="	<xsd:complexType name=\"XVectorType\">\n";
		str +="		<xsd:attribute name=\"X\" type=\"xsd:double\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"Y\" type=\"xsd:double\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"Z\" type=\"xsd:double\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"_id\" type=\"xsd:ID\"/>\n";
		str +="		<xsd:attribute name=\"_archetype\" type=\"xsd:IDREF\"/>\n";
		str +="		<xsd:attribute name=\"_derived\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_instances\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_desynched_atts\" type=\"xsd:string\"/>\n";
		str +="		<xsd:attribute name=\"_real_archetype\" type=\"xsd:boolean\"/>\n";
		str +="		<xsd:attribute name=\"_subtype\" type=\"xsd:boolean\"/>\n";
		str +="	</xsd:complexType>\n";
		str +="\n";
		str +="	<xsd:complexType name=\"YVectorType\">\n";
		str +="		<xsd:attribute name=\"X\" type=\"xsd:double\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"Y\" type=\"xsd:double\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"Z\" type=\"xsd:double\" use=\"required\"/>\n";
		str +="		<xsd:attribute name=\"_id\" type=\"xsd:ID\"/>\n";
		str +="		<xsd:attribute name=\"_archetype\" type=\"xsd:IDREF\"/>\n";
		str +="		<xsd:attribute name=\"_derived\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_instances\" type=\"xsd:IDREFS\"/>\n";
		str +="		<xsd:attribute name=\"_desynched_atts\" type=\"xsd:string\"/>\n";
		str +="		<xsd:attribute name=\"_real_archetype\" type=\"xsd:boolean\"/>\n";
		str +="		<xsd:attribute name=\"_subtype\" type=\"xsd:boolean\"/>\n";
		str +="	</xsd:complexType>\n";
		str +="\n";
		str +=" <xsd:element name=\"CADDatumEditor\" type=\"CADDatumEditorType\"/>\n";
		str +="\n";
		str +="</xsd:schema>\n";
		str +="\n";
	}
		return str;
}
} //namespace
#endif
