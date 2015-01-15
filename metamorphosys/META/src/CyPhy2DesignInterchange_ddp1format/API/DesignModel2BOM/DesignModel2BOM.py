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
import json

def RecursivelyGetComponentInstances(dict_Container):
    list_ComponentInstances = []

    if "Containers" in dict_Container:
        for dict_NestedContainer in dict_Container["Containers"]:
            list_NestedComponentInstances = RecursivelyGetComponentInstances(dict_NestedContainer)
            for dict_NestedComponentInstance in list_NestedComponentInstances:
                list_ComponentInstances.append(dict_NestedComponentInstance)

    if "ComponentInstances" in dict_Container:
        for dict_ComponentInstance in dict_Container["ComponentInstances"]:
            list_ComponentInstances.append(dict_ComponentInstance)

    return list_ComponentInstances

class BOM:
    def __init__(self):
        self.Components = []
        self.Name = ""
        self.DesignID = ""

if __name__ == '__main__':
    #logger = logging.getLogger('root.StepFileUtility')

    num_Args = len(sys.argv)
    if num_Args == 3:
        path_DesignModel = sys.argv[1]
        path_BOMOutput = sys.argv[2]
    else:
        print "USAGE: DesignModel2BOM  path_to_design_model  [path_for_BOM_output_file]"
        sys.exit(1)

    file_DesignModel = open(path_DesignModel,"r")
    dict_DesignModelRoot = json.load(file_DesignModel)

    list_ComponentInstances = RecursivelyGetComponentInstances(dict_DesignModelRoot)

    # Build a dictionary for all components.
    # The key is built from ID, Version, Revision
    dict_BOM = dict()
    for dict_ComponentInstance in list_ComponentInstances:
        str_InstanceName = str(dict_ComponentInstance["Name"])
        str_AVMID = str(dict_ComponentInstance["ComponentID"])
        str_Version = str(dict_ComponentInstance["ComponentVersion"])
        str_Revision = str(dict_ComponentInstance["ComponentRevision"])

        # Build primary key
        str_Key = str_AVMID,str_Version,str_Revision
        if str_Key in dict_BOM:
            dict_Value = dict_BOM[str_Key]
            dict_Value["InstanceNames"].append(str_InstanceName)
            dict_Value["Frequency"] += 1
            dict_BOM[str_Key] = dict_Value
        else:
            dict_Value = dict()
            dict_Value["InstanceNames"] = [str_InstanceName]
            dict_Value["AVMID"] = str_AVMID
            dict_Value["Version"] = str_Version
            dict_Value["Revision"] = str_Revision
            dict_Value["Frequency"] = 1
            dict_BOM[str_Key] = dict_Value

    bom = BOM()
    bom.Name = dict_DesignModelRoot["Name"]
    bom.DesignID = dict_DesignModelRoot["DesignID"]
    for k,v in dict_BOM.items():
        bom.Components.append(v)

    fo = open(path_BOMOutput, 'w')
    json_BOM = json.dumps(bom.__dict__,indent=2)
    fo.write(json_BOM)
    fo.close()