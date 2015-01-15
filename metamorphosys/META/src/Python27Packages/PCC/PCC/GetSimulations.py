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

from operator import mul
from numpy import *

#return the number of simulations expected for any given method
def GetSimulations(method, inputs, num=1280, nodes=None):
    if nodes==None:
        nodes=[5 for i in range(inputs)]
    elif isinstance(nodes, int):
        nodes=[nodes for i in range(inputs)]

    if method == 1:   # MCS: Monte Carlo Simulation
        return num
    elif method == 2:   # TS: Taylor Series Approximation
        return 1+2*inputs
    elif method == 3:   # MPP: Most Probable Point Method
        return nan
    elif method == 4:   # FFNI: Full Factorial Numerical Integration
        return reduce(mul, nodes)*inputs
    elif method == 5:   # UDR: Univariate Dimension Reduction Method
        return sum(nodes) + 1
    elif method == 6:   # PCE: Polynomial Chaos Expansion
        return reduce(mul, nodes)
    elif method == 7:   # Sobols Method (SOBOL)
        return 2*num+inputs*num
    elif method == 9:   # FAST Method (FAST)
        MI = 4#: maximum number of fourier coefficients that may be retained in
        OM = SETFREQ(inputs)
        OMAX = int(OM[inputs-1])
        return 2 * MI * OMAX + 1
    elif method == 10:   # Extended FAST Method (EFAST)
        NR = 1#: no. of search curvesint(floor((num / NR - 1) / (2 * MI) / num))
        MI = 4#: maximum number of fourier coefficients that may be retained in calculating
        OMi = int(floor((num / NR - 1) / (2 * MI) / inputs))
        return inputs*(2 * MI * OMi + 1)
    elif method == 11:   # DPCE: Dakota implementation of Polynomial Chaos Expansion
        return reduce(mul, nodes)
    else:
        return nan

# Selection of a frequency set. Done recursively as described in:
# A computational implementation of FAST [McRae et al.]
def SETFREQ(inpt):
    OMEGA = [0, 3, 1, 5, 11, 1, 17, 23, 19, 25, 41, 31, 23, 87, 67, 73, 85, 143, 149, 99, 119, 237, 267, 283, 151, 385, 157, 215, 449, 163, 337, 253, 375, 441, 673, 773, 875, 873, 587, 849, 623, 637, 891, 943, 1171, 1225, 1335, 1725, 1663, 2019]
    DN = [4, 8, 6, 10, 20, 22, 32, 40, 38, 26, 56, 62, 46, 76, 96, 60, 86, 126, 134, 112, 92, 128, 154, 196, 34, 416, 106, 208, 328, 198, 382, 88, 348, 186, 140, 170, 284, 568, 302, 438, 410, 248, 448, 388, 596, 216, 100, 488, 166, 0]
    OM = zeros(inpt)
    OM[0] = OMEGA[inpt-1]
    for i in range(1,inpt):
        OM[i] = OM[i - 1] + DN[inpt - 1  - i]
    # to use the same frequencies as SimLab...
    if inpt == 2:    
        OM = [5, 9]
    elif inpt == 3:    
        OM = [1, 9, 15]

    return OM
