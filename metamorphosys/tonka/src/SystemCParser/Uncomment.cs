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
//  Uncomment.cs
//
//  This file defines a class used to remove comments from a SystemC file,
//  to help the SystemC CAT create ports from a SystemC model.
//
//  See also: MOT-419 "CAT module for SystemC"
//
//  Henry Forson, 11/11/2014
//
//--------------------------------------------------------------------

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemCParser
{
    public class Uncomment
    {
        public string result;

        public Uncomment(string fileName)
        {
            result = "";

            var blockComments = @"/\*(.*?)\*/";
            var lineComments = @"//(.*?)\r?\n";
            var strings = @"""((\\[^\n]|[^""\n])*)""";
            var verbatimStrings = @"@(""[^""]*"")+";

            string rawInput = "";

            // Read the file
            try
            {
                if ((fileName == null) || (!File.Exists(fileName)))
                {
                    throw new Exception(string.Format("Error: File '{0}' does not exist.", fileName));
                }

                rawInput = System.IO.File.ReadAllText(fileName);

            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                string msg = string.Format("Exception while reading file '{0}':\n {1}",
                            fileName, e.Message);
                throw new Exception(msg, e);
            }

            // Now, "input" has the raw file text as a big string.

            // Trim the comments.  See: http://stackoverflow.com/questions/3524317/regex-to-strip-line-comments-from-c-sharp/3524689#3524689
            result = Regex.Replace(rawInput,
                blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
                me =>
                {
                    if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                        return me.Value.StartsWith("//") ? Environment.NewLine : "";
                    // Keep the literal strings
                    return me.Value;
                },
                RegexOptions.Singleline);
        }
    }
}
