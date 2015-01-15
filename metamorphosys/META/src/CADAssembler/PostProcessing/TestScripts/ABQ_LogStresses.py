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

import sys
from optparse import OptionParser
from odbAccess import *
#from abaqus import *
from abaqusConstants import *
#import visualization


# ===================================================================================================
# Global Variables 
# ===================================================================================================
gFile = open('StressTensors.txt', 'w')


# ===================================================================================================


# ===================================================================================================
# Functions
#

def PrintStressTensors(fileName):
    global gFile

    odbFileName=fileName

    stress = 'S'

    # Shows how to print key()
    #elemset = assembly.elementSets.keys()
    #for item in elemset:
    #    Write2Log(str(item) +'\n')

    gFile.write('MaxPrincipal,MidPrincipal,MinPrincipal,Tresca,Mises,Press,Tensor')
    try:
        myOdb = openOdb(path=odbFileName)
        lastFrame = myOdb.steps['STEP_1_SOL_101_LC'].frames[-1]
        #stressFields = lastFrame.fieldOutputs[stress]
        stressFields = lastFrame.fieldOutputs[stress]
        print 'Available Invariants of Stress:'
        print stressFields.validInvariants
        for stressValue in stressFields.values:
            gFile.write('%10.4E,%10.4E,%10.4E,%10.4E,%10.4E,%10.4E \n' % (stressValue.maxPrincipal, stressValue.midPrincipal, stressValue.minPrincipal, stressValue.tresca, stressValue.mises, stressValue.press) )
            for data in stressValue.data:
                gFile.write(',%10.5E' % data)

            
        myOdb.close()           # close odb
        gFile.close()
            
    except KeyError:
        print 'Key Error'
        myOdb.close()
        gFile.close()
        sys.exit(0)
    except AbaqusException, value:
        print 'Error:', value
        myOdb.close()
        gFile.close()
        sys.exit(0)


  
# ===================================================================================================

# ===================================================================================================
# START
#

if __name__ == '__main__':
    odbName = None
    paramFile = None
    argList = sys.argv
    argc = len(argList)
    i = 0
    while (i < argc):
        if (argList[i][:2] == '-i'):
            i+=1
            odbName = argList[i]
        i+=1
    PrintStressTensors(odbName)

