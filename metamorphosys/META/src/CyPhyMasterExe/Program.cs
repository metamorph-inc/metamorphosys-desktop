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
using System.Text;
using GME.MGA;
using GME.CSharp;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CyPhyMasterExe
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                // parse command line arguments
                string projectConnStr = args[0];
                string originalSubjectID = args[1];
                string[] configIDs = args.Skip(2).ToArray();

                if (projectConnStr.StartsWith("MGA=") == false)
                {
                    // use the full absolute path
                    projectConnStr = "MGA=" + Path.GetFullPath(projectConnStr);
                }
                
                MgaProject project = new MgaProject();
                bool ro_mode;
                project.Open(projectConnStr, out ro_mode);

                try
                {
                    // get an instance of the master interpreter
                    using (var master = new CyPhyMasterInterpreter.CyPhyMasterInterpreterAPI(project))
                    {
                        // create a configuration for the run
                        var configLight = new CyPhyMasterInterpreter.ConfigurationSelectionLight();
                        configLight.ContextId = originalSubjectID;
                        configLight.SelectedConfigurationIds = configIDs;
                        configLight.KeepTemporaryModels = false;
                        configLight.PostToJobManager = true;

                        // run master interpreter on configuration
                        var results = master.RunInTransactionWithConfigLight(configLight);

                        // summarize results
                        master.WriteSummary(results);
                    }
                }
                finally
                {
                    project.Close(true);
                }
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine(e.ToString());
                System.Environment.Exit(5);
            }
        }
    }
}
