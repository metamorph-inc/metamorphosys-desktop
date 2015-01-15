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

#include "UdmComparator.hpp"
#include "CyPhyML.h"

int main( int argc, char **argv ) {
	try
	{
		if (argc < 2) {
			std::cerr << "Usage: CyPhyMLComparator project1.mga project2.mga" << std::endl;
			return 2;
		}

		Udm::SmartDataNetwork udmSmartDataNetwork1( CyPhyML::diagram );
		udmSmartDataNetwork1.OpenExisting( argv[1], "", Udm::CHANGES_LOST_DEFAULT );

		Udm::Object rootObject1 = udmSmartDataNetwork1.GetRootObject();


		Udm::SmartDataNetwork udmSmartDataNetwork2( CyPhyML::diagram );
		udmSmartDataNetwork2.OpenExisting( argv[2], "", Udm::CHANGES_LOST_DEFAULT );

		Udm::Object rootObject2 = udmSmartDataNetwork2.GetRootObject();


		UdmComparator udmComparator;

		UdmComparator::StringSet exclusiveClassNameSet;
		exclusiveClassNameSet.insert( "ReferenceCoordinateSystem" );
		exclusiveClassNameSet.insert( "RootFolder" );

		UdmComparator::StringStringSetMap exclusiveClassNameAttributeNameSetMap;
		UdmComparator::StringSet exclusiveAttributeNameSet;
		exclusiveAttributeNameSet.insert( "position" );
		exclusiveClassNameAttributeNameSetMap.insert(  std::make_pair( "MgaObject", exclusiveAttributeNameSet )  );

		exclusiveAttributeNameSet.clear();
		exclusiveAttributeNameSet.insert( "InstanceGUID" );
		exclusiveClassNameAttributeNameSetMap.insert(  std::make_pair( "ComponentRef", exclusiveAttributeNameSet )  );
		
		exclusiveAttributeNameSet.clear();
		exclusiveAttributeNameSet.insert( "Path" );
		exclusiveClassNameAttributeNameSetMap.insert(  std::make_pair( "Component", exclusiveAttributeNameSet )  );

		UdmComparator::ClassNameFilter classNameFilter;
		classNameFilter.setExclusiveClassNameSet( exclusiveClassNameSet );
		classNameFilter.setExclusiveClassNameAttributeNameMap( exclusiveClassNameAttributeNameSetMap );
		udmComparator.setClassNameFilter( classNameFilter );
		
		bool result = true;
		result = udmComparator.compareNode( rootObject1, rootObject2 );

		std::cerr << UdmComparator::Report::get_singleton().getString() << std::endl;

		udmSmartDataNetwork1.CloseNoUpdate();
		udmSmartDataNetwork2.CloseNoUpdate();

		return result ? 0 : 1;
	}
	catch (const udm_exception& e)
	{
		std::cerr << e.what() << std::endl;
		return 3;
	}
}