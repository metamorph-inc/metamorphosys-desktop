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

#
# MetaMorph Software
#
# Author: J. Scott
#
#

import os
import glob
import zipfile
import datetime 
import json
import sys
import argparse
from lxml import etree

# http://eli.thegreenplace.net/2012/03/15/processing-xml-in-python-with-elementtree/

class Layout_to_CadAssembly(object):
    """
    Class definition

    """
    # hard-coded names
    local_cadassembly_name = 'CADAssembly.xml'
    modified_cadassembly_name = 'CADAssembly.xml'
    layout_json_name = ''
    boardWidth = 0.0
    boardHeight = 0.0

    # dictionaries
    layout_json_dict = {}
    PCB_Element = {}
    PCB_Element_ParentCADComponent = {}
    PCB_Element_ComponentID = 0

    def __init__(self):
        """
        Constructor

        """
        # paths
        project_root = ''
        path_to_stats_folder = ''
        #self.project_root = os.path.abspath(os.pardir)
        #self.path_to_stats_folder = os.path.join(self.project_root, 'stats')
        #self.right_now = datetime.datetime.now().strftime('%Y%m%d_%H%M%S')


    def readLayout(self, name):
        """
        parse layout-input.json file
        """
        self.layout_json_name = name
        with open(self.layout_json_name, 'r') as f_in:
            self.layout_json_dict = json.load(f_in)
        print 'layout_json_dict'
        self.boardWidth = self.layout_json_dict['boardWidth']
        self.boardHeight = self.layout_json_dict['boardHeight']
        print 'boardWidth: ' + str(self.layout_json_dict['boardWidth'])
        print 'boardHeight: ' + str(self.layout_json_dict['boardHeight'])
        print 'numLayers: ' + str(self.layout_json_dict['numLayers'])

    def modifyCadAssembly2(self):
        # 
        print ''



    def modifyCadAssembly(self):
        # 
        # find PCB in CadAssembly.xml
        # for each component in the layout do the following:
        #   1) Find component for layout.json in <UnassembledComponents> [LATER: or <Assembly>]
        #       1a) Move this entry into <Assembly><CADComponent> (parent Assembly/CADComponent of PCBBoard)
        #       1b) Add a constraint  e.g. CS -> CS0
        #   2) Add component x,y location to CS0_X and CS0_Y paramters in PCB in CADAssembly.xml
        #   x) Delete or leave <UnassembledComponents> section(?)
        #
            import xml.etree.ElementTree as ET
            tree = ET.parse(self.local_cadassembly_name)
            root = tree.getroot()
         
            # find PCB Component under <Assemblies><Assembly>  [LATER: look under <UnassembledComponents> and construct <Assemblies>..
            for ass_elem in tree.iter(tag='Assembly'):  
                for cc_parent in ass_elem.findall('CADComponent'):
                    #print 'Found a parent component in CADAssembly.xml [' + str(cc_parent.attrib['DisplayName']) + ']'
                    for cc in cc_parent.findall('CADComponent'):
                        #print 'Found a component in CADAssembly.xml [' + str(cc.attrib['DisplayName']) + ']'
                        try:
                            #if cc.attrib['DisplayName'] == 'PCB_Board':
                            if cc.attrib['Classification'] == 'passive.pcb_board':
                                self.PCB_Element = cc
                                self.PCB_Element_ComponentID = cc.get('ComponentID')
                                self.PCB_Element_ParentCADComponent = cc_parent
                                print 'Found PCB board component in CADAssembly.xml parent[' + str(self.PCB_Element_ParentCADComponent.get('DisplayName')) + ']'
                                # set PCB Height & Wdith
                                # recurse PCB Component to find CADParameters
                                for pp in self.PCB_Element.findall('ParametricParameters'):
                                    for cp in pp.findall('CADParameter'):
                                        if cp.get('Name') == 'HEIGHT':
                                            print 'set HEIGHT: ' + str(self.layout_json_dict['boardHeight'])
                                            cp.set('Value', str(self.layout_json_dict['boardHeight']))
                                        if cp.get('Name') == 'WIDTH':
                                            print 'set WIDTH: ' + str(self.layout_json_dict['boardWidth'])
                                            cp.set('Value', str(self.layout_json_dict['boardWidth']))
                                # TODO: Add THICKNESS to layout.json
                        except:
                            print 'WARNING: Component [' + cc.attrib['DisplayName'] + '] is missing a classification string'
            if self.PCB_Element_ParentCADComponent == {}:
                print 'FATAL: Unable to find PCB component in CADAssembly.xml'
                print 'Please check that the design contains a component with Classification = passive.pcb_board'
                #self.assertEqual(the_exception.code, 3)
                f_log = open('log\Synthesize_PCB_CAD_connections.log', "a+")
                f_log.write('FATAL: Unable to find PCB component in CADAssembly.xml\n')
                f_log.write('Please check that the design contains a "PCB_Board" component\n')
                f_log.close()
                f_fail = open('_FAILED.txt', "a+")
                f_fail.write('FATAL: Unable to find PCB component in CADAssembly.xml\n')
                f_fail.write('Please check that the design contains a "PCB_Board" component\n')
                sys.exit(1) 

            #   1) Find component for layout.json in <UnassembledComponents> [LATER: or <Assembly>]
            #constraint_idx = 1   # 0 used for case to pcb connection
            constraint_top_idx = 1   # 0 used for case to pcb connection
            constraint_bottom_idx = 10000
            uid_idx = 12999 # TODO: should find max and increment from there
            for comp in self.layout_json_dict['packages']:
                print comp['name']
                #print comp['ComponentID']
                if comp['layer'] == 0:
                    cidx = constraint_top_idx
                    print 'TOP cidx: [' + str(cidx) + ']'
                else:
                    cidx = constraint_bottom_idx
                    print 'BOTTOM cidx: [' + str(cidx) + ']'
                    if cidx > 10009:
                        print 'BOTTOM - CIDX > 10009     cidx: [' + str(cidx) + ']'
                        continue   # ignore for now...
                #if comp['layer'] > 0:
                #    continue
                w = comp['width']
                h = comp['height']
                rotation_val = comp['rotation']*90.0                

                if comp['layer'] == 0:  # top layer
                    l = 1.0
                    xx = comp['x']
                    yy = comp['y']
                else:  # bottom layer
                    l = -1.0       # part offset subtract factor to compensate for PCB ref on opposite side on bottom
                    xx = self.boardWidth - comp['x']  # compensate for PCB ref on opposite side on bottom
                    yy = comp['y']
                    #print '  %%% self.boardWidth:[' + str(self.boardWidth) + ']'
                    #print '  %%% comp[x]:[' + str(comp['x']) + ']'
                    #print '  %%% xx:[' + str(xx) + ']'

                if rotation_val == 90:
                    x_loc = xx+0.5*h*l;
                    y_loc = yy+0.5*w;
                elif rotation_val == 180:
                    x_loc = xx+0.5*w*l;
                    y_loc = yy+0.5*h;
                elif rotation_val == 270:
                    x_loc = xx+0.5*h*l;
                    y_loc = yy+0.5*w;
                else:  # rot == 0
                    x_loc = xx+0.5*w*l;
                    y_loc = yy+0.5*h;

                #print '  %%% x_loc:[' + str(x_loc) + ']    y_loc:[' + str(y_loc) + ']'

                comp_from_layout_isFound = False               
                #       - 1a) Move this entry into <Assembly><CADComponent> 
                for elem in tree.iter(tag='UnassembledComponents'):
                    for cc in elem.findall('CADComponent'):
                        #if cc.attrib['DisplayName'] == comp['name']:

                        if comp['ComponentID'] == cc.attrib['ComponentID']: # MOT-161

                            print 'FOUND COMP in UnassembledComponents [' + str(comp['name']) + ']'
                            # add cc to Assembly/CADComponent parent of PCB board
                            print 'MOVE Component [' + cc.attrib['DisplayName'] + '] to PCB Board Assembly structure'
                            self.PCB_Element_ParentCADComponent.append(cc)
                            # delete this from <UnassembledComponents> section:
                            for uc_elem in tree.iter(tag='UnassembledComponents'):
                                for uc_cc in uc_elem.findall('CADComponent'):
                                    if uc_cc.attrib['ComponentID'] == cc.attrib['ComponentID']:
                                        print 'DELETE component [' + uc_cc.attrib['DisplayName'] + '] from <UnassembledComponents> section'
                                        uc_elem.remove(uc_cc)


                            # - 1b) Add a constraint  e.g. CS -> CS0 to Component
                            comp_from_layout_isFound = True
                            # add <Constraint>
                            print 'Append <Constraint>'
                            co = ET.SubElement(cc, 'Constraint')
                            co.set('_id', 'id'+str(uid_idx)) 
                            uid_idx=uid_idx+1
                            pair = ET.SubElement(co, 'Pair')
                            pair.set('FeatureAlignmentType', 'CSYS')
                            pair.set('FeatureGeometryType', 'CSYS')
                            pair.set('FeatureInterfaceType', 'CAD_DATUM')
                            pair.set('_id', 'id'+str(uid_idx))
                            uid_idx=uid_idx+1

                            cf1 = ET.SubElement(pair, 'ConstraintFeature')
                            cf1.set('ComponentID', self.PCB_Element_ComponentID)
                            cf1.set('FeatureName', 'CS'+str(cidx) )
                            cf1.set('FeatureOrientationType', 'NONE')
                            cf1.set('_id', 'id'+str(uid_idx))
                            uid_idx=uid_idx+1
                            cf2 = ET.SubElement(pair, 'ConstraintFeature')
                            cf2.set('ComponentID', cc.get('ComponentID') )
                            cf2.set('FeatureName', 'CS')
                            cf2.set('FeatureOrientationType', 'NONE')
                            cf2.set('_id', 'id'+str(uid_idx))
                            uid_idx=uid_idx+1
 
                            #  2) also add x,y to PCB_Board parameters
                            #     recurse PCB_Board Component to find CADParameters
                            for pp in self.PCB_Element.findall('ParametricParameters'):
                                for cp in pp.findall('CADParameter'):
                                    CSn_X = 'CS'+str(cidx)+'_X'
                                    CSn_Y = 'CS'+str(cidx)+'_Y'
                                    CSn_ROT = 'CS'+str(cidx)+'_ROT'
                                    if cp.get('Name') == CSn_X:
                                        print 'set '+CSn_X+' to ['+str(xx)+']'
                                        cp.set('Value', str(x_loc))  # center... 
                                    if cp.get('Name') == CSn_Y:
                                        print 'set '+CSn_Y+' to ['+str(yy)+']'
                                        cp.set('Value', str(y_loc))  # center... 
                                    if cp.get('Name') == CSn_ROT:
                                        print 'set '+CSn_ROT+' to ['+str(rotation_val)+']'
                                        cp.set('Value', str(rotation_val))  # rotation of "1" in layout.json = 90.0 deg, etc.

                            print 'SET CSn_X = [' +CSn_X+ ']  CSnY = [' +CSn_Y+ ']  CSnROT = [' +CSn_ROT+ ']'

                            if comp['layer'] == 0: 
                                constraint_top_idx = constraint_top_idx + 1
                                print 'INC TOP'
                            else:
                                constraint_bottom_idx = constraint_bottom_idx + 1
                                print 'INC BOTTOM'


                            #constraint_idx = constraint_idx + 1

                if comp_from_layout_isFound == False:
                    print 'WARNING: Unable to find component [' + comp['name'] + '] ['+comp['ComponentID']+'] from ' + self.layout_json_name + ' in CADAssembly.xml'
                    print 'Please check the following:'
                    print '- Does this component have a CAD model defined?'
                    print '- Is the layout.json file selected is consistent with the current model selected for analysis in this testbench?'
                    f_log = open('log\Synthesize_PCB_CAD_connections.log', "a+")
                    f_log.write('WARNING: Unable to find component [' + comp['name'] + '] ['+comp['ComponentID']+'] from ' + self.layout_json_name + ' in CADAssembly.xml\n')
                    f_log.write('Please check the following:\n')
                    f_log.write('- Does this component have a CAD model defined?\n')
                    f_log.write('- Is the layout.json file selected is consistent with the current model selected for analysis in this testbench\n')
                    f_log.close()							
                    f_fail = open('_WARNING.txt', "a+")
                    f_fail.write('WARNING: Unable to find component [' + comp['name'] + '] ['+comp['ComponentID']+'] from ' + self.layout_json_name + ' in CADAssembly.xml\n')
                    f_fail.write('Please check the following:\n')
                    f_fail.write('- Does this component have a CAD model defined?\n')
                    f_fail.write('- Is the layout.json file selected is consistent with the current model selected for analysis in this testbench\n')
                    #sys.exit(1)  

            # delete entire <UnassembledComponents> section (could cause problems if mainline CAD is every needs these)
            for Assys in tree.iter(tag='Assemblies'): 
                for uc in Assys.findall('UnassembledComponents'):
                    Assys.remove(uc)
            
            tree.write(self.modified_cadassembly_name)
            print 'end'
      
def main(argv=sys.argv):

    ltc = Layout_to_CadAssembly()

    parser = argparse.ArgumentParser()
    parser.add_argument("layoutpath", help="path to layout file", metavar="LAYOUTPATH")
    args = parser.parse_args()
    ltc.layout_json_name = args.layoutpath
    print 'Layout File: {0}'.format(ltc.layout_json_name)

    ## copy layout.json file to working results directory for debug
    local_path = os.path.basename(ltc.layout_json_name)
    print 'Layout File base: {0}'.format(local_path)
    fin = open(ltc.layout_json_name)
    fout = open(local_path+'_backup', "w")
    for line in fin.readlines():
        fout.write(line)

    ## copy CADAssembly.xml file to working results directory for debug
    fin_cadassy = open(ltc.local_cadassembly_name)
    fout_cadassy = open(ltc.local_cadassembly_name+'_backup', "w")
    for line in fin_cadassy.readlines():
        fout_cadassy.write(line)

    ltc.readLayout(ltc.layout_json_name)
    ltc.modifyCadAssembly()

    return 0

if __name__ == '__main__':
    sys.exit(main())
    
