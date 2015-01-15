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

import sys
import datetime
import urlparse
import json
import itertools
import glob
import datetime
import sqlite3

from delete_old_jobs import get, post, root_url

from collections import defaultdict, OrderedDict

results = defaultdict(int)
users = defaultdict(int)
dates = OrderedDict()

if __name__=='__main__':
    conn = sqlite3.connect('jobs.db')
    c = conn.cursor()
    for row in c.execute('SELECT name,json from jobs'):
        job_name,job = row
        job = json.loads(job)
        print job['url']
        if job['lastCompletedBuild'] and not job['lastCompletedBuild']['building']:
            build = job['lastCompletedBuild']
            #print build['result']
            results[build['result']] = results[build['result']] + 1
            causes = [d for d in build['actions'] if isinstance(d, dict) and 'causes' in d]
            if causes:
                user = [d.get('userName', 'unknown') for d in causes[0].get('causes', {}) if isinstance(d, dict) and 'userName' in d]
                user = (user or ['unknown'])[0]
                #print user
                users[user] += 1
            #import pdb;pdb.set_trace()
            date = datetime.datetime.fromtimestamp(job['lastCompletedBuild']['timestamp']/1000).date().isoformat()
            dates[date] = dates.get(date, 0) + 1
        else:
            results['NOT_BUILT'] = results['NOT_BUILT'] + 1

#lastSuccessfulBuild':{'number':1,'url':'http://10.2.204.106:8080/job/Sandeep_Neema_CyPhy2Modelica_NewComponentAssembly_cfg18_00051_C52EA554/1/'}       
#{'causes':[{'shortDescription':'Started by user neemask','userName':'neemask'}

    print dict(results)
    print dict(users)
    print dates