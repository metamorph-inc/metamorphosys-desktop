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
import os
import os.path
import posixpath
import subprocess

THIS_DIR = os.path.dirname(os.path.abspath(__file__))

def is_dev_branch(branch_name):
    return 'META-' in branch_name

def get_branch_name():
    try:
        svn_info = subprocess.check_output(['svn', 'info'], cwd=os.path.join(THIS_DIR, '..'))
    except subprocess.CalledProcessError as e:
        return 'export'
    svn_info_lines = svn_info.replace('\r', '').split('\n')
    return posixpath.basename([line for line in svn_info_lines if line.find('URL: ') == 0][0][5:])

def svnversion():
    version = subprocess.check_output(['svnversion.exe'], cwd=os.path.abspath(os.path.join(THIS_DIR, '..'))).strip()
    # memoize
    setattr(sys.modules[svnversion.__module__], 'svnversion', lambda: version)
    return version

def last_cad_rev():
    meta_path = os.path.normpath(os.path.join(THIS_DIR, '..'))
    return max((last_changed_rev(os.path.join(meta_path, path)) for path in ('src/CADAssembler', 'meta/CAD')))

def last_mdl2mga_rev():
    meta_path = os.path.normpath(os.path.join(THIS_DIR, '..'))
    return max((last_changed_rev(os.path.join(meta_path, path)) for path in ('externals', 'meta/Cyber')))
   
def last_changed_rev(path):
    output = subprocess.check_output(['svn.exe', 'info', path])
    import re
    return int(re.search('Last Changed Rev: (\\d+)', output).groups()[0])
    
def prerelease_suffix():
    branch = get_branch_name()
    if is_dev_branch(branch):
        return '-' + branch + 'r' + svnversion().split(':')[0]
    return ''

def update_version(version, svnversion):
    version = version.split(".")
    version[-1] = str(svnversion)
    return ".".join(version)
    
if __name__ == '__main__':
    print get_branch_name()
