# Copyright (C) 2013-2015 MetaMorph Software, Inc

# Permission is hereby granted, free of charge, to any person obtaining a
# copy of this data, including any software or models in source or binary
# form, as well as any drawings, specifications, and documentation
# (collectively "the Data"), to deal in the Data without restriction,
# including without limitation the rights to use, copy, modify, merge,
# publish, distribute, sublicense, and/or sell copies of the Data, and to
# permit persons to whom the Data is furnished to do so, subject to the
# following conditions:

# The above copyright notice and this permission notice shall be included
# in all copies or substantial portions of the Data.

# THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
# THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
# LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
# OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
# WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

# =======================
# This version of the META tools is a fork of an original version produced
# by Vanderbilt University's Institute for Software Integrated Systems (ISIS).
# Their license statement:

# Copyright (C) 2011-2014 Vanderbilt University

# Developed with the sponsorship of the Defense Advanced Research Projects
# Agency (DARPA) and delivered to the U.S. Government with Unlimited Rights
# as defined in DFARS 252.227-7013.

# Permission is hereby granted, free of charge, to any person obtaining a
# copy of this data, including any software or models in source or binary
# form, as well as any drawings, specifications, and documentation
# (collectively "the Data"), to deal in the Data without restriction,
# including without limitation the rights to use, copy, modify, merge,
# publish, distribute, sublicense, and/or sell copies of the Data, and to
# permit persons to whom the Data is furnished to do so, subject to the
# following conditions:

# The above copyright notice and this permission notice shall be included
# in all copies or substantial portions of the Data.

# THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
# THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
# LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
# OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
# WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

import datetime
import json

class ValueWithUnit:
    def __init__(self):
        self.Name = None
        self.Unit = None
        self.Value = None

class Artifact:
    def __init__(self):
        self.Location = None
        self.Tag = None

class Parameter(ValueWithUnit):
    def __init__(self):
        ValueWithUnit.__init__(self)

class Metric(ValueWithUnit):
    def __init__(self):
        ValueWithUnit.__init__(self)

class Step:
    def __init__(self):
        self.Invocation = None
        self.PreProcess = None
        self.PostProcess = None
        self.Description = None

        # REST, Python, or CMD
        self.Type = None

        # Use ISO-8601 via datetime.datatime.now().isoformat()
        self.ExecutionStartTimestamp = None
        self.ExecutionCompletionTimestamp = None

        self.Parameters = []

class TestBenchManifest:
    def __init__(self):
        self.Steps = []
        self.Metrics = []
        self.Artifacts = []
        self.Parameters = []

        self.Name = None
        self.Created = datetime.datetime.now().isoformat()
        self.DesignID = None

        # unexecuted, in-progress, complete
        self.Status = "unexecuted"

    def SerializeToString(self):
        return json.dumps(self.__dict__,sort_keys=True,indent=2,cls=self.TBJSONEncoder)

    def SerializeToFile(self,path):
        f = open(path,'w')
        json.dump(self,f,indent=2,sort_keys=True,cls=self.TBJSONEncoder)

    class TBJSONEncoder(json.JSONEncoder):
        def default(self,obj):
            return obj.__dict__