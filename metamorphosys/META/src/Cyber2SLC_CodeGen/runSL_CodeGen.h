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

#ifndef RUNSL_CODEGEN
#define RUNSL_CODEGEN

#include "CyberComposition.h"

void __declspec(dllimport) runCyberSF_CodeGen_dllimported(const CyberComposition::SimulinkWrapper &slmodel, std::string cyber_file, Uml::Diagram &cyber_meta_diagram, std::string logFile, std::string directory="");
bool __declspec(dllexport) runCyberSL_CodeGen_dllimported(const CyberComposition::SimulinkWrapper &slmodel, std::string cyber_file, Uml::Diagram &cyber_meta_diagram, std::string directory="");
bool runCyberSL_CodeGen(const CyberComposition::SimulinkWrapper &slmodel, std::string cyber_file, Udm::DataNetwork* sdn_cyber, std::string directory="");
void logGeneratedFiles(std::string workDir);

class ComponentCodeGen
{
public:
	ComponentCodeGen(CyberComposition::SimulinkWrapper &slmodel, const std::string &outputDirectory, const std::string &comname);
	void runCodeGen(const std::string &logFile);
	set<std::string> getGenerated_H_Files();
	set<std::string> getGenerated_C_Files();
private:
	CyberComposition::SimulinkWrapper _slmodel;
	std::string _slname;
	std::string _directory;
	set<std::string> _cfiles;
	set<std::string> _hfiles;
	bool errorReported;

public:
	bool getError( void ) {
		return errorReported;
	}
	void setError( void ) {
		errorReported = true;
	}
};

#endif