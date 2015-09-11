"""
Applies tonka-specific mods to CyPhyML_org.xme

..\..\externals\common-scripts\gmepy.exe  xme2mga CyPhyML_org.xme
.\tonka_mods.py
..\..\externals\common-scripts\gmepy.exe  mga2xme CyPhyML_org.mga
start CyPhyML_org.xme
refresh in CyPhyML.xme, etc
"""

import win32com.client

mods = {
"/@1_Component/@1x_Component/@Resource":
 { "Icon": "resource.png" },
"/@1_Component/@4x_Port/@Connector|kind=Model":
 { "Decorator": "Mga.CPMDecorator",
   "GeneralPreferences": "fillColor = 0xdddddd\nportLabelLength=0\ntreeIcon=connector_port.png\nexpandedTreeIcon=connector_port.png",
   "Icon": "connector.png",
    "IsTypeInfoShown": False,
   "PortIcon": "connector_port.png" },
"/@1_Component/@4x_Port/@ModelicaConnector|kind=Model":
 { "Decorator": "Mga.CPMDecorator",
   "GeneralPreferences": "fillColor = 0xdddddd\nportLabelLength=0",
   "Icon": "modelica_connector.png",
   "PortIcon": "modelica_connector_port.png",
   "IsTypeInfoShown": False },
"/@3_Domains/@3_SolidModeling/@CADModel":
 { "Decorator": "Mga.CPMDecorator",
   "GeneralPreferences": "fillColor = 0xdddddd\nhelp=$META_DOCROOT$34d163d3-f7d6-4178-bcae-6c469f52be14.html\nportLabelLength=0",
   "Icon": "cad_model.png" },
"/@2_ComponentBehaviorModels/@BehaviorModels/@ModelicaModel":
    { "Decorator": "Mga.CPMDecorator",
    },
"/@1_Component/@3_Properties_Parameters/@IsProminent":
    { "BooleanDefault": False
    }
}
 
project = win32com.client.DispatchEx("Mga.MgaProject")
project.Open("MGA=" + "CyPhyML_org.mga")

project.BeginTransactionInNewTerr()
for kind, attrs in mods.iteritems():
    model = project.RootFolder.GetObjectByPathDisp(kind)
    print model.AbsPath + " " + kind
    for attrname, attrvalue in attrs.iteritems():
        #print model.Meta.Name
        #print [a.Name for a in model.Meta.DefinedAttributes]
        if isinstance(attrvalue, basestring):
            model.SetStrAttrByNameDisp(attrname, attrvalue)
            print "  " + attrname + "=" + attrvalue
        else:
            print "  " + attrname
            model.SetBoolAttrByNameDisp(attrname, attrvalue)

project.CommitTransaction()
project.Save("", True)
