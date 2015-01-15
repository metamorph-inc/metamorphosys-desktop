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

#ifndef CAD2CYPHY_H
#define CAD2CYPHY_H

#include <string>
#include "MainDialog.h"
#include "CADMetrics.h"
#include "CyPhyML.h"
#include <UdmStatic.h>


#define MASS "Mass"
#define VOLUME "Volume"
#define DENSITY "Density"
#define SURF_AREA "SurfaceArea"
#define CENTER_GRAVITY "CG"


class CAD2CyPhy
{
public:		
	CAD2CyPhy();

	~CAD2CyPhy(){
		if (m_sdnMetrics)
		{
			if (m_sdnMetrics->isOpen())
				m_sdnMetrics->CloseNoUpdate();
			delete m_sdnMetrics;
			m_sdnMetrics = 0;
		}
	}

	typedef map<string, CADMetrics::MetricsBase> ComponentMetricsMap;	///< typedef map[dsID, MetricsBase]

	void UpdateMetrics(const CyPhyML::RootFolder&);	
	bool Initialize();
	bool ParseMetricsFile();
	void ProcessMetricsComponent(const CADMetrics::MetricComponents&);
	void ProcessMetricsAssemblies(const CADMetrics::Assemblies&);
	void ProcessMetricsAssembly(const CADMetrics::Assembly&, ComponentMetricsMap&);
	void ProcessMetricsCADComponent(const CADMetrics::CADComponent&, ComponentMetricsMap&);	
	void FindConfigurations(const CyPhyML::RootFolder&);
	void FindConfigurations(CyPhyML::ComponentAssembly&);
	void FindConfigurations(CyPhyML::ComponentAssemblies&);

	void UpdateConfigurations();										///< updates everything from metrics file
	void UpdateConfigurations(string configID);							///< updates a particular configuration specified by configID
	void UpdateComponentAssembly(CyPhyML::ComponentAssembly&, ComponentMetricsMap&);
	void UpdateMetrics(CyPhyML::Component&, ComponentMetricsMap&);
	void UpdateMetrics(CyPhyML::ComponentRef&, ComponentMetricsMap&);
	void UpdateMetrics(map<string, pair<string, string>>&, CyPhyML::DesignElement&);			//void UpdateMetrics(map<string, string>&, CyPhyML::DesignElement&);
	void UpdateMetrics(map<string, pair<string, string>>&, CyPhyML::ComponentRef&);			//void UpdateMetrics(map<string, string>&, CyPhyML::ComponentRef&);

	// helpers
	bool IsSize2FitParametric(const CyPhyML::Component&);
	void MakeMetricValuePairs(map<string, pair<string, string>>&, CADMetrics::MetricComponent&);		//void MakeMetricValuePairs(map<string, string>&, CADMetrics::MetricComponent&);

private:
	Udm::SmartDataNetwork* m_sdnMetrics;								///< smart data network of the metrics file
	string m_metricsFile;
	map<string, ComponentMetricsMap> m_configurationMetricsMap;			///< component's metric lookup map[configID, ComponentMetricsMap] for each configuration (configurationID is used as key)
	map<long, CADMetrics::MetricsBase> m_metricsLookup;					///< main metrics lookup map[mID, MetricsBase]
	map<string, CyPhyML::ComponentAssembly> m_cyPhyConfigurationMap;		///< configuration map - only doing this because the metric file could have metrics for multiple configurations!
	map<string, string> m_unitsLookup;
};

#endif                                           