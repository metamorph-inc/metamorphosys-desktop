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

namespace JobManager.Jenkins.Build
{
    public class Caus
    {
        public string shortDescription { get; set; }
        public object userId { get; set; }
        public string userName { get; set; }
    }

    public class Action
    {
        public List<Caus> causes { get; set; }
        public int? failCount { get; set; }
        public int? skipCount { get; set; }
        public int? totalCount { get; set; }
        public string urlName { get; set; }
    }

    public class Artifact
    {
        public string displayPath { get; set; }
        public string fileName { get; set; }
        public string relativePath { get; set; }
    }

    public class Revision
    {
        public string module { get; set; }
        public int revision { get; set; }
    }

    public class ChangeSet
    {
        public List<object> items { get; set; }
        public string kind { get; set; }
        public List<Revision> revisions { get; set; }
    }

    public class Culprit
    {
        public string absoluteUrl { get; set; }
        public string fullName { get; set; }
    }

    public class Build
    {
        public List<Action> actions { get; set; }
        public List<Artifact> artifacts { get; set; }
        public bool building { get; set; }
        public object description { get; set; }
        public int duration { get; set; }
        public int estimatedDuration { get; set; }
        public object executor { get; set; }
        public string fullDisplayName { get; set; }
        public string id { get; set; }
        public bool keepLog { get; set; }
        public int number { get; set; }
        public string result { get; set; }
        public long timestamp { get; set; }
        public string url { get; set; }
        public string builtOn { get; set; }
        public ChangeSet changeSet { get; set; }
        public List<Culprit> culprits { get; set; }
    }
}
