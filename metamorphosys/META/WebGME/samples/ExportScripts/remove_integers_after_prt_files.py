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

"""
This script looks in specified folder for files with either .prt.x or .asm.x extension, where x is an integer.
These file will be renamed by removing the .x parts. 
- If multiple files map to the same name - the highest integer is used.
- If a file without the .x part already exists - it will be overwritten. 

"""
import os
import sys

COMPONENT_DIR = 'components'

def find_prt_files(input_dir):
    print 'input_dir : {0}'.format(input_dir)
    prt_files = {}
    for root, dirs, files in os.walk(input_dir):
        for ff in files:
            if '.prt.' in ff or '.asm.' in ff:
                new_name = ff
                while is_integer(new_name[-1]):
                    new_name = new_name[:-1]
                new_name = new_name[:-1]
                new_name_full = os.path.join(root, new_name)
                if new_name_full in prt_files:
                    prt_files[new_name_full]['file_names'].append(ff)
                else:
                    prt_files[new_name_full] = {}
                    prt_files[new_name_full]['file_names'] = [ff]
                    prt_files[new_name_full]['root'] = root
                    prt_files[new_name_full]['new_name'] = new_name
                    

    return prt_files

def is_integer(char):
    try:
        int(char)
    except ValueError:
        return False
    return True

if __name__ == '__main__':
    files = find_prt_files(COMPONENT_DIR)
    for key, value in files.iteritems():
        print 'In dir: {0:35}  {1:20} --> {2}'.format(value['root'], value['new_name'], value['file_names'])
    ipt = raw_input('found {0} prt files with integer extension, would you like to rename these y/n [n]?'.format(len(files.keys())))
    print '"{0}"'.format(ipt)
    if not ipt in ['y', 'Y']:
        print 'Will not rename files...'
        sys.exit(0)
    for key, value in files.iteritems():
        if os.path.isfile(key):
            print 'Found file name without integer extension "{0}" in {1}, removing...'.format(value['new_name'], value['root'])
            os.remove(key)
        max_value = -1
        the_winner = ''
        for prt_file in value['file_names']:
            ext_value = int(prt_file.split('.')[-1])
            if ext_value > max_value:
                the_winner = prt_file
                max_value = ext_value
        print 'We have a winner "{0}" from {1}'.format(the_winner, value['file_names'])
        os.rename(os.path.join(value['root'], the_winner), key)
        print 'Changed (full) name to {0}'.format(key)
        for to_remove in value['file_names']:
            full_name = os.path.join(value['root'], to_remove)
            if os.path.isfile(full_name):
                os.remove(full_name)
                print 'Removed {0}'.format(full_name)
            else:
                print 'Does not exist any more {0}'.format(full_name)
    print 'Done, files have been renamed!'
    
    
    
    
    
    
    
    
    
    
    
    