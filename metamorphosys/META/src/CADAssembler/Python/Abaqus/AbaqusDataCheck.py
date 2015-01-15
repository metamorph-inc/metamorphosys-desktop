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

AbaqusDataCheck.py, version 1.4.0

For use with Abaqus 6.13-1 (Python 2.6.2).

Created by Ozgur Yapar   <oyapar@isis.vanderbilt.edu>
           Robert Boyles <rboyles@isis.vanderbilt.edu>

    - This script checks the Abaqus model for constraint errors
        & attempts to resolve any issues automatically.

"""

from abaqus import *
from abaqusConstants import *
import caeModules, jobMessage, odbAccess
import interaction, visualization, regionToolset, job, mesh

import csv
import glob
import math, re, sys, json, os
import operator
from optparse import OptionParser
import shutil
import string as STR
import traceback
import time
import uuid, ctypes
import xml.etree.ElementTree as ET
import _winreg
import logging

from AbaqusProcessData import CreateOverlapPNG


def dataCheck_final(myModel, args):
    logger = logging.getLogger()
    logger.info("**********************************************************************************" + '\n')
    logger.info("Checking the model for possible constraint errors (final check)." + '\n')
    dataCheckJobName3 = 'DataCheckFinal'
    myDataCheckJob3 = mdb.Job(name=dataCheckJobName3, model=myModel.name,
        description='Final Data Check job',multiprocessingMode=DEFAULT,
        parallelizationMethodExplicit=DOMAIN,numCpus=args.parallelCores,
        numDomains=args.parallelCores)
    myDataCheckJob3.submit(datacheckJob=True)
    myDataCheckJob3.waitForCompletion()
    logger.info("Final data check completed with success" + '\n')


def dataCheck_two(myModel, args):
    logger = logging.getLogger()
    logger.info("**********************************************************************************" + '\n')
    logger.info("Checking the model for possible constraint errors (2nd check)." + '\n')
    dataCheckJobName2 = 'DataCheck2'
    myDataCheckJob2 = mdb.Job(name=dataCheckJobName2, model=myModel.name,
        description='2nd Data Check job',multiprocessingMode=DEFAULT,
        parallelizationMethodExplicit=DOMAIN,numCpus=args.parallelCores,
       numDomains=args.parallelCores)
    myDataCheckJob2.submit(datacheckJob=True)
    myDataCheckJob2.waitForCompletion()
    logger.info("2nd data check completed with success" + '\n')


def tryModifyingConstraints(myModel, problemConstraints, errorNodes, first_attmpt):
    logger = logging.getLogger()
    logger.info("Attempting to modify constraints" + '\n')
    for key in errorNodes.instanceNames:
        stringKey = str(key)
        for surf in myModel.rootAssembly.allSurfaces.keys():
            surfKey = str(surf)
            if stringKey in surfKey:
                logger.info("Modifying nodes on surface " + str(surfKey) + " on instance " + str(stringKey)+ '\n')
                CPswitchKey = surfKey.replace(stringKey,"").strip()
                for swap in myModel.constraints.keys():
                    swapInstance = str(swap)
                    if swap.startswith(CPswitchKey) == True:
                        logger.info("Modifying constraint " + str(swap) + '\n')
                        if first_attmpt:
                            myModel.constraints[swap].swapSurfaces()
                            problemConstraints.append(CPswitchKey)
                        else:
                            if CPswitchKey in problemConstraints:
                                myModel.constraints[swap].swapSurfaces()


def dataCheck_one(myModel, args):
    logger = logging.getLogger()
    logger.info("**********************************************************************************" + '\n')
    logger.info("Checking the model for possible constraint errors (1st check)." + '\n')
    dataCheckJobName = 'DataCheck'
    myDataCheckJob = mdb.Job(name=dataCheckJobName, model=myModel.name,
        description='1st Data Check job',multiprocessingMode=DEFAULT,
        parallelizationMethodExplicit=DOMAIN,numCpus=args.parallelCores,
        numDomains=args.parallelCores)
    myDataCheckJob.submit(datacheckJob=True)
    myDataCheckJob.waitForCompletion()
    fControl = True
    logger.info("1st data check completed with success" + '\n')
    
    return fControl

    
def checkOverlap(odb_inst, jobName):
    logger = logging.getLogger()
    overlapCheck = False
    try:
        overlapCheck = odb_inst.rootAssembly.elementSets['ErrElemVolSmallNegZero']
    except:
        pass
    if overlapCheck:
        logger.info("There are elements in the model with zero or negative volume" + '\n')
        logger.info("Possibly parts are overlapping each other excessively" + '\n')
        logger.info("Unable to run the analyis, please fix your CAD model" + '\n')
        logger.info("Terminating" + '\n')
        
        CreateOverlapPNG(jobName,overlapCheck, os.getcwd())
        logger.info("Check \"results/abcdef/Analysis/Abaqus/Contour_and_BC_plots/Overlapping_Elements_#.png\" files to determine problematic parts/elements" + '\n')
        raise
    

def checkConstraintErrors(myModel, args):
    logger = logging.getLogger()
    try:
        fControl = dataCheck_one(myModel, args)
    except:
        fControl = False
        logger.info("1st data check failed" + '\n')
        logger.info("Constraint issues might be present" + '\n')
    
    problemConstraints = []
    if not fControl:
        jobName = 'DataCheck'
        odb = odbAccess.openOdb(path=jobName + '.odb')
        try:
            errorNodes = odb.rootAssembly.nodeSets['ErrNodeOverconTieSlave']
            tryModifyingConstraints(myModel, problemConstraints, errorNodes, True)
        except:
            logger.info("WARNING: Cannot modify constraints" + '\n')
            logger.info("WARNING: Turning ON the option for adjusting initial surface positions and specifying initial clearances" + '\n')
            for key in myModel.constraints.keys():
                if key[:5] == 'Rigid':
                    continue
                myModel.constraints[key].setValues(adjust=ON)
                if key[0:3] == 'CC-':
                    myModel.constraints[key].setValues(tieRotations=OFF)
    
        try:
            dataCheck_two(myModel, args)
        except:
            logger.info("2nd data check failed" + '\n')
            jobName2 = 'DataCheck2'
            odb2 = odbAccess.openOdb(path=jobName2 + '.odb')
            overlapCheck = False
            
            try:
                errorNodes = odb2.rootAssembly.nodeSets['ErrNodeOverconTieSlave']
                tryModifyingConstraints(myModel, problemConstraints, errorNodes, True)
            except:
                pass
            checkOverlap(odb2, jobName2) 
    try:
        dataCheck_final(myModel, args)
    except:
        logger.info(STR.join(traceback.format_exception(*sys.exc_info())))
        logger.info("Final data check failed" + '\n')
        jobName3 = 'DataCheckFinal'
        odb3 = odbAccess.openOdb(path=jobName3 + '.odb')
        
        checkOverlap(odb3, jobName3)
        
    logger.info("**********************************************************************************" + '\n')


