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

MSBUILD = os.path.join(os.environ['windir'], r"Microsoft.NET\Framework\v4.0.30319\MSBuild.exe")
THIS_DIR = os.path.dirname(os.path.abspath(__file__))
nuget = os.path.join(THIS_DIR, "../.nuget/NuGet.exe")
version_filename = os.path.join(THIS_DIR, 'Properties/AssemblyFileVersion.cs')

def system(args, dirname=THIS_DIR):
    import subprocess
    subprocess.check_call(args, shell=True, cwd=dirname)

def get_svnversion(filename):
    import subprocess
    p = subprocess.Popen(['svnversion', '-n', filename], stdout=subprocess.PIPE)
    out, err = p.communicate()
    if p.returncode:
        raise subprocess.CalledProcessError(p.returncode, 'svnversion')
    return out

def update_version():
    # system([MSBUILD, os.path.join(THIS_DIR, "../.nuget/NuGet.Targets"), "/t:CheckPrerequisites", "/p:DownloadNuGetExe=True"])
    cyphy_version = int(get_svnversion(os.path.join(THIS_DIR, "../../meta/CyPhyML/CyPhyML.xme")))
    print "CyPhyML.xme version: " + str(cyphy_version)
    version_str = '1.0.0.%d' % cyphy_version
    cyphy_version_data = 'using System.Reflection;\n' + '[assembly: AssemblyFileVersion("%s")]\n' % version_str
    if not os.path.isfile(version_filename) or cyphy_version_data != file(version_filename, 'rb').read():
        with file(version_filename, 'wb') as cyphy_version_cs:
            cyphy_version_cs.write(cyphy_version_data)


def build():
    system([MSBUILD, os.path.join(THIS_DIR, "../CyPhyML.sln"), "/t:CyPhyLanguage\\CyPhyMLCS", "/p:Configuration=Release;Platform=Mixed Platforms", "/m", "/nodeReuse:false"])

def _parse_version():
    return file(version_filename, 'rb').read().split('"')[1]
    
def pack_nuget():
    system([nuget, "pack", os.path.join(THIS_DIR, "CyPhyMLCS.csproj"),
        "-Verbosity", "detailed",
        "-Version", _parse_version(),
        "-BasePath", THIS_DIR])

def push_nuget():
    system([nuget, "push", "META.CyPhyML.%s.nupkg" % _parse_version(),
        "-Source", "http://build.isis.vanderbilt.edu/"])
    

if __name__ == '__main__':
    for command in sys.argv[1:]:
        getattr(sys.modules['__main__'], command)()
