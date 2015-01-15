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

from numpy import *
import json


def except_NaN(x):
    #print "Found invalid {0}".format(x)
    #if x == 'NaN':
    #    return NaN
    
    ## This is dealt with in the generated scripts
    pass

def jsonout(Outfile, Results, Methd, Input):
    '''Writes the results as a json object

    Results is a dict containing 'Moments', 'Distribution', 'PCC', 'CorrelationMatrix',
    'CovarianceMatrix'.  'PCC' is assumed to have an extra value used as 'JointPCC'.

    Methd is a str indicating the method used

    Input is assumed to be of the format described in the default, taken directly from Inputformat.json
    '''

    #The json module doesn't like arrays, matrices, or complex numbers
    unarray(Results)

    # import pdb; pdb.set_trace()

    #The PCCConfigMetrics list is built by extracting each output's values
    #from the value lists
    PCCConfigMetrics = []
    for i in range(len(Input['Configurations']['Configuration']['PCCInputArguments']['OutputIDs'])):
        ## Edited by Patrik Meijer @ ISIS - Copy ModelicaVariableNames as well.
        PCCConfigMetrics.append(dict((k, Input['Configurations']['Configuration']['PCCInputArguments']['PCCMetrics'][i][k])\
            for k in ('ID','Name','TestBenchMetricName','Limits')))
        try: # The try blocks only apply to UP methods (9/4/12)
            PCCConfigMetrics[i]['PCC'] = Results['PCC'][i]
            for grouping in ['Distribution','Moments','Plotting']:
                PCCConfigMetrics[i][grouping] = {}
                for key, value in Results[grouping].items():
                    PCCConfigMetrics[i][grouping][key] = value[i]
        except KeyError:
            pass

    #The InputDistributions list is copied directly from the Input
    InputDistributions = Input['Configurations']['Configuration']['PCCInputArguments'][
      'StochasticInputs']['InputDistributions']

    #The Method dict puts everything together, including everything else in Results
    Method = {'InputDistributions': InputDistributions, 'PCCConfigMetrics': PCCConfigMetrics}
    for k in set(Results)-set(PCCConfigMetrics[0]):
        Method[k] = Results[k]
    try:
        Method['JointPCC'] = Results['PCC'][-1]
    except KeyError:
        pass

    #Update the report file, allowing for more than one method
    with open(Outfile,'r') as f:
        json_tree = json.load(f, parse_constant=except_NaN)

    json_tree['PCCResults'] = json_tree.get('PCCResults',{})
    json_tree['PCCResults'][Methd] = Method
    json_tree['Status'] = 'OK'

    with open(Outfile,'w') as f:
        json.dump(json_tree,f, indent=4)

def unarray(dic):
    '''Turns all arrays in dict dic into nested lists recursively'''
    for (key, item) in dic.items():
        if type(item) is dict:
            unarray(dic[key])
        else:
            try:
                dic[key] = item.tolist()
            except AttributeError:
                pass

