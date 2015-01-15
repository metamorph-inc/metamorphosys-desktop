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


# OR from console, delete unrun jobs:
# var scr = document.createElement("script"); scr.src = "https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"; document.body.appendChild(scr);
# x = $( "div.dashboard" ).find("img[alt='Aborted']").parent().parent().find("a[href^=job]").not("a[href*=build]")
# x.each(function() { $.ajax(this.href + 'doDelete', {type:'POST' }) })
# also try alt='Not Built'

# clear queue:
# var scr = document.createElement("script"); scr.src = "https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"; document.body.appendChild(scr);
# $( "table#buildQueue" ).find("a.model-link[href*='/component_unit_test']").parent().parent().find("a[href$='cancelQueue']").each(function() { $.ajax(this.href) } )

# x = $( "table#buildQueue" ).find("a.model-link").parent().parent().find("a[href*='/job/']")
# x.each(function() { $.ajax(this.href + 'doDelete', {type:'POST'}) })

import sys
import datetime
import json
import urlparse
import requests

if len(sys.argv) > 1:
    root_url = sys.argv[1]
else:
    #root_url = 'http://meta-vm0/'
    #root_url = 'http://kms1.isis.vanderbilt.edu:8888/'
    root_url = 'http://localhost:8080/'
    #root_url = 'http://129.59.105.249:8080/'

delta = datetime.timedelta(days=-10)

jobname_prefix = None
#jobname_prefix = 'pmeijer'
#jobname_prefix = 'meta'
#jobname_prefix = 'ksmyth'

username='ksmyth'

def _request(method, *args, **kwargs):
    args = list(args)
    # TODO: configure for firewall/reverse proxy/etc
    #args[0] = args[0].replace(':8080', '')
    url = urlparse.urlparse(args[0])
    rurl = urlparse.urlparse(root_url)
    args[0] = urlparse.ParseResult(rurl.scheme, rurl.netloc, url.path, url.params, url.query, url.fragment).geturl()
    kwargs.setdefault('cookies', {})
    kwargs['cookies'].update(dict(username=username))
    kwargs.setdefault('headers', {})
    kwargs['headers']['X-Forwarded-User'] = username
    import requests
    m = getattr(requests, method)
    ret = m(*args, **kwargs)
    if ret.status_code > 399:
        ret.raise_for_status()
    return ret

def get(*args, **kwargs):
    return _request('get', *args, **kwargs)

def post(*args, **kwargs):
    return _request('post', *args, **kwargs)

def get_build_time(url):
    t = get(url + 'buildTimestamp?format=yyyy/MM/dd%20HH:mm:ss')
    return datetime.datetime.strptime(t.content, '%Y/%m/%d %H:%M:%S')

def delete_job(url):
    try:
        post(url + 'doWipeOutWorkspace')
    except requests.exceptions.HTTPError:
        pass
    post(url + 'doDelete') 

def main():
    root = get(root_url + "/api/json")
    for job in json.loads(root.content)['jobs']:
        print job['url']
        if jobname_prefix and job['url'].find(jobname_prefix) == -1:
            continue
        j = get(job['url'] + 'api/json')
        j_json = json.loads(j.content)
        if j_json['name'] == "test_job" or j_json['name'] == "component_unit_test": continue # monitored by VF
        lastFailed = lastSuccess = datetime.datetime.max
        if j_json['lastSuccessfulBuild']:
            lastSuccess = get_build_time(j_json['lastSuccessfulBuild']['url'])
        if j_json['lastFailedBuild']:
            lastFailed = get_build_time(j_json['lastFailedBuild']['url'])
        if lastSuccess < datetime.datetime.now() + delta or lastFailed < datetime.datetime.now() + delta:
            print 'delete ' + j_json['name']
            delete_job(j_json['url'])
        if jobname_prefix and j_json['name'].startswith(jobname_prefix):
            print 'delete ' + j_json['name']
            delete_job(j_json['url'])
        if lastFailed == lastSuccess == datetime.datetime.max:
            print 'never built ' + j_json['name']
            # delete_job(j_json['url'])

if __name__=='__main__':
    main()
