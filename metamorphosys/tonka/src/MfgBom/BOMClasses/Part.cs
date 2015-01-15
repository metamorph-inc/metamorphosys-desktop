﻿/*
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

namespace MfgBom.Bom
{
    public partial class Part
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public Part()
        {
            instances_in_design = new List<ComponentInstance>();
        }

        /// <summary>
        /// The Octopart MPN of this part.
        /// </summary>
        public String octopart_mpn;

        /// <summary>
        /// The number of instances of this part within the design.
        /// </summary>
        public int quantity
        {
            get
            {
                return instances_in_design.Count;
            }
        }

        /// <summary>
        /// A comma-separated list of reference designators, as a string.
        /// </summary>
        /// <remarks>
        /// Typically, a reference designator consists of a prefix of one or two letters 
        /// followed by a natural number, e.g. R13 or C1002.
        /// No two components on the PCB may have the same reference designator.
        /// 
        /// Reference designators don't depend on Octopart info.  The prefix typically
        /// comes from an Eagle CAD component attribute, and the suffix is assigned during
        /// schematic or PCB design.
        /// 
        /// The list of reference designators should be sorted in a natural numeric order, as shown in
        /// the example, so that "C9" would come before "C10".
        /// </remarks>
        /// <example>"C1, C2, C7, C9, C10, C14, C15"</example>
        private String referenceDesignators;

        public String ReferenceDesignators
        {
            get { return referenceDesignators; }
            private set { referenceDesignators = value; }
        }

        /// <summary>
        /// The name of the maker of this part.
        /// Found in Octopart's PartsMatchResult items manufacturer.name.
        /// <example>"Texas Instruments"</example>
        /// </summary>
        private String manufacturer;

        public String Manufacturer
        {
            get { return manufacturer; }
            private set { manufacturer = value; }
        }

        /// <summary>
        /// The manufacturer's part number for this part.
        /// Found in Octopart's PartsMatchResult items manufacturer.name.
        /// <example>"SN74S74N"</example>
        /// </summary>
        private String manufacturerPartNumber;

        public String ManufacturerPartNumber
        {
            get { return manufacturerPartNumber; }
            private set { manufacturerPartNumber = value; }
        }

        /// <summary>
        /// Text describing the part.
        /// </summary>
        /// <remarks>
        /// Found in Octopart's PartsMatchResult items descriptions (Description value), from
        /// a preferred source,  based on (Description attribution sources name).
        /// </remarks>
        /// <example>"RES 1.0K OHM 1/10W 5% 0603 SMD"</example>
        /// <seealso>http://octopart.com/api/docs/v3/overview#descriptions</seealso> 
        private String description;

        public String Description
        {
            get { return description; }
            private set { description = value; }
        }

        /// <summary>
        /// The package designation.
        /// </summary>
        /// <remarks>
        /// Found in Octopart's PartsMatchResult specs case_package value.
        /// </remarks>
        /// <example>"0603", "8-PDIP", or "32-TQFP"</example>
        /// <seealso>http://octopart.com/api/docs/v3/overview#technical-specs</seealso>
        private String package;

        public String Package
        {
            get { return package; }
            private set { package = value; }
        }

        /// <summary>
        /// A distributor or manufacturer who sells the part. 
        /// </summary>
        /// <remarks>
        /// Found in Octopart's PartsMatchResult items offers PartOffer seller name.
        /// The supplier may be selected based on affinity, or other factors such as
        /// pricing and availability.
        /// </remarks>
        /// <example>"Digi-Key", "Mouser", or "Newark".</example>
        /// <seealso>http://octopart.com/api/docs/v3/overview#pricing-and-availability</seealso> 
        public String SelectedSupplierName
        {
            get;
            set;
        }

        /// <summary>
        /// How the supplier identifies this part.
        /// </summary>
        /// <remarks>
        /// Found in Octopart's PartsMatchResult items offers PartOffer sku.
        /// </remarks>
        /// <example>"296-1734-5-ND"</example>
        /// <seealso>http://octopart.com/api/docs/v3/overview#pricing-and-availability</seealso> 
        public String SelectedSupplierSku
        {
            get;
            set;
        }

        /// <summary>
        /// The cost-per-unit for the selected supplier, SKU, and quantity.
        /// </summary>
        /// <example>0.01</example>
        /// <seealso>http://octopart.com/api/docs/v3/overview#pricing-and-availability</seealso> 
        public float? SelectedSupplierPartCostPerUnit
        {
            get;
            set;
        }

        ///////////////////////////////////////////////////////////////////
        //
        //      Structures and classes for Octopart offer information
        //
        ///////////////////////////////////////////////////////////////////

        // Structure holding an element of a price-break list:
        public struct PricePoint
        {
            public int quantity;
            public float price;

            public PricePoint(int qty, float price)
            {
                this.quantity = qty;
                this.price = price;
            }
        }

        // Structure holding a map of the 3-letter currency codes to price-break lists.
        public class CurrencyMapStruct
        {
            // public string currencyCode; // Key
            // public List<PricePoint> priceBreaks; // Value
            public Dictionary<string, List<PricePoint>> currencyMap;

            public CurrencyMapStruct()
            {
                this.currencyMap = new Dictionary<string, List<PricePoint>>();
            }
        }

        // Structure holding a map of seller SKUs to CurrencyMapStructs.
        public class SkuMapStruct
        {
            // public string sku; // Key
            // public CurrencyMapStruct currencyMap; // Value
            public Dictionary<string, CurrencyMapStruct> skuMap;

            public SkuMapStruct()
            {
                this.skuMap = new Dictionary<string, CurrencyMapStruct>();
            }
        }

        // Structure holding a map of sellers to SkuMapStructs.
        public class SellerMapStruct
        {
            // public string seller; // Key
            // public SkuMapStruct skuMap; // Value
            public Dictionary<string, SkuMapStruct> sellerMap;

            public SellerMapStruct()
            {
                this.sellerMap = new Dictionary<string, SkuMapStruct>();
            }
        }

        // Structure holding the parsed, but unselected, seller information:
        public SellerMapStruct SellerMapStructure;

        ////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The price breaks for supplier1, as a list of key-value pairs,
        /// where the "keys" are quantities, and the "values" are prices.
        /// </summary>
        /// <remarks>
        /// The list is stored in sorted order from low to high quantities.
        /// </remarks>
        public List<KeyValuePair<int, float>> SelectedSupplierPriceBreaks
        {
            get;
            private set;
        }

        /// <summary>
        /// Miscellaneous text info about the part.
        /// </summary>
        /// <example>"Lead Free / RoHS Compliant"</example>
        /// <remarks>
        /// Found in Octopart's PartsMatchResult specs lead_free_status/rohs_status value.
        /// </remarks>
        /// <seealso>http://octopart.com/api/docs/v3/overview#technical-specs</seealso>
        private String notes;

        public String Notes
        {
            get { return notes; }
            private set { notes = value; }
        }

        /// <summary>
        /// All instances of this part within the design.
        /// </summary>
        public List<ComponentInstance> instances_in_design { get; private set; }

        /// <summary>
        /// Add a ComponentInstance to the list of this part's instances.
        /// </summary>
        /// <param name="instance"></param>
        public void AddInstance(ComponentInstance instance)
        {
            instances_in_design.Add(instance);
        }
    }
}
