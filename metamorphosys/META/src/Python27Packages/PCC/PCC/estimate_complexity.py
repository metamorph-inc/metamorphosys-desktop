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

## Chelsea He and Emily Clements, MIT
## estimate_complexity.py
## June 26, 2012
##
## This function estimates the complexity of a random variable Z
## based on either the pdf of Z or samples of Z.  The complexity 
## metric used is exponential entropy as defined by Campbell (1966)

# Import libraries
from numpy import *
import scipy.stats.kde as kde
from scipy.stats import norm

def with_distribution(dist,limits,mean,variance,numbins):

    if limits[0] > -inf:
        lb = limits[0]-3*math.sqrt(variance)
    else:
        lb = mean-5*math.sqrt(variance)
    if limits[1] < inf:
        ub = limits[1]+3*math.sqrt(variance)
    else:
        ub = mean+5*math.sqrt(variance)

    bins = linspace(lb,ub,numbins)

    # Generate Gaussian pdf
    f_z = norm.pdf(bins, mean, math.sqrt(variance))

    # Estimate complexity based on pdf
    return with_pdf(bins,f_z)


############### Method I: supply pdf ############### 
def with_pdf(bins,f_z):

    # Compute bin size
    binsize = bins[1]-bins[0]

    # Initialize entropy value with log(binsize) -- correction term for discretizing pdf
    entsum = log(binsize)

    # Compute differential entropy and complexity     
    for fz in f_z:

        # Consider only terms where f_z > 0 (otherwise log(f_z) --> log(0) will cause trouble)
        if fz*binsize > 1e-320:
            entsum = entsum - fz*binsize*log(fz*binsize) 
            
    entropy =  entsum   
    complexity = exp(entropy)
    return complexity

####### Method II: supply Monte Carlo samples #######
def with_samples(Z,numbins):

    # Turn list into array
    z = array(Z)
    
    # Density estimation, discretized into bins
    bins = linspace(min(z),max(z),numbins)
    binsize = bins[1]-bins[0]
    f_z = kde.gaussian_kde(z).evaluate(bins) 

    # Initialize entropy value with log(binsize) -- correction term for discretizing pdf
    entsum = log(binsize)

    # Compute differential entropy and complexity 
    for fz in f_z:
        
        # Consider only terms where f_z > 0 (otherwise log(f_z) --> log(0) will cause trouble)
        if fz > 0:
            entsum = entsum - fz*binsize*log(fz*binsize)
            
    entropy =  entsum 
    complexity = exp(entropy)
    return complexity
