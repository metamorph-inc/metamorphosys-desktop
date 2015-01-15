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

#include <DiagnosticUtilities.h>

//#include <StringToEnumConversions.h>

namespace isis
{

void stream_CADComponentsTree( std::auto_ptr<CADComponentsType>  &in_CADComponents_ptr,
							   std::ostream &out_Stream )
{
	/////////////////////////////
	// Display contents of tree
	////////////////////////////
	
	for ( CADComponentsType::CADComponent_const_iterator i(in_CADComponents_ptr->CADComponent().begin()); 
		  i != in_CADComponents_ptr->CADComponent().end(); ++i )
	{
		out_Stream << std::endl << "CADComponent";   
		out_Stream << std::endl << "   Name              " << i->Name();
		out_Stream << std::endl << "   Type              " << i->Type();
		out_Stream << std::endl << "   MetricsOutputFile " << i->MetricsOutputFile();

		if ( i->ParametricParameters().present() )
		{	
			
			if (i->ParametricParameters().get().Increment().present())
			{
				for ( CADComponentsType::CADComponent_type::ParametricParameters_type::Increment_type::CADIncrementParameter_const_iterator j( i->ParametricParameters().get().Increment().get().CADIncrementParameter().begin());
						j != i->ParametricParameters().get().Increment().get().CADIncrementParameter().end();
					  ++j )	
				{
					out_Stream << std::endl << "      CADIncrementParameter";
					out_Stream << std::endl << "         Name        "  << j->Name();
					out_Stream << std::endl << "         Type        "  << j->Type();
					out_Stream << std::endl << "         StartValue  "  << j->StartValue();
					out_Stream << std::endl << "         EndValue    "  << j->EndValue();
					out_Stream << std::endl << "         Increment   "  << j->Increment();
				}
			}

			if (i->ParametricParameters().get().Read().present())
			{
				for ( CADComponentsType::CADComponent_type::ParametricParameters_type::Read_type::CADReadParameter_const_iterator j( i->ParametricParameters().get().Read().get().CADReadParameter().begin());
						j != i->ParametricParameters().get().Read().get().CADReadParameter().end();
					  ++j )	
				{
					out_Stream << std::endl << "      CADReadParameter";
					out_Stream << std::endl << "         Name        "  << j->Name();
				}
			}

		}  // if ( i->ParametricParameters().present() )

	}

} // end stream_CADComponentsTree



} // END namespace isis