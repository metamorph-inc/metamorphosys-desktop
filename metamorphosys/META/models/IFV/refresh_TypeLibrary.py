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

class Args(object):
    def __init__(self, **kwargs):
        self.__dict__.update(kwargs)

IFV_typelib = Args(lib_mga="..\\TypeLibrary\\TypeLibrary.mga", input_file="IFV.mga", outputfilename="IFV_updatedTypeLibrary", old_lib_name='TypeLibrary')
wps = "..\\WheeledPerformanceSurrogates\\WheeledVehiclePerformanceSurrogate_Library"
WPS_typelib = Args(lib_mga="..\\TypeLibrary\\TypeLibrary.mga", input_file=wps + ".mga", outputfilename=wps + "_updatedTypeLibrary", old_lib_name='TypeLibrary')
IFV_WPS = Args(lib_mga=wps + "_updatedTypeLibrary.mga", input_file="IFV_updatedTypeLibrary.mga", outputfilename="IFV_updatedWPS", old_lib_name='WheeledPerformanceSurrogates')

#updates = (IFV_typelib, WPS_typelib, IFV_WPS)
updates = (WPS_typelib,)

import win32com.client
# Disable early binding: full of race conditions writing the cache files,
# and changes the semantics since inheritance isn't handled correctly
import win32com.client.gencache
_savedGetClassForCLSID = win32com.client.gencache.GetClassForCLSID
win32com.client.gencache.GetClassForCLSID = lambda x: None


def RefreshLibrary(args):
	project = win32com.client.DispatchEx("Mga.MgaProject")

	output = None
	#output = open("log", "w")
	project.Open("MGA=" + args.input_file)
	try:
	    project.BeginTransactionInNewTerr(2)
	    oldlib = [f for f in project.RootFolder.ChildFolders if f.LibraryName.find(args.old_lib_name) != -1][0]
	    newlib = project.RootFolder.AttachLibrary("MGA=" + args.lib_mga)
	    newlib.LibraryName = args.old_lib_name

	    switcher = win32com.client.DispatchEx("MGA.Interpreter.ReferenceSwitcher")
	    switcher.SwitchReferences([oldlib], [newlib])
	    # FlushUndoQueue, or oom
	    project.CommitTransaction()
	    project.FlushUndoQueue()
	    project.BeginTransactionInNewTerr(2)
	    if output:
	        import collections
	        q = collections.deque()
	        q.append(oldlib)
	        while len(q) != 0:
	            o = q.pop()
	            if o.ObjType == 6:
	                q.extend(o.ChildFolders)
	            if o.ObjType == 1 or o.ObjType == 6:
	                q.extend(o.ChildFCOs)
	            # print o.Name
	            if o.ObjType != 6:
	                for ref in o.ReferencedBy:
	                    output.write(ref.AbsPath)
	                    output.write("\n")
	    oldlib.DestroyObject()
	    project.CommitTransaction()
	    #project.Close(True)
	    project.Save("MGA=" + args.outputfilename + ".mga")
	    print "Saved " + args.outputfilename + ".mga"
	    project.FlushUndoQueue()
	    dumper = win32com.client.DispatchEx("Mga.MgaDumper")
	    dumper.DumpProject(project, args.outputfilename + ".xme")
	    print "Saved " + args.outputfilename + ".xme"
	finally:
	    project.Close(True)

for update in updates:
	RefreshLibrary(update)
