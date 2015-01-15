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

/*! \file isis_include_ptc_headers.h
    \brief .h file that includes the necessary Creo headers.

	In addition to including the necessary Creo Toolkit headers, 
	this file wraps the includes in extern "C".  This is necessary to use the includes 
	with C++.
*/
#ifndef ISIS_INCLUDE_PTC_HEADERS_H
#define ISIS_INCLUDE_PTC_HEADERS_H

extern "C"
{
#include <ProToolkit.h>
#include <user_tk_error.h>

#include <ProCore.h>
#include <ProMdl.h>
#include <ProMenu.h>
#include <ProModelitem.h>
#include <ProObjects.h>
#include <ProSizeConst.h>
#include <ProUtil.h>
#include <ProTKRunTime.h>
#include <ProMechanica.h>
#include <ProMechGeomref.h>

#include <ProFeature.h>
#include <ProFeatType.h>
#include <PTApplsUnicodeUtils.h>
#include <ProAsmcomp.h>
#include <ProSolid.h>
#include <ProMdlUnits.h>
#include <ProParameter.h>
#include <ProCsys.h>
#include <ProAxis.h>
#include <ProWstring.h>
#include <ProMaterial.h>
#include <ProFemMesh.h>
#include <ProWindows.h>
#include <ProDtmPln.h>
#include <ProDtmPnt.h>
#include <ProDtmAxis.h>
#include <ProDtmCsys.h>
#include <UtilString.h>
#include <ProIntfimport.h>
#include <ProSimprep.h>
#include <ProFit.h>
#include <ProIntf3Dexport.h>
//#include <TestError.h>
}

#endif