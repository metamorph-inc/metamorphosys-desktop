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
using Xunit;
using System.IO;
using System.Net;


// Xref2Html.cs
// This file includes a method to make an HTML file containing a cross reference between
// ECAD component reference designators, such as "R1" or "C11", and GME component instance paths.

namespace CyPhy2Schematic
{
    public class XrefItem
    {
        public string ReferenceDesignator;
        public string GmePath;
    }

    public class Xref2Html
    {

        /// <summary>
        /// Make an HTML-page string with a cross-reference-table, and optionally save it to a file.
        /// </summary>
        /// <param name="designName">The name of the GME design</param>
        /// <param name="tableData">A list of cross-reference items that will become an HTML table.</param>
        /// <param name="OutputFileName"></param>
        /// <seealso>MOT-307 Build GME Path --> EAGLE Reference Designator map as part of CyPhy2Schematic's EAGLE generation</seealso>/>
        /// <returns></returns>
        static public string makeHtmlFile(
            string designName,
            List<XrefItem> tableData,
            string OutputFileName = ""
        )
        {
            string rVal = "";
            designName = designName != null ? designName : "";
            List<string> pathList = tableData.Select( x => x.GmePath ).ToList();
            string subtitle = findLongestLeftCommonSubstring(pathList);
            string title = designName + " Component Reference Designator Cross Reference";
            List<string> colHeaders = new List<string>() { "Reference Designator", "GME path" };
            List<List<string>> tableList = new List<List<string>>();
            int startIndex = subtitle.Length;

            foreach (var item in tableData)
            {
                List<string> rowList = new List<string>();
                rowList.Add(item.ReferenceDesignator);
                int newPathLength = item.GmePath.Length - startIndex;
                rowList.Add( item.GmePath.Substring( startIndex, newPathLength ) );
                tableList.Add(rowList);
            }
            rVal = makeHtmlString(title, subtitle, colHeaders, tableList);

            // To do: Write the file, if filename isn't null.
            if (OutputFileName.Length > 0)
            {
                try
                {
                    File.WriteAllText(OutputFileName, rVal);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                    throw e;
                }
            }

            return rVal;
        }

        /// <summary>
        /// Find the longest substring, starting at the left, common to a list of strings.
        /// </summary>
        /// <param name="stringList">The input list of strings</param>
        /// <returns>The longest substring, starting at the left, common to the input list of strings</returns>
        static string findLongestLeftCommonSubstring(List<string> stringList)
        {
            string rVal = "";
            if (stringList.Count() > 0)
            {
                string firstString = stringList[0];
                int maxMatch = firstString.Length;

                foreach (string item in stringList)
                {
                    for (int matched = 0; matched < maxMatch; matched++)
                    {
                        if (item[matched] != firstString[matched])
                        {
                            maxMatch = matched;
                        }
                    }
                }
                rVal = firstString.Substring(0, maxMatch);
            }
            return rVal;
        }

        /// <summary>
        /// Make an HTML-page string for a sortable table.
        /// </summary>
        /// <param name="pageTitle">Appears as text on the top line of the HTML page.</param>
        /// <param name="pageSubtitle">Appears as text below the title of the HTML page.</param>
        /// <param name="columnHeaders">List of strings for the column headers, starting on the left.</param>
        /// <param name="tableData">List for each row, containing a list of column data for that row.</param>
        /// <returns>A string containing the generated HTML.</returns>
        static string makeHtmlString(
            string pageTitle = "",  // Appears as text on the top line of the HTML page.
            string pageSubtitle = "", // Appears as text below the title of the HTML page.
            List<string> columnHeaders = null,  // List of strings for the column headers, starting on the left.
            List<List<string>> tableData = null // List for each row, containing a list of column data for that row.         
            )
        {
            string rVal = "";
            const string headString = @"<html><!-- DataTables CSS -->
                <link rel=""stylesheet"" type=""text/css"" 
                href=""http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.4/css/jquery.dataTables.css"">  
                <!-- jQuery --> <script type=""text/javascript"" charset=""utf8""
                src=""http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.2.min.js""></script> 
                <!-- DataTables --> <script type=""text/javascript"" charset=""utf8"" 
                src=""http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.4/jquery.dataTables.min.js""></script> 
                <script type=""text/javascript""> 
                $(document).ready(function() { 
                    $(""table"").dataTable( {
                        ""bAutoWidth"": false,
                        ""iDisplayLength"": -1,
                        ""aLengthMenu"": [[-1, 100, 50, 25, 10, 5], [""All"", 100, 50, 25, 10, 5 ]]
                    }  ); } );
                </script>
                <style>
                p {margin-left:20px;}
                body {font: 90%/1.45em Consolas, ""Lucida Console"", Verdana, Arial, sans-serif;
                    padding: 10px;
                    margin: 10px;
                }
                td.col1 {text-align:center; width: 200px;}
                th.col1 {text-align:center; width: 200px;}
                td.col2 {text-align:left;}
                th.col2 {text-align:left;}
                </style>
                <body>
                <div style=""position:absolute; left:5%; right:5%"";padding-top:10px;>";
            const string headString2 =
                @"<div style=""font-weight: bold; font-size: x-large;"">{0}</div>
                <div style=""font-weight: bold; font-size: medium;"">{1}</div><p>
                <div id=""table""><table>";
            // Start with the HTML header and page title.
            rVal = headString;
            rVal += string.Format(headString2, WebUtility.HtmlEncode(pageTitle), WebUtility.HtmlEncode(pageSubtitle));
            // Add the column headers
            rVal += "\n<thead>\n<tr>\n";
            int colNum = 1;
            foreach (string header in columnHeaders)
            {
                rVal += string.Format("<th class=\"col{1}\">{0}</th>", WebUtility.HtmlEncode(header), colNum++);
            }
            rVal += "\n</tr>\n</thead>\n";
            // Add the table data
            foreach (var row in tableData)
            {
                rVal += "<tr>";
                colNum = 1;
                foreach (var datum in row)
                {
                    rVal += string.Format("<td class=\"col{1}\">{0}</td>\n", WebUtility.HtmlEncode(datum), colNum++);
                }
                rVal += "</tr>";
            }
            // Add the HTML tail boilerplate
            rVal += @"</table></div><div class=""clear""/></div></body></html>";
            return rVal;
        }
    }

    /// <summary>
    /// Unit tests
    /// </summary>
    public class Xref2HtmlTests
    {
        [Fact]
        public void FakeXrefCreation()
        {
            string designName = "Unit Test Fake Design";
            string OutputFileName = "xrefUnitTest.html";
            File.Delete(OutputFileName);
            Assert.False(File.Exists(OutputFileName));

            List<XrefItem> fakeTable = new List<XrefItem>() {
                { new XrefItem() { ReferenceDesignator = "R1", GmePath = @"path\To\R100-3"} },
                { new XrefItem() { ReferenceDesignator = "R2", GmePath = @"path\To\R100-2"} },
                { new XrefItem() { ReferenceDesignator = "R3", GmePath = @"path\To\R100-1"} },
                { new XrefItem() { ReferenceDesignator = "C1", GmePath = @"path\To\C9$1"} },
                { new XrefItem() { ReferenceDesignator = "C2", GmePath = @"path\To\C9-2"} },
                { new XrefItem() { ReferenceDesignator = "C3", GmePath = @"path\To\C9"} },
                { new XrefItem() { ReferenceDesignator = "T1", GmePath = @"apath\To\<b>TVS-1</b>"} },
                { new XrefItem() { ReferenceDesignator = "U1", GmePath = @"upath\To\74SN00-07"} },
                { new XrefItem() { ReferenceDesignator = "U2", GmePath = @"upath\To\74SN00-05"} },
                { new XrefItem() { ReferenceDesignator = "U3", GmePath = @"upath\To\74SN00-03"} },
                { new XrefItem() { ReferenceDesignator = "U4", GmePath = @"upath\To\74SN00-21"} },
                { new XrefItem() { ReferenceDesignator = "U5", GmePath = @"upath\To\74SN00-18"} },
            };

            string result = Xref2Html.makeHtmlFile(
                designName,
                fakeTable,
                OutputFileName);

            Assert.False(String.IsNullOrWhiteSpace(result));
            Assert.True(File.Exists(OutputFileName));
            int numLines = result.Split('\n').Length;
            Assert.InRange(numLines, 40, 60);
            Console.WriteLine("Output contains {0} lines:", numLines);
            Console.WriteLine("{0}", result);
        }
    }
}
