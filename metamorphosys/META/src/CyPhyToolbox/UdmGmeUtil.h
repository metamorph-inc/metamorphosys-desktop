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

#pragma once

#include "UdmBase.h"

// Caveats:
// will crash if destination or objectsToCopy is not backed by a Gme object
// throws _com_error if:
//  objectsToCopy containts a reference with externally-connected refports
//  objectsToCopy and destination are in different DataNetworks
//  there is no open RW transaction
// does nothing if destination is not a model or folder
void CopyGMEObjects(const Udm::Object& destination, const std::set<Udm::Object>& objectsToCopy);
// returns: the copy of fcoToCopy
Udm::Object CopyGMEObjects(const Udm::Object& destination, const Udm::Object& fcoToCopy, const std::string& rolename="");
//Udm::Object CopyGMEObjects(const Udm::Object& destination, const Udm::Object& fcoToCopy, const Uml::CompositionChildRole& rolename=Udm::NULLCHILDROLE);

std::string GetChildRole(const Udm::Object& o);

void RedirectReference(Udm::Object& reference, Udm::Object& to);

Udm::Object GetConnectionEnd(const Udm::Object& connection, const wchar_t* dstOrSrc);

bool IsDestRefport(const Udm::Object& connection);
Udm::Object GetRefport(const Udm::Object& connection, const wchar_t* dstOrSrc);

Udm::Object createComposition(const Uml::Class type,const Udm::Object& src,const Udm::Object& dst,const Udm::Object& parent,const Udm::Object& srcRefParent,const Udm::Object& dstRefParent, const char* compositionRole=NULL);

Udm::Object changeConnection(const wchar_t* dstOrSrc, const Udm::Object& connection, const Udm::Object& endpoint, Udm::Object refportParent);

void SwitchConnections(Udm::Object fcoFrom, Udm::Object fcoTo, Udm::Object throughRefport, Udm::Object parent);
void SwitchConnections(Udm::Object connectionContainer, const map<Udm::Object, Udm::Object>& old_to_new_map);
void SwitchRefportConnections(Udm::Object referenceFrom, Udm::Object referenceTo, std::map<Udm::Object, Udm::Object>& portMap, UdmGme::GmeDataNetwork& dn);

Udm::Object getReferredOrNull(Udm::Object& refOrAnything);

void DetachFromArchetype(Udm::Object derived);
