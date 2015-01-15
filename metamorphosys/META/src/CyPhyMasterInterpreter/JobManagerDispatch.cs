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
using System.Linq;
using System.Text;
using JobManager;
using META;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace CyPhyMasterInterpreter
{
    public class JobManagerDispatch
    {
        public Uri JobServerConnection = new Uri("tcp://" + System.Net.IPAddress.Loopback.ToString() + ":35010/JobServer");

        public Queue<KeyValuePair<JobServer, Job>> jobsToAdd = new Queue<KeyValuePair<JobServer, Job>>();
        public void AddJobs()
        {
            try
            {
                foreach (var j in jobsToAdd)
                {
                    j.Value.Status = Job.StatusEnum.Ready;
                    j.Key.AddJob(j.Value);
                }
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                throw new Exception("JobManager is not running. Please start it.", ex);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("=> Job cannot be posted: {0}", e.ToString()));
            }
            finally
            {
                jobsToAdd.Clear();
            }
        }

        public Queue<KeyValuePair<JobServer, SoT>> sotsToAdd = new Queue<KeyValuePair<JobServer, SoT>>();
        public void AddSoTs()
        {
            try
            {
                foreach (var sot in sotsToAdd)
                {
                    sot.Key.AddSoT(sot.Value);
                }
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                throw new Exception("JobManager is not running. Please start it.", ex);
            }
            catch (Exception e)
            {
                // TODO: just log
                throw new Exception(string.Format("=> Job cannot be posted: {0}", e.ToString()));
            }
            finally
            {
                sotsToAdd.Clear();
            }
        }

        public bool EnqueueJob(
            string runCommand,
            string title,
            string testbenchName,
            string workingDirectory,
            ComComponent interpreter,
            Job.TypeEnum type = Job.TypeEnum.Command)
        {
            // TODO: cut down the number of input variables. interpreter and title should be enough
            try
            {
                JobServer manager;
                Job j = CreateJob(out manager);

                j.RunCommand = runCommand;
                j.Title = title;
                j.TestBenchName = testbenchName;
                j.WorkingDirectory = workingDirectory;
                j.Type = type;

                // TODO: allow empty Labels
                j.Labels = String.IsNullOrWhiteSpace(interpreter.result.Labels) ?
                    Job.DefaultLabels :
                    interpreter.result.Labels;

                j.BuildQuery = String.IsNullOrWhiteSpace(interpreter.result.BuildQuery) ?
                    Job.DefaultBuildQuery :
                    interpreter.result.BuildQuery;

                if (String.IsNullOrWhiteSpace(interpreter.result.ZippyServerSideHook) == false)
                {
                    j.ResultsZip = interpreter.result.ZippyServerSideHook as string;
                }

                jobsToAdd.Enqueue(new KeyValuePair<JobServer, Job>(manager, j));
                return true;

            }
            catch (System.Net.Sockets.SocketException ex)
            {
                throw new Exception("JobManager is not running. Please start it.", ex);
            }
            catch (Exception e)
            {
                // TODO: just log
                throw new Exception(string.Format("=> Job cannot be posted: {0}", e.ToString()));
            }
        }

        public bool EnqueueSoT(string workingDirectory)
        {
            try
            {
                JobServer manager;
                SoT sot = CreateSoT(out manager);
                sot.WorkingDirectory = workingDirectory;

                sotsToAdd.Enqueue(new KeyValuePair<JobServer, SoT>(manager, sot));
                return true;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                throw new Exception("JobManager is not running. Please start it.", ex);
            }
            catch (Exception e)
            {
                // TODO: just log
                throw new Exception(string.Format("=> Job cannot be posted: {0}", e.ToString()));
            }
        }

        private SoT CreateSoT(out JobServer manager)
        {
            SoT sot;
            try
            {
                manager = (JobServer)Activator.GetObject(typeof(JobServer), JobServerConnection.OriginalString);
                sot = manager.CreateSoT();
            }
            catch (System.Net.Sockets.SocketException)
            {
                this.StartJobManager();
                manager = (JobServer)Activator.GetObject(typeof(JobServer), JobServerConnection.OriginalString);
                sot = manager.CreateSoT();
            }
            return sot;
        }

        private Job CreateJob(out JobServer manager)
        {
            Job j;
            try
            {
                manager = (JobServer)Activator.GetObject(typeof(JobServer), JobServerConnection.OriginalString);
                j = manager.CreateJob();
            }
            catch (System.Net.Sockets.SocketException)
            {
                this.StartJobManager();
                manager = (JobServer)Activator.GetObject(typeof(JobServer), JobServerConnection.OriginalString);
                j = manager.CreateJob();
            }
            return j;
        }

        private void StartJobManager()
        {
            // n.b. Assembly.Location is wrong with Shadow Copy enabled
            string assemblyDir = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            string exe = Path.Combine(assemblyDir, "JobManager.exe");
            if (!File.Exists(exe))
                exe = Path.Combine(assemblyDir, "..\\..\\..\\JobManager\\JobManager\\bin\\Release\\JobManager.exe");
            if (!File.Exists(exe))
                exe = Path.Combine(assemblyDir, "..\\..\\..\\JobManager\\JobManager\\bin\\Debug\\JobManager.exe");
            if (File.Exists(exe))
            {
                Process proc = new Process();
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.FileName = exe;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();
                proc.WaitForInputIdle(10 * 1000);
                proc.StandardOutput.ReadLine(); // matches Console.Out.WriteLine("JobManager has started"); in JobManager
                //System.Threading.Thread.Sleep(3 * 1000);
            }
            else
            {
                throw new Exception("Job Manager was not found on your computer. Make sure your META installer is healthy.");
            }
        }
    }
}
