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
import logging

# Calculate nodes and weights for Guassian quadrature using probability densities.
# Adapted from Netlib routine gaussq.f
def gaussquad(n=None, dist=None, param1=None, param2=None):
    if n < 0:
        logging.error('need non-negative number of nodes')
        raise ValueError,'need non-negative number of nodes'
    elif n == 0:
        nodes = 0
        weights = 0
    elif n == 1:
        if dist == 'UNIF':
            x = (param1 + param2) / 2
        elif dist == 'BETA':
            x = param1 / (param1 + param2)
        elif dist == 'NORM':
            x = param1
        elif dist == 'LNORM':
            x = exp(param1)
        else:
            logging.error('Unexpected distribution type')
            raise ValueError,'Unexpected distribution type'
        nodes = x
        weights = 1
    else:
        if dist=='BETA' and param1 == 0.5 and param2 == 0.5:
            dist = 'beta1'
        if dist=='BETA' and param1 == 1.5 and param2 == 1.5:
            dist = 'beta2'

        i = arange(1.0,n+1)
        i1 = arange(1.0,n)
        if dist == 'UNIF':
            a = zeros((n, 1))
            b = i1 / sqrt(4 * i1 ** 2 - 1)
        elif dist == 'beta1':
            a = zeros((n, 1))
            b = 0.5 * ones((n - 1, 1))
            b[0] = sqrt(0.5)
        elif dist == 'beta2':
            a = zeros((n, 1))
            b = 0.5 * ones((n - 1, 1))
        elif dist == 'NORM':
            a = zeros((n, 1))
            b = sqrt(i1 / 2.)
        elif dist == 'LNORM':
            a = zeros((n, 1))
            b = sqrt(i1 / 2.)
        elif dist == 'BETA':
            ab = param1 + param2
            a = i
            a[0] = param1 - param2 / ab
            i2 = arange(2,n+1)
            abi = ab - 2 + 2 * i2
            a[1:n] = ((param1 - 1) ** 2 - (param2 - 1) ** 2) / (abi - 2) / abi
            b = i1
            b[0] = sqrt(4 * param1 * param2 / ab ** 2. / (ab + 1))
            i2 = i1[1:(n - 1)]
            abi = ab - 2 + 2 * i2
            b[1:(n - 1)] = sqrt(4. * i2 * (i2 + param1 - 1) * (i2 + param2 - 1) * (i2 + ab - 2) / (abi ** 2 - 1) / abi ** 2)
        else:
            logging.error('Unexpected distribution type.')
            raise ValueError,'Unexpected distribution type.'
        A = zeros((n * n, 1))
        for j in range(0,n):
            A[(n + 1) * j] = a[j]
        for j in range(0,n - 1):
            A[(n + 1) * j + 1] = b[j]
            A[(n + 1) * (j+1)-1] = b[j]
        A = A.reshape(n, n)
        x, V = linalg.eig(A)
        w = V[0,:]
        w = w**2
        if dist == 'UNIF':
            x = param1 + (param2 - param1) * (x + 1) / 2.
        elif dist == 'BETA':
            x = (x + 1) / 2.
        elif dist == 'beta1':
            x = (x + 1) / 2.
        elif dist == 'beta2':
            x = (x + 1) / 2.
        elif dist == 'NORM':
            x = param1 + sqrt(2) * param2 * x
        elif dist == 'LNORM':
            x = exp(param1 + sqrt(2) * param2 * x)
        nodes = x
        weights = w

    return nodes, weights
