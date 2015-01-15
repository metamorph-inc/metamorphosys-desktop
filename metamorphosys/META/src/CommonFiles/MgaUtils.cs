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

using System.IO;
using GME.MGA.Parser;
using GME.MGA;
using GME.Util;

namespace GME.MGA
{
    public static class MgaUtils
    {
        public static void ImportXMEForTest(string xmePath, out string connectionString)
        {
            string mgaPath = Path.Combine(Path.GetDirectoryName(xmePath), Path.GetFileNameWithoutExtension(xmePath) + "_test.mga");

            ImportXMEForTest(xmePath, mgaPath, out connectionString);
        }

        public static void ImportXMEForTest(string xmePath, string mgaPath, out string connectionString)
        {
            if (File.Exists(mgaPath))
            {
                // delete the file if exists.
                // it could be a test to check if the importer has generated the mga file or not.
                File.Delete(mgaPath);
            }

            connectionString = "MGA=" + mgaPath;
            ImportXME(xmePath, mgaPath);
        }

        public static void ImportXME(string xmePath, string mgaPath, bool enableAutoAddons=false)
        {
            MgaParser parser = new MgaParser();
            string paradigm;
            string paradigmVersion;
            object paradigmGuid;
            string basename;
            string version;
            parser.GetXMLInfo(xmePath, out paradigm, out paradigmVersion, out paradigmGuid, out basename, out version);

            parser = new MgaParser();
            MgaProject project = new MgaProject();
            MgaResolver resolver = new MgaResolver();
            resolver.IsInteractive = false;
            dynamic dynParser = parser;
            dynParser.Resolver = resolver;
            project.Create("MGA=" + Path.GetFullPath(mgaPath), paradigm);
            if (enableAutoAddons)
            {
                project.EnableAutoAddOns(true);
            }
            try
            {
                parser.ParseProject(project, xmePath);
                project.Save();
            }
            finally
            {
                project.Close();
            }
        }
    }
}