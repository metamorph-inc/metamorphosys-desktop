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

#perform Latin Hypercube Sampling

from __future__ import division # tells python to use "true" division throughout
import random
from numpy import *

#Normal LHS sampling
def LHS(dim, iterations):
    points = GetLHSpoints(dim, iterations)

    unused = []  
    for d in range(dim):
        unused.append(range(iterations))

    samples = []
    for i in range(iterations):
        sample = []
        for d in range(dim):
            rpoint = int(random.random()*len(unused[d]))
            segpoint = unused[d][rpoint]
            sample.append(points[d, segpoint])
            del unused[d][rpoint]
#            unused[d].remove(segpoint)
        samples.append(sample)

    return samples

#use Orthogonal LHS sampling for even better sampling performance
def OrthLHS(dim, iterations):
    points = GetLHSpoints(dim, iterations)
    secsizes = GetSectionSizes(dim, iterations)
    print secsizes

    unused = []  
    for d in range(dim):
        sections = []
        start = 0
        for s in range(secsizes[d]):
            secsize = int(ceil((iterations-start)/(secsizes[d]-s)))
            sections.append(range(start,start+secsize))
            start += secsize
        unused.append(sections)

    samples = []
    sampleidx = []
    sampleavail = []
    #GetOrthLHSsamples(0,dim,samples,unused,points,secsizes,[0]*dim)

    indices = GetOrthLHSsampleIndices(dim,secsizes)
    
    print unused
    for s in range(iterations):
        maxv = 0
        for i in indices:
            val = 0
            for d in range(dim):
                val += len(unused[d][i[d]])
            if val==maxv:
                available.append(i)
            if val>maxv:
                maxv = val
                available = [i]
        print 'Available indices:',available
        rpoint = int(random.random()*len(available))
        index = available[rpoint]
        if maxv<dim:
            for d in range(dim):
                for j,u in enumerate(unused[d]):
                    if len(u)>0:
                        index[d]=j
                        break
            i = sampleidx.index(index)
            last = list(index)
            print 'Choosing',index,'instead'
            sample = GetSample(index,dim,unused)
            print 'giving sample',sample
            print unused
            samples.append(sample)
            sampleidx.append(index)
            sampleavail.append(available)
            while True:
                print 'Redoing step',i
                if(len(sampleavail[i])<2):
                    print 'Could not complete'
                while last==sampleidx[i]:
                    last = sampleavail[i][int(random.random()*len(sampleavail[i]))]
                print 'For step',i,'choosing',last,'instead'
                for d in range(dim):
                    unused[d][sampleidx[i][d]].append(samples[i][d])
                print unused
                samples[i]=GetSample(last,dim,unused)
                print samples[i]
                print unused
                sampleidx[i]=last
                try:
                    i = sampleidx.index(last)
                except ValueError:
                    break
        else:
            print 'Choosing index',index,'with value',maxv,


            sample = GetSample(index,dim,unused)
            print 'giving sample',sample
            print unused
            del indices[rpoint]
            samples.append(sample)
            sampleidx.append(index)
            sampleavail.append(available)

    return samples

# recursive function to obtain LHS samples
def GetOrthLHSsamples(depth,dim,samples,unused,points,secsizes,index):
    if depth==dim:
        print 'index=',index
        for d in range(dim):    #check if there are sections available for this sample. If not, we can't take the sample
            if unused[d][index[d]] == []:
                return
        sample = GetSample(index,dim,unused)
        print sample
        samples.append(sample)
        print unused        
    else:
        for s in range(secsizes[depth]):
            index[depth]=s
            GetOrthLHSsamples(depth+1,dim,samples,unused,points,secsizes,index)

def GetSample(index,dim,unused):
    sample = []
    for d in range(dim):
        rpoint = int(random.random()*len(unused[d][index[d]]))
        segpoint = unused[d][index[d]][rpoint]
#            sample.append(points[d, segpoint])
        sample.append(segpoint)    # for testing. Comment out previous line and uncomment this line to see how the samples are distributed.
        del unused[d][index[d]][rpoint]
    return sample

def GetOrthLHSsampleIndices(dim,secsizes,depth=0,index=None):
    if depth==dim:
        return [list(index)]   #must return a copy of the index, not a reference to it
    else:
        if depth==0:
            index = [0]*dim
        Indices = []
        for s in range(secsizes[depth]):
            index[depth]=s
            Indices += GetOrthLHSsampleIndices(dim,secsizes,depth+1,index)
        return Indices

#gets how many sections each dimension should be split into
def GetSectionSizes(dim, iterations):
    sdims = []
    iters = iterations 
    for d in range(dim):
        sdims.append(int(ceil(iters ** (1/(dim-d)))))
        iters = ceil(iters/sdims[d])
    return sdims

#get a set of sample points (on 0-1)
def GetLHSpoints(dim, iterations):
    points = zeros((dim,iterations))
    segmentSize = 1 / iterations
    for d in range(dim):
        for i in range(iterations):
            segmentMin = i * segmentSize
            points[d,i] = segmentMin + (random.random() * segmentSize)
    return points

#print LHS(2,4)
#print len(LHS(4,1000))
#print len(LHS(4,10000))
#print len(LHS(4,50000))

#print OrthLHS(2,5)

#for i in range(4,20):
#    print i,len(OrthLHS(2,i))
