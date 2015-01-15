﻿/*
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
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CyPhy2CADPCB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.InteropServices;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    [ProgId("ISIS.META.CyPhy2CADPCB_Settings")]
    [Guid("54448B4E-F898-41CA-B706-E1E6B27AA7E8")]

    public class CyPhy2CADPCB_Settings : CyPhyGUIs.IInterpreterConfiguration
    {
        public const string ConfigFilename = "CyPhy2CADPCB_config.xml";

        public bool Verbose { get; set; }
        public String SavedLayoutFilePath { get; set; }
        [CyPhyGUIs.WorkflowConfigItem]
        public string LayoutFilePath { get; set; }
        public string GetLayoutPath
        {
            get
            {
                if (String.IsNullOrWhiteSpace(LayoutFilePath)
                    )
                {
                    return SavedLayoutFilePath;
                }
                else
                {
                    return LayoutFilePath;
                }
            }
        }

        public CyPhy2CADPCB_Settings()
        {            
            this.Verbose = false;
        }
    }
}

