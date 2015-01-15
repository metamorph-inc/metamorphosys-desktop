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

import time
import subprocess
import sys
import os
import _winreg as wr

def _query_registry():
    """
    Queries the Windows registry for META_PATH in order to get the location of
    python virtual environment containing all necessary packages and modules. 
    """

    try:
        # try to obtain META_PATH from the registry
        key = wr.OpenKey(wr.HKEY_LOCAL_MACHINE, r'software\meta', 0, wr.KEY_READ)
        meta_path = wr.QueryValueEx(key, 'META_PATH')[0]
        py_path = os.path.join(meta_path, r'bin\Python27\Scripts\python')
    except WindowsError:
        sys.stderr.write('Could not find META_PATH in registry, attempting to use default python.')
        py_path = 'python'
    
    return py_path


def main():
    py_path = _query_registry()
    command = '"{0}" sot.py'.format(py_path)
    
    print 'Calling "{0}" as a subprocess.'.format(command)
    try:
        popen = subprocess.Popen(command, shell=True, stdout=subprocess.PIPE, stderr=None)
        (stdoutdata, _) = popen.communicate()
        print stdoutdata
        popen.wait()
        if popen.returncode != 0:
            raise subprocess.CalledProcessError(popen.returncode, command)
        
        #execution_time = time.time() - t_1
        #_write_out_stat(execution_time, iso_time)
    except subprocess.CalledProcessError, err:
        sys.stderr.write('Out-print : {0}\n\n{1}\n'.format(err, err.output))
        sys.stderr.write('Failed calling {0}\n'.format(command))
        sys.exit(5)
        
    return 0

if __name__ == '__main__':
    sys.exit(main())
