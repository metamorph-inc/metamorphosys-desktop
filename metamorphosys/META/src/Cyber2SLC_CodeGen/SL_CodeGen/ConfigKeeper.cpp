/*
Copyright (C) 2013-2015 MetaMorph Software, Inc

Permission is hereby granted, free of charge, to any person obtaining a
copy of this data, including any software or models in source or binary
form, as well as any drawings, specifications, and documentation
(collectively "the Data"), to deal in the Data without restriction,
including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Data, and to
permit persons to whom the Data is furnished to do so, subject to the
following conditions:

The above copyright notice and this permission notice shall be included
in all copies or substantial portions of the Data.

THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

=======================
This version of the META tools is a fork of an original version produced
by Vanderbilt University's Institute for Software Integrated Systems (ISIS).
Their license statement:

Copyright (C) 2011-2014 Vanderbilt University

Developed with the sponsorship of the Defense Advanced Research Projects
Agency (DARPA) and delivered to the U.S. Government with Unlimited Rights
as defined in DFARS 252.227-7013.

Permission is hereby granted, free of charge, to any person obtaining a
copy of this data, including any software or models in source or binary
form, as well as any drawings, specifications, and documentation
(collectively "the Data"), to deal in the Data without restriction,
including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Data, and to
permit persons to whom the Data is furnished to do so, subject to the
following conditions:

The above copyright notice and this permission notice shall be included
in all copies or substantial portions of the Data.

THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  
*/

/*** Included Header Files ***/
#include "ConfigKeeper.hpp"
#include <iostream>


/***************************************************************************************/


ConfigKeeper::ConfigKeeper( void ) : _desc( "Allowed options" ), _hidden( "Hidden options" ), _all( "All options" ) {
	// Add the command line switches
	_desc.add_options()
		( "help,h", "Usage information." )
		( "output-directory,p", boost::program_options::value< std::string >(), "Output directory for models." )
		( "c,c", "Output code in the \"C\" language (default)" )
		( "java,j", "Output code in the \"Java\" language" )
		( "libdir,L", boost::program_options::value<  std::vector< std::string >  >(), "Specify directory in which to search for m-files" )
		( "annotations,a", "generate java pathfinder annotations (for java code generation only)" )
	;
	// Add a hidden switch for input file
	_hidden.add_options()
		( "slsf-file,f", boost::program_options::value< std::string >(), "SLSF model file (*.mga or *.xml)." );
	// Add all the switches together
	_all.add( _desc ).add( _hidden );
	// Make sure that input file is first
	_posDesc.add( "slsf-file", 1 );
}


bool ConfigKeeper::processCommandLineArguments( int argc, char *argv[] ) {
	// Try to parse command line options
	try {
		boost::program_options::store(
			boost::program_options::command_line_parser( argc, argv ).options( _all ).positional( _posDesc ).run(), _variablesMap );
	}
	// Catch any exceptions
	catch (boost::program_options::ambiguous_option& e) {
		std::cout << "Ambiguous option " + e.get_option_name() << std::endl;
		std::cout << this->_all << std::endl;
		return false;
	}
	catch (...) {
		std::cout << "Invalid command line format.\n";
		std::cout << this->_all << std::endl;
		return false;
	}
	// Update the variables map
	boost::program_options::notify( _variablesMap );
	return true;
}


/***************************************************************************************/

