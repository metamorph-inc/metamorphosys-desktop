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

import os, fnmatch

def locate(pattern, root=os.curdir):
    '''Locate all files matching supplied filename pattern in and below
    supplied root directory.'''
    for path, dirs, files in os.walk(os.path.abspath(root)):
        for filename in fnmatch.filter(files, pattern):
            yield os.path.join(path, filename)

d_terms = {
    'RangeType': 'String',
    'RangeType?': 'String',
    'UnitType': 'String',
    'UnitType?': 'String',
    'Path': 'String',
    'URI': 'String',
    'URI?': 'String',
    'Real': 'Double',
    'Real?': 'Double?',
    'DistributionParameterType': 'String',
    'DistributionParameterType?': 'String',
    'ID': 'String',
    'HashType?': 'String',
    'HashType': 'String',
    'AVMID': 'String',
    'AVMID?': 'String',
    'Date': 'String',
    'Date?': 'String',
    'Integer?': 'int?',
    'Integer': 'int',
    'String?': 'String',
    'String[*]': 'String[]',
	'DimType?': 'String',
	'DimType': 'String',
    'XML': 'String'
}

def TransformFile(s_path):
    f = open(s_path, 'r')
    sa_code = f.readlines()
    f.close()

    b_changesInFile = 0
    for i in range(len(sa_code)):
        s_codeLine = sa_code[i]

        b_changesInLine = 0
        for k, v in d_terms.iteritems():
            s_find = ' ' + k + ' '
            s_replace = ' ' + v + ' '
            if s_codeLine.find(s_find) != -1:
                b_changesInLine += 1
            s_codeLine = s_codeLine.replace(s_find, s_replace)

            s_find = '<' + k + '>'
            s_replace = '<' + v + '>'
            if s_codeLine.find(s_find) != -1:
                b_changesInLine += 1
            s_codeLine = s_codeLine.replace(s_find, s_replace)

        if s_codeLine.find("public enum ") != -1 and s_codeLine.find(" : int") != -1:
            s_codeLine = s_codeLine.replace(" : int", "")
            b_changesInLine += 1

        if s_codeLine.find("::") != -1:
            s_codeLine = s_codeLine.replace("::", ".")
            b_changesInLine += 1

        if s_codeLine.find("IEnumerable") != -1:
            s_codeLine = s_codeLine.replace("IEnumerable", "List")
            b_changesInLine += 1

        if s_codeLine.find(": ValueType") != -1:
            s_codeLine = s_codeLine.replace(": ValueType", ": META.Design.ValueType")
            b_changesInLine += 1

        if b_changesInLine > 0:
            b_changesInFile += b_changesInLine
            sa_code[i] = s_codeLine

    if b_changesInFile > 0:
        f = open(s_path, 'w')
        f.writelines(sa_code)
        f.close()

if __name__ == "__main__":
    # We are going to get all *.cs files in "GeneratedCode" and
    #   do some find-and-replace in them

    print os.curdir

    for s_path in locate("*.cs", os.curdir + '\..\..\GeneratedCode'):
        TransformFile(s_path)