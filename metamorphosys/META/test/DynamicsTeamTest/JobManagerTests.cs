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

// -----------------------------------------------------------------------
// <copyright file="JobManagerTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DynamicsTeamTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;
    using System.IO;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class JobManagerTests
    {
        private List<string> searchLocations = new List<string>()
            {
                // n.b. Assembly.Location is wrong with Shadow Copy enabled
                 META.VersionInfo.MetaPath + @"bin\JobManager.exe",
                 META.VersionInfo.MetaPath + @"src\JobManager\JobManager\bin\Release\JobManager.exe",
                 META.VersionInfo.MetaPath + @"src\JobManager\JobManager\bin\Debug\JobManager.exe",
            };

        [Fact]
        [Trait("JobManager", "OpenAndClose")]
        public void OpenAndCloseJobManager()
        {
            Assert.DoesNotThrow(() =>
            {
                string exe = searchLocations.Where(File.Exists).FirstOrDefault();
                if (exe == null)
                {
                    throw new Exception(string.Format("Job Manager was not found on your computer. Make sure your META installer is healthy. Search locations: {0}", string.Join(", ", searchLocations)));
                }

                Console.Out.WriteLine(exe);

                Process proc = new Process();
                proc.StartInfo.Arguments = "-i";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.FileName = exe;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                ManualResetEvent task = new ManualResetEvent(false);
                using (proc)
                using (task)
                {
                    StringBuilder stdoutData = new StringBuilder();
                    StringBuilder stderrData = new StringBuilder();
                    proc.OutputDataReceived += (o, args) =>
                    {
                        if (args.Data != null)
                        {
                            lock (stdoutData)
                            {
                                stdoutData.Append(args.Data);
                                // matches Console.Out.WriteLine("JobManager has started"); in JobManager
                                try
                                {
                                    task.Set();
                                }
                                catch (ObjectDisposedException) { }
                            }
                        }
                    };
                    proc.ErrorDataReceived += (o, args) =>
                    {
                        if (args.Data != null)
                        {
                            lock (stderrData)
                            {
                                stderrData.Append(args.Data);
                            }
                        }
                    };
                    proc.Start();
                    proc.BeginErrorReadLine();
                    proc.BeginOutputReadLine();
                    proc.StandardInput.Close();

                    try
                    {
                        var tokenSource = new CancellationTokenSource();
                        int timeOut = 10000; // ms
                        if (task.WaitOne(timeOut) == false)
                        {
                            Console.WriteLine("The Task timed out!");
                            Assert.True(false, string.Format("JobManager did not write anything to the standard output after start. Operation timed out after {0}  ms.", timeOut));
                        }
                        lock (stdoutData)
                        {
                            Assert.True(stdoutData.ToString().Contains("JobManager has started"));
                        }
                    }
                    finally
                    {
                        if (proc.HasExited == false)
                        {
                            // successfully opened now kill it
                            try
                            {
                                proc.Kill();
                            }
                            catch (System.InvalidOperationException) { } // possible race with proc.HasExited
                        }
                    }
                }

            });
        }
    }
}
