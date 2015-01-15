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
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace InterchangeXmlComparator
{
    class Program
    {
        static int Main(string[] args)
        {
            var options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, options)) Environment.Exit(1);

            var exportedXmlFi = new FileInfo(options.ExportedFile);
            var desiredResultXmlFi = new FileInfo(options.DesiredResultFile);

            if (!exportedXmlFi.Exists)
            {
                Console.Error.WriteLine("File doesn't exist: {0}", exportedXmlFi.FullName);
                Environment.Exit(-1);
            }
            if (!desiredResultXmlFi.Exists)
            {
                Console.Error.WriteLine("File doesn't exist: {0}", desiredResultXmlFi.FullName);
                Environment.Exit(-1);
            }

            var exportedXdoc = XDocument.Load(exportedXmlFi.FullName);
            var desiredXdoc = XDocument.Load(desiredResultXmlFi.FullName);

            if (options.ComparerMode == ComparerModes.Component)
            {
                var compComparator = new ComponentComparator(exportedXdoc, desiredXdoc);
                compComparator.Check();
            }
            else if (options.ComparerMode == ComparerModes.Design)
            {
                var designComparator = new DesignComparator(exportedXdoc, desiredXdoc);
                designComparator.Check();
            }

            #region Feedbacks

            if (!Feedback.FeedBacklist.Any())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The two files are identical: {0} == {1}", Path.GetFileName(options.ExportedFile), Path.GetFileName(options.DesiredResultFile));
                Environment.Exit(0);
            }
            else
            {
                foreach (var feedback in Feedback.FeedBacklist)
                {
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine(feedback.Message);
                    switch (feedback.FeedbackType)
                    {
                        case FeedbackType.Warning:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case FeedbackType.Error:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    Console.Error.WriteLine("  Exported: {0}\n    Path: {1}\n  Desired: {2}\n    Path: {3}", feedback.ExportedNodeName,
                                      feedback.ExportedNodePath, feedback.DesiredNodeName, feedback.DesiredNodePath);
                    Console.Error.WriteLine();
                }
                Environment.Exit(-1);
            }

            #endregion
            Console.ForegroundColor = ConsoleColor.White;
            return 0;
        }

        

       

        

        

        
    }
}
