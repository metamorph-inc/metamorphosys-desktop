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
import requests
import datetime
import json
import urlparse

if len(sys.argv) > 1:
    root_url = sys.argv[1]
else:
    root_url = 'http://localhost:8080/'
    #root_url = 'http://meta-vm0/'
    #root_url = 'http://kms1.isis.vanderbilt.edu:9999/'


node_prefix = 'template-test-slave'
num_new_slaves = 3

#slave_executors = 4
#slave_root_fs = r'c:\jenkins_builds'
#slave_labels = 'Windows13.1 OpenModelica py_modelica13.1 Windows12.12 Dymola PCC12.08'

username='zsolt'

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
    post(url + 'doWipeOutWorkspace')
    post(url + 'doDelete') 

    
    
def main():
    root = get(root_url + "computer/api/json")
    template_node = None
    node_names = []
    for computer in json.loads(root.content)['computer']:
        node_names.append(computer['displayName'])
        if node_prefix == computer['displayName']:
            template_node= computer
    
    print node_names
    
    if not template_node:
        print node_prefix + ' was NOT found'
        return
    
    if node_prefix:
        for i in range(0, num_new_slaves):
            new_node_name = '{0}-{1}'.format(node_prefix, i)
            print new_node_name
            # if node exists delete first
            if new_node_name in node_names:
                print 'deleting first ' + new_node_name
                get(root_url + "computer/" + new_node_name + "/delete")
    
            with requests.session() as s: 
                print 'Sending create ' + new_node_name
                s.post(root_url + 'computer/createItem', {
                    'name':new_node_name,
                    'mode':'copy',
                    'from': node_prefix,
                    'json': {
                        "name": new_node_name, 
                        "mode": "copy",
                        "from": node_prefix,
                        "Submit": "OK"
                        }})

if __name__=='__main__':
    main()