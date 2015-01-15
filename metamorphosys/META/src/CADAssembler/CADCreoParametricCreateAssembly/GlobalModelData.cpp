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

#include "stdafx.h"
#include "GlobalModelData.h"
#include "ProObjects.h"

namespace isis{

	GlobalModelData& GlobalModelData::Instance = GlobalModelData();

	void GlobalModelData::Clear()
	{
		CadComponentData.clear();
		CadAssemblies.DataExchangeSpecifications.clear();
		CadAssemblies.materials.clear();
		CadAssemblies.topLevelAssemblies.clear();
		CadAssemblies.unassembledComponents.clear();
		ComponentEdit.avmId.clear();
		ComponentEdit.mdl = 0;
		ComponentEdit.resourceId.clear();
	}

	void GlobalModelData::Lock()
	{
		locker.lock();
	}

	void GlobalModelData::UnLock()
	{
		locker.unlock();
	}

	ProSolid GlobalModelData::GetTopModel()
	{
		return CadComponentData[CadAssemblies.topLevelAssemblies.begin()->assemblyComponentID].modelHandle;
	}

	ProSolid GlobalModelData::GetModelFromGuid(const std::string &guid)
	{
		for (std::map<std::string, isis::CADComponentData>::const_iterator it = CadComponentData.begin(); it != CadComponentData.end(); ++it)
		{
			if (it->second.componentID==guid) return it->second.modelHandle;
		}
		return 0;
	}

	/**
	This reverse key for cad component data has one parts.
	  1) the ProSolid (ProMdl) identifier

	  ** This appoach may result in an incorrect guid **
	*/
	std::string GlobalModelData::GetGuidFromModel(ProSolid sld)
	{
		for (std::map<std::string, isis::CADComponentData>::const_iterator it = CadComponentData.begin(); it != CadComponentData.end(); ++it)
		{
			isis::CADComponentData candidate = it->second;
			if (candidate.modelHandle != sld) continue;
			return candidate.componentID;
		}
		return "";
	}

	/**
	The reverse key for cad component data has three parts.
	  1) the ProSolid (ProMdl) identifier
	  2) the ProAsmcomppath : where in the assembly the component is placed
	  3) the assembly tree : ascending the tree should lead to the top
	*/
    inline bool areEquivalentCollectionsOfIntegers(const std::list<int> &lhs, const ProIdTable &rhs) {
		if (lhs.size() != sizeof(rhs)) return false;
		int ix=0;
		std::list<int>::const_iterator lhsIter;
        for (lhsIter = lhs.begin(); lhsIter != lhs.end(); ++lhsIter, ++ix) {
			if (*lhsIter != rhs[ix]) return false;
		}
		return true;
	}
	/**
	Is the lhs a prefix of the rhs?
	*/
	inline bool isPrefixMatch(const std::list<int> &lhs, const ProIdTable &rhs) {
		if (lhs.size() > sizeof(rhs)) return false;
		int ix=0;
		std::list<int>::const_iterator lhsIter;
        for (lhsIter = lhs.begin(); lhsIter != lhs.end(); ++lhsIter, ++ix) {
			if (*lhsIter != rhs[ix]) return false;
		}
		return true;
	}
	inline bool isInTopComponent(const std::string in_Top, std::map<std::string, isis::CADComponentData> &in_CompMap, const isis::CADComponentData &in_Candidate) {
		CADComponentData ancestor = in_Candidate;
		// and empty parent component id indicates the genesis of the tree
		while (! ancestor.parentComponentID.empty()) {
		    ancestor = in_CompMap[ancestor.parentComponentID];
		}
		return (ancestor.componentID == in_Top) ? true : false;
	}
	std::string GlobalModelData::GetGuidFromModel(const ProAsmcomppath &in_CompPath, const ProSolid &sld)
	{
		std::string top = CadAssemblies.topLevelAssemblies.begin()->assemblyComponentID;
		for (std::map<std::string, isis::CADComponentData>::const_iterator it = CadComponentData.begin(); it != CadComponentData.end(); ++it)
		{
			isis::CADComponentData candidate = it->second;
			// if (candidate.modelHandle != sld) continue;
			std::list<int> asmPath = it->second.componentPaths;
			// if ( ! areEquivalentCollectionsOfIntegers(candidate.componentPaths, in_CompPath.comp_id_table) ) continue;
			if ( ! isPrefixMatch( candidate.componentPaths, in_CompPath.comp_id_table ) ) continue;
			if ( ! isInTopComponent(top, CadComponentData, candidate) ) continue;
			return candidate.componentID;
		}
		return "";
	}
	
	std::string GlobalModelData::GetAvmIdFromModel(ProSolid sld)
	{
		for (std::map<std::string, isis::CADComponentData>::const_iterator it = CadComponentData.begin(); it != CadComponentData.end(); ++it)
		{
			if (it->second.modelHandle==sld) return it->second.avmComponentId;
		}
		return "";
	}
}