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

namespace CyPhy2Schematic.Schematic
{
    public abstract class ModelBase<T> : IComparable<ModelBase<T>> where T : ISIS.GME.Common.Interfaces.FCO
    {

        private string _name = null;

        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_name))
                {
                    this._name = this.Impl.Name.Replace(' ', '_');
                }

                return this._name;
            }
            set 
            {
                this._name = value;
            }
        }
        public T Impl { get; set; }
        public float CanvasX { get; set; }
        public float CanvasY { get; set; }
        public float CenterX { get; set; }
        public float CenterY { get; set; }
        public float CanvasWidth { get; set; }
        public float CanvasHeight { get; set; }

        public ModelBase(T impl)
        {
            this.Impl = impl;
            this.Name = impl.Name;
            CanvasX = 0;
            CanvasY = 0;
            CanvasWidth = 0;
            CanvasHeight = 0;
        }

        public int CompareTo(ModelBase<T> other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}
