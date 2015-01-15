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



#ifndef CREATE_CREO_SELECTOR_H
#define CREATE_CREO_SELECTOR_H

#include <ProToolkit.h>
#include <ProFeature.h>
#include <ProReference.h>
#include <ProDtmPln.h>

#include <log4cpp/Category.hh>
#include <log4cpp/OstreamAppender.hh>
#include "CommonDefinitions.h"

#include <string>
#include <vector>
#include <boost/shared_ptr.hpp>

namespace isis {
namespace creo {
namespace feature {

class Selector {
private:
	log4cpp::Category& m_log_f;
	log4cpp::Category& m_log_cf;

	ProSelection m_self;
public:

	Selector( ProSelection in_selection );
	Selector( ProMdl in_model, ProFeature in_feature, ProType in_geom_type = PRO_TYPE_UNUSED );
	~Selector();

	ProSelection& operator*();
	ProSelection  operator&();
	ProSelection* operator->();

	ProValueData getValueData();
};


}
}
}

#endif // CREATE_CREO_SELECTOR_H