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

public class Statistics
{
    /// <summary>
    /// Vehicle forge user Id
    /// </summary>
    public string UserId { get; set; }

    public int Id { get; set; }

    public bool IsRemote { get; set; }

    /// <summary>
    /// VehicleForge instance where the job got submitted
    /// </summary>
    public string VFUrl { get; set; }

    public class Execution
    {
        /// <summary>
        /// Start time of this execution
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// Job got into the queue
        /// </summary>
        public class StatusTrace
        {
            public string Status { get; set; }
            public string TimeStamp { get; set; }
        }
        public List<StatusTrace> StatusTraces { get; set; }

        public Execution()
        {
            this.StatusTraces = new List<StatusTrace>();
        }
    }

    public List<Execution> Executions { get; set; }

    /// <summary>
    /// Job received from the master interpreter
    /// </summary>
    public string JobReceived { get; set; }

    /// <summary>
    /// Name of the job
    /// </summary>
    public string JobName { get; set; }

    /// <summary>
    /// Command that is being executed
    /// </summary>
    public string RunCommand { get; set; }

    /// <summary>
    /// Size of source_data.zip
    /// </summary>
    public string SizeOfSourcePackage { get; set; }

    /// <summary>
    /// Creation time of this file
    /// </summary>
    public object ToolSpecifics { get; set; }

    public Statistics()
    {
        this.Executions = new List<Execution>();
    }
}
