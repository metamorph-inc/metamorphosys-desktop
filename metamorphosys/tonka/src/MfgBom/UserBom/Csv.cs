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

namespace MfgBom
{
    // See http://stackoverflow.com/questions/769621/dealing-with-commas-in-a-csv-file
    public static class Csv
    {
        public static string Escape(string s)
        {
            // If sting is empty/null, quit now.
            // Otherwise, the string has content, so proceed.
            if (String.IsNullOrWhiteSpace(s))
            {
                return "";
            }
            
            string t = s.Trim();
            if (t.Contains(QUOTE))
            {
                t = t.Replace(QUOTE, ESCAPED_QUOTE);
            }

            // Test if the string is a number (float or int)
            Boolean isNumber = false;
            float tmp;
            isNumber = float.TryParse(t, out tmp);
            int tmp2;
            isNumber = isNumber || int.TryParse(t, out tmp2);
            if (isNumber)
            {
                t = "'" + t;    // Prevent numeric strings from being turned into numbers, so the BOM
                                // won't drop leading 0's, such as in "0603" -> 603.
            }

            if (t.IndexOfAny(CHARACTERS_THAT_MUST_BE_QUOTED) > -1)
            {
                t = QUOTE + t + QUOTE;
            }

            return t;
        }

        public static string Escape(Nullable<float> f)
        {
            if (f.HasValue == false)
            {
                return "";      // MOT-338
            }

            return f.ToString();
        }

        public static string Unescape(string s)
        {
            if (s.StartsWith(QUOTE) && s.EndsWith(QUOTE))
            {
                s = s.Substring(1, s.Length - 2);

                if (s.Contains(ESCAPED_QUOTE))
                    s = s.Replace(ESCAPED_QUOTE, QUOTE);
            }

            return s;
        }


        private const string QUOTE = "\"";
        private const string ESCAPED_QUOTE = "\"\"";
        private static char[] CHARACTERS_THAT_MUST_BE_QUOTED = { ';', '"', ' ', ',', '\n' };
    }
}
