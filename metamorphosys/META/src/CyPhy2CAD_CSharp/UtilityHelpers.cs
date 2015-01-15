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

=======================
This version of the META tools is a fork of an original version produced
by Vanderbilt University's Institute for Software Integrated Systems (ISIS).
Their license statement:

Copyright (C) 2011-2014 Vanderbilt University

Developed with the sponsorship of the Defense Advanced Research Projects
Agency (DARPA) and delivered to the U.S. Government with Unlimited Rights
as defined in DFARS 252.227-7013.

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
using System.Text.RegularExpressions;
using System.IO;

namespace CyPhy2CAD_CSharp
{
    public class UtilityHelpers
    {
        public static string CleanString(string original, int size = 31, string additionalspecial = "")
        {
            string specialchars = (@",\.=\(\)!@#\$%&~`'\+ ^\*\[\]{}/\?:;<>\|") + additionalspecial;       // "-" is not in the list
            string cleanstr = Regex.Replace(original, specialchars, "_");
            if (cleanstr.Length > size)
                cleanstr.Remove(31);

            return cleanstr;
        }

        public static string CleanString2(string original, int size = 31, string specialchars = "")
        {
	        StringBuilder sbuilder = new StringBuilder();
	        string Special_Char_String = ",.=()!@#$%&~`'+ ^*[]{}/?:;<>|";		// 11-28-2012: removed "-" from list

	        if (specialchars != "")
		        Special_Char_String += specialchars;
                                    
	        foreach (char c in original)
	        {
		        if (!Special_Char_String.Contains(c))
			        sbuilder.Append(c);
		        else
		        {
			        if (c == '<')
				        sbuilder.Append("<");
			        else if (c == '>')
				        sbuilder.Append(">");
			        else
				        sbuilder.Append("_");
		        }
	        }

            if (sbuilder.Length > size)
                sbuilder.Remove(size-1, sbuilder.Length - size);
            return sbuilder.ToString();
        }

        private static int UdmID = 0;
        public static string MakeUdmID()
        {
            UdmID++;
            return "id" + UdmID.ToString();
        }

        public static void CopyFiles(string srcDir,
                                     string dstDir)
        {
            if (Directory.Exists(srcDir))
            {
                if (!Directory.Exists(dstDir))
                    Directory.CreateDirectory(dstDir);

                DirectoryInfo dir = new DirectoryInfo(srcDir);
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    string temppath = Path.Combine(dstDir, file.Name);
                    file.CopyTo(temppath, true);
                }
            }
        }
    }
}
