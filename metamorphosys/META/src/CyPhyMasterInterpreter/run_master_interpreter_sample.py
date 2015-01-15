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

import win32com.client

# Disable early binding: full of race conditions writing the cache files,
# and changes the semantics since inheritance isn't handled correctly
import win32com.client.gencache
_savedGetClassForCLSID = win32com.client.gencache.GetClassForCLSID
win32com.client.gencache.GetClassForCLSID = lambda x: None

project = win32com.client.DispatchEx("Mga.MgaProject")
project.Open("MGA=" + r'D:\Projects\META\development\models\DynamicsTeam\MasterInterpreter\MasterInterpreter.mga')

# config_light = win32com.client.DispatchEx("CyPhyMasterInterpreter.ConfigurationSelectionLight")

# # GME id, or guid, or abs path or path to Test bench or SoT or PET
# config_light.ContextId = '{6d24a596-ec4f-4910-895b-d03a507878c3}'

# print config_light.SelectedConfigurationIds
# config_light.SetSelectedConfigurationIds(['id-0065-000000f1'])

# #config_light.KeepTemporaryModels = True
# #config_light.PostToJobManager = True

# master = win32com.client.DispatchEx("CyPhyMasterInterpreter.CyPhyMasterInterpreterAPI")
# master.Initialize(project)
# results = master.RunInTransactionWithConfigLight(config_light)


# It works only this way and does not worth the time to figure out the other way.
# will run ALL configurations.
focusobj = None

try:
    project.BeginTransactionInNewTerr()
    focusobj = project.GetObjectByID('id-0065-00000635')
finally:
    project.AbortTransaction()
    
selectedobj=win32com.client.DispatchEx("Mga.MgaFCOs")

interpreter = "MGA.Interpreter.CyPhyMasterInterpreter"
launcher = win32com.client.DispatchEx("Mga.MgaLauncher")
launcher.RunComponent(interpreter, project, focusobj, selectedobj, 128)

project.Close()