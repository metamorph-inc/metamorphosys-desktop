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

namespace CyPhy2MfgBomTest
{
    public class OctopartParsingFixture : IDisposable
    {
        public String mockOctopartResult_SN74S74N { get; private set; }
        private String pathMockOctopartResult_SN74S74N = Path.Combine(META.VersionInfo.MetaPath,
                                                             "..",
                                                             "tonka",
                                                             "test",
                                                             "CyPhy2MfgBomTest",
                                                             "MockOctopartResults",
                                                             "part_result.json");

        public String mockOctopartResult_ERJ_2GE0R00X { get; private set; }
        private String pathMockOctopartResult_ERJ_2GE0R00X = Path.Combine(META.VersionInfo.MetaPath,
                                                             "..",
                                                             "tonka",
                                                             "test",
                                                             "CyPhy2MfgBomTest",
                                                             "MockOctopartResults",
                                                             "ERJ-2GE0R00X.json");

        public OctopartParsingFixture()
        {
            mockOctopartResult_SN74S74N = File.ReadAllText(pathMockOctopartResult_SN74S74N);
            Assert.False(String.IsNullOrWhiteSpace(mockOctopartResult_SN74S74N));

            mockOctopartResult_ERJ_2GE0R00X = File.ReadAllText(pathMockOctopartResult_ERJ_2GE0R00X, Encoding.Unicode );
            Assert.False(String.IsNullOrWhiteSpace(mockOctopartResult_ERJ_2GE0R00X));
        }

        public void Dispose()
        {
            // Nothing to do
        }
    }

    public class OctopartParsing : IUseFixture<OctopartParsingFixture>
    {
        #region fixture
        OctopartParsingFixture fixture;
        public void SetFixture(OctopartParsingFixture data)
        {
            fixture = data;
        }
        #endregion

        [Fact]
        public void Package()
        {
            var package = MfgBom.Bom.Part.GetPackage(fixture.mockOctopartResult_SN74S74N);
            Assert.Equal("DIP-14", package);
        }

        [Fact]
        public void Notes()
        {
            var notes = MfgBom.Bom.Part.GetNotes(fixture.mockOctopartResult_SN74S74N);
            Assert.Equal("Lead Free, RoHS Compliant, Lifecycle Status Active", notes);
        }

        [Fact]
        public void Description_WithAffinity_FirstSourceExists()
        {
            var affinity = new List<String>()
            {
                "Digi-Key",
                "Component Electronics"
            };
                        
            var description = MfgBom.Bom.Part.GetDescription(fixture.mockOctopartResult_SN74S74N, affinity);

            // Should return Digi-Key's description, not Mouser's or anybody else's.
            Assert.Equal("IC D-TYPE POS TRG DUAL 14DIP", description);
        }

        [Fact]
        public void Description_WithAffinity_FirstSourceDoesNotExist()
        {
            var affinity = new List<String>()
            {
                "SomeGuy, Inc.",
                "Digi-Key"
            };

            var description = MfgBom.Bom.Part.GetDescription(fixture.mockOctopartResult_SN74S74N, affinity);

            // Should return Digi-Key's description, not anybody else's.
            Assert.Equal("IC D-TYPE POS TRG DUAL 14DIP", description);
        }

        [Fact]
        public void Description_WithAffinity_SourcesDoNotExist()
        {
            var affinity = new List<String>()
            {
                "SomeGuy, Inc.",
                "Sketchy Hungarian Corp"
            };

            var description = MfgBom.Bom.Part.GetDescription(fixture.mockOctopartResult_SN74S74N, affinity);

            // Should return Digi-Key's description, since it's closest to the target description length.
            Assert.Equal("IC D-TYPE POS TRG DUAL 14DIP", description);
        }

        [Fact]
        public void Description_NoAffinity()
        {
            var description = MfgBom.Bom.Part.GetDescription(fixture.mockOctopartResult_SN74S74N);

            // Should return Digi-Key's description, since it's closest to the target description length.
            Assert.Equal("IC D-TYPE POS TRG DUAL 14DIP", description);
        }

        [Fact]
        public void Manufacturer()
        {
            var manufacturer = MfgBom.Bom.Part.GetManufacturer(fixture.mockOctopartResult_SN74S74N);
            Assert.Equal("Texas Instruments", manufacturer);
        }

        
        [Fact]
        public void ManufacturerPartNumber()
        {
            var manufacturerPartNumber = MfgBom.Bom.Part.GetManufacturerPartNumber(fixture.mockOctopartResult_SN74S74N);
            Assert.Equal("SN74S74N", manufacturerPartNumber);
        }


        //[Fact]
        //public void Supplier_WithAffinity_FirstSupplierExists()
        //{
        //    var affinity = new List<String>()
        //    {
        //        "Digi-Key",
        //        "Component Electronics"
        //    };

        //    string supplierName = "";
        //    string supplierPartNumber = "";
        //    List<KeyValuePair<int, float>> supplierPriceBreaks;

        //    MfgBom.Bom.Part.GetSupplierNameAndPartNumberAndPriceBreaks(
        //        out supplierName,
        //        out supplierPartNumber,
        //        out supplierPriceBreaks,
        //        fixture.mockOctopartResult_SN74S74N,
        //        affinity);

        //    // Should return Digi-Key's name.
        //    Assert.Equal("Digi-Key", supplierName);

        //    // Should return Digi-Key's part number.
        //    Assert.Equal("296-1734-5-ND", supplierPartNumber);
        //}

        //[Fact]
        //public void Supplier_WithAffinity_FirstSupplierDoesNotExist()
        //{
        //    var affinity = new List<String>()
        //    {
        //        "Some Bogus Distributor Name",
        //        "Digi-Key",
        //        "Component Electronics"
        //    };

        //    string supplierName = "";
        //    string supplierPartNumber = "";
        //    List<KeyValuePair<int, float>> supplierPriceBreaks;

        //    MfgBom.Bom.Part.GetSupplierNameAndPartNumberAndPriceBreaks(
        //        out supplierName,
        //        out supplierPartNumber,
        //        out supplierPriceBreaks,
        //        fixture.mockOctopartResult_SN74S74N,
        //        affinity);

        //    // Should return Digi-Key's name.
        //    Assert.Equal("Digi-Key", supplierName);

        //    // Should return Digi-Key's part number.
        //    Assert.Equal("296-1734-5-ND", supplierPartNumber);

        //    // We should have 7 price breaks.
        //    Assert.Equal(7, supplierPriceBreaks.Count());
        //}

        //[Fact]
        //public void Supplier_WithAffinity_SuppliersDoNotExist()
        //{
        //    var affinity = new List<String>()
        //    {
        //        "Digi-Lock",
        //        "Mousey",
        //        "NewWalk"
        //    };

        //    string supplierName = "";
        //    string supplierPartNumber = "";
        //    List<KeyValuePair<int, float>> supplierPriceBreaks;

        //    MfgBom.Bom.Part.GetSupplierNameAndPartNumberAndPriceBreaks(
        //        out supplierName,
        //        out supplierPartNumber,
        //        out supplierPriceBreaks,
        //        fixture.mockOctopartResult_SN74S74N,
        //        affinity);

        //    // Should return some suppliers name.
        //    Assert.True(supplierName.Length > 0);

        //    // Should return some part number.
        //    Assert.True(supplierPartNumber.Length > 0);

        //    // Quest has the most stock of suppliers with price breaks in US dollars.
        //    Assert.Equal("Quest", supplierName);

        //    // Check Quest's part number.
        //    Assert.Equal("SN74S74N", supplierPartNumber);
        //}

        [Fact]
        public void GetSellerMapStructureTest()
        {
            var sellermapStructure = MfgBom.Bom.Part.GetSellerMapStructure(fixture.mockOctopartResult_SN74S74N);
            var sellermapStructure2 = MfgBom.Bom.Part.GetSellerMapStructure(fixture.mockOctopartResult_ERJ_2GE0R00X);
        }
    }
}
