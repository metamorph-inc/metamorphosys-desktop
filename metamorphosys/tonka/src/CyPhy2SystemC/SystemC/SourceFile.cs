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
using CyPhy = ISIS.GME.Dsml.CyPhyML.Interfaces;
using CyPhyClasses = ISIS.GME.Dsml.CyPhyML.Classes;

namespace CyPhy2SystemC.SystemC
{
    public class SourceFile : IComparable<SourceFile>
    {
        public enum SourceType { Header, Implemnation, Arduino, Unknown};

        public SourceFile(string path)
        {
            Path = path;
        }

        public virtual string Path { get; set; }

        public virtual SourceType Type
        {
            get
            {
                string ext = System.IO.Path.GetExtension(Path);
                if ( ext == ".h" || ext == ".hpp" )
                {
                    return SourceType.Header;
                }
                if (ext == ".cc" || ext == ".cpp")
                {
                    return SourceType.Implemnation;
                }
                if (ext == ".ino")
                {
                    return SourceType.Arduino;
                }
                return SourceType.Unknown;
            }
        }
        
        public virtual int CompareTo(SourceFile other)
        {
            return this.Path.CompareTo(other.Path);
        }
    }
}
