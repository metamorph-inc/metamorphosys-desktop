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


# for file in ../src/*/ComponentConfig.h; do /c/python27/python gen_GME_interpreter_wxi.py  "$file"; done

import sys
import os

_template = '''<?xml version="1.0" encoding="utf-8"?>
#set $backslash = '\\\\'
<Wix xmlns="http://schemas.microsoft.com/wix/2006/wi">
<Fragment>
  <DirectoryRef Id="INSTALLDIR_BIN" />
</Fragment>
<Fragment>
<ComponentGroup Id="${COMPONENT_NAME}">
  <Component Id="${COMPONENT_NAME}.dll" Directory="INSTALLDIR_BIN">
    <File Source="\\$(var.InterpreterBin)$backslash${COMPONENT_NAME}.dll">
      <TypeLib Id="$TYPELIB_UUID" Description="$TYPELIB_NAME" Language="0" MajorVersion="1" MinorVersion="0">
        <Class Id="{$COCLASS_UUID}" Context="InprocServer32" Description="$COCLASS_PROGID">
          <ProgId Id="$COCLASS_PROGID" Description="$COCLASS_PROGID" />
        </Class>
      </TypeLib>
    </File>
    <RegistryKey Root='HKLM' Key='Software\GME\Components$backslash${COCLASS_PROGID}'>
      <RegistryValue Name='Description' Type='string' Value='$COMPONENT_NAME'/>
      <RegistryValue Name='Icon' Type='string' Value=',IDI_COMPICON'/>
      <RegistryValue Name='Paradigm' Type='string' Value='$PARADIGMS'/>
      <RegistryValue Name='Tooltip' Type='string' Value='$TOOLTIP_TEXT'/>
## FIXME: 1==Interpreter. Can we support Addons?
      <RegistryValue Name='Type' Type='integer' Value='1'/>

      <RegistryKey Key='Associated'>
        <RegistryValue Name='$PARADIGMS' Type='string' Value=''/>
      </RegistryKey>

    </RegistryKey>
  </Component>
</ComponentGroup>
</Fragment>
</Wix>
'''

if __name__=='__main__':
    if os.environ.has_key("UDM_3RDPARTY_PATH"):
        sys.path.append(os.path.join(os.environ["UDM_3RDPARTY_PATH"], r"Cheetah-2.4.4\build\lib.win32-2.6"))
    from Cheetah.Template import Template
    import re
    with open(sys.argv[1], 'r') as config:
        lines = config.readlines()
    defines = {}
    for line in filter(lambda line: line.find('define') != -1, lines):
        match = re.match(r"^.define\s+(\w+)\s+\"?([\w.,/ \(\)-]+)\"?\s*$", line)
        if match:
            defines[match.groups()[0]] = match.groups()[1]
        else:
            sys.stderr.write("Warning: nonmatching line " + line + "\n")
    with open(defines['COMPONENT_NAME'] + ".wxi", 'wb') as output:
        output.write(str(Template(_template, searchList=(defines,))))

