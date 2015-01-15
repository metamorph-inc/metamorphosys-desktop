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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CyPhy2DesignInterchange
{
    internal class LayoutDataWrapper
    {
        private object obj;
        public LayoutDataWrapper(object avmObj)
        {
            obj = avmObj;
        }

        private PropertyInfo GetProperty(String name)
        {
            return obj.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
        }

        private void SetProperty(String name, object value)
        {
            PropertyInfo propInfo = GetProperty(name);
            if (null != propInfo && propInfo.CanWrite)
                propInfo.SetValue(obj, value, null);
        }

        private Boolean PropertyExistsAndIsAndTrue(String name)
        {
            PropertyInfo propInfo = GetProperty(name);
            if (propInfo == null)
                return false;

            var propVal = propInfo.GetValue(obj, null);
            if (propVal == null)
                return false;

            return System.Convert.ToBoolean(propVal);
        }

        public Boolean hasLayoutData
        {
            get
            {
                return (PropertyExistsAndIsAndTrue("XPositionSpecified")
                        && PropertyExistsAndIsAndTrue("YPositionSpecified"));
            }
        }

        public UInt32 xpos
        {
            get
            {
                if (false == PropertyExistsAndIsAndTrue("XPositionSpecified"))
                    return 0;
                else
                    return System.Convert.ToUInt32(GetProperty("XPosition").GetValue(obj, null));
            }
            set
            {
                SetProperty("XPosition", value);
                SetProperty("XPositionSpecified", true);
            }
        }

        public UInt32 ypos
        {
            get
            {
                if (false == PropertyExistsAndIsAndTrue("YPositionSpecified"))
                    return 0;
                else
                    return System.Convert.ToUInt32(GetProperty("YPosition").GetValue(obj, null));
            }
            set
            {
                SetProperty("YPosition", value);
                SetProperty("YPositionSpecified", true);
            }
        }
    }
}
