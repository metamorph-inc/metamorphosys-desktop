#!/bin/bash
#
# Copyright (c) 2015 Metamorph, Inc.
# Build Systemc 2.3.1 on UNIX/Linux platforms for the ARA/Tonka library
# 
# Author: Peter Volgyesi <pvolgyesi@metamorphsoftwre.com>
# 

SYSTEMC_ROOT=`dirname "$(readlink -f "$0")"`

mkdir -p $SYSTEMC_ROOT/objdir
pushd $SYSTEMC_ROOT/objdir
../configure && make && make check && make install
popd
