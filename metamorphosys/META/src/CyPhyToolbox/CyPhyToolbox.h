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

#pragma once

#include "CyPhyElaborate.h"

/*******************
Toolbox use cases
	User runs FormulaEvaluator
		Expand, evaluate, collapse
	Other interpreter runs FormulaEvaluator
		evaluate
		assume master knows what he's doing, and that we are elaborated
	(User | Interpreter) runs Elaborator to expand
		Expand, evaluate
	(User | Interpreter) runs Elaborator to collapse
		just collapse
		we do this because we don't know if the model 
		  is fully elaborated and "safe" to evaluate
***********************/

void FormulaEvaluator(const Udm::Object &focusObject,bool elaborateAndCollapse) {
	if (elaborateAndCollapse) {
		CyPhyElaborate cpe;
		Uml::Class type = focusObject.type();
		if (Uml::IsDerivedFrom(type, CyPhyML::TestBenchType::meta)) {
			TestBenchType tb_ = TestBenchType::Cast(focusObject);
			TestBenchType tb;

			try {
				// Elaborate includes traverse.
				tb = cpe.elaborate(tb_);
			} catch (udm_exception &exc) {
				string what = exc.what();
				
				// Don't rethrow ValueFlow errors
				if (what.substr(0,16) != "ValueFlow error:")
					throw exc;
				GMEConsole::Console::writeLine("Udm exception: " + what, MSG_ERROR);
			} 
		} else if (type == CyPhyML::ComponentAssembly::meta) {
			ComponentAssembly ca_ = ComponentAssembly::Cast(focusObject);
			ComponentAssembly ca;

			try {
				// Elaborate includes traverse
				ca = cpe.elaborate(ca_);
			} catch (udm_exception &exc) {
				string what = exc.what();
				
				// Don't rethrow ValueFlow errors
				if (what.substr(0,16) != "ValueFlow error:")
					throw exc;
				GMEConsole::Console::writeLine("Udm exception: " + what, MSG_ERROR);
			}
		} else if (Uml::IsDerivedFrom(type,CyPhyML::ComponentType::meta)) {  // ZL 11/20/2013 test component support
			Traverse(focusObject);
		} else if (type == CyPhyML::TestBenchSuite::meta) {
			Traverse(focusObject);
		}
	} else
		Traverse(focusObject);
};

void FormulaEvaluator(const std::set<Udm::Object> &selectedObjects,bool elaborateAndCollapse) {
	for (set<Udm::Object>::const_iterator i = selectedObjects.begin(); i != selectedObjects.end(); i++) {
		Udm::Object oi(*i);
		FormulaEvaluator(oi,elaborateAndCollapse);
	}
};
