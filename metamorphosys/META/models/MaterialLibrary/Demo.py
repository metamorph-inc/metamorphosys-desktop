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

#Be sure to import the LibraryManager class
from MaterialLibraryInterface import LibraryManager
#from pprint import pprint

"""
This file is a demo for the LibraryManager methods and
 demonstrates their usage. Many of the print statements
 are commented out, so that you can comment them back in
 and see what they do. 
"""

if __name__ == "__main__": #run only if main
    
    """
    First, we need to create a LibraryManager object. We'll
     call it "library_manager" so that we know what it is. 
    The parameter passed to it is the location of the .json
     document.
    """
    
    library_manager = LibraryManager()
    

    """
    This class has a number of methods we'll demonstrate. The first
     method of importance is the materialData method, which takes as
     a parameter a string representing the name of the material for
     which you need data. 
    """
    
    steelData = library_manager.materialData("steel_stainless_316_l")
    
    """
    The steelData variable now contains all the data about the 
     steel_stainless_316_l material. As it stands, it isn't 
     incredibly useful to us. We need to iterate through the 
     data structure to extract the useful information.
    """
    #pprint(steelData)
    #pprint(steelData["density"])
    
    #This assumes list is only one level deep for compound properties
    for keys in steelData:
        unit = ""
        if "unit" in steelData[keys]:
            unit = steelData[keys]["unit"]
        #print keys + " = " + str(steelData[keys]["value"]) + " " + unit
            
        
    
    #In the current version, this will print all materials, even if 
    # the values are listed as "none"
    
    #In future versions, the properties with values of "None" will not 
    # be listed in the JSON. Currently, we display these properties 
    # because other functions rely upon it
    
    #===========================================================================
    # If you don't want to see values of "None," make a conditional statement
    #  to filter these out of your results:
    #===========================================================================
    for keys in steelData:
        unit = ""
        if "unit" in steelData[keys]:
            unit = steelData[keys]["unit"]
        if steelData[keys]["value"] != "None":
            pass
            #print keys + " = " + str(steelData[keys]["value"]) + " " + unit
        
    """
    Next, we demonstrate the listAllProperties() method. As you might
     expect, it produces a list of all the material properties an 
     object might possess.  
    """

    propList =  library_manager.listAllProperties()
    
    """
    Of course, since this is a list, we need to traverse it in order to
     access its data.
    """
    
    numProperties = 0
    for prop in propList:
        #print prop
        numProperties += 1
    #print numProperties
    
    """
    Finally, we have listAllMaterials(), which, as you might
     expect, lists all the materials in the library_manager
     by name. 
    """
    
    allMaterials =  library_manager.listAllMaterials()
    
    """
    As in the previous example, if you want to access
     the variables, you'll need to traverse the list.
    """
    
    for mat in allMaterials:
        pass
        #print mat
    
    print "Checking version number: "
    version = library_manager.checkVersion(username, password)
    print "Version: " + str(version)
    library_manager.updateJSON(username, password)
    print "\nDone with the demo!"