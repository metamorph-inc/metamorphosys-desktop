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
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpiceLib;
using Xunit;

namespace SpiceLibTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Parse myParser = new Parse();
            string[] filenames = { "test1.cir", "multiparams.cir", "test2.cir", "test3.cir", "q2n222a.cir", "missing.cir", "nocircuit.cir",
                                 "shortSubcircuit.cir", "bogusName.cir", "bogusparams1.cir", "firstLineCommentTest.cir", 
                                 "nameCharsTestFail.cir", "nameCharsTestPass.cir", "braceTest.cir" };
            foreach( string filename in filenames )
            {
                try
                {
                    ComponentInfo result = myParser.ParseFile(filename );
                    Console.WriteLine("Parsing file '{0}' found {1}",
                        filename, result.ToString() );
                }
                catch (Exception e)
                {
                    // Let the user know what went wrong.
                    Console.WriteLine("ParseFile( {0} ) error:", filename);
                    Console.WriteLine(e.Message);
                }
            }
            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

        }
    }
}
