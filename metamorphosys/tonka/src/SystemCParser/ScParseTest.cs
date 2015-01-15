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

//--------------------------------------------------------------------
//
//  ScParseTest.cs
//
//  XUNIT tests for the ScParse class.
//
//  See also: MOT-419 "CAT module for SystemC"
//
//  Henry Forson, 12/2/2014
//
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SystemCParser;
using Xunit;
using System.IO;
using System.Reflection;

namespace SystemCParser
{
    public class ScParsingTests
    {
        // Path_TestModels = "C:\Users\Henry\repos\tonka\META\test\ComponentAndArchitectureTeamTest\bin\Release\..\..\..\..\models\SystemCTestModels"
        // = "C:\Users\Henry\repos\tonka\META\models\SystemCTestModels"
        private static String Path_TestModels = Path.Combine(
            Path.GetDirectoryName(Assembly.GetAssembly(typeof(ScParsingTests)).CodeBase.Substring("file:///".Length)),
                                                                "..", "..", "..", "..",
                                                                "models",
                                                                "SystemCTestModels");

        //--------------------------------------------------------------------------------------------
        /// <summary>
        /// Test CCLED SystemC header parsing.
        /// </summary>
        [Fact]
        public void testCCLED()
        {
            const int expectedPinCount = 13;
            int i;
            pinData_s[] expectedPinData = 
            {
                new pinData_s( "le", "sc_in", "sc_logic", 1 ),
                new pinData_s( "oeBar", "sc_in", "sc_logic", 1 ),

                new pinData_s( "out0", "sc_out", "sc_logic", 1 ),
                new pinData_s( "out1", "sc_out", "sc_logic", 1 ),
                new pinData_s( "out2", "sc_out", "sc_logic", 1 ),
                new pinData_s( "out3", "sc_out", "sc_logic", 1 ),
                new pinData_s( "out4", "sc_out", "sc_logic", 1 ),
                new pinData_s( "out5", "sc_out", "sc_logic", 1 ),
                new pinData_s( "out6", "sc_out", "sc_logic", 1 ),
                new pinData_s( "out7", "sc_out", "sc_logic", 1 ),

                new pinData_s( "sdi", "sc_in", "sc_logic", 1 ),
                new pinData_s( "sdi_clk", "sc_in", "sc_logic", 1 ),

                new pinData_s( "sdo", "sc_out", "sc_logic", 1 ),
            };

            // Get path to test file.
            string filePath = Path.Combine(Path_TestModels, "ccled.h");

            //  Verify that it exists.
            Assert.True(File.Exists(filePath));

            // read the file and remove comments
            Uncomment nocomment = new Uncomment(filePath);

            // Verify that there is still some text.
            Assert.NotNull(nocomment.result);
            Assert.True(nocomment.result.Length > 0);

            ScParse parsed = new ScParse(nocomment.result);

            Assert.Equal<string>("local_ccled", parsed.scModuleName);

            Assert.Equal<int>( expectedPinCount, parsed.pinList.Count );

            for (i = 0; i < expectedPinCount; i++)
            {
                Assert.NotNull( parsed.pinList[i] );
                Assert.Equal<string>(expectedPinData[i].name, parsed.pinList[i].name);
                Assert.Equal<string>(expectedPinData[i].direction, parsed.pinList[i].direction);
                Assert.Equal<string>(expectedPinData[i].type, parsed.pinList[i].type);
                Assert.Equal<int>(expectedPinData[i].dimension, parsed.pinList[i].dimension);
            }

        }
                //--------------------------------------------------------------------------------------------
        /// <summary>
        /// Test SCBus SystemC header parsing.
        /// </summary>
        [Fact]
        public void testSCBus()
        {
            int i;
            pinData_s[] expectedPinData = 
            {
                new pinData_s( "clk", "sc_in", "bool", 1 ),
                new pinData_s( "rst", "sc_in", "bool", 1 ),
                new pinData_s( "rx_avail", "sc_out", "bool", 1 ),
                new pinData_s( "rx_data", "sc_out", "sc_uint", 8 ),
                new pinData_s( "rx_data_rd", "sc_in", "bool", 1 ),
                new pinData_s( "tx_data", "sc_in", "sc_uint", 8 ),
                new pinData_s( "tx_data_wr", "sc_in", "bool", 1 ),
                new pinData_s( "tx_empty", "sc_out", "bool", 1 ),
            };

            int expectedPinCount = expectedPinData.Length;

            // Get path to test file.
            string filePath = Path.Combine(Path_TestModels, "SCBus.h");

            //  Verify that it exists.
            Assert.True(File.Exists(filePath));

            // read the file and remove comments
            Uncomment nocomment = new Uncomment(filePath);

            // Verify that there is still some text.
            Assert.NotNull(nocomment.result);
            Assert.True(nocomment.result.Length > 0);

            ScParse parsed = new ScParse(nocomment.result);

            Assert.Equal<string>("SCBus", parsed.scModuleName);

            Assert.Equal<int>( expectedPinCount, parsed.pinList.Count );

            for (i = 0; i < expectedPinCount; i++)
            {
                Assert.NotNull( parsed.pinList[i] );
                Assert.Equal<string>(expectedPinData[i].name, parsed.pinList[i].name);
                Assert.Equal<string>(expectedPinData[i].direction, parsed.pinList[i].direction);
                Assert.Equal<string>(expectedPinData[i].type, parsed.pinList[i].type);
                Assert.Equal<int>(expectedPinData[i].dimension, parsed.pinList[i].dimension);
            }
        }
    }
}

