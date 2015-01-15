// Copyright (C) 2013-2015 MetaMorph Software, Inc

// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this data, including any software or models in source or binary
// form, as well as any drawings, specifications, and documentation
// (collectively "the Data"), to deal in the Data without restriction,
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Data, and to
// permit persons to whom the Data is furnished to do so, subject to the
// following conditions:

// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Data.

// THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

// =======================
// This version of the META tools is a fork of an original version produced
// by Vanderbilt University's Institute for Software Integrated Systems (ISIS).
// Their license statement:

// Copyright (C) 2011-2014 Vanderbilt University

// Developed with the sponsorship of the Defense Advanced Research Projects
// Agency (DARPA) and delivered to the U.S. Government with Unlimited Rights
// as defined in DFARS 252.227-7013.

// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this data, including any software or models in source or binary
// form, as well as any drawings, specifications, and documentation
// (collectively "the Data"), to deal in the Data without restriction,
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Data, and to
// permit persons to whom the Data is furnished to do so, subject to the
// following conditions:

// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Data.

// THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

#include "AutoTransController_sl.h"



/* SIMPLIFIED PROGRAM FUNCTIONS */
void AutoTransController_main( AutoTransController_100000117_context *context, float w_2, float *gear_ratio_3, float *gear_num_4 ) {
  AutoTransController_100000117_main( context, w_2, gear_ratio_3, gear_num_4 );
}

void AutoTransController_init( AutoTransController_100000117_context *context ) {
  AutoTransController_100000117_init( context );
}

void AutoTransController_100000117_main( AutoTransController_100000117_context *context, float w_2, float *gear_ratio_3, float *gear_num_4 )
{
  float sig_0;
  float sig_1;
  float sig_2;
  float sig_3;
  float sig_4;
  float sig_5;
  float sig_6;
  float sig_7;

  sig_0 = ( *context ).X0592;
  sig_2 = ( *context ).Value597;
  sig_1 = ( *context ).Value601;
  sig_4 = sig_0;
  sig_3 = 0.120000;
  if ( sig_7 > sig_5 * 2 * 3.14159265358979310000 / 60 && sig_0 < 4 )
  {
    sig_4 = sig_0 + 1;
  }
  else if ( sig_7 < sig_6 * 2 * 3.14159265358979310000 / 60 && sig_0 > 1 )
  {
    sig_4 = sig_0 - 1;
  }

  if ( sig_0 < 1 )
  {
    sig_4 = 1;
  }

  if ( sig_0 > 4 )
  {
    sig_4 = 4;
  }

  if ( sig_4 == 1 )
  {
    sig_3 = 0.034285714285714280000;
  }
  else if ( sig_4 == 2 )
  {
    sig_3 = 0.0545454545454545430000;
  }
  else if ( sig_4 == 3 )
  {
    sig_3 = 0.0857142857142857150000;
  }
  else if ( sig_4 == 4 )
  {
    sig_3 = 0.120000;
  }

  ( *context ).X0592 = sig_4;
}

void AutoTransController_100000117_init( AutoTransController_100000117_context *context )
{
  ( *context ).X0592 = 0;
  ( *context ).Value597 = 2400;
  ( *context ).Value601 = 1400;
}

