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

#!/usr/bin/python
###
# This module provides the 'make CF card' function to OM
# The two functions "update_fs" and "prepare_fs" should
# be provided to the user of the OM so that the user 
# can perform either one at any time
#
# At the bottom there is an example of running the function.
# In the OM you'll need to have an option for the user to select
# which device enumeration to use (or input it through text)
###
import copy, os, shutil, subprocess, string, glob, fnmatch, shlex
import threading
import time
import sys

def scan_for_CAD_files(mypath):
    print "Starting test script for ExtractACM-XMLfromCASModules.exe"
        
    from os import listdir
    from os.path import isfile, join, getsize

    matches = []
    for root, dirs, files in os.walk(mypath):
        for filename in fnmatch.filter(files, '*.prt*') + fnmatch.filter(files, '*.asm*'):
            if not filename.endswith('.xml'):
                matches.append(os.path.join(root, filename))

    max_threads = 1
    threads = []
    for fn in matches:
        while count_alive_threads(threads) >= max_threads:
            time.sleep(1)
        newThread = threading.Thread(target=run_the_extractor, kwargs={"filename": fn})
        newThread.start()
        threads.append(newThread)

def count_alive_threads(thread_array):
    count = 0
    for t in thread_array:
        if t.isAlive():
            count += 1
    return count

def run_the_extractor(filename):
    print "converting " + filename
    outfilename = filename + '.xml'
    exe_path = os.getenv("PROE_ISIS_EXTENSIONS") + 'bin\ExtractACM-XMLfromCreoModels.exe'
    arguments = ' -c "'+filename+'" -x "' + outfilename + '"'
    command = exe_path + arguments
    return_code = subprocess.call(command)
    if return_code:
        print " Error on converting file "+ filename + " (return code " + str(return_code) + ")"


if __name__ == "__main__":
    if len(sys.argv) != 2:
        print "Syntax: testExtractACM <PathtoScan>"
        exit()
    mypath =  sys.argv[1]
    scan_for_CAD_files(mypath)
