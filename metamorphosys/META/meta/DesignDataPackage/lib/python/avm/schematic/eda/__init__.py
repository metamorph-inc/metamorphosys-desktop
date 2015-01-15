# .\_eda.py
# -*- coding: utf-8 -*-
# PyXB bindings for NM:9f5b2e4c02a063822535af58fedb94550ecc79cc
# Generated 2014-11-18 14:25:23.927000 by PyXB version 1.2.3
# Namespace eda [xmlns:eda]

import pyxb
import pyxb.binding
import pyxb.binding.saxer
import io
import pyxb.utils.utility
import pyxb.utils.domutils
import sys

# Unique identifier for bindings created at the same time
_GenerationUID = pyxb.utils.utility.UniqueIdentifier('urn:uuid:0613818f-6f61-11e4-85c1-542696dd94ef')

# Version of PyXB used to generate the bindings
_PyXBVersion = '1.2.3'
# Generated bindings are not compatible across PyXB versions
if pyxb.__version__ != _PyXBVersion:
    raise pyxb.PyXBVersionError(_PyXBVersion)

# Import bindings for namespaces imported into schema
import avm.schematic as _ImportedBinding__schematic
import avm as _ImportedBinding__avm
import pyxb.binding.datatypes

# NOTE: All namespace declarations are reserved within the binding
Namespace = pyxb.namespace.NamespaceForURI(u'eda', create_if_missing=True)
Namespace.configureCategories(['typeBinding', 'elementBinding'])

def CreateFromDocument (xml_text, default_namespace=None, location_base=None):
    """Parse the given XML and use the document element to create a
    Python instance.

    @param xml_text An XML document.  This should be data (Python 2
    str or Python 3 bytes), or a text (Python 2 unicode or Python 3
    str) in the L{pyxb._InputEncoding} encoding.

    @keyword default_namespace The L{pyxb.Namespace} instance to use as the
    default namespace where there is no default namespace in scope.
    If unspecified or C{None}, the namespace of the module containing
    this function will be used.

    @keyword location_base: An object to be recorded as the base of all
    L{pyxb.utils.utility.Location} instances associated with events and
    objects handled by the parser.  You might pass the URI from which
    the document was obtained.
    """

    if pyxb.XMLStyle_saxer != pyxb._XMLStyle:
        dom = pyxb.utils.domutils.StringToDOM(xml_text)
        return CreateFromDOM(dom.documentElement)
    if default_namespace is None:
        default_namespace = Namespace.fallbackNamespace()
    saxer = pyxb.binding.saxer.make_parser(fallback_namespace=default_namespace, location_base=location_base)
    handler = saxer.getContentHandler()
    xmld = xml_text
    if isinstance(xmld, unicode):
        xmld = xmld.encode(pyxb._InputEncoding)
    saxer.parse(io.BytesIO(xmld))
    instance = handler.rootObject()
    return instance

def CreateFromDOM (node, default_namespace=None):
    """Create a Python instance from the given DOM node.
    The node tag must correspond to an element declaration in this module.

    @deprecated: Forcing use of DOM interface is unnecessary; use L{CreateFromDocument}."""
    if default_namespace is None:
        default_namespace = Namespace.fallbackNamespace()
    return pyxb.binding.basis.element.AnyCreateFromDOM(node, default_namespace)


# List simple type: [anonymous]
# superclasses pyxb.binding.datatypes.anySimpleType
class STD_ANON (pyxb.binding.basis.STD_list):

    """Simple type that is a list of pyxb.binding.datatypes.anyURI."""

    _ExpandedName = None
    _XSDLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 63, 10)
    _Documentation = None

    _ItemType = pyxb.binding.datatypes.anyURI
STD_ANON._InitializeFacetMap()

# List simple type: [anonymous]
# superclasses pyxb.binding.datatypes.anySimpleType
class STD_ANON_ (pyxb.binding.basis.STD_list):

    """Simple type that is a list of pyxb.binding.datatypes.anyURI."""

    _ExpandedName = None
    _XSDLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 76, 10)
    _Documentation = None

    _ItemType = pyxb.binding.datatypes.anyURI
STD_ANON_._InitializeFacetMap()

# Atomic simple type: {eda}RotationEnum
class RotationEnum (pyxb.binding.datatypes.string, pyxb.binding.basis.enumeration_mixin):

    """An atomic simple type."""

    _ExpandedName = pyxb.namespace.ExpandedName(Namespace, u'RotationEnum')
    _XSDLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 84, 2)
    _Documentation = None
RotationEnum._CF_enumeration = pyxb.binding.facets.CF_enumeration(value_datatype=RotationEnum, enum_prefix=None)
RotationEnum.r0 = RotationEnum._CF_enumeration.addEnumeration(unicode_value=u'r0', tag=u'r0')
RotationEnum.r90 = RotationEnum._CF_enumeration.addEnumeration(unicode_value=u'r90', tag=u'r90')
RotationEnum.r180 = RotationEnum._CF_enumeration.addEnumeration(unicode_value=u'r180', tag=u'r180')
RotationEnum.r270 = RotationEnum._CF_enumeration.addEnumeration(unicode_value=u'r270', tag=u'r270')
RotationEnum._InitializeFacetMap(RotationEnum._CF_enumeration)
Namespace.addCategoryObject('typeBinding', u'RotationEnum', RotationEnum)

# Atomic simple type: {eda}LayerEnum
class LayerEnum (pyxb.binding.datatypes.string, pyxb.binding.basis.enumeration_mixin):

    """An atomic simple type."""

    _ExpandedName = pyxb.namespace.ExpandedName(Namespace, u'LayerEnum')
    _XSDLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 92, 2)
    _Documentation = None
LayerEnum._CF_enumeration = pyxb.binding.facets.CF_enumeration(value_datatype=LayerEnum, enum_prefix=None)
LayerEnum.Top = LayerEnum._CF_enumeration.addEnumeration(unicode_value=u'Top', tag=u'Top')
LayerEnum.Bottom = LayerEnum._CF_enumeration.addEnumeration(unicode_value=u'Bottom', tag=u'Bottom')
LayerEnum._InitializeFacetMap(LayerEnum._CF_enumeration)
Namespace.addCategoryObject('typeBinding', u'LayerEnum', LayerEnum)

# Atomic simple type: {eda}LayerRangeEnum
class LayerRangeEnum (pyxb.binding.datatypes.string, pyxb.binding.basis.enumeration_mixin):

    """An atomic simple type."""

    _ExpandedName = pyxb.namespace.ExpandedName(Namespace, u'LayerRangeEnum')
    _XSDLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 98, 2)
    _Documentation = None
LayerRangeEnum._CF_enumeration = pyxb.binding.facets.CF_enumeration(value_datatype=LayerRangeEnum, enum_prefix=None)
LayerRangeEnum.Either = LayerRangeEnum._CF_enumeration.addEnumeration(unicode_value=u'Either', tag=u'Either')
LayerRangeEnum.Top = LayerRangeEnum._CF_enumeration.addEnumeration(unicode_value=u'Top', tag=u'Top')
LayerRangeEnum.Bottom = LayerRangeEnum._CF_enumeration.addEnumeration(unicode_value=u'Bottom', tag=u'Bottom')
LayerRangeEnum._InitializeFacetMap(LayerRangeEnum._CF_enumeration)
Namespace.addCategoryObject('typeBinding', u'LayerRangeEnum', LayerRangeEnum)

# Complex type {eda}Parameter with content type ELEMENT_ONLY
class Parameter_ (_ImportedBinding__avm.DomainModelParameter_):
    """Complex type {eda}Parameter with content type ELEMENT_ONLY"""
    _TypeDefinition = None
    _ContentTypeTag = pyxb.binding.basis.complexTypeDefinition._CT_ELEMENT_ONLY
    _Abstract = False
    _ExpandedName = pyxb.namespace.ExpandedName(Namespace, u'Parameter')
    _XSDLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 25, 2)
    _ElementMap = _ImportedBinding__avm.DomainModelParameter_._ElementMap.copy()
    _AttributeMap = _ImportedBinding__avm.DomainModelParameter_._AttributeMap.copy()
    # Base type is _ImportedBinding__avm.DomainModelParameter_
    
    # Element Value uses Python identifier Value
    __Value = pyxb.binding.content.ElementDeclaration(pyxb.namespace.ExpandedName(None, u'Value'), 'Value', '__eda_Parameter__Value', False, pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 29, 10), )

    
    Value = property(__Value.value, __Value.set, None, None)

    
    # Attribute Notes inherited from {avm}DomainModelParameter
    
    # Attribute XPosition inherited from {avm}DomainModelParameter
    
    # Attribute YPosition inherited from {avm}DomainModelParameter
    
    # Attribute Locator uses Python identifier Locator
    __Locator = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'Locator'), 'Locator', '__eda_Parameter__Locator', pyxb.binding.datatypes.string, required=True)
    __Locator._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 31, 8)
    __Locator._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 31, 8)
    
    Locator = property(__Locator.value, __Locator.set, None, None)

    _ElementMap.update({
        __Value.name() : __Value
    })
    _AttributeMap.update({
        __Locator.name() : __Locator
    })
Namespace.addCategoryObject('typeBinding', u'Parameter', Parameter_)


# Complex type {eda}PcbLayoutConstraint with content type EMPTY
class PcbLayoutConstraint_ (_ImportedBinding__avm.ContainerFeature_):
    """Complex type {eda}PcbLayoutConstraint with content type EMPTY"""
    _TypeDefinition = None
    _ContentTypeTag = pyxb.binding.basis.complexTypeDefinition._CT_EMPTY
    _Abstract = True
    _ExpandedName = pyxb.namespace.ExpandedName(Namespace, u'PcbLayoutConstraint')
    _XSDLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 35, 2)
    _ElementMap = _ImportedBinding__avm.ContainerFeature_._ElementMap.copy()
    _AttributeMap = _ImportedBinding__avm.ContainerFeature_._AttributeMap.copy()
    # Base type is _ImportedBinding__avm.ContainerFeature_
    
    # Attribute XPosition uses Python identifier XPosition
    __XPosition = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'XPosition'), 'XPosition', '__eda_PcbLayoutConstraint__XPosition', pyxb.binding.datatypes.unsignedInt)
    __XPosition._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 38, 8)
    __XPosition._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 38, 8)
    
    XPosition = property(__XPosition.value, __XPosition.set, None, None)

    
    # Attribute YPosition uses Python identifier YPosition
    __YPosition = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'YPosition'), 'YPosition', '__eda_PcbLayoutConstraint__YPosition', pyxb.binding.datatypes.unsignedInt)
    __YPosition._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 39, 8)
    __YPosition._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 39, 8)
    
    YPosition = property(__YPosition.value, __YPosition.set, None, None)

    _ElementMap.update({
        
    })
    _AttributeMap.update({
        __XPosition.name() : __XPosition,
        __YPosition.name() : __YPosition
    })
Namespace.addCategoryObject('typeBinding', u'PcbLayoutConstraint', PcbLayoutConstraint_)


# Complex type {eda}EDAModel with content type ELEMENT_ONLY
class EDAModel_ (_ImportedBinding__schematic.SchematicModel_):
    """Complex type {eda}EDAModel with content type ELEMENT_ONLY"""
    _TypeDefinition = None
    _ContentTypeTag = pyxb.binding.basis.complexTypeDefinition._CT_ELEMENT_ONLY
    _Abstract = False
    _ExpandedName = pyxb.namespace.ExpandedName(Namespace, u'EDAModel')
    _XSDLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 11, 2)
    _ElementMap = _ImportedBinding__schematic.SchematicModel_._ElementMap.copy()
    _AttributeMap = _ImportedBinding__schematic.SchematicModel_._AttributeMap.copy()
    # Base type is _ImportedBinding__schematic.SchematicModel_
    
    # Element Parameter uses Python identifier Parameter
    __Parameter = pyxb.binding.content.ElementDeclaration(pyxb.namespace.ExpandedName(None, u'Parameter'), 'Parameter', '__eda_EDAModel__Parameter', True, pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 15, 10), )

    
    Parameter = property(__Parameter.value, __Parameter.set, None, None)

    
    # Element Pin (Pin) inherited from {schematic}SchematicModel
    
    # Attribute UsesResource inherited from {avm}DomainModel
    
    # Attribute Author inherited from {avm}DomainModel
    
    # Attribute Notes inherited from {avm}DomainModel
    
    # Attribute XPosition inherited from {avm}DomainModel
    
    # Attribute YPosition inherited from {avm}DomainModel
    
    # Attribute Name inherited from {avm}DomainModel
    
    # Attribute Library uses Python identifier Library
    __Library = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'Library'), 'Library', '__eda_EDAModel__Library', pyxb.binding.datatypes.string)
    __Library._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 17, 8)
    __Library._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 17, 8)
    
    Library = property(__Library.value, __Library.set, None, None)

    
    # Attribute DeviceSet uses Python identifier DeviceSet
    __DeviceSet = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'DeviceSet'), 'DeviceSet', '__eda_EDAModel__DeviceSet', pyxb.binding.datatypes.string)
    __DeviceSet._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 18, 8)
    __DeviceSet._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 18, 8)
    
    DeviceSet = property(__DeviceSet.value, __DeviceSet.set, None, None)

    
    # Attribute Device uses Python identifier Device
    __Device = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'Device'), 'Device', '__eda_EDAModel__Device', pyxb.binding.datatypes.string)
    __Device._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 19, 8)
    __Device._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 19, 8)
    
    Device = property(__Device.value, __Device.set, None, None)

    
    # Attribute Package uses Python identifier Package
    __Package = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'Package'), 'Package', '__eda_EDAModel__Package', pyxb.binding.datatypes.string)
    __Package._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 20, 8)
    __Package._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 20, 8)
    
    Package = property(__Package.value, __Package.set, None, None)

    
    # Attribute HasMultiLayerFootprint uses Python identifier HasMultiLayerFootprint
    __HasMultiLayerFootprint = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'HasMultiLayerFootprint'), 'HasMultiLayerFootprint', '__eda_EDAModel__HasMultiLayerFootprint', pyxb.binding.datatypes.boolean, unicode_default=u'false')
    __HasMultiLayerFootprint._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 21, 8)
    __HasMultiLayerFootprint._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 21, 8)
    
    HasMultiLayerFootprint = property(__HasMultiLayerFootprint.value, __HasMultiLayerFootprint.set, None, None)

    _ElementMap.update({
        __Parameter.name() : __Parameter
    })
    _AttributeMap.update({
        __Library.name() : __Library,
        __DeviceSet.name() : __DeviceSet,
        __Device.name() : __Device,
        __Package.name() : __Package,
        __HasMultiLayerFootprint.name() : __HasMultiLayerFootprint
    })
Namespace.addCategoryObject('typeBinding', u'EDAModel', EDAModel_)


# Complex type {eda}ExactLayoutConstraint with content type EMPTY
class ExactLayoutConstraint_ (PcbLayoutConstraint_):
    """Complex type {eda}ExactLayoutConstraint with content type EMPTY"""
    _TypeDefinition = None
    _ContentTypeTag = pyxb.binding.basis.complexTypeDefinition._CT_EMPTY
    _Abstract = False
    _ExpandedName = pyxb.namespace.ExpandedName(Namespace, u'ExactLayoutConstraint')
    _XSDLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 43, 2)
    _ElementMap = PcbLayoutConstraint_._ElementMap.copy()
    _AttributeMap = PcbLayoutConstraint_._AttributeMap.copy()
    # Base type is PcbLayoutConstraint_
    
    # Attribute XPosition inherited from {eda}PcbLayoutConstraint
    
    # Attribute YPosition inherited from {eda}PcbLayoutConstraint
    
    # Attribute X uses Python identifier X
    __X = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'X'), 'X', '__eda_ExactLayoutConstraint__X', pyxb.binding.datatypes.double, required=True)
    __X._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 46, 8)
    __X._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 46, 8)
    
    X = property(__X.value, __X.set, None, None)

    
    # Attribute Y uses Python identifier Y
    __Y = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'Y'), 'Y', '__eda_ExactLayoutConstraint__Y', pyxb.binding.datatypes.double, required=True)
    __Y._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 47, 8)
    __Y._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 47, 8)
    
    Y = property(__Y.value, __Y.set, None, None)

    
    # Attribute Layer uses Python identifier Layer
    __Layer = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'Layer'), 'Layer', '__eda_ExactLayoutConstraint__Layer', LayerEnum, unicode_default=u'Top')
    __Layer._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 48, 8)
    __Layer._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 48, 8)
    
    Layer = property(__Layer.value, __Layer.set, None, None)

    
    # Attribute Rotation uses Python identifier Rotation
    __Rotation = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'Rotation'), 'Rotation', '__eda_ExactLayoutConstraint__Rotation', RotationEnum)
    __Rotation._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 49, 8)
    __Rotation._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 49, 8)
    
    Rotation = property(__Rotation.value, __Rotation.set, None, None)

    
    # Attribute ConstraintTarget uses Python identifier ConstraintTarget
    __ConstraintTarget = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'ConstraintTarget'), 'ConstraintTarget', '__eda_ExactLayoutConstraint__ConstraintTarget', pyxb.binding.datatypes.anyURI, required=True)
    __ConstraintTarget._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 50, 8)
    __ConstraintTarget._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 50, 8)
    
    ConstraintTarget = property(__ConstraintTarget.value, __ConstraintTarget.set, None, None)

    _ElementMap.update({
        
    })
    _AttributeMap.update({
        __X.name() : __X,
        __Y.name() : __Y,
        __Layer.name() : __Layer,
        __Rotation.name() : __Rotation,
        __ConstraintTarget.name() : __ConstraintTarget
    })
Namespace.addCategoryObject('typeBinding', u'ExactLayoutConstraint', ExactLayoutConstraint_)


# Complex type {eda}RangeLayoutConstraint with content type EMPTY
class RangeLayoutConstraint_ (PcbLayoutConstraint_):
    """Complex type {eda}RangeLayoutConstraint with content type EMPTY"""
    _TypeDefinition = None
    _ContentTypeTag = pyxb.binding.basis.complexTypeDefinition._CT_EMPTY
    _Abstract = False
    _ExpandedName = pyxb.namespace.ExpandedName(Namespace, u'RangeLayoutConstraint')
    _XSDLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 54, 2)
    _ElementMap = PcbLayoutConstraint_._ElementMap.copy()
    _AttributeMap = PcbLayoutConstraint_._AttributeMap.copy()
    # Base type is PcbLayoutConstraint_
    
    # Attribute XPosition inherited from {eda}PcbLayoutConstraint
    
    # Attribute YPosition inherited from {eda}PcbLayoutConstraint
    
    # Attribute XRangeMin uses Python identifier XRangeMin
    __XRangeMin = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'XRangeMin'), 'XRangeMin', '__eda_RangeLayoutConstraint__XRangeMin', pyxb.binding.datatypes.double)
    __XRangeMin._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 57, 8)
    __XRangeMin._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 57, 8)
    
    XRangeMin = property(__XRangeMin.value, __XRangeMin.set, None, None)

    
    # Attribute XRangeMax uses Python identifier XRangeMax
    __XRangeMax = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'XRangeMax'), 'XRangeMax', '__eda_RangeLayoutConstraint__XRangeMax', pyxb.binding.datatypes.double)
    __XRangeMax._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 58, 8)
    __XRangeMax._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 58, 8)
    
    XRangeMax = property(__XRangeMax.value, __XRangeMax.set, None, None)

    
    # Attribute YRangeMin uses Python identifier YRangeMin
    __YRangeMin = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'YRangeMin'), 'YRangeMin', '__eda_RangeLayoutConstraint__YRangeMin', pyxb.binding.datatypes.double)
    __YRangeMin._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 59, 8)
    __YRangeMin._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 59, 8)
    
    YRangeMin = property(__YRangeMin.value, __YRangeMin.set, None, None)

    
    # Attribute YRangeMax uses Python identifier YRangeMax
    __YRangeMax = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'YRangeMax'), 'YRangeMax', '__eda_RangeLayoutConstraint__YRangeMax', pyxb.binding.datatypes.double)
    __YRangeMax._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 60, 8)
    __YRangeMax._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 60, 8)
    
    YRangeMax = property(__YRangeMax.value, __YRangeMax.set, None, None)

    
    # Attribute LayerRange uses Python identifier LayerRange
    __LayerRange = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'LayerRange'), 'LayerRange', '__eda_RangeLayoutConstraint__LayerRange', LayerRangeEnum, unicode_default=u'Either')
    __LayerRange._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 61, 8)
    __LayerRange._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 61, 8)
    
    LayerRange = property(__LayerRange.value, __LayerRange.set, None, None)

    
    # Attribute ConstraintTarget uses Python identifier ConstraintTarget
    __ConstraintTarget = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'ConstraintTarget'), 'ConstraintTarget', '__eda_RangeLayoutConstraint__ConstraintTarget', STD_ANON, required=True)
    __ConstraintTarget._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 62, 8)
    __ConstraintTarget._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 62, 8)
    
    ConstraintTarget = property(__ConstraintTarget.value, __ConstraintTarget.set, None, None)

    _ElementMap.update({
        
    })
    _AttributeMap.update({
        __XRangeMin.name() : __XRangeMin,
        __XRangeMax.name() : __XRangeMax,
        __YRangeMin.name() : __YRangeMin,
        __YRangeMax.name() : __YRangeMax,
        __LayerRange.name() : __LayerRange,
        __ConstraintTarget.name() : __ConstraintTarget
    })
Namespace.addCategoryObject('typeBinding', u'RangeLayoutConstraint', RangeLayoutConstraint_)


# Complex type {eda}RelativeLayoutConstraint with content type EMPTY
class RelativeLayoutConstraint_ (PcbLayoutConstraint_):
    """Complex type {eda}RelativeLayoutConstraint with content type EMPTY"""
    _TypeDefinition = None
    _ContentTypeTag = pyxb.binding.basis.complexTypeDefinition._CT_EMPTY
    _Abstract = False
    _ExpandedName = pyxb.namespace.ExpandedName(Namespace, u'RelativeLayoutConstraint')
    _XSDLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 70, 2)
    _ElementMap = PcbLayoutConstraint_._ElementMap.copy()
    _AttributeMap = PcbLayoutConstraint_._AttributeMap.copy()
    # Base type is PcbLayoutConstraint_
    
    # Attribute XPosition inherited from {eda}PcbLayoutConstraint
    
    # Attribute YPosition inherited from {eda}PcbLayoutConstraint
    
    # Attribute XOffset uses Python identifier XOffset
    __XOffset = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'XOffset'), 'XOffset', '__eda_RelativeLayoutConstraint__XOffset', pyxb.binding.datatypes.double)
    __XOffset._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 73, 8)
    __XOffset._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 73, 8)
    
    XOffset = property(__XOffset.value, __XOffset.set, None, None)

    
    # Attribute YOffset uses Python identifier YOffset
    __YOffset = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'YOffset'), 'YOffset', '__eda_RelativeLayoutConstraint__YOffset', pyxb.binding.datatypes.double)
    __YOffset._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 74, 8)
    __YOffset._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 74, 8)
    
    YOffset = property(__YOffset.value, __YOffset.set, None, None)

    
    # Attribute ConstraintTarget uses Python identifier ConstraintTarget
    __ConstraintTarget = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'ConstraintTarget'), 'ConstraintTarget', '__eda_RelativeLayoutConstraint__ConstraintTarget', STD_ANON_, required=True)
    __ConstraintTarget._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 75, 8)
    __ConstraintTarget._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 75, 8)
    
    ConstraintTarget = property(__ConstraintTarget.value, __ConstraintTarget.set, None, None)

    
    # Attribute Origin uses Python identifier Origin
    __Origin = pyxb.binding.content.AttributeUse(pyxb.namespace.ExpandedName(None, u'Origin'), 'Origin', '__eda_RelativeLayoutConstraint__Origin', pyxb.binding.datatypes.anyURI, required=True)
    __Origin._DeclarationLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 80, 8)
    __Origin._UseLocation = pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 80, 8)
    
    Origin = property(__Origin.value, __Origin.set, None, None)

    _ElementMap.update({
        
    })
    _AttributeMap.update({
        __XOffset.name() : __XOffset,
        __YOffset.name() : __YOffset,
        __ConstraintTarget.name() : __ConstraintTarget,
        __Origin.name() : __Origin
    })
Namespace.addCategoryObject('typeBinding', u'RelativeLayoutConstraint', RelativeLayoutConstraint_)


Parameter = pyxb.binding.basis.element(pyxb.namespace.ExpandedName(Namespace, u'Parameter'), Parameter_, location=pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 6, 2))
Namespace.addCategoryObject('elementBinding', Parameter.name().localName(), Parameter)

PcbLayoutConstraint = pyxb.binding.basis.element(pyxb.namespace.ExpandedName(Namespace, u'PcbLayoutConstraint'), PcbLayoutConstraint_, location=pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 7, 2))
Namespace.addCategoryObject('elementBinding', PcbLayoutConstraint.name().localName(), PcbLayoutConstraint)

EDAModel = pyxb.binding.basis.element(pyxb.namespace.ExpandedName(Namespace, u'EDAModel'), EDAModel_, location=pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 5, 2))
Namespace.addCategoryObject('elementBinding', EDAModel.name().localName(), EDAModel)

ExactLayoutConstraint = pyxb.binding.basis.element(pyxb.namespace.ExpandedName(Namespace, u'ExactLayoutConstraint'), ExactLayoutConstraint_, location=pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 8, 2))
Namespace.addCategoryObject('elementBinding', ExactLayoutConstraint.name().localName(), ExactLayoutConstraint)

RangeLayoutConstraint = pyxb.binding.basis.element(pyxb.namespace.ExpandedName(Namespace, u'RangeLayoutConstraint'), RangeLayoutConstraint_, location=pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 9, 2))
Namespace.addCategoryObject('elementBinding', RangeLayoutConstraint.name().localName(), RangeLayoutConstraint)

RelativeLayoutConstraint = pyxb.binding.basis.element(pyxb.namespace.ExpandedName(Namespace, u'RelativeLayoutConstraint'), RelativeLayoutConstraint_, location=pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 10, 2))
Namespace.addCategoryObject('elementBinding', RelativeLayoutConstraint.name().localName(), RelativeLayoutConstraint)



Parameter_._AddElement(pyxb.binding.basis.element(pyxb.namespace.ExpandedName(None, u'Value'), _ImportedBinding__avm.Value_, scope=Parameter_, location=pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 29, 10)))

def _BuildAutomaton ():
    # Remove this helper function from the namespace after it is invoked
    global _BuildAutomaton
    del _BuildAutomaton
    import pyxb.utils.fac as fac

    counters = set()
    cc_0 = fac.CounterCondition(min=0L, max=1, metadata=pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 29, 10))
    counters.add(cc_0)
    states = []
    final_update = set()
    final_update.add(fac.UpdateInstruction(cc_0, False))
    symbol = pyxb.binding.content.ElementUse(Parameter_._UseForTag(pyxb.namespace.ExpandedName(None, u'Value')), pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 29, 10))
    st_0 = fac.State(symbol, is_initial=True, final_update=final_update, is_unordered_catenation=False)
    states.append(st_0)
    transitions = []
    transitions.append(fac.Transition(st_0, [
        fac.UpdateInstruction(cc_0, True) ]))
    st_0._set_transitionSet(transitions)
    return fac.Automaton(states, counters, True, containing_state=None)
Parameter_._Automaton = _BuildAutomaton()




EDAModel_._AddElement(pyxb.binding.basis.element(pyxb.namespace.ExpandedName(None, u'Parameter'), Parameter_, scope=EDAModel_, location=pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 15, 10)))

def _BuildAutomaton_ ():
    # Remove this helper function from the namespace after it is invoked
    global _BuildAutomaton_
    del _BuildAutomaton_
    import pyxb.utils.fac as fac

    counters = set()
    cc_0 = fac.CounterCondition(min=0L, max=None, metadata=pyxb.utils.utility.Location(u'avm.schematic.xsd', 10, 10))
    counters.add(cc_0)
    cc_1 = fac.CounterCondition(min=0L, max=None, metadata=pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 15, 10))
    counters.add(cc_1)
    states = []
    final_update = set()
    final_update.add(fac.UpdateInstruction(cc_0, False))
    symbol = pyxb.binding.content.ElementUse(EDAModel_._UseForTag(pyxb.namespace.ExpandedName(None, u'Pin')), pyxb.utils.utility.Location(u'avm.schematic.xsd', 10, 10))
    st_0 = fac.State(symbol, is_initial=True, final_update=final_update, is_unordered_catenation=False)
    states.append(st_0)
    final_update = set()
    final_update.add(fac.UpdateInstruction(cc_1, False))
    symbol = pyxb.binding.content.ElementUse(EDAModel_._UseForTag(pyxb.namespace.ExpandedName(None, u'Parameter')), pyxb.utils.utility.Location(u'avm.schematic.eda.xsd', 15, 10))
    st_1 = fac.State(symbol, is_initial=True, final_update=final_update, is_unordered_catenation=False)
    states.append(st_1)
    transitions = []
    transitions.append(fac.Transition(st_0, [
        fac.UpdateInstruction(cc_0, True) ]))
    transitions.append(fac.Transition(st_1, [
        fac.UpdateInstruction(cc_0, False) ]))
    st_0._set_transitionSet(transitions)
    transitions = []
    transitions.append(fac.Transition(st_1, [
        fac.UpdateInstruction(cc_1, True) ]))
    st_1._set_transitionSet(transitions)
    return fac.Automaton(states, counters, True, containing_state=None)
EDAModel_._Automaton = _BuildAutomaton_()

