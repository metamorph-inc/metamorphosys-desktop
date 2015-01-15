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

#ifndef DATA_EXCHANGE_H
#define DATA_EXCHANGE_H
#include <isis_application_exception.h>
#include <string>
#include <map>
#include <CommonStructures.h>

namespace isis
{

void ExportDataExchangeFiles(
    const MultiFormatString								&in_ModelName,
    ProMdlType											in_ModelType,
    const std::string									&in_WORKING_DIR,
    const std::list<DataExchangeSpecification>          &in_DataExchangeSpecifications,
    bool												in_LogProgress = false)
throw (isis::application_exception);

void ExportDataExchangeFiles(
    const MultiFormatString								&in_ComponentID,  // Could be an assembly or part
    const std::string									&in_WORKING_DIR,
    const std::list<DataExchangeSpecification>          &in_DataExchangeSpecifications,
    std::map<string, isis::CADComponentData>			&in_CADComponentData_map,
    bool												in_LogProgress )
throw (isis::application_exception);

void ExportRasterImage(	const std::string									&in_AssemblyComponentID,
                        std::map<string, isis::CADComponentData>			&in_CADComponentData_map )
throw (isis::application_exception);

//bool SeparateSTEPPartFilesSpecified(const  std::list<DataExchangeSpecification> &in_DataExchangeSpecifications);

bool DataExchange_Format_Version_InList(
    e_DataExchangeFormat  in_DataExchangeFormat,  // Only currently support DATA_EXCHANGE_FORMAT_STEP
    e_DataExchangeVersion in_DataExchangeVersion, // AP203_SINGLE_FILE, AP203_E2_SINGLE_FILE, AP203_E2_SEPARATE_PART_FILES...
    const  std::list<DataExchangeSpecification> &in_DataExchangeSpecifications );

}  // End namespace isis

#endif