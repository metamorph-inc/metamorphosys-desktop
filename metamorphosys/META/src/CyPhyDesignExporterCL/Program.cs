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
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;
using GME.CSharp;
using GME.MGA;

using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;
using CyPhyClasses = ISIS.GME.Dsml.CyPhyML.Classes;

namespace CyPhyDesignExporterCL
{
    public class Program
    {
        private static String Safeify(String test)
        {
            return string.IsNullOrEmpty(test) ? string.Empty : test.Replace("\\", "_").Replace("/", "_");
        }

        public static int Main(string[] args)
        {
            var options = new Options();
            if (!Parser.Default.ParseArguments(args, options)) return -1;

            var project = GetProject(options.MgaFile);

            if (project == null)
            {
                Environment.Exit(1);
            }

            try
            {
                var mgaGateway = new MgaGateway(project);
                project.CreateTerritoryWithoutSink(out mgaGateway.territory);

                var designList = new List<CyPhy.DesignEntity>();
                var designName = Safeify(options.DesignName);


                bool bExceptionOccurred = false;
                mgaGateway.PerformInTransaction(delegate
                {
                    try
                    {
                        #region Collect DesignEntities

                        MgaFilter filter = project.CreateFilter();
                        filter.Kind = "ComponentAssembly";
                        foreach (var item in project.AllFCOs(filter).Cast<MgaFCO>().Where(x => x.ParentFolder != null))
                        {
                            designList.Add(CyPhyClasses.ComponentAssembly.Cast(item));
                        }

                        filter = project.CreateFilter();
                        filter.Kind = "DesignContainer";
                        foreach (var item in project.AllFCOs(filter).Cast<MgaFCO>().Where(x => x.ParentFolder != null))
                        {
                            designList.Add(CyPhyClasses.DesignContainer.Cast(item));
                        }

                        #endregion

                        #region Process DesignEntities

                        foreach (CyPhy.DesignEntity de in designList)
                        {
                            var currentDesignName = Safeify(de.Name);

                            if (!string.IsNullOrEmpty(options.DesignName) && currentDesignName != designName) continue;

                            var dm = CyPhy2DesignInterchange.CyPhy2DesignInterchange.Convert(de);
                            var outFilePath = String.Format("{0}\\{1}.adm", new FileInfo(options.MgaFile).DirectoryName, currentDesignName);
                            XSD2CSharp.AvmXmlSerializer.SaveToFile(outFilePath, dm);
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Exception: {0}", ex.Message.ToString());
                        Console.Error.WriteLine("Stack: {0}", ex.StackTrace.ToString());
                        bExceptionOccurred = true;
                    }
                });

                if (bExceptionOccurred)
                    return -1;

                return 0;
            }
            finally
            {
                project.Close(true);
            }
        }

        private static MgaProject GetProject(String filename)
        {
            MgaProject result = null;

            if (!string.IsNullOrEmpty(filename))
            {
                if (Path.GetExtension(filename) == ".mga")
                {
                    result = new MgaProject();
                    if (File.Exists(filename))
                    {
                        Console.Out.Write("Opening {0} ... ", filename);
                        var roMode = true;
                        result.Open("MGA=" + filename, out roMode);
                        Console.Out.WriteLine("Done.");
                    }
                    else
                    {
                        Console.Error.WriteLine("{0} file must be an existing mga project.", filename);
                    }
                }
                else
                {
                    Console.Error.WriteLine("{0} file must be an mga project.", filename);
                }
            }
            else
            {
                Console.Error.WriteLine("Please specify an Mga project.");
            }

            return result;
        }
    }
}
