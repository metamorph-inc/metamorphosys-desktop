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

#title           :Calculix_PostProcess.py
#description     :This script performs post processing on Calculix output files (.frd).
#author          :Di Yao
#date            :2012-6-19
#version         :1.0.0.0
#usage           :python pyscript.py
#notes           :
#python_version  :2.7
#==============================================================================

import sys
import ComputedMetricsSummary
import math
import AnalysisFunctions
import re
import utility_functions

def ParseCalculixOutputFile(feaName):
    skipKey = False
    
    sectionData = list()

    f = open(feaName+'.dat', 'r')
    for line in f:
        line = line.strip()
        if line == '': continue
        if (line.startswith('stresses')):
            skipKey = False
            if (len(sectionData) > 0):
                CalculateMetrics(sectionData)
                #print '=============================='
                #print sectionData
                sectionData = []           
                       
        elif (line.startswith('displacements')):
            skipKey = False
            if (len(sectionData) > 0):
                CalculateMetrics(sectionData)
                #print '=============================='
                #print sectionData
                sectionData = []
        elif (line.startswith('forces')):
            skipKey = True
            if (len(sectionData) > 0):
                CalculateMetrics(sectionData)
                sectionData=[]
            continue

        if (skipKey == False):
            sectionData.append(line)

    if (len(sectionData) > 0):
        CalculateMetrics(sectionData)
        #print '=============================='
        #print sectionData
        sectionData = []

    f.close()

def CalculateMetrics(sectionData):
    keyLine = sectionData.pop(0)
    if (keyLine.startswith('stresses')):
        keys = keyLine.split()
        ELSet_ID = keys[5]
        maxMises = 0
        maxShear = 0
        maxBearing = 0
        
        for data in sectionData:
            splittedLine = data.split()
            stressMatrix = splittedLine[2:]       #stressLevels
            tmpMise, tmpBear, tmpShear = AnalysisFunctions.FindStressMetrics(stressMatrix)
            maxMises = max(maxMises, tmpMise)
            maxShear = max(maxShear, tmpShear)
            maxBearing = max(maxBearing, tmpBear)
        # FactorOfSafety
        if (ComputedMetricsSummary.gComponentList.has_key(ELSet_ID)):
            tmpComponent = ComputedMetricsSummary.gComponentList[ELSet_ID]
            #factorOfSafety = min(float(tmpComponent.MaterialProperty['Shear'])/maxShear,
            #                 float(tmpComponent.MaterialProperty['Bearing'])/maxBearing,
            #                 float(tmpComponent.MaterialProperty['Mises'])/maxMises)
            factorOfSafety = float(tmpComponent.MaterialProperty['Mises'])/maxMises
            if (tmpComponent.MetricsInfo.has_key('Shear')):
                tmpComponent.MetricsOutput[tmpComponent.MetricsInfo['Shear']] = maxShear
            if (tmpComponent.MetricsInfo.has_key('Mises')):
                tmpComponent.MetricsOutput[tmpComponent.MetricsInfo['Mises']] = maxMises
            if (tmpComponent.MetricsInfo.has_key('Bearing')):
                tmpComponent.MetricsOutput[tmpComponent.MetricsInfo['Bearing']] = maxBearing
            if (tmpComponent.MetricsInfo.has_key('FactorOfSafety')):
                tmpComponent.MetricsOutput[tmpComponent.MetricsInfo['FactorOfSafety']] = factorOfSafety
            ComputedMetricsSummary.gComponentList[ELSet_ID] = tmpComponent      #?            

    elif (keyLine.startswith('displacements')):
        displacementData = dict()
        for data in sectionData:
            splittedLine = data.split()
            displacementData[splittedLine[0]] = AnalysisFunctions.FindDisplacementMagnitude ( float(splittedLine[1]),
                                                                                    float(splittedLine[2]),
                                                                                    float(splittedLine[3]))
    
if __name__ == '__main__':
    try:
        feaName = None
        paramFile = None
        argList = sys.argv
        argc = len(argList)
        i = 0
        while (i < argc):
            if (argList[i][:2] == '-i'):
                i+=1
                feaName = utility_functions.right_trim(argList[i], '.dat')
            elif (argList[i][:2] == '-p'):
                i+=1
                paramFile = argList[i]
            i+=1
        if not feaName or not paramFile:
            exit(1)
        ComputedMetricsSummary.ParseXMLFile(paramFile)
        ComputedMetricsSummary.PrintComponentList(ComputedMetricsSummary.gComponentList)    
        ParseCalculixOutputFile(feaName)
        ComputedMetricsSummary.WriteXMLFile(ComputedMetricsSummary.gComponentList)

        
    except Exception as e:
        print e
        print type(e)  # prints the type of exception
        print type(e).__name__  # prints the type's name
    except ZeroDivisionError:
        print "division by zero!"
        