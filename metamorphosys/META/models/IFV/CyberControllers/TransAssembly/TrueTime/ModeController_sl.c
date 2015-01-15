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

#include "ModeController_sl.h"



/* SIMPLIFIED PROGRAM FUNCTIONS */
void ModeController_main( ModeController_100000151_context *context, float Velocity_2, float BatteryVoltage_3, float VelocityDemand_4, float *Mode_5 ) {
  ModeController_100000151_main( context, Velocity_2, BatteryVoltage_3, VelocityDemand_4, Mode_5 );
}

void ModeController_init( ModeController_100000151_context *context ) {
  ModeController_100000151_init( context );
}

void ModeController_100000151_main( ModeController_100000151_context *context, float Velocity_2, float BatteryVoltage_3, float VelocityDemand_4, float *Mode_5 )
{
  float sig_0;
  float sig_1;
  float sig_2;
  float sig_3;
  float sig_4;
  float sig_5;
  float sig_6;
  float sig_7;

  sig_7 = ( *context ).Value771;
  sig_6 = ( *context ).Value776;
  if ( sig_4 < 200 * sig_1 )
  {
    sig_5 = 1;
  }
  else if ( sig_3 < sig_2 )
  {
    sig_5 = 0;
  }
  else if ( sig_4 < 200 * sig_0 )
  {
    sig_5 = 1;
  }
  else if ( 1 )
  {
    sig_5 = 2;
  }

}

void ModeController_100000151_init( ModeController_100000151_context *context )
{
  ( *context ).Value771 = 0.939999999999999950000;
  ( *context ).Value776 = 0.0500000000000000030000;
}

