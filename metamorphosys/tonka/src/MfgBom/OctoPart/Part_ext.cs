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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;

namespace MfgBom.Bom
{
    public partial class Part
    {
        public bool QueryOctopartData(String apikey = "22becbab")
        {
            if (String.IsNullOrWhiteSpace(octopart_mpn))
            {
                return false;
            }

            var querier = new OctoPart.Querier(apikey);
            String octopart_result = null;
            Random rnd = new Random();

            var retries = 5;
            while (octopart_result == null)
            {
                try
                {
                    octopart_result = querier.QueryMpn(octopart_mpn);
                }
                catch (OctoPart.OctopartQueryRateException ex)
                {
                    if (retries-- > 0)
                    {
                        System.Threading.Thread.Sleep(rnd.Next(5000, 10000));
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            
            this.manufacturer = GetManufacturer(octopart_result);
            this.manufacturerPartNumber = GetManufacturerPartNumber(octopart_result);
            this.description = GetDescription(octopart_result);
            this.package = GetPackage(octopart_result);
            this.notes = GetNotes(octopart_result);
            this.SellerMapStructure = GetSellerMapStructure(octopart_result);

            return true;
        }

        /// <summary>
        /// Gets the manufacturer's name from the JSON result string.
        /// </summary>
        /// <param name="OctopartResult"></param>
        /// <returns></returns>
        public static String GetManufacturer(String OctopartResult)
        {
            string rVal = "";
            dynamic dynJson = JsonConvert.DeserializeObject(OctopartResult);
            // (string)myObj["manufacturer"]["name"];
            if ((dynJson != null) && (dynJson.manufacturer != null) && (dynJson.manufacturer.name != null))
            {
                rVal = dynJson.manufacturer.name;
            }
            else
            {
                rVal = "Unknown";
            }
            return rVal;
        }

        /// <summary>
        /// Gets the manufacturer's part number from the JSON result string.
        /// </summary>
        /// <param name="OctopartResult"></param>
        /// <returns></returns>
        public static String GetManufacturerPartNumber(String OctopartResult)
        {
            string rVal = "";
            dynamic dynJson = JsonConvert.DeserializeObject(OctopartResult);
            // (string)myObj["manufacturer"]["name"];
            if ((dynJson != null) && (dynJson.mpn != null) )
            {
                rVal = dynJson.mpn;
            }
            else
            {
                rVal = "Unknown";
            }
            return rVal;
        }

        /// <summary>
        /// Returns the best part description from the JSON results string.
        /// </summary>
        /// <remarks>
        /// In general, the JSON string contains multiple descriptions of a part, with varying
        /// levels of detail.  Each description is associated with a source of the info.
        /// We try to select the best one, based on a list of favored info sources.
        /// 
        /// If no favored info source is found, we use a "Goldilocks" approach, trying
        /// to find a description that is about the right length for the BOM spreadsheet column.
        /// </remarks>
        /// <param name="OctopartResult">The JSON results string to parse.</param>
        /// <param name="SourceAffinity">A list of preferred data sources, with the most favored first.</param>
        /// <returns>The part's description</returns>
        /// <seealso>
        /// http://stackoverflow.com/questions/11132288/iterating-over-json-object-in-c-sharp
        /// </seealso>
        public static String GetDescription(String OctopartResult, List<String> SourceAffinity = null)
        {
            Dictionary<string, string> mapSourceToDescription = new Dictionary<string, string>();
            dynamic dynJson = JsonConvert.DeserializeObject(OctopartResult);
            string rVal = "";
            bool done = false;

            if ((dynJson != null) && (dynJson.descriptions != null))
            {
                // Create a mapping of sources to descriptions.
                foreach (var item in dynJson.descriptions)
                {
                    string desc = item.value;
                    string supplier = item.attribution.sources[0].name;
                    mapSourceToDescription.Add(supplier, desc);
                }
            }

            // Check if any of our favorite sources have provided a description
            if (null != SourceAffinity)
            {
                foreach (string favSource in SourceAffinity)
                {
                    if (mapSourceToDescription.ContainsKey(favSource))
                    {
                        // We have a description from a favorite source...
                        rVal = mapSourceToDescription[favSource];
                        done = true;    // so, we are done.
                        break;
                    }
                }
            }

            if (!done)
            {
                // No favorite source was found, so find the description that's closest to a target length.
                const int targetLength = 25;
                int minError = Int32.MaxValue;
                foreach (KeyValuePair<string, string> entry in mapSourceToDescription)
                {
                    int score = Math.Abs(entry.Value.Length - targetLength);
                    if (score < minError)
                    {
                        minError = score;
                        rVal = entry.Value;
                    }
                }
            }

            return rVal;
        }

        /// <summary>
        /// Gets the package name from the JSON result string.
        /// </summary>
        /// <param name="OctopartResult">The JSON result string to parse</param>
        /// <remarks>
        /// Octopart has some package names, like "DIP", that need the number of pins
        /// added to give a good idea of the package size.
        /// </remarks>
        /// <returns></returns>
        public static String GetPackage(String OctopartResult)
        {
            dynamic dynJson = JsonConvert.DeserializeObject(OctopartResult);
            string rVal = "";

            if ((dynJson != null) && 
                (dynJson.specs != null) &&
                (dynJson.specs.case_package != null) &&
                 (dynJson.specs.case_package.value != null) )
            {
                rVal = dynJson.specs.case_package.value[0];
            }

            // Check if a number is already part of the package string.
            if ((rVal.Length > 0) && (!rVal.Any(c => char.IsDigit(c))))
            {
                // No digits found in the package name.  Add the number of pins.
                if ((dynJson.specs.pin_count != null) &&
                    (dynJson.specs.pin_count.value != null))
                {
                    rVal += "-" + dynJson.specs.pin_count.value[0];
                }
            }
            return rVal;
        }        

        /// <summary>
        /// Generates miscellaneous notes from the JSON results string.
        /// </summary>
        /// <param name="OctopartResult">The input JSON results string</param>
        /// <returns>miscellaneous notes</returns>
        /// <example>
        /// "Lead Free, RoHS Compliant, Lifecycle Status Active"
        /// </example>
        public static String GetNotes(String OctopartResult)
        {
            dynamic dynJson = JsonConvert.DeserializeObject(OctopartResult);
            string rVal;
            string leadFreeStatus = "";
            string lifecycleStatus = "";
            string rohsStatus = "";

            if ((dynJson != null) &&
                (dynJson.specs != null))
            {
                if ((dynJson.specs.lead_free_status != null) && (dynJson.specs.lead_free_status.value != null))
                {
                    leadFreeStatus = dynJson.specs.lead_free_status.value[0];
                }
                if ((dynJson.specs.rohs_status != null) && (dynJson.specs.rohs_status.value != null))
                {
                    rohsStatus = dynJson.specs.rohs_status.value[0];
                }
                if ((dynJson.specs.lifecycle_status != null) && (dynJson.specs.lifecycle_status.value != null))
                {
                    lifecycleStatus = dynJson.specs.lifecycle_status.value[0];
                }
            }

            // Start with the lead-free status.
            rVal = leadFreeStatus;

            // Add RoHS if we know it.
            if (rohsStatus.Length > 0)
            {
                rVal += ", RoHS " + rohsStatus;
            }

            // Add Lifecycle Status if we know it.
            if (lifecycleStatus.Length > 0)
            {
                rVal += ", Lifecycle Status " + lifecycleStatus;
            }

            return rVal;
        }

        ///// <summary>
        ///// Gets the best supplier's name, part number, and price breaks, 
        ///// from the JSON result string, based on supplier affinity.
        ///// </summary>
        ///// <remarks>
        ///// For our current purposes of costing the board in US dollars, only suppliers
        ///// who have price lists in US dollars are considered qualified.
        ///// 
        ///// To find the best supplier for this part, first we create two dictionaries:
        ///// one that maps all qualified suppliers to their quantity in stock, and one
        ///// that maps all qualified suppliers to their US-dollar price list.
        ///// 
        ///// Then, the best supplier is the first supplier we find on the affinity list
        ///// who is also in our dictionary of qualified suppliers.
        ///// 
        ///// If none of the affinity suppliers make the cut, then the best is considered
        ///// to be the qualified supplier with the maximum number of these parts in stock.
        ///// </remarks>
        ///// <param name="supplierName">The best supplier's name</param>
        ///// <param name="supplierPartNumber">The best supplier's part number</param>
        ///// <param name="supplierPriceBreaks">The best supplier's price breaks, in US dollars</param>
        ///// <param name="OctopartResult">The input JSON results string</param>
        ///// <param name="SourceAffinity">A list of preferred suppliers, with the most favored first.</param>
        //public static void GetSupplierNameAndPartNumberAndPriceBreaks(
        //        out string supplierName, 
        //        out string supplierPartNumber,
        //        out List<KeyValuePair<int, float>> supplierPriceBreaks,
        //        String OctopartResult, 
        //        List<String> SourceAffinity = null)
        //{
        //    supplierName = "";
        //    supplierPartNumber = "";
        //    supplierPriceBreaks = new List<KeyValuePair<int, float>>();

        //    dynamic dynJson = JsonConvert.DeserializeObject(OctopartResult);
        //    bool done = false;

        //    Dictionary<string, string> mapSupplierToPartNumber = new Dictionary<string, string>();
        //    Dictionary<string, int> mapSupplierToQtyInStock = new Dictionary<string, int>();

        //    // Create a mapping of suppliers to part numbers and qualified in-stock quantities.
        //    // For now, only suppliers that have a price list in US dollars are considered qualified.
        //    if ((dynJson != null) &&
        //        (dynJson.offers != null))
        //    {
        //        foreach (var item in dynJson.offers)
        //        {
        //            string partNumber = item.sku;
        //            string supplier = item.seller.name;
        //            int qtyInStock = item.in_stock_quantity;

        //            if ((item.prices != null) && (item.prices.USD != null))
        //            {
        //                // Only choose suppliers who have price breaks in US dollars.
        //                mapSupplierToPartNumber.Add(supplier, partNumber);
        //                mapSupplierToQtyInStock.Add(supplier, qtyInStock);
        //            }
        //        }
        //    }

        //    // Check if any of our favorite sources are listed
        //    if (null != SourceAffinity)
        //    {
        //        foreach (string favSource in SourceAffinity)
        //        {
        //            if (mapSupplierToPartNumber.ContainsKey(favSource))
        //            {
        //                // We found a favorite source...
        //                supplierName = favSource;
        //                supplierPartNumber = mapSupplierToPartNumber[favSource];
        //                done = true;    // so, we are done.
        //                break;
        //            }
        //        }
        //    }

        //    if (!done)
        //    {
        //        // No favorite source was found, so find a supplier with maximum stock available.
        //        int maxAvailable = -1;
        //        foreach (KeyValuePair<string, int> entry in mapSupplierToQtyInStock)
        //        {
        //            int qtyInStock = entry.Value;
        //            if (qtyInStock > maxAvailable)
        //            {
        //                maxAvailable = qtyInStock;
        //                supplierName = entry.Key;
        //                supplierPartNumber = mapSupplierToPartNumber[supplierName];
        //            }
        //        }
        //    }

        //    // Find the price breaks for this supplier.
        //    if( supplierName.Length > 0 )
        //    {
        //        // Find our chosen seller's offer.
        //        foreach (var item in dynJson.offers)
        //        {
        //            if( item.seller.name == supplierName )
        //            {
        //                // We found the seller's offer
        //                if ((item.prices != null) && (item.prices.USD != null))
        //                {
        //                    // only take price lists in US dollars for now.
        //                    foreach (var pair in item.prices.USD)
        //                    {
        //                        int qty = pair[0];
        //                        float price = float.Parse((string)pair[1]);

        //                        KeyValuePair<int, float> pricePoint = new KeyValuePair<int, float>(qty, price);
        //                        supplierPriceBreaks.Add(pricePoint);
        //                    }
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Fill in the sellermapStructure from the Octopart results string.
        /// </summary>
        /// <param name="OctopartResult">The Octopart results string to parse.</param>
        /// <returns>the filled-in sellermapStructure</returns>
        public static SellerMapStruct GetSellerMapStructure( String OctopartResult)
        {
            var sellermapStructure = new SellerMapStruct();
            sellermapStructure.sellerMap = new Dictionary<string,SkuMapStruct>();
            dynamic dynJson = JsonConvert.DeserializeObject(OctopartResult);

            if ((dynJson != null) && (dynJson.offers != null))
            {
                foreach (var offer in dynJson.offers)
                {
                    // get the sku
                    string sku = "";
                    if (offer.sku != null)
                    {
                        sku = offer.sku;
                    }

                    // get the seller name
                    string sellerName = "";
                    if ((offer.seller != null) && (offer.seller.name != null))
                    {
                        sellerName = offer.seller.name;
                    }

                    // Check if the seller isn't already in the seller map
                    if (!sellermapStructure.sellerMap.ContainsKey(sellerName))
                    {
                        // We need to add it with an empty SKU map structure.
                        SkuMapStruct emptySkuMapStruct = new SkuMapStruct();
                        sellermapStructure.sellerMap.Add(sellerName, emptySkuMapStruct);
                    }

                    // Check if the SKU is in the SKU map structure.
                    if (!sellermapStructure.sellerMap[sellerName].skuMap.ContainsKey(sku))
                    {
                        CurrencyMapStruct emptyCurrencyMapStruct = new CurrencyMapStruct();
                        sellermapStructure.sellerMap[sellerName].skuMap.Add(sku, emptyCurrencyMapStruct);
                    }

                    // Cerate an alias to avoid a few levels of indirection
                    CurrencyMapStruct thisCurrencyMapStruct = sellermapStructure.sellerMap[sellerName].skuMap[sku];

                    // get the prices
                    if (offer.prices != null)
                    {
                        foreach (var currency in offer.prices)
                        {
                            string currencyCode = "";
                            // Get the currency code
                            currencyCode = currency.Name;

                            List<PricePoint> priceBreaks = new List<PricePoint>();
                            foreach (var pricePair in currency.Value)
                            {
                                int qty = pricePair[0];
                                float price = float.Parse((string)pricePair[1]);
                                PricePoint pricePoint = new PricePoint(qty,price);
                                priceBreaks.Add(pricePoint);
                            }

                            // Add the price breaks to the currency map
                            thisCurrencyMapStruct.currencyMap[currencyCode] = priceBreaks;
                        }
                    }
                }
            }

            return sellermapStructure;
        }
    }
}
