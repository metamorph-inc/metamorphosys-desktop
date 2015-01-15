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

#include "ExteriorShell.h"

namespace isis {
	namespace hydrostatic {

		/**
		a function which determines if the subject value is in the axis range.
		*/
		bool PolatedAxis::isInRange(const double subject) const {
			if (m_axis.empty()) return false;
			if (subject < m_axis.front()) return false;
			if (m_axis.back() < subject) return false;
			return true;
		}
		/**
		returns the lower index and the interpolation ratio.
		*/
		inline PolatedAxis::inter_type locate(const double subject, const axis_type axis) {
			if (axis.empty()) throw  false;
			int below = 0;
			int above = (int)axis.size() - 1;
			if (subject == axis[above]) {
				return ::boost::make_tuple(::boost::make_tuple(above, above), 0.0);
			}
			while( below + 1 < above ) {
				const int split = std::div(below + above, 2).quot;
				const double candidate = axis[split];
				if (subject == candidate) {
					return ::boost::make_tuple(::boost::make_tuple(split,split), 0.0);
				}
				if (subject < candidate) {
					above = split;
				} else {
					below = split;
				}
			}
			const double ratio = (subject - axis[below])/(axis[above] - axis[below]);
			return ::boost::make_tuple(::boost::make_tuple(below, above), ratio);
		}
		/** 
		returns a tuple (index, ratio) over the axis
		*/
		PolatedAxis::inter_type PolatedAxis::interpolate(const double key) const {
			return locate(key, m_axis);
		}
		/** 
		returns a tuple (index, ratio) over all axies
		*/
		PolatedAxis::extra_type PolatedAxis::extrapolate(const double key) const {
			return locate(key, m_axis);
		}

	
	} // namespace hydrostatic 
} // namespace isis 