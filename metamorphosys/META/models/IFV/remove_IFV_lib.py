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


lib_xme = "CompLib_DesignSpace_andDesigns.xme"
input = r"..\..\generated\IFV\models\IFV.mga"
output = "IFV_no_lib"

import win32com.client

MGAPREF_NO_NESTED_TX = 0x00000080

project = win32com.client.DispatchEx("Mga.MgaProject")

project.Open("MGA=" + input)
try:
    # BUG: GME overwrites the RootFolder name on ParserProject
    project.BeginTransactionInNewTerr()
    rf_name = project.RootFolder.Name
    project.Preferences = MGAPREF_NO_NESTED_TX | project.Preferences
    project.CommitTransaction()

    parser = win32com.client.DispatchEx("Mga.MgaParser")
    parser.ParseProject(project, lib_xme)
    
    project.BeginTransactionInNewTerr()
    libroot = [f for f in project.RootFolder.ChildFolders if f.LibraryName.find('CompLib') != -1][0]
    libfolders = sorted(libroot.ChildFolders, key=lambda x: x.Name)
    folders = []
    for libfolder in libfolders:
        fs = [f for f in project.RootFolder.ChildFolders if f.Name == libfolder.Name]
        if len(fs) != 1:
            raise Exception("No equivalent for lib folder " + libfolder.Name)
        folders.append(fs[0])
    print 'Updating ' + ", ".join([f.Name for f in folders])
    switcher = win32com.client.DispatchEx("MGA.Interpreter.ReferenceSwitcher")
    switcher.SwitchReferences(libfolders, folders)
    project.RootFolder.Name = rf_name
    project.Name = rf_name
    # FlushUndoQueue, or oom
    project.CommitTransaction()
    project.FlushUndoQueue()
    project.BeginTransactionInNewTerr()
    libroot.DestroyObject()
    project.CommitTransaction()
    project.Save("MGA=" + output + ".mga")
    project.FlushUndoQueue()
    dumper = win32com.client.DispatchEx("Mga.MgaDumper")
    dumper.DumpProject(project, output + ".xme")
finally:
    project.Close(True)

