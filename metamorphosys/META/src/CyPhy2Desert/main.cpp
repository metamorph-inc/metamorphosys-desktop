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

// CyPhy2Desert.cpp : Defines the entry point for the console application.
//
#include "CyPhy2Desert.h"

UDM_USE_DOM
UDM_USE_MGA

void usage()
{
	std::cout << "Usage: cyphy2desert.exe <CyPhyML mga File Name> [<Desert xml File Name>] "<< std::endl;
}

int main(int argc, char *argv[])
{
	if( argc== 1)
	{
		usage( );
		return -1;
	}

	std::string cyphy_file = argv[1];
	std::string desert_file = cyphy_file;

	size_t extPos = cyphy_file.find(".mga");
	if(extPos == string::npos)
	{
		extPos = cyphy_file.find(".MGA");
		if(extPos == string::npos)
		{
			std::cout<<"Input file must be MGA file. Aborting."<<std::endl;
			return -1;
		}
	}

	if(argc == 3)
	{
		desert_file = argv[2];
		extPos = desert_file.find(".xml");
		if(extPos == string::npos) desert_file += ".xml";
	}
	else desert_file.replace(extPos,4,"_Desert.xml");

	cout << "Converting " << cyphy_file << " ...... ";

	try
	{
		Udm::SmartDataNetwork cyphy(CyPhyML::diagram);
		cyphy.OpenExisting(argv[1],"CyPhyML", Udm::CHANGES_LOST_DEFAULT);

		CyPhyML::RootFolder cyphy_rf = CyPhyML::RootFolder::Cast(cyphy.GetRootObject());

		Udm::SmartDataNetwork desert(DesertIface::diagram);
		{	
			desert.CreateNew(desert_file, "DesertIface.xsd", DesertIface::DesertSystem::meta,Udm::CHANGES_LOST_DEFAULT);
			DesertIface::DesertSystem desert_top = DesertIface::DesertSystem::Cast(desert.GetRootObject());

			map<CyPhyML::DesignEntity, DesertIface::Element> com2elem;
			CyPhy2Desert c2d(desert_top);
			c2d.generateDesert(cyphy_rf);
		}
		desert.CloseWithUpdate();
		cyphy.CloseWithUpdate();
	
		cout<<desert_file<<endl;
		cout<<"Done."<<endl;
	}
	catch(udm_exception &e)
	{
		cout<<e.what()<<endl;
		return 1;
	}

	return 0;
}

