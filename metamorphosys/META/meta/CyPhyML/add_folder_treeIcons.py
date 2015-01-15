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
import sys
import win32com.client

# this config can be read from a json file
config = {
    'metaName' : 'CyPhyML',
    'extension' : 'png',
    'collapsedSuffix' : 'Folder',
    'expandedSuffix' : 'Folder_b'
}


if len(sys.argv) > 1:
	file = sys.argv[1]
else:
	import os.path
	file = os.path.abspath(config['metaName'] + '.mta')

base = os.path.splitext(file)[0]

if os.path.exists(file) == False:
	print '%s does not exist' % file
	xmplog = base + '.xmp.log'
	if os.path.exists(xmplog):
		with open(xmplog, 'r') as f_p:
			for line in iter(f_p):
				print line

import win32com.client.gencache
_savedGetClassForCLSID = win32com.client.gencache.GetClassForCLSID
win32com.client.gencache.GetClassForCLSID = lambda x: None

project = win32com.client.DispatchEx('Mga.MgaMetaProject')
project.Open('MGA=' + file)
project.BeginTransaction()

icon_list_collapsed = list()
icon_list_expanded = list()

# get all defined folders
for folder in project.RootFolder.DefinedFolders:
	# add folder kind and icon files to the lists
	# replace :: namespace delimiter to __ in filenames
	icon_list_collapsed.append(
		(folder.Name,
		folder.Name.replace(':','_') + config['collapsedSuffix'] + '.' + config['extension']))

	icon_list_expanded.append(
		(folder.Name,
		folder.Name.replace(':','_') + config['expandedSuffix'] + '.' + config['extension']))

## iterate through kind, icon pairs and add those as registry nodes
#print 'Collapsed icons:'
for kind, icon in icon_list_collapsed:
	#print ' - ' + kind + ' : ' + icon
	cas = project.RootFolder.GetDefinedFolderByNameDisp(kind, True)
	cas.GetRegistryNodeDisp('treeIcon').Value = icon
	#for regnode in cas.RegistryNodes:
	#	print kind + ': ' + regnode.Name + '=' + regnode.Value
	
#print 'Expanded icons:'
for kind, icon in icon_list_expanded:
	#print ' - ' + kind + ' : ' + icon
	cas = project.RootFolder.GetDefinedFolderByNameDisp(kind, True)
	cas.GetRegistryNodeDisp('expandedTreeIcon').Value = icon
	#for regnode in cas.RegistryNodes:
	#	print kind + ': ' + regnode.Name + '=' + regnode.Value
	

project.CommitTransaction()
project.Close()
