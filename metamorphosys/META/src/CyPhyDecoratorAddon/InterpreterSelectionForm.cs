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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using GME.Util;
using GME.MGA;
using META;

namespace CyPhyDecoratorAddon
{
    public partial class InterpreterSelectionForm : Form
    {
        public InterpreterSelectionForm()
        {
            InitializeComponent();
        }

        internal CyPhyDecoratorAddon addon { get; set; }

        internal void Init()
        {
            LoadInterpreters();
        }

        private void LoadInterpreters()
        {
            List<ComComponent> ComComponents = new List<ComComponent>();

            MgaRegistrar registrar = new MgaRegistrar();
            regaccessmode_enum r = regaccessmode_enum.REGACCESS_BOTH;
            IEnumerable components = (IEnumerable)(object)registrar.GetComponentsDisp(r);

            string paradigm = "CyPhyML";

            foreach (string comProgId in components)
            {
                try
                {
                    bool isAssociated;
                    bool canAssociate;

                    registrar.IsAssociated(comProgId, paradigm, out isAssociated, out canAssociate, r);
                    string DllPath = registrar.LocalDllPath[comProgId];

                    componenttype_enum Type;
                    string desc;
                    registrar.QueryComponent(
                        comProgId,
                        out Type,
                        out desc,
                        regaccessmode_enum.REGACCESS_BOTH);

                    bool isInterpreter = false;
                    isInterpreter = (Type == componenttype_enum.COMPONENTTYPE_INTERPRETER);

                    if (canAssociate &&
                        File.Exists(DllPath) &&
                        isInterpreter)
                    {
                        ComComponent component = new ComComponent(comProgId);
                        if (component.isValid)
                        {
                            ComComponents.Add(component);
                        }
                    }
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    // don't add to list
                }
            }

            lbInterpreters.Items.Clear();
            ComComponents.Sort((Comparison<ComComponent>)((a, b) => a.ToString().CompareTo(b.ToString())));
            foreach (ComComponent c in ComComponents)
            {
                lbInterpreters.Items.Add(c);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }


    }
}
