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

#ifndef _UDM_COMPARATOR_
#define _UDM_COMPARATOR_

#include <iostream>
#include <set>
#include <map>
#include <boost/lexical_cast.hpp>
#include <unordered_set>
#include <unordered_map>

#include "UdmBase.h"
#include "Uml.h"

class UdmComparator {
public:
	typedef std::map< std::string, Uml::Attribute > NameUmlAttributeMap;
	typedef std::set< Uml::Attribute > UmlAttributeSet;
	typedef std::set< Uml::AssociationRole > UmlAssociationRoleSet;

private:
	static NameUmlAttributeMap getNameUmlAttributeMap( Uml::Class umlClass );
	static UmlAttributeSet getAllUmlAttributes( Uml::Class umlClass );
	static UmlAssociationRoleSet getAllUmlAssociationRoles( Uml::Class umlClass );

	class ObjectName {
	public:
		struct UdmObjectDnHash {
			size_t operator()(const Udm::Object& o) const
			{
				return o.uniqueId() + o.__impl()->__getdn()->uniqueId();
			}
		};
		struct UdmObjectDnEqual {
			size_t operator()(const Udm::Object& o1, const Udm::Object& o2) const
			{
				return o1.uniqueId() == o2.uniqueId() && o1.__impl()->__getdn()->uniqueId() == o2.__impl()->__getdn()->uniqueId();
			}
		};

		unordered_map<Udm::Object, std::string, UdmObjectDnHash, UdmObjectDnEqual> ObjectNameCache;

		typedef map< Uml::Class, Uml::Attribute > UmlClassNameAttributeMap;

	private:
		// FIXME: just this map should be static, and ObjectName should be non-static
		UmlClassNameAttributeMap _umlClassNameAttributeMap;
	
	public:
		std::string operator()( Udm::Object udmObject );
	};

public:
	class Report {
	public:
		typedef std::map< Udm::Object, int > UdmObjectDifferenceMap;

	private:
		std::string _reportString;
		UdmObjectDifferenceMap _objectDifferences;

	public:
		std::string &getString( void ) {
			return _reportString;
		}

		void setString( const std::string &reportString ) {
			_reportString = reportString;
		}

		void addToReport( Udm::Object udmObject ) {
			int differences = 0;
			UdmObjectDifferenceMap::iterator udmItr = _objectDifferences.find( udmObject );
			if ( udmItr != _objectDifferences.end() ) differences = udmItr->second;
			if ( differences <= 0 ) return;

			std::string addString;

			for( int ix = 0 ; ix < udmObject.depth_level() ; ++ix ) addString += "    ";

			addString += udmObject.getPath( "/" ) + ": " + boost::lexical_cast< std::string >( differences ) + "\n";
			_reportString = addString + _reportString;
		}

		void incrementDifferences( Udm::Object udmObject ) {
			UdmObjectDifferenceMap::iterator udmItr = _objectDifferences.find( udmObject );
			if ( udmItr == _objectDifferences.end() ) udmItr = _objectDifferences.insert(  std::make_pair( udmObject, 0 )  ).first;
			++(udmItr->second);
		}

		int getDifferences( Udm::Object udmObject ) {
			UdmObjectDifferenceMap::iterator udmItr = _objectDifferences.find( udmObject );
			if ( udmItr == _objectDifferences.end() ) udmItr = _objectDifferences.insert(  std::make_pair( udmObject, 0 )  ).first;
			return udmItr->second;
		}

		void addToDifferences( Udm::Object udmChildObject ) {
			UdmObjectDifferenceMap::iterator childUDMItr = _objectDifferences.find( udmChildObject );
			if ( childUDMItr == _objectDifferences.end() ) return;

			Udm::Object udmParentObject = udmChildObject.GetParent();
			UdmObjectDifferenceMap::iterator udmItr = _objectDifferences.find( udmParentObject );
			if ( udmItr == _objectDifferences.end() ) udmItr = _objectDifferences.insert(  std::make_pair( udmParentObject, 0 )  ).first;
			udmItr->second += childUDMItr->second;
		}

		static Report &get_singleton( void ) {
			static Report report;
			return report;
		}
	};

	static ObjectName &getObjectNameSingleton( void ) {
		static ObjectName objectName;
		return objectName;
	}

	static size_t string_hash(const char *str)
	{
		size_t hash = 5381;
		int c;

		while (c = *str++)
			hash = hash * 33 + c;

		return hash;
	}

	struct ChildObjectComparator {
		size_t hash;
		Udm::Object object;

		bool operator()( const Udm::Object &udmObject1, const Udm::Object &udmObject2 );
		bool operator ==(const ChildObjectComparator &object2) const;
		bool operator !=(const ChildObjectComparator &object2) const
		{
			return *this != object2;
		}

		ChildObjectComparator(Udm::Object o)
			: object(o)
		{
			if (o == Udm::null)
				hash = 0;
			else
			{
				Uml::CompositionChildRole umlCompositionChildRole1;
				object.GetParent().GetChildRole(object, umlCompositionChildRole1);
				hash = object.type().uniqueId() + (umlCompositionChildRole1 * 53) + string_hash(UdmComparator::getObjectNameSingleton()(o).c_str());
			}
		}
	};

	struct ChildObjectHash {
		size_t operator() (const ChildObjectComparator& object) const
		{
			return object.hash;
		}
	};

	struct AssociationObject {
		size_t hash;
		Udm::Object object;

		bool operator ==(const AssociationObject &udmObject2) const;
		bool operator !=(const AssociationObject &udmObject2) const
		{
			return *this != udmObject2;
		}

		
		AssociationObject(Udm::Object o)
			: object(o)
		{
			if (o == Udm::null)
				hash = 0;
			else
				hash = object.type().uniqueId() + string_hash(UdmComparator::getObjectNameSingleton()(o).c_str());
		}
	};

	struct AssociationObjectHash {
		size_t operator() (const AssociationObject& object) const
		{
			return object.hash;
		}
	};

	typedef set< Udm::Object > UdmObjectSet;
	typedef unordered_set< ChildObjectComparator, ChildObjectHash > UdmChildObjectSet;
	typedef unordered_set< AssociationObject, AssociationObjectHash > UdmAssociationObjectSet;

	typedef set< Uml::Class > UmlClassSet;

	typedef std::set< std::string > StringSet;
	typedef std::map< std::string, StringSet > StringStringSetMap;

	class ClassNameFilter {
	private:
		StringSet _inclusiveClassNameSet;
		StringSet _exclusiveClassNameSet;

		StringSet _includedClassNameSet;
		StringSet _excludedClassNameSet;

		StringStringSetMap _inclusiveClassNameAttributeNameSetMap;
		StringStringSetMap _exclusiveClassNameAttributeNameSetMap;

	public:
		void setInclusiveClassNameSet( const StringSet &inclusiveClassNameSet ) {
			_inclusiveClassNameSet = inclusiveClassNameSet;
		}
		StringSet getInclusiveClassNameSet( void ) {
			return _inclusiveClassNameSet;
		}

		StringSet getIncludedClassNameSet( void ) {
			return _includedClassNameSet;
		}

		void setExclusiveClassNameSet( const StringSet &exclusiveClassNameSet ) {
			_exclusiveClassNameSet = exclusiveClassNameSet;
		}
		StringSet getExclusiveClassNameSet( void ) {
			return _exclusiveClassNameSet;
		}

		StringSet getExcludedClassNameSet( void ) {
			return _excludedClassNameSet;
		}

		void setInclusiveClassNameAttributeNameMap( const StringStringSetMap &inclusiveClassNameAttributeNameMap ) {
			_inclusiveClassNameAttributeNameSetMap = inclusiveClassNameAttributeNameMap;
		}

		void setExclusiveClassNameAttributeNameMap( const StringStringSetMap &exclusiveClassNameAttributeNameMap ) {
			_exclusiveClassNameAttributeNameSetMap = exclusiveClassNameAttributeNameMap;
		}

		UdmObjectSet filterUdmObjectSet( const UdmObjectSet &udmObjectSet );
		UdmObjectSet filterConnections( const UdmObjectSet &udmObjectSet );

		UmlAttributeSet filterUmlAttributeSet( Uml::Class umlClass );
	};

	struct UdmObjectHash {
		size_t operator()(const Udm::Object& o) const
		{
			return o.uniqueId();
		}
	};

	typedef unordered_map<Udm::Object, Udm::Object, UdmObjectHash> UdmObjectMap;

	typedef set< Uml::CompositionChildRole > CompositionChildRoleSet;


private:
	UdmObjectMap _udmObjectMap;
	ClassNameFilter _classNameFilter;

public:
	void setClassNameFilter( const ClassNameFilter &classNameFilter ) {
		_classNameFilter = classNameFilter;
	}

	bool compareNode( Udm::Object udmObject1, Udm::Object udmObject2 );

private:
	bool compareNodeAux( Udm::Object udmObject1, Udm::Object udmObject2 );

	typedef std::pair< int, int > IntPair;
	typedef std::map< std::string, IntPair > ChildrenClassesTallyMap;

	void tallyChildrenClasses( const UdmObjectSet &udmObjectSet1, const UdmObjectSet &udmObjectSet2 );
};

#endif