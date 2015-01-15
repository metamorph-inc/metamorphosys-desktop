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
using Xunit;
using System.IO;
using Newtonsoft.Json;
using MfgBom.CostEstimation;
using System.Reflection;

namespace CyPhy2MfgBomTest
{
    public class EstimateAndSelect
    {
        private static String TEST_DIRECTORY = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Substring("file:///".Length)),
                                "..", "..",
                                "..", "..",
                                "test",
                                "CyPhy2MfgBomTest");

        [Fact]
        public void FPGA_Quantity1()
        {
            int design_quantity = 1;
            float expected_cost = 30.4F;
            SelectFPGASupplier(design_quantity, expected_cost);
        }
                
        [Fact]
        public void FPGA_Quantity25()
        {
            int design_quantity = 25;
            float expected_cost = 26.45F;
            SelectFPGASupplier(design_quantity, expected_cost);
        }

        [Fact]
        public void FPGA_Quantity100()
        {
            int design_quantity = 100;
            float expected_cost = 24.3875F;
            SelectFPGASupplier(design_quantity, expected_cost);
        }

        [Fact]
        public void FPGA_Quantity0()
        {
            int design_quantity = 0;
            float expected_cost = 30.4F;
            SelectFPGASupplier(design_quantity, expected_cost);
        }

        [Fact]
        public void NoSuppliers()
        {
            var part = new MfgBom.Bom.Part();
            part.AddInstance(new MfgBom.Bom.ComponentInstance());
            part.SelectSupplier(1);

            Assert.Null(part.SelectedSupplierName);
            Assert.Null(part.SelectedSupplierPartCostPerUnit);
            Assert.Null(part.SelectedSupplierSku);
        }

        [Fact]
        public void NoUSDOffers()
        {
            var pathMockSellerMapStructure = Path.Combine(TEST_DIRECTORY,
                                                          "MockDataStructures",
                                                          "NoUSDOffers.SellerMapStructure.json");
            var jsonMockSellerMapStructure = File.ReadAllText(pathMockSellerMapStructure);

            var part = new MfgBom.Bom.Part()
            {
                SellerMapStructure = JsonConvert.DeserializeObject<MfgBom.Bom.Part.SellerMapStruct>(jsonMockSellerMapStructure)
            };
            part.AddInstance(new MfgBom.Bom.ComponentInstance());

            part.SelectSupplier(1);

            Assert.Null(part.SelectedSupplierName);
            Assert.Null(part.SelectedSupplierPartCostPerUnit);
            Assert.Null(part.SelectedSupplierSku);
        }
        
        private static void SelectFPGASupplier(int design_quantity, float expected_cost)
        {
            var pathMockSellerMapStructure = Path.Combine(TEST_DIRECTORY,
                                                          "MockDataStructures",
                                                          "LFE3-17EA-6MG328C.SellerMapStructure.json");
            var jsonMockSellerMapStructure = File.ReadAllText(pathMockSellerMapStructure);

            var part = new MfgBom.Bom.Part()
            {
                SellerMapStructure = JsonConvert.DeserializeObject<MfgBom.Bom.Part.SellerMapStruct>(jsonMockSellerMapStructure)
            };
            part.AddInstance(new MfgBom.Bom.ComponentInstance());

            part.SelectSupplier(design_quantity);

            Assert.False(String.IsNullOrWhiteSpace(part.SelectedSupplierName));
            Assert.Equal("Digi-Key", part.SelectedSupplierName);
            Assert.Equal(expected_cost, part.SelectedSupplierPartCostPerUnit);
        }
    }
}
