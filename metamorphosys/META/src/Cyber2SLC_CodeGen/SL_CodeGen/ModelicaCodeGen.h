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

#ifndef __MOCODEGEN_H__
#define __MOCODEGEN_H__

#include "CyberComposition.h"

class MoCodeGen
{
public:
//	MoCodeGen(CyPhyML::Component &cyphyComponent, const std::string &outputDirectory, const std::string logFile);
//	MoCodeGen(CyPhyML::TestBench &testbench, const std::string &outputDirectory);
	MoCodeGen(CyberComposition::SimulinkWrapper &slmodel, const std::string &outputDirectory, const std::string logFile);
	void gen();
	void setPackageName(const std::string &pkg);
private:
	void genWrapper_source();
	void genWrapper_proj();
	void genModelica();
	void genPackageMo();
	std::string generateGUID();
	std::string convertMoType(const std::string &slType);
	//std::string getDataVarFullPath(const CyPhyML::SF_DataParameterBase &obj);
	//std::string getParaVarFullPath(const CyberComposition::Simulink::SF_Parameter &param);
	std::string getParaVarFullPath(const CyberComposition::ParameterBase &param);
//	CyPhyML::BusPort getBusPort(const CyPhyML::SignalFlowBusPortInterface &busportInterface);
	int getValueArraySize(const CyberComposition::Simulink::SF_Parameter &sf_parameter);

	//CyPhyML::Component _com;
	CyberComposition::SimulinkWrapper _sl;
	std::string _slName;
	std::string _directory;
	std::string _wrapperName;
	std::string _topSubSystemName;
	std::string _contextName;
	std::string _pkgName;
	std::string _logFileName;
	
	list<CyberComposition::Simulink::Subsystem> _topSubSystems;

	list<CyberComposition::Simulink::InputPort> _inputs;
	list<CyberComposition::Simulink::OutputPort> _outputs;
	map<CyberComposition::Simulink::InputPort, CyberComposition::Simulink::TypeBaseRef> _inputMap;
	map<CyberComposition::Simulink::OutputPort, CyberComposition::Simulink::TypeBaseRef>  _outputMap;
//	map<CyPhyML::SignalFlowBusPortInterface, CyPhyML::BusPort> _busportMap;
	map<CyberComposition::ParameterRef, std::string > _parameterRefMap;
	map<CyberComposition::ParameterRef, std::string > _sfdataRefMap;

	set<std::string> _hfiles;
	set<std::string> _cfiles;
	std::string _mainArgs;

	struct InputPortSorter {
		bool operator()( const CyberComposition::Simulink::InputPort &port1, const CyberComposition::Simulink::InputPort &port2 ) const {
				return static_cast< int >(port1.Number() ) < static_cast< int >( port2.Number() );
			}
		};

	struct OutputPortSorter {
		bool operator()( const CyberComposition::Simulink::OutputPort &port1, const CyberComposition::Simulink::OutputPort &port2 ) const {
				return static_cast< int >(port1.Number() ) < static_cast< int >( port2.Number() );
			}
		};
};

#endif