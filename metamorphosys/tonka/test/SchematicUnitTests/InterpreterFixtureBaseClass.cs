using System;
using System.IO;
using GME.MGA;
using Xunit;

namespace SchematicUnitTests
{
    public abstract class InterpreterFixtureBaseClass : IDisposable
    {
        public String path_Test
        {
            get
            {
                return Path.GetDirectoryName(path_XME);
            }
        }

        public abstract String path_XME { get; }

        public readonly String path_MGA;
        public MgaProject proj { get; private set; }

        public InterpreterFixtureBaseClass()
        {
            String mgaConnectionString;
            GME.MGA.MgaUtils.ImportXMEForTest(path_XME, out mgaConnectionString);
            path_MGA = mgaConnectionString.Substring("MGA=".Length);

            Assert.True(File.Exists(Path.GetFullPath(path_MGA)),
                        String.Format("{0} not found. Model import may have failed.", path_MGA));

            if (Directory.Exists(Path.Combine(path_Test, "output")))
            {
                Directory.Delete(Path.Combine(path_Test, "output"), true);
            }
            if (Directory.Exists(Path.Combine(path_Test, "results")))
            {
                Directory.Delete(Path.Combine(path_Test, "results"), true);
            }

            proj = new MgaProject();
            bool ro_mode;
            proj.Open("MGA=" + Path.GetFullPath(path_MGA), out ro_mode);
            proj.EnableAutoAddOns(true);

            // Ensure "~/Documents/eagle" exists
            var pathDocEagle = Path.Combine(Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%"),
                                            "Documents",
                                            "eagle");
            if (!Directory.Exists(pathDocEagle))
            {
                Directory.CreateDirectory(pathDocEagle);
            }
        }

        public void Dispose()
        {
            proj.Save();
            proj.Close();
            proj = null;
        }
    }
}
