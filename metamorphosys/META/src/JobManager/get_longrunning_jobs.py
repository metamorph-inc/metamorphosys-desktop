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

#!/home/ubuntu/requests/bin/python

import jenkinsapi
from jenkinsapi.jenkins import Jenkins
import requests
import time
import calendar
import datetime


old_threshold = datetime.timedelta(hours=24)

def munge_url(url):
    return url.replace("10.2.204.106:8080", "localhost:9999")

import jenkinsapi.jenkinsbase
def _poll(self):
    url = self.python_api_url(self.baseurl)
    url = munge_url(url)
    #print url
    return self.get_data(url)

jenkinsapi.jenkinsbase.JenkinsBase._poll = _poll

import jenkinsapi.build
def _poll(self):
    #For build's we need more information for downstream and upstream builds
    #so we override the poll to get at the extra data for build objects
    url = self.python_api_url(self.baseurl) + '?depth=2'
    url = munge_url(url)
    #print url
    return self.get_data(url)

jenkinsapi.build.Build._poll = _poll


j = Jenkins('http://localhost:9999')
execs = []
for (name, node) in j.get_nodes().iteritems():
    execs.extend((exec_ for exec_ in j.get_executors(name).__iter__() if not exec_.is_idle()))

for exec_ in execs:
    exec_url = exec_.get_current_executable()['url']
    timestamp = requests.get(munge_url(exec_url + 'api/json')).json()['timestamp']
    delta = datetime.timedelta(seconds=calendar.timegm(time.gmtime()) - timestamp/1000)
    if delta > old_threshold:
        print exec_url + " " + str(delta)

#import pytz
#naive_timestamp = datetime.datetime(*time.gmtime(timestamp / 1000.0)[:6])
#pytz.utc.localize(naive_timestamp)

    
#for (job_name, job) in j.get_jobs():
#    if job.is_running():
#        print job_name

