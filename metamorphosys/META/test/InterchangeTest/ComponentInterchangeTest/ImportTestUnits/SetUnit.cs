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
using Xunit;
using System.IO;
using GME.MGA;

namespace ComponentImporterUnitTests
{    
    public class Unit
    {
        private static readonly string testPath = Path.Combine(
            META.VersionInfo.MetaPath,
            "test",
            "InterchangeTest",
            "ComponentInterchangeTest",
            "ImportTestModels",
            "Unit");

        private static readonly string orgXmePathInputModel = Path.Combine(
            META.VersionInfo.MetaPath,
            "test",
            "InterchangeTest",
            "ComponentInterchangeTest",
            "SharedModels",
            "BlankInputModel",
            "InputModel.xme");

        private static readonly string xmePathInputModel = Path.Combine(
            testPath,
            "Model.mga");
        
        private static readonly string acmPath = Path.Combine(
            testPath,
            "Component.acm");

        [Fact]
        public void SetUnit()
        {
            File.Copy(orgXmePathInputModel, xmePathInputModel, true);

            String connString;
            MgaUtils.ImportXMEForTest(xmePathInputModel, out connString);
            var mgaPath = connString.Substring("MGA=".Length);
            Assert.True(File.Exists(mgaPath), "Input model not found; import may have failed.");

            var project = GetProject(mgaPath);

            var importer = new CyPhyComponentImporter.CyPhyComponentImporterInterpreter();
            importer.Initialize(project);

            project.PerformInTransaction(delegate
            {
                var fco = importer.ImportFile(project, testPath, acmPath);
                ISIS.GME.Dsml.CyPhyML.Interfaces.Component comp = ISIS.GME.Dsml.CyPhyML.Classes.Component.Cast(fco);
                
                var millimeter = comp.Children.PropertyCollection
                                              .First(p => p.Name.Equals("mm"))
                                              .Referred.unit;
                Assert.NotNull(millimeter);
                Assert.Equal("Millimeter", millimeter.Name);

                var henry = comp.Children.PropertyCollection
                                         .First(p => p.Name.Equals("Henry"))
                                         .Referred.unit;
                Assert.NotNull(henry);
                Assert.Equal("Henry", henry.Name);

                var acre = comp.Children.PropertyCollection
                                        .First(p => p.Name.Equals("acre"))
                                        .Referred.unit;
                Assert.NotNull(acre);
                Assert.Equal("Acre", acre.Name);
            });

            project.Save();
            project.Close();
        }


        public static MgaProject GetProject(String filename)
        {
            MgaProject result = null;

            if (filename != null && filename != "")
            {
                if (Path.GetExtension(filename) == ".mga")
                {
                    result = new MgaProject();
                    if (System.IO.File.Exists(filename))
                    {
                        Console.Out.Write("Opening {0} ... ", filename);
                        bool ro_mode;
                        result.Open("MGA=" + Path.GetFullPath(filename), out ro_mode);
                    }
                    else
                    {
                        Console.Out.Write("Creating {0} ... ", filename);
                        result.Create("MGA=" + filename, "CyPhyML");
                    }
                    Console.Out.WriteLine("Done.");
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
