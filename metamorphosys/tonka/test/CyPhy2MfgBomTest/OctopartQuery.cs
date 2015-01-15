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
using System.Threading.Tasks;

namespace CyPhy2MfgBomTest
{
    public class OctopartQuery
    {
        private static String API_KEY = "22becbab";

        [Fact]
        public void ExceedRateLimit()
        {
            var querier = new MfgBom.OctoPart.Querier(API_KEY);

            List<int> number = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                number.Add(i);
            }

            Assert.Throws<MfgBom.OctoPart.OctopartQueryRateException>(delegate
            {
                try
                {
                    Parallel.ForEach(number, i =>
                    {
                        querier.QueryMpn("SN74S74N");
                    });
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            });
        }

        [Fact]
        public void ExceedLimitAndRecover()
        {
            ExceedRateLimit();

            var querier = new MfgBom.OctoPart.Querier(API_KEY);
            var part = new MfgBom.Bom.Part()
            {
                octopart_mpn = "SN74S74N"
            };
            Assert.True(part.QueryOctopartData(API_KEY));
        }

        [Fact]
        public void QueryAndParse_ManyMPNs()
        {
            var listMpn = new List<String>()
            {
                "SN74S74N",
                "ERJ-2GE0R00X",
                "2-406549-1",
                "CRCW06031K00FKEA",
                "SMD100F-2",
                "TDGL002",
                "ATMEGA48-20AU"
            };

            var dictFailures = new Dictionary<String, Exception>();

            foreach (var mpn in listMpn)
            {
                var part = new MfgBom.Bom.Part()
                {
                    octopart_mpn = mpn
                };

                try
                {
                    part.QueryOctopartData();
                }
                catch (Exception ex)
                {
                    dictFailures.Add(mpn, ex);
                }
            }

            if (dictFailures.Any())
            {
                var msg = String.Format("Exception(s) encountered when processing {0} MPN(s):" + Environment.NewLine, 
                                        dictFailures.Count);

                foreach (var kvp in dictFailures)
                {
                    msg += kvp.Key + ": " + kvp.Value + Environment.NewLine + Environment.NewLine;
                }

                Assert.False(dictFailures.Any(), msg);
            }
        }
    }
}
