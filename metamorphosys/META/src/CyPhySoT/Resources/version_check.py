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

#!/bin/python
def check_version(min_version=(2,7,3), 
                  max_version=(2,7,4),
                  architecture='32bit',
                  msg_requirement='Please use Python 2.7.3 win32'):
    """
    Parameter architecture takes values '32bit', '64bit' or '' if check is not required. 
    
    If we only accept one version (a,b,c) -> min_version = (a,b,c) and max_version (a,b,c+1), 
    e.g., to specify 2.7.2 then set min_version=(2,7,2) and max_version=(2,7,3)
    
    """
    is_version_ok = None
    is_arch_type_ok = None

    # from python 1.5.2
    import sys
    print(sys.version)

    if not 'version_info' in dir(sys):
        print('Python version is too old')
    else:
        # from python 2.0
        print(sys.version_info)
        
        # TODO: what if one of them is not defined? min or max or both?
        if sys.version_info > min_version and sys.version_info < max_version:
            print('OK version number.')
            is_version_ok = True
        else:
            print('Wrong version number.')
            
        if architecture:
            if sys.version_info < (2, 3):
                print('Cannot check architecture python version is too old.')
            else:
                import platform
                if platform.architecture()[0] == architecture:
                    print('OK architecture ' + str(platform.architecture()))
                    is_arch_type_ok = True
                else:
                    print('Wrong architecture: ' + str(platform.architecture()) + ' required : ' + architecture)
        else:
            is_arch_type_ok = eval("True")
    return  is_version_ok and is_arch_type_ok

