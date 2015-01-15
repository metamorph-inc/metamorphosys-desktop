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
using MfgBom;
using Xunit;


namespace CyPhy2MfgBomTest
{
    public class UserBomTableTests
    {
        [Fact]
        public void FakeBomCsvStringCreation()
        {
            UserBomTable myBomTable = new UserBomTable();

            myBomTable.BomTitle = "User BOM Table Tests";
            myBomTable.QueryDateTime = DateTime.Now.ToString("f");
            myBomTable.HowManyBoards = 10000;

            UserBomTableRow row1 = new UserBomTableRow()
            {
                Quantity = 3,
                ReferenceDesignators = "R1, R2, R3",
                Manufacturer = "Yageo",
                ManufacturerPartNumber = "RC0603JR-071KL",
                Description = "RES 1.0K OHM 1/10W 5% 0603 SMD",
                Package = "0603",
                Supplier1 = "Digi-Key",
                Supplier1PartNumber = "311-1.0KGRCT-ND",
                Notes = "Lead free / RoHS Compliant",
                Supplier1UnitPrice = 0.01f
            };
            myBomTable.Rows.Add(row1);

            UserBomTableRow row2 = new UserBomTableRow()
            {
                Quantity = 10,
                ReferenceDesignators = "C1, C2, C3, C4, C5, C6, C7, C8, C9, C10",
                Manufacturer = "Murata Electronics North America",
                ManufacturerPartNumber = "GRM188R71E104KA01D",
                Description = "CAP CER 0.1UF 25V 10% X7R 0603",
                Package = "0603",
                Supplier1 = "Digi-Key",
                Supplier1PartNumber = "490-1524-1-ND",
                Notes = "Lead free / RoHS Compliant",
                Supplier1UnitPrice = 0.151f
            };
            myBomTable.Rows.Add(row2);
            Console.WriteLine("{0}", myBomTable.ToCsv());
        }
        [Fact]
        public void FakeBomHtmlStringCreation()
        {
            UserBomTable myBomTable = new UserBomTable();

            myBomTable.BomTitle = "User BOM Table Tests";
            myBomTable.QueryDateTime = DateTime.Now.ToString("f");
            myBomTable.HowManyBoards = 10000;

            UserBomTableRow row1 = new UserBomTableRow()
            {
                Quantity = 3,
                ReferenceDesignators = "R1, R2, R3",
                Manufacturer = "Yageo",
                ManufacturerPartNumber = "RC0603JR-071KL",
                Description = "RES 1.0K OHM 1/10W 5% 0603 SMD",
                Package = "0603",
                Supplier1 = "Digi-Key",
                Supplier1PartNumber = "311-1.0KGRCT-ND",
                Notes = "Lead free / RoHS Compliant",
                Supplier1UnitPrice = 0.01f
            };
            myBomTable.Rows.Add(row1);

            UserBomTableRow row2 = new UserBomTableRow()
            {
                Quantity = 10,
                ReferenceDesignators = "C1, C2, C3, C4, C5, C6, C7, C8, C9, C10",
                Manufacturer = "Murata Electronics North America",
                ManufacturerPartNumber = "GRM188R71E104KA01D",
                Description = "CAP CER 0.1UF 25V 10% X7R 0603",
                Package = "0603",
                Supplier1 = "Digi-Key",
                Supplier1PartNumber = "490-1524-1-ND",
                Notes = "Lead free / RoHS Compliant",
                Supplier1UnitPrice = 0.151f
            };
            myBomTable.Rows.Add(row2);
            Console.WriteLine("{0}", myBomTable.ToHtml());
        }
    }
}
