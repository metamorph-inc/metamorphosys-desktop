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

namespace MfgBom.Converters
{
    public static class CostEstResult2UserBomTable
    {
        public static UserBomTable Convert(CostEstimation.CostEstimationResult result)
        {
            var rtn = new UserBomTable();

            rtn.BomTitle = result.result_bom.designName;      // Add BOM info for MOT-256.
            rtn.QueryDateTime = DateTime.Now.ToString("f");
            rtn.HowManyBoards = (uint) result.request.design_quantity;

            rtn.Rows = result.result_bom
                             .Parts
                             .AsParallel()                             
                             .Select(p => Convert(p))
                             .ToList();

            return rtn;
        }

        private static UserBomTableRow Convert(MfgBom.Bom.Part part)
        {
            return new UserBomTableRow() {
                Description = part.Description,
                Manufacturer = part.Manufacturer,
                ManufacturerPartNumber = part.ManufacturerPartNumber,
                Notes = part.Notes,
                Package = part.Package,
                Quantity = part.quantity,
                ReferenceDesignators = String.Join(", " + Environment.NewLine, 
                                                   part.instances_in_design
                                                       .Select(ci => ci.path)),
                Supplier1 = part.SelectedSupplierName,
                Supplier1PartNumber = part.SelectedSupplierSku,
                Supplier1UnitPrice = part.SelectedSupplierPartCostPerUnit
            };
        }
    }
}
