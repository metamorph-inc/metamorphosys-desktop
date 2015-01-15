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
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace JobManager
{
    public abstract class Job : MarshalByRefObject
    {
        // TODO: change this id to guid
        public int Id { get; protected set; }
        public string Title { get; set; }
        public string TestBenchName { get; set; }
        public string WorkingDirectory { get; set; }
        public string RunCommand { get; set; }

        public DateTime TimePosted { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeDone { get; set; }

        public TimeSpan TimeInQueue { get; set; }
        public TimeSpan TimeTotal { get; set; }

        public enum StatusEnum
        {
            WaitingForStart,
            Ready,
            QueuedLocal,
            RunningLocal,
            UploadPackage,
            ZippingPackage,
            PostedToServer,
            StartedOnServer,
            QueuedOnServer,
            RunningOnServer,
            DownloadResults,
            RedownloadQueued,
            AbortOnServerRequested,
            Succeeded,
            FailedToUploadServer,
            FailedToDownload,
            FailedAbortOnServer,
            FailedExecution,
            Failed,
        }

        public abstract StatusEnum Status
        {
            get;
            set;
        }

        public enum TypeEnum
        {
            Command,
            Matlab,
            CAD,
        }

        public bool IsFailed()
        {
            return Job.IsFailedStatus(Status);
        }

        public static bool IsFailedStatus(Job.StatusEnum status)
        {
            return status == StatusEnum.FailedExecution ||
                   status == StatusEnum.FailedAbortOnServer ||
                   status == StatusEnum.FailedToUploadServer ||
                   status == StatusEnum.FailedToDownload ||
                   status == StatusEnum.Failed;
        }

        public TypeEnum Type { get; set; }

        public string Labels { get; set; }

        public string BuildQuery { get; set; }

        /// <summary>
        /// zip.py server side hook
        /// </summary>
        public string ResultsZip { get; set; }

        public const string LabelVersion = "14.10";
        public const string DefaultLabels = "Windows" + LabelVersion;
        public const string DefaultBuildQuery = "";
    }

    public abstract class JobServer : MarshalByRefObject
    {
        public string JenkinsUrl { get; set; }
        public string UserName { get; set; }
        public bool IsRemote { get; set; }

        public abstract Job CreateJob();

        public abstract SoT CreateSoT();

        public List<Job> Jobs { get; set; }
        public List<SoT> SoTs { get; set; }

        public abstract void AddJob(Job job);

        public abstract void AddSoT(SoT sot);
    }

}
