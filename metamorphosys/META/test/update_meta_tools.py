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

import urllib
import subprocess
import argparse
import sys

from msilib import OpenDatabase, MSIDBOPEN_READONLY

def GetMsiProperty(path ,property):
    db = OpenDatabase(path, MSIDBOPEN_READONLY)
    view = db.OpenView ("SELECT Value FROM Property WHERE Property='" + property + "'")
    view.Execute(None)
    result = view.Fetch()
    #print dir(result)
    return result.GetString(1)
  
def query_META_install():
    import _winreg
    current_version = r'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall'
 
    current_version_key = _winreg.OpenKey(_winreg.HKEY_LOCAL_MACHINE, current_version, 0,
                                         _winreg.KEY_READ | _winreg.KEY_WOW64_64KEY)
 
    number_of_keys =  _winreg.QueryInfoKey(current_version_key)[0]  # 0 means number of sub_keys

    info = {}
    for sub_key_id in range(0, number_of_keys):
        sub_key_name = _winreg.EnumKey(current_version_key, sub_key_id)
        sub_key = _winreg.OpenKey(current_version_key, sub_key_name)
        
        number_of_values = _winreg.QueryInfoKey(sub_key)[1]
        found = False
        for value_id in range(0, number_of_values):
            value_tuple = _winreg.EnumValue(sub_key, value_id)
            value_name = value_tuple[0]
            value = value_tuple[1]
            
            if value_name == 'DisplayName' and value == 'META toolchain':
                found = True
                break
        
        if found:
            for value_id in range(0, number_of_values):
                value_tuple = _winreg.EnumValue(sub_key, value_id)
                value_name = value_tuple[0]
                value = value_tuple[1]
                info[value_name] = value
            break
        

    return info

def main():

    parser = argparse.ArgumentParser()
    parser.add_argument("-i", "--installer", default="http://build.isis.vanderbilt.edu/view/META_ReleaseCandidate/job/META_ReleaseCandidate_14.10",
                        help="Specify the jenkins job name from where the installer needs to be downloaded. e.g. http://build.isis.vanderbilt.edu/view/META_ReleaseCandidate/job/META_ReleaseCandidate_14.10")
                        
    parser.add_argument("-b", "--build", default="lastSuccessfulBuild",
                        help="Specify the jenkins job name from where the installer needs to be downloaded.") 
 
    parser.add_argument("-r", "--release", default="14.10",
                        help="Release number like 14.01") 
 
    args = parser.parse_args()

    installer = args.installer
    release = args.release # we should get this number based on the url
    build_number = args.build
    installer_name = 'META_x64'
    
    # TODO: take the build number as parameter if empty use last successful build
    url = "{0}/{1}/artifact/META/deploy/{2}.msi".format(installer, build_number, installer_name)
    
    print " == Arguments =="
    print installer
    print build_number    
    print release
    print url

    print "Getting new META installer from build.isis Release: {0} build: #{1} ".format(release, build_number)
    print url

    downloaded_msi = "{0}_{1}_build_{2}.msi".format(installer_name, release, build_number)

    # download new META installer
    urllib.urlretrieve(url, downloaded_msi)
    print downloaded_msi

    META_install = query_META_install()

    msiVersion = GetMsiProperty(downloaded_msi ,"ProductVersion")
    print 'Downloaded version: ' + msiVersion    
    
    if 'UninstallString' in META_install:
        print 'Installed version: ' + msiVersion
        #if META_install['DisplayVersion'] == msiVersion:
        #    print 'Skipping uninstall and reinstall while versions are matching.'
        #    return 0

        # uninstall first
        print META_install['DisplayVersion']
        print META_install['UninstallString'] + ' /qb'
        print 'Uninstalling old version of META tool chain ...'
        subprocess.check_call(META_install['UninstallString'] + ' /qb', shell=True)

    # install
    print 'Installing new version of META tool chain ...'
    install_cmd = 'msiexec.exe /qb /i {0} /lewmi {0}.log'.format(downloaded_msi)
    print install_cmd
    subprocess.check_call(install_cmd, shell=True)

    # ready to use
    META_install_new = query_META_install()

    if 'DisplayVersion' in META_install_new:
        print META_install_new['DisplayVersion']
    
    return 0
    
if __name__ == "__main__":
    sys.exit(main())