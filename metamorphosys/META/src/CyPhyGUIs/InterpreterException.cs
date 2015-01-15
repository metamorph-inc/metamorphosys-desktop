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

// -----------------------------------------------------------------------
// <copyright file="InterpreterException.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace META
{
    using System;
    using System.Runtime.Serialization;

    // following http://stackoverflow.com/questions/9361491/create-new-exception-type

    [Serializable]
    public class InterpreterException : Exception
    {

        //readonly ComComponent data;

        public InterpreterException() { }

        //public InterpreterException(ComComponent data)
        //    : base(FormatMessage(data))
        //{
        //    this.data = data;
        //}

        public InterpreterException(string message) : base(message) { }

        //public InterpreterException(ComComponent data, Exception inner)
        //    : base(FormatMessage(data), inner)
        //{
        //    this.data = data;
        //}

        public InterpreterException(String message, Exception inner) : base(message, inner) { }

        //protected InterpreterException(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //    if (info == null)
        //    {
        //        throw new ArgumentNullException("info");
        //    }
        //    this.data = info.GetValue("data", typeof(ComComponent)) as ComComponent;
        //}

        //public override void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    if (info == null)
        //    {
        //        throw new ArgumentNullException("info");
        //    }
        //    info.AddValue("data", this.data);
        //    base.GetObjectData(info, context);
        //}

        //public ComComponent Data { get { return this.data; } }

        //private static string FormatMessage(ComComponent data)
        //{
        //    return string.Format("Interpreter exception with data {0}, valid: {1}.", data.ProgId, data.isValid);
        //}

    }
}
