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


import io
import json
import csv

class save_results():

    jsonDict = dict({'inputNames': ''})
    jsonDict.update({'inputNames': list()})
    jsonDict['outputNames'] = []
    jsonDict['result'] = []
    
    Assembly = None
    
    def __init__(self, assembly, results):
        self.Assembly = assembly
        # Get name
        self.jsonDict['name'] = assembly.name
        
        # Get type
        self.jsonDict['type'] = 'TODO TODO TODO' #assembly.driver.type
        
        # Get input names
        for i in assembly.driver.list_param_targets():
            self.jsonDict['inputNames'].append(i)
        # Get output names
        for i in assembly.driver.case_outputs:
            self.jsonDict['outputNames'].append(i)
        
        
        # Get results
        for c in assembly.driver.recorders[0].get_iterator():
            thisResult = dict();
            for i in assembly.driver.list_param_targets():
                thisResult[i] = c[i]
            # Get output names
            for i in assembly.driver.case_outputs:
                thisResult[i] = c[i]
                
            self.jsonDict['result'].append(thisResult)
            
    def save(self, filename):
        # save dictionary into a JSON file
        value = json.dumps(self.jsonDict, indent = 4)
        f = open(filename, 'w+')
        f.write(value)
        f.close()
        
        
        with open('output.csv', 'wb') as fcsv:
            writer = csv.writer(fcsv)
            j = 0
            
            thisResult = ["#id"]
            
            for i in self.Assembly.driver.list_param_targets():
                thisResult.append(i)
            # Get output names
            for i in self.Assembly.driver.case_outputs:
                thisResult.append(i)
            
            writer.writerow(thisResult)
            
            for c in self.Assembly.driver.recorders[0].get_iterator():
                j = j + 1;
                thisResult = [j]
                
                for i in self.Assembly.driver.list_param_targets():
                    thisResult.append(c[i])
                # Get output names
                for i in self.Assembly.driver.case_outputs:
                    thisResult.append(c[i])
                    
                
                writer.writerow(thisResult)
    
        # return with JSON in a string format
        return value

    def getJSON(self):
        # return with the dictionary
        return json.dumps(self.jsonDict)