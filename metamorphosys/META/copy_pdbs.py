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

'''
Apparently symstore.exe /r inspects every file. This program will hard link files of interest to a new temp dir, then call symstore on that.
'''

import sys
import os
import os.path
import datetime
import win32file
import win32process
import win32api
import tempfile
import pywintypes

top_dir = os.path.dirname(os.path.abspath(__file__))
extensions = set(('.pdb', '.dll', '.exe', '.ocx'))
files_copied = 0

starttime = datetime.datetime.now()
print "Start " + starttime.isoformat()
destdir = tempfile.mkdtemp()
print "Destination " + destdir


for root, dirs, files in os.walk(top_dir):
    #print dirs
    for filename in (f for f in files if f[-4:] in extensions):
        #print os.path.join(root, filename)
        try:
            win32file.CreateHardLink(os.path.join(destdir, filename), os.path.join(root, filename))
            files_copied += 1
        except pywintypes.error as err:
            if err.winerror != 183: #pywintypes.error (183, 'CreateHardLink', 'Cannot create a file when that file already exists.')
                raise
    for exclude in ('.git', '.svn', 'CVS', '.hg', '3rdParty', 'Python27', 'Python26'):
        if exclude in dirs:
            dirs.remove(exclude)

print "%d files copied" % files_copied            

startup = win32process.STARTUPINFO()
startup.dwFlags += win32process.STARTF_USESTDHANDLES
startup.hStdInput = win32file.INVALID_HANDLE_VALUE
security_attributes = pywintypes.SECURITY_ATTRIBUTES()
security_attributes.bInheritHandle = 1
startup.hStdOutput = startup.hStdError = win32file.CreateFile(os.path.join(destdir, "log"), win32file.GENERIC_WRITE, win32file.FILE_SHARE_READ, security_attributes, win32file.CREATE_ALWAYS, 0, None)
win32file.WriteFile(startup.hStdOutput, 'log started\n')

(hProcess, hThread, processId, threadId) = win32process.CreateProcess(r"C:\Program Files (x86)\Debugging Tools for Windows (x86)\symstore.exe", 
    "symstore.exe add /r /f \"%s\" /s C:\\symstore /t META" % destdir, None, None, True, win32process.CREATE_BREAKAWAY_FROM_JOB, None, None, startup)
win32api.CloseHandle(startup.hStdOutput)
win32api.CloseHandle(hThread)
    
# Don't need to wait here, but it doesn't take long, and we can remove the temp dir
import win32event
win32event.WaitForSingleObject(hProcess, win32event.INFINITE)
print "symstore exited with code " + str(win32process.GetExitCodeProcess(hProcess))
import shutil
#shutil.rmtree(destdir)

win32api.CloseHandle(hProcess)

# print "\n".join(open(os.path.join(destdir, 'log'), 'r'))

print "Finish " + datetime.datetime.now().isoformat()
print "Elapsed time %d seconds" % (datetime.datetime.now() - starttime).seconds
