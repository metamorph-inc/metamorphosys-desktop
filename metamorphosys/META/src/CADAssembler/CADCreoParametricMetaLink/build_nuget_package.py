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

MSBUILD = os.path.join(os.environ['windir'], r"Microsoft.NET\Framework\v4.0.30319\MSBuild.exe")
THIS_DIR = os.path.dirname(os.path.abspath(__file__))
nuget = os.path.join(THIS_DIR, "../../.nuget/NuGet.exe")

sys.path.append(os.path.join(THIS_DIR, '../../../deploy'))
import svn_info

def system(args, dirname=THIS_DIR):
    subprocess.check_call(args, shell=True, cwd=dirname)

def _get_version_number(filename):
    info = GetFileVersionInfo (filename, "\\")
    ms = info['FileVersionMS']
    ls = info['FileVersionLS']
    return (HIWORD(ms), LOWORD(ms), HIWORD(ls), LOWORD(ls))

def _get_version():
    version = ".".join(map(str, _get_version_number(os.path.join(THIS_DIR, r"..\x64\Release\CADCreoParametricMetaLink.exe"))))
    return svn_info.update_version(version, svn_info.last_cad_rev())
  
def pack_nuget():
    with open(os.path.join(THIS_DIR, 'svnversion'), 'wb') as svnversion:
        svnversion.write(svn_info.svnversion())
    int(svn_info.svnversion()) # fail if there are local modifications (or partly switched, or mixed versions)
    system([nuget, "pack", os.path.join(THIS_DIR, "CADCreoParametricMetaLink.nuspec"),
        "-Verbosity", "detailed",
        "-Version", _get_version(),
        "-BasePath", THIS_DIR])

def push_nuget():
    system([nuget, "push", os.path.join(THIS_DIR, "META.CADCreoParametricMetaLink.%s.nupkg" % _get_version()),
        "-Source", "http://build.isis.vanderbilt.edu/"])
    

if __name__ == '__main__':
    for command in sys.argv[1:]:
        getattr(sys.modules['__main__'], command)()
