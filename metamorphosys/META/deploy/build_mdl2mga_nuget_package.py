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

import os
import os.path
import sys
from win32api import GetFileVersionInfo, HIWORD, LOWORD
import subprocess

THIS_DIR = os.path.dirname(os.path.abspath(__file__))
nuget = os.path.join(THIS_DIR, "../src/.nuget/NuGet.exe")

sys.path.append(THIS_DIR)
import svn_info

package_name = 'MDL2MGACyber'

def system(args, dirname=THIS_DIR):
    subprocess.check_call(args, shell=True, cwd=dirname)

def _get_version():
    from xml.etree import ElementTree
    nuspec = ElementTree.parse(os.path.join(THIS_DIR, '%s.nuspec' % package_name))
    nuspec_version = nuspec.find('metadata/version').text

    _version = svn_info.update_version(nuspec_version, svn_info.last_mdl2mga_rev())
    return _version

def pack_nuget():
    with open(os.path.join(THIS_DIR, 'svnversion'), 'wb') as svnversion:
        svnversion.write(svn_info.svnversion())
    system([nuget, "pack", os.path.join(THIS_DIR, package_name + ".nuspec"),
        "-Verbosity", "detailed",
        "-Version", _get_version(),
        "-BasePath", THIS_DIR])

def push_nuget():
    system([nuget, "push", "META.%s.%s.nupkg" % (package_name, _get_version()),
        "-Source", "http://build.isis.vanderbilt.edu/"])
    
if __name__ == '__main__':
    for command in sys.argv[1:] or ['pack_nuget']:
        getattr(sys.modules['__main__'], command)()
