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
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CyPhy2Schematic
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
    [ProgId("ISIS.META.CyPhy2Schematic_Settings")]
    [Guid("91D39BEB-0026-431D-8315-E600F0D84FAA")]

    public class CyPhy2Schematic_Settings : CyPhyGUIs.IInterpreterConfiguration
    {
        public const string ConfigFilename = "CyPhy2Schematic_config.xml";
        
        public CyPhy2Schematic_Settings()
        {
            this.Verbose = false;
            this.doChipFit = null;
            this.doPlaceRoute = null;
            this.doPlaceOnly = null;
            this.doSpice = null;
            this.doSpiceForSI = null;
            this.skipGUI = null;
            this.showChipFitVisualizer = null;
        }

        public bool Verbose { get; set; }

        [CyPhyGUIs.WorkflowConfigItem]
        public string doChipFit { get; set; }

        [CyPhyGUIs.WorkflowConfigItem]
        public string doPlaceRoute { get; set; }

        [CyPhyGUIs.WorkflowConfigItem]
        public string doPlaceOnly { get; set; }

        [CyPhyGUIs.WorkflowConfigItem]
        public string doSpice  { get; set; }

        [CyPhyGUIs.WorkflowConfigItem]
        public string doSpiceForSI { get; set; }

        [CyPhyGUIs.WorkflowConfigItem]
        public string showChipFitVisualizer { get; set; }

        [CyPhyGUIs.WorkflowConfigItem]
        [System.Xml.Serialization.XmlIgnore]
        public string skipGUI { get; set; }
    }
}

