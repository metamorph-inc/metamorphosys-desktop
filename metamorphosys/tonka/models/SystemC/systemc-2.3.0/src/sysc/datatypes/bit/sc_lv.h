/*****************************************************************************

  The following code is derived, directly or indirectly, from the SystemC
  source code Copyright (c) 1996-2006 by all Contributors.
  All Rights reserved.

  The contents of this file are subject to the restrictions and limitations
  set forth in the SystemC Open Source License Version 2.4 (the "License");
  You may not use this file except in compliance with such restrictions and
  limitations. You may obtain instructions on how to receive a copy of the
  License at http://www.systemc.org/. Software distributed by Contributors
  under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
  ANY KIND, either express or implied. See the License for the specific
  language governing rights and limitations under the License.

 *****************************************************************************/

/*****************************************************************************

  sc_lv.h -- Arbitrary size logic vector class.

  Original Author: Gene Bushuyev, Synopsys, Inc.

 *****************************************************************************/

/*****************************************************************************

  MODIFICATION LOG - modifiers, enter your name, affiliation, date and
  changes you are making here.

      Name, Affiliation, Date:
  Description of Modification:

 *****************************************************************************/

// $Log: sc_lv.h,v $
// Revision 1.1.1.1  2006/12/15 20:20:04  acg
// SystemC 2.3
//
// Revision 1.3  2006/01/13 18:53:53  acg
// Andy Goodrich: added $Log command so that CVS comments are reproduced in
// the source.
//

#ifndef SC_LV_H
#define SC_LV_H


#include "sysc/datatypes/bit/sc_lv_base.h"


namespace sc_dt
{

// classes defined in this module
template <int W> class sc_lv;


// ----------------------------------------------------------------------------
//  CLASS TEMPLATE : sc_lv<W>
//
//  Arbitrary size logic vector class.
// ----------------------------------------------------------------------------

template <int W>
class sc_lv
    : public sc_lv_base
{
public:

    // constructors

    sc_lv()
	: sc_lv_base( W )
	{}

    explicit sc_lv( const sc_logic& init_value )
	: sc_lv_base( init_value, W )
	{}

    explicit sc_lv( bool init_value )
	: sc_lv_base( sc_logic( init_value ), W )
	{}

    explicit sc_lv( char init_value )
	: sc_lv_base( sc_logic( init_value ), W )
	{}

    sc_lv( const char* a )
	: sc_lv_base( W )
	{ sc_lv_base::operator = ( a ); }

    sc_lv( const bool* a )
	: sc_lv_base( W )
	{ sc_lv_base::operator = ( a ); }

    sc_lv( const sc_logic* a )
	: sc_lv_base( W )
	{ sc_lv_base::operator = ( a ); }

    sc_lv( const sc_unsigned& a )
	: sc_lv_base( W )
	{ sc_lv_base::operator = ( a ); }

    sc_lv( const sc_signed& a )
	: sc_lv_base( W )
	{ sc_lv_base::operator = ( a ); }

    sc_lv( const sc_uint_base& a )
	: sc_lv_base( W )
	{ sc_lv_base::operator = ( a ); }

    sc_lv( const sc_int_base& a )
	: sc_lv_base( W )
	{ sc_lv_base::operator = ( a ); }

    sc_lv( unsigned long a )
	: sc_lv_base( W )
	{ sc_lv_base::operator = ( a ); }

    sc_lv( long a )
	: sc_lv_base( W )
	{ sc_lv_base::operator = ( a ); }

    sc_lv( unsigned int a )
	: sc_lv_base( W )
	{ sc_lv_base::operator = ( a ); }

    sc_lv( int a )
	: sc_lv_base( W )
	{ sc_lv_base::operator = ( a ); }

    sc_lv( uint64 a )
	: sc_lv_base( W )
	{ sc_lv_base::operator = ( a ); }

    sc_lv( int64 a )
	: sc_lv_base( W )
	{ sc_lv_base::operator = ( a ); }

    template <class X>
    sc_lv( const sc_proxy<X>& a )
	: sc_lv_base( W )
	{ sc_lv_base::operator = ( a ); }

    sc_lv( const sc_lv<W>& a )
	: sc_lv_base( a )
	{}


    // assignment operators

    template <class X>
    sc_lv<W>& operator = ( const sc_proxy<X>& a )
	{ sc_lv_base::operator = ( a ); return *this; }

    sc_lv<W>& operator = ( const sc_lv<W>& a )
	{ sc_lv_base::operator = ( a ); return *this; }

    sc_lv<W>& operator = ( const char* a )
	{ sc_lv_base::operator = ( a ); return *this; }

    sc_lv<W>& operator = ( const bool* a )
	{ sc_lv_base::operator = ( a ); return *this; }

    sc_lv<W>& operator = ( const sc_logic* a )
	{ sc_lv_base::operator = ( a ); return *this; }

    sc_lv<W>& operator = ( const sc_unsigned& a )
	{ sc_lv_base::operator = ( a ); return *this; }

    sc_lv<W>& operator = ( const sc_signed& a )
	{ sc_lv_base::operator = ( a ); return *this; }

    sc_lv<W>& operator = ( const sc_uint_base& a )
	{ sc_lv_base::operator = ( a ); return *this; }

    sc_lv<W>& operator = ( const sc_int_base& a )
	{ sc_lv_base::operator = ( a ); return *this; }

    sc_lv<W>& operator = ( unsigned long a )
	{ sc_lv_base::operator = ( a ); return *this; }

    sc_lv<W>& operator = ( long a )
	{ sc_lv_base::operator = ( a ); return *this; }

    sc_lv<W>& operator = ( unsigned int a )
	{ sc_lv_base::operator = ( a ); return *this; }

    sc_lv<W>& operator = ( int a )
	{ sc_lv_base::operator = ( a ); return *this; }

    sc_lv<W>& operator = ( uint64 a )
	{ sc_lv_base::operator = ( a ); return *this; }

    sc_lv<W>& operator = ( int64 a )
	{ sc_lv_base::operator = ( a ); return *this; }
};

} // namespace sc_dt


#endif
