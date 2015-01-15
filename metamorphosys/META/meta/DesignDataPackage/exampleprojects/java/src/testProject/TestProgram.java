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

package testProject;
import javax.xml.bind.*;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.StringWriter;
import avm.*;
import modelica.*;
import cad.*;

public class TestProgram {
	/**
	 * @param args
	 */
	public static void main(String[] args) {
		/* Create and marshal a new component */
		try
		{
			System.out.println("hello");
			avm.ObjectFactory of = new avm.ObjectFactory();
			Component c = of.createComponent();
			c.setName("TestComponent");
			
			modelica.ModelicaModel mm = new ModelicaModel();
			mm.setClazz("path.to.class");
			c.getDomainModel().add(mm);
			
			StringWriter writer = new StringWriter();
			JAXBContext context = JAXBContext.newInstance(Component.class);
			Marshaller m = context.createMarshaller();
		    m.setProperty( Marshaller.JAXB_FORMATTED_OUTPUT, Boolean.TRUE );
			m.marshal(c, writer);
			
			System.out.println(writer.toString());
		}
		catch (JAXBException jex)
		{
			System.out.println(jex.getMessage());		
		}
		
		/* Unmarshal the XML file from the Python version */
		try {
			System.out.println("--------------------------------");
			System.out.println("Unmarshalled from Python example");
			System.out.println("--------------------------------");
						
			FileInputStream fis = new FileInputStream("../python/python_test_out.acm");
			JAXBContext context = JAXBContext.newInstance(Component.class);
			Unmarshaller um = context.createUnmarshaller();
			Component c = (Component)um.unmarshal(fis);
			
			System.out.println("Component: " + c.getName());
			
			for (DomainModel dm : c.getDomainModel())
			{
				System.out.println("\nDomain Model (" + dm.getClass().getName() + ")");
				if (dm.getClass() == CADModel.class)
				{
					CADModel cm = (CADModel)dm;
					System.out.println("Notes: " + cm.getNotes());
				} 
				else if (dm.getClass() == ModelicaModel.class)
				{
					ModelicaModel mm = (ModelicaModel)dm;
					System.out.println("Class: " + mm.getClazz());					
				}
			}
			
			/* Serialize it right back out */
			try {
				FileOutputStream fos;
				fos = new FileOutputStream("java_test_out.acm");
				Marshaller m = context.createMarshaller();
			    m.setProperty( Marshaller.JAXB_FORMATTED_OUTPUT, Boolean.TRUE );
				m.marshal(c,fos);
			} catch (FileNotFoundException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (JAXBException jes) {
			jes.printStackTrace();
		}
	}
}
