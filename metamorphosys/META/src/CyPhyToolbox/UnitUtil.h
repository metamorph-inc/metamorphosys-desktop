// Copyright (C) 2013-2015 MetaMorph Software, Inc

// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this data, including any software or models in source or binary
// form, as well as any drawings, specifications, and documentation
// (collectively "the Data"), to deal in the Data without restriction,
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Data, and to
// permit persons to whom the Data is furnished to do so, subject to the
// following conditions:

// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Data.

// THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

// =======================
// This version of the META tools is a fork of an original version produced
// by Vanderbilt University's Institute for Software Integrated Systems (ISIS).
// Their license statement:

// Copyright (C) 2011-2014 Vanderbilt University

// Developed with the sponsorship of the Defense Advanced Research Projects
// Agency (DARPA) and delivered to the U.S. Government with Unlimited Rights
// as defined in DFARS 252.227-7013.

// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this data, including any software or models in source or binary
// form, as well as any drawings, specifications, and documentation
// (collectively "the Data"), to deal in the Data without restriction,
// including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Data, and to
// permit persons to whom the Data is furnished to do so, subject to the
// following conditions:

// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Data.

// THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

#ifndef UNIT_UTIL_H
#define UNIT_UTIL_H

#include "CyPhyML.h"
#include <map>

using namespace CyPhyML;

struct UnitUtil
{
	// concept of compound si

	struct DimensionRep
	{
		DimensionRep():
			length(0),
			mass(0),
			time(0),
			current(0),
			temperature(0),
			amountSubstance(0),
			luminous(0) {}

		double length;		// length
		double mass;		// mass
		double time;		// time
		double current;		// electric current
		double temperature;		// temperature
		double amountSubstance;		// amount sustance
		double luminous;		// luminous intensity



		DimensionRep& operator=(const DimensionRep &rhs)
		{
			current = rhs.current;
			amountSubstance = rhs.amountSubstance;
			length = rhs.length;
			luminous = rhs.luminous;
			mass = rhs.mass;
			time = rhs.time;
			temperature = rhs.temperature;
			return *this;
		}

		bool operator==(const DimensionRep &rhs) const
		{
			return ( (rhs.amountSubstance == amountSubstance)
				&& (rhs.current == current)
				&& (rhs.length == length)
				&& (rhs.luminous == luminous)
				&& (rhs.mass == mass)
				&& (rhs.temperature == temperature)
				&& (rhs.time == time) );
		}

		bool operator!=(const DimensionRep &rhs) const
		{
			return !(this->operator==(rhs));
		}

		DimensionRep & DimensionRep::operator+=(const DimensionRep &rhs) 
		{
			current += rhs.current;
			luminous += rhs.luminous;
			temperature += rhs.temperature;
			mass += rhs.mass;
			length += rhs.length;
			amountSubstance += rhs.amountSubstance;
			time += rhs.time;
		    return *this;
		}

		void MultiplyByN(double n)
		{
			current *= n;
			luminous *= n;
			temperature *= n;
			mass *= n;
			length *= n;
			amountSubstance *= n;
			time *= n;
		}

	};

	struct ValueUnitRep
	{
		ValueUnitRep() :
			siValue(0),
			actualValue(0){}		
		ValueUnitRep(double v) :
			siValue(v),
			actualValue(0){}
		ValueUnitRep(UnitUtil& unitUtil, double, unit&);

		double siValue;
		double actualValue;
		CyPhyML::unit cyphyRef;
		DimensionRep unitRep;

		std::string strValue;
		enum { DOUBLE, STRING } type;
	};



	// map<string, int> PrefixTable;
	// map<unit, double> SI_ConversionFactorMap;

	map<unit,DimensionRep> DimensionRep_Table;

	// conversion f() to SI base units
	double ConvertToSIEquivalent(unit&, double, DimensionRep&);
	double ConvertToSIEquivalent(conversion_based_unit&, DimensionRep&, double, double exponent = 1);
	double ConvertToSIEquivalent(derived_unit&, double, DimensionRep&);
	double ConvertValue_To_SIEquivalent(double value, double conversionFactor, double conversionOffset, double exponent);			// conversion function

	// conversion f() from SI base units
	double ConvertFromSIEquivalent(unit&, double);
	double ConvertFromSIEquivalent(conversion_based_unit&, double, double exponent = 1);
	double ConvertFromSIEquivalent(derived_unit&, double);
	double ConvertValue_From_SIEquivalent(double value, double conversionFactor, double conversionOffset, double exponent);			// conversion function


	void GetDimensionRep(si_unit&, DimensionRep&);

	// New Version
	struct ConversionInfo
	{
		ConversionInfo(double exp, double off, double factor):
			exponent(exp),
			offset(off),
			conversion_factor(factor){}
		double exponent;
		double offset;
		double conversion_factor;
	};

	class UnitConverter
	{
	public:
		double ToSI(double input)
		{
			double output = input;
			for (unsigned int i = 0; i<conversionVector.size(); i++)
			{
				output = pow((pow(output, 1/conversionVector[i].exponent)*conversionVector[i].conversion_factor - conversionVector[i].offset), conversionVector[i].exponent);
			}
			return output;
		}
		double FromSI(double input)
		{
			double output = input;
			for (unsigned int i = 0; i<conversionVector.size(); i++)
			{
				output = pow(((pow(output, 1/conversionVector[i].exponent) + conversionVector[i].offset)/conversionVector[i].conversion_factor), conversionVector[i].exponent);
			}
			return output;
		}

		vector<ConversionInfo> conversionVector;
		DimensionRep dimensions;

	};	
	
	map<CyPhyML::unit, UnitConverter> ConverterMap;

	double ConvertToSIEquivalent_WLookup(unit&, double, DimensionRep&);
	double ConvertFromSIEquivalent_WLookup(unit&, double);
	void CreateConverter(unit&, UnitConverter&);
	void CreateConverter(conversion_based_unit&, UnitConverter&, double exponent = 1);
	void CreateConverter(derived_unit&, UnitConverter&);


};

#endif