﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace AVM.META
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class DataType
	{
		public virtual string Name
		{
			get;
			set;
		}

		public virtual String Definition
		{
			get;
			set;
		}

		public virtual DirectionEnum Direction
		{
			get;
			set;
		}

		public virtual Boolean? IsFaultMessage
		{
			get;
			set;
		}

		public virtual List<DataTypeField> Fields
		{
			get;
			set;
		}

	}
}

