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
import shutil
import os.path
sys.path.append(os.path.join(os.path.dirname(os.path.abspath(__file__)), r'..\..\..\externals\common-scripts'))
import gme
import win32com.client

paradigm = "CyberComposition"

meta_path = ''
if paradigm == "CyberComposition" and ( not 'META_PATH' in os.environ or os.environ[ 'META_PATH' ] == '' ):
    print "META_PATH environment variable not set and CyberComposition paradigm specified"
    print "If CyberComposition paradigm is specified, META_PATH environment variable must be set to trunk of Cyber code base to run this program"
    sys.exit( 1 )
else:
    meta_path = os.environ[ 'META_PATH' ]

destdir = meta_path + r"\src\Cyber2SLC_CodeGen\SL_CodeGen"
destfile = destdir + r"\Cyber2SLC_udm.mga"

if not os.path.exists( destdir ):
    os.makedirs( destdir )

gme.xme2mga( destdir + r"\Cyber2SLC_udm.xme", destfile )

mga = win32com.client.DispatchEx("Mga.MgaProject")
mga.Open( "MGA=" + destfile )

gme.mga2xme( meta_path + r"\generated\Cyber\models\CyberComposition_uml.mga" )
xme = win32com.client.DispatchEx("Mga.MgaParser")
xme.ParseProject(mga, meta_path + r"\generated\Cyber\models\CyberComposition_uml.xme")

mga.BeginTransactionInNewTerr()

Subsystem = mga.RootFolder.ObjectByPath('/@CyberComposition|kind=Package/@Simulink|kind=Namespace/@Dataflow|kind=ClassDiagram/@Subsystem|kind=Class')
Subsystem_ref = mga.RootFolder.ObjectByPath('/@LINKS/@NewClassDiagram/@Subsystem')
Subsystem_ref.Referred = Subsystem

SF_TypeBase = mga.RootFolder.ObjectByPath('/@CyberComposition|kind=Package/@Simulink|kind=Namespace/@Types|kind=ClassDiagram/@SF_TypeBase|kind=Class')
TypeBase = mga.RootFolder.ObjectByPath('/LINKS/NewClassDiagram/TypeBase')
TypeBase.Referred = SF_TypeBase

SF_Parameter = mga.RootFolder.ObjectByPath('/@CyberComposition|kind=Package/@Simulink|kind=Namespace/@Dataflow|kind=ClassDiagram/@SF_Parameter|kind=Class')
Parameter = mga.RootFolder.ObjectByPath('/LINKS/NewClassDiagram/Parameter')
Parameter.Referred = SF_Parameter

SF_Port = mga.RootFolder.ObjectByPath('/@CyberComposition|kind=Package/@Simulink|kind=Namespace/@Dataflow|kind=ClassDiagram/@SF_Port|kind=Class')
Port = mga.RootFolder.ObjectByPath('/LINKS/NewClassDiagram/Port')
Port.Referred = SF_Port

SF_TriggerPort =  mga.RootFolder.ObjectByPath('/@CyberComposition|kind=Package/@Simulink|kind=Namespace/@Dataflow|kind=ClassDiagram/@TriggerPort|kind=Class')
TriggerPort = mga.RootFolder.ObjectByPath('/LINKS/NewClassDiagram/TriggerPort')
TriggerPort.Referred = SF_TriggerPort

SF_Primitive = mga.RootFolder.ObjectByPath('/@CyberComposition|kind=Package/@Simulink|kind=Namespace/@Dataflow|kind=ClassDiagram/@Primitive|kind=Class')
Primitive = mga.RootFolder.ObjectByPath('/LINKS/NewClassDiagram/Primitive')
Primitive.Referred = SF_Primitive

SF_TypeBaseRef = mga.RootFolder.ObjectByPath('/@CyberComposition|kind=Package/@Simulink|kind=Namespace/@Types|kind=ClassDiagram/@TypeBaseRef|kind=Class')
TypeBaseRef = mga.RootFolder.ObjectByPath('/LINKS/NewClassDiagram/TypeBaseRef')
TypeBaseRef.Referred = SF_TypeBaseRef

mga.Name = "Cyber2SLC"
mga.RootFolder.Name = "Cyber2SLC"

mga.CommitTransaction()
mga.Save( "MGA=" + destfile )