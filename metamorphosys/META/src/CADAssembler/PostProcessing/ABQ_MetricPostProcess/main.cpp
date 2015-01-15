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

// junk_udm_64bit.cpp : Defines the entry point for the console application.
//
//#include "stdafx.h"
#include <iostream>

#include <odb_API.h>
#include "AnalyzeODB.h"
#include <map>
#include <fstream>


string gPath;
string gConfigID;
map<string, vector<Metric>> gMetricPairs;
map<string, string> gElementSets;
map<string, Material> gMaterials;
std::ofstream gFile;

using namespace std;




void Usage()
{
	cout << "Usage:" << endl;
	cout << "-f <ODB_File_Path>" << endl;
	cout << "-e <Element Sets>" << endl;
	cout << "-c <Config ID>" << endl;
	cout << "-m <MetricID:Name Pairs>" << endl;
	cout << "-p <Material Properties>" << endl;
}

float main(int argc, char* argv[])
{
	float maxVonMises = 1;

	gFile.open("FEAStressOutput.txt");	

	try
	{
		if (argc < 5)
		{
			cout << "Insufficient # of arguments provided!" << endl;
			Usage();
			return 0;
		}

		gPath = "";
		string elementNames,
			metricPairs,
			materialProperties;		
		
		for (int i = 1; i < argc; i++)
		{
			string anArg = argv[i];

			if (anArg == "-f")
			{
				gPath = argv[i+1];
			}
			else if (anArg == "-e")
			{
				elementNames = argv[i+1];
				ParseElementIDs(elementNames);
			}
			else if (anArg == "-c")
			{
				gConfigID = argv[i+1];
			}
			else if (anArg == "-m")
			{
				metricPairs = argv[i+1];
				ParseMetricIDs(metricPairs);
			}
			else if (anArg == "-p")
			{
				materialProperties = argv[i+1];
				ParseMaterials(materialProperties);
			}			
		}
		maxVonMises = AnalyzeFEAResults();
		WriteMetricsFile();
	}
	catch (odb_Exception& e)
	{
		std::string msg = e.ErrMsg().text();
		int errorN = e.ErrorNo();
		std::cout << "ODB Exception: " << msg.c_str() << " ErrorNo: " << errorN << std::endl;

		double var;
		std::cin >> var;
	}
	catch (analyzer_exception& e)
	{
		std::cout << "Analyzer Exception: " << e.what();
	}
	catch (...)
	{
		std::string msg = "None Abaqus related error!";
		std::cout<< "None ODB exception!" << std::endl;
	}

	gFile.close();
	return maxVonMises;
}

