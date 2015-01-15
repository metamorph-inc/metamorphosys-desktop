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

namespace JobManager.Jenkins.Job
{
    public class Action
    {
    }

    public class Build
    {
        public int number { get; set; }
        public string url { get; set; }
    }

    public class FirstBuild
    {
        public int number { get; set; }
        public string url { get; set; }
    }

    public class HealthReport
    {
        public string description { get; set; }
        public string iconUrl { get; set; }
        public int score { get; set; }
    }

    public class LastBuild
    {
        public int number { get; set; }
        public string url { get; set; }
    }

    public class LastCompletedBuild
    {
        public int number { get; set; }
        public string url { get; set; }
    }

    public class LastFailedBuild
    {
        public int number { get; set; }
        public string url { get; set; }
    }

    public class LastStableBuild
    {
        public int number { get; set; }
        public string url { get; set; }
    }

    public class LastSuccessfulBuild
    {
        public int number { get; set; }
        public string url { get; set; }
    }

    public class LastUnstableBuild
    {
        public int number { get; set; }
        public string url { get; set; }
    }

    public class LastUnsuccessfulBuild
    {
        public int number { get; set; }
        public string url { get; set; }
    }

    public class Scm
    {
    }

    public class Task
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class QueueItem
    {
        public bool blocked { get; set; }
        public bool buildable { get; set; }
        public int id { get; set; }
        public long inQueueSince { get; set; }
        public string @params { get; set; }
        public bool stuck { get; set; }
        public Task task { get; set; }
        public string why { get; set; }
        public long buildableStartMilliseconds { get; set; }
    }

    public class Job
    {
        // public List<Action> actions { get; set; }
        // public string description { get; set; }
        // public string displayName { get; set; }
        // public object displayNameOrNull { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public bool buildable { get; set; }
        public List<Build> builds { get; set; }
        public string color { get; set; }
        public FirstBuild firstBuild { get; set; }
        public List<HealthReport> healthReport { get; set; }
        public bool inQueue { get; set; }
        public bool keepDependencies { get; set; }
        public LastBuild lastBuild { get; set; }
        public LastCompletedBuild lastCompletedBuild { get; set; }
        public LastFailedBuild lastFailedBuild { get; set; }
        public LastStableBuild lastStableBuild { get; set; }
        public LastSuccessfulBuild lastSuccessfulBuild { get; set; }
        public LastUnstableBuild lastUnstableBuild { get; set; }
        public LastUnsuccessfulBuild lastUnsuccessfulBuild { get; set; }
        public int nextBuildNumber { get; set; }
        public List<object> property { get; set; }
        public QueueItem queueItem { get; set; }
        public bool concurrentBuild { get; set; }
        public List<object> downstreamProjects { get; set; }
        public Scm scm { get; set; }
        public List<object> upstreamProjects { get; set; }
    }
}
