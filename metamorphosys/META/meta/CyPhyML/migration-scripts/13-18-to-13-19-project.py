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

# This is a migrator for CyPhy projects.
# We recently started tracking a Component's "backend folder" location
# inside CyPhy itself, instead of in the project.manifest.json.
# This migrator will take component location data from the project.manifest.json
# and insert it into the CyPhy model.


# Algorithm:
# - For each XME file, open it and export it again
#   - Ensure latest paradigm, and that the new Path attribute will exist for components
# - Crawl the CyPhy model
#   - For each component found, locate the analogue in the project.manifest.json
#   - Populate the Path field in the XME


### For each XME, import it and export it again.


import json
import fnmatch
import os
import xml.etree.ElementTree as ET


def get_component_path_fom_manifest(avmid):
    for c in a_manifestComponents:
        if c["avmid"] == avmid:
            path = c["modelpath"]
            return os.path.dirname(path)


def get_cyphy_component_avmid(component):
    for child in component.iter('attribute'):
        if child.attrib['kind'] == 'AVMID':
            return child.find('value').text


def set_cyphy_component_path(component, path):
    for child in component.findall("attribute[@kind='Path']"):
        child.find('value').text = path
        del child.attrib['status']


def migrate_xme(xme_path):
    tree = ET.parse(xme_path)
    root = tree.getroot()

    components = root.findall(".//folder/model[@kind='Component']")
    for component in components:
        avmid = get_cyphy_component_avmid(component)
        path = get_component_path_fom_manifest(avmid)

        if path is not None:
            set_cyphy_component_path(component, path)
        else:
            print "Could not find path in manifest for: ", component.find('name').text

    _xml = """<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE project SYSTEM "mga.dtd">

""" + ET.tostring(root)

    xme_newpath = os.path.join(os.path.dirname(xme_path),
                               os.path.splitext(os.path.basename(xme_path))[0] + '_migrated.xme')
    with open(xme_newpath, 'w') as f:
        f.write(_xml)


### Open up the manifest.project.json file and get all component entries
with open("manifest.project.json") as f:
    d_manifest = json.load(f)

a_manifestComponents = d_manifest["Project"]["Components"]

### Using XPath, crawl the XME for Components
ap_xme = []
for file in os.listdir('.'):
    if fnmatch.fnmatch(file, '*.xme'):
        xml = migrate_xme(file)

print "done"