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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using GME.MGA;
using GME.CSharp;
using System.IO;
using Xunit;
using System.Reflection;
using GME.MGA.Parser;
using GME.Util;

namespace MasterInterpreterTest
{
    public static class MgaHelper
    {
        public static void CheckParadigmVersionUpgrade(MgaProject project)
        {
            GME.Util.MgaRegistrar registar = new GME.Util.MgaRegistrar();
            string MetaConnStr;
            object MetaGuid = null;
            object ProjectMetaGuid;
            project.BeginTransactionInNewTerr();
            try
            {
                registar.QueryParadigm(project.MetaName, out MetaConnStr, ref MetaGuid, GME.Util.regaccessmode_enum.REGACCESS_BOTH);
                ProjectMetaGuid = project.MetaGUID;
            }
            finally
            {
                project.AbortTransaction();
            }
            if (((Array)ProjectMetaGuid).Cast<byte>().SequenceEqual(((Array)MetaGuid).Cast<byte>()) == false)
            {
                Xunit.Assert.True(false, string.Format("Please upgrade {0} to the latest registered {1} paradigm", project.ProjectConnStr, project.MetaName));
            }
        }
    }

    public class Test : IUseFixture<JobManagerFixture>
    {
        [STAThread]
        static int Main(string[] args)
        {
            int ret = Xunit.ConsoleClient.Program.Main(new string[] {
                Assembly.GetAssembly(typeof(Test)).CodeBase.Substring("file:///".Length),
                //"/noshadow",
            });
            Console.In.ReadLine();
            return ret;
        }


        public static string ImportXME2Mga(string projectDir, string xmeFileName)
        {
            var xmePath = Path.Combine("..", "..", "..", "..", "models", "DynamicsTeam", projectDir, xmeFileName);
            string projectConnStr;
            MgaUtils.ImportXMEForTest(xmePath, out projectConnStr);
            return projectConnStr.Substring("MGA=".Length);
        }

        JobManagerFixture fixture;
        public void SetFixture(JobManagerFixture data)
        {
            fixture = data;
        }
    }
}
