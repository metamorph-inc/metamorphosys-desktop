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
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GME.CSharp;
using GME;
using GME.MGA;
using GME.MGA.Core;
using ISIS.GME.Common;

using CyPhyML = ISIS.GME.Dsml.CyPhyML.Interfaces;
using CyPhyMLClasses = ISIS.GME.Dsml.CyPhyML.Classes;

namespace CyPhyMLMerge {
    class Program {

        private static MgaProject GetProject(String filename) {
            MgaProject result = null;

            if (filename != null && filename != "") {
                if (Path.GetExtension(filename) == ".mga") {
                    result = new MgaProject();
                    if (System.IO.File.Exists(filename)) {
                        Console.Out.Write("Opening {0} ... ", filename);
                        bool ro_mode;
                        result.Open("MGA=" + filename, out ro_mode);
                    } else {
                        Console.Out.Write("Creating {0} ... ", filename);
                        result.Create("MGA=" + filename, "CyPhyML");
                    }
                    Console.Out.WriteLine("Done.");
                } else {
                    Console.Error.WriteLine("{0} file must be an mga project.", filename);
                }
            } else {
                Console.Error.WriteLine("Please specify an Mga project.");
            }

            return result;
        }

        static void usage() {
            Console.Error.WriteLine("Usage: CyPhyMLMerge FILE-TO-MERGE-INTO.mga < FILE-TO-MERGE.mga/PATH/TO/ITEM/TO/MERGE ... >");
            Environment.Exit( 1 );
        }

        static int Main(string[] args) {

            if (args.Length < 2) usage();

            MgaProject mainMgaProject = GetProject( args[0] );

            if( mainMgaProject == null ) {
                Environment.Exit( 2 );
            }

            int retval = 0;
            MgaGateway mgaGateway = new MgaGateway(mainMgaProject);

            mainMgaProject.CreateTerritoryWithoutSink(out mgaGateway.territory);
            mgaGateway.PerformInTransaction(delegate {

                for (int ix = 1; ix < args.Length; ++ix) {
                    string[] mergeInfo = args[ix].Split(new string[] { ".mga/" }, 2, StringSplitOptions.None);

                    string filename = mergeInfo[0] + ".mga";
                    string path = "/" + mergeInfo[1];

                    MgaFCO currentObject = mainMgaProject.get_ObjectByPath(path) as MgaFCO;
                    if (currentObject == null) {
                        Console.Error.WriteLine("Error: could not find object of path \"" + path + "\" in model of file \"" + args[0] + "\", cannot merge file \"" + filename + "\"");
                        retval |= (int)SubTreeMerge.SubTreeMerge.Errors.PathError;
                        continue;
                    }

                    SubTreeMerge.SubTreeMerge subTreeMerge = new SubTreeMerge.SubTreeMerge();
                    subTreeMerge.gmeConsole = new SubTreeMerge.FlexConsole( SubTreeMerge.FlexConsole.ConsoleType.CONSOLE );

                    subTreeMerge.merge(currentObject, filename);
                    retval = (int)subTreeMerge.exitStatus;
                }

            });

            mainMgaProject.Save();

            if (mgaGateway.territory != null) {
                mgaGateway.territory.Destroy();
            }

            return retval;
        }
    }
}
