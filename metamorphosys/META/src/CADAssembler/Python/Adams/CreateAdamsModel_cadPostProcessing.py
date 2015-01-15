# Copyright (C) 2013-2015 MetaMorph Software, Inc

# Permission is hereby granted, free of charge, to any person obtaining a
# copy of this data, including any software or models in source or binary
# form, as well as any drawings, specifications, and documentation
# (collectively "the Data"), to deal in the Data without restriction,
# including without limitation the rights to use, copy, modify, merge,
# publish, distribute, sublicense, and/or sell copies of the Data, and to
# permit persons to whom the Data is furnished to do so, subject to the
# following conditions:

# The above copyright notice and this permission notice shall be included
# in all copies or substantial portions of the Data.

# THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
# THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
# LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
# OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
# WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

# =======================
# This version of the META tools is a fork of an original version produced
# by Vanderbilt University's Institute for Software Integrated Systems (ISIS).
# Their license statement:

# Copyright (C) 2011-2014 Vanderbilt University

# Developed with the sponsorship of the Defense Advanced Research Projects
# Agency (DARPA) and delivered to the U.S. Government with Unlimited Rights
# as defined in DFARS 252.227-7013.

# Permission is hereby granted, free of charge, to any person obtaining a
# copy of this data, including any software or models in source or binary
# form, as well as any drawings, specifications, and documentation
# (collectively "the Data"), to deal in the Data without restriction,
# including without limitation the rights to use, copy, modify, merge,
# publish, distribute, sublicense, and/or sell copies of the Data, and to
# permit persons to whom the Data is furnished to do so, subject to the
# following conditions:

# The above copyright notice and this permission notice shall be included
# in all copies or substantial portions of the Data.

# THE DATA IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
# THE AUTHORS, SPONSORS, DEVELOPERS, CONTRIBUTORS, OR COPYRIGHT HOLDERS BE
# LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
# OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
# WITH THE DATA OR THE USE OR OTHER DEALINGS IN THE DATA.  

#!/usr/bin/env python
# -*- coding: utf-8 -*-

#
# Generated Wed Jun 04 14:17:15 2014 by generateDS.py version 2.12d.
#
# Command line options:
#   ('-o', 'a')
#
# Command line arguments:
#   CADPostProcessingParameters.xsd
#
# Command line:
#   \users\snyako\Desktop\generateDS-2.12d\generateDS.py -o "a" CADPostProcessingParameters.xsd
#
# Current working directory (os.getcwd()):
#   Adams
#

import sys
import getopt
import re as re_
import base64
import datetime as datetime_

etree_ = None
Verbose_import_ = False
(
    XMLParser_import_none, XMLParser_import_lxml,
    XMLParser_import_elementtree
) = range(3)
XMLParser_import_library = None
try:
    # lxml
    from lxml import etree as etree_
    XMLParser_import_library = XMLParser_import_lxml
    if Verbose_import_:
        print("running with lxml.etree")
except ImportError:
    try:
        # cElementTree from Python 2.5+
        import xml.etree.cElementTree as etree_
        XMLParser_import_library = XMLParser_import_elementtree
        if Verbose_import_:
            print("running with cElementTree on Python 2.5+")
    except ImportError:
        try:
            # ElementTree from Python 2.5+
            import xml.etree.ElementTree as etree_
            XMLParser_import_library = XMLParser_import_elementtree
            if Verbose_import_:
                print("running with ElementTree on Python 2.5+")
        except ImportError:
            try:
                # normal cElementTree install
                import cElementTree as etree_
                XMLParser_import_library = XMLParser_import_elementtree
                if Verbose_import_:
                    print("running with cElementTree")
            except ImportError:
                try:
                    # normal ElementTree install
                    import elementtree.ElementTree as etree_
                    XMLParser_import_library = XMLParser_import_elementtree
                    if Verbose_import_:
                        print("running with ElementTree")
                except ImportError:
                    raise ImportError(
                        "Failed to import ElementTree from any known place")


def parsexml_(*args, **kwargs):
    if (XMLParser_import_library == XMLParser_import_lxml and
            'parser' not in kwargs):
        # Use the lxml ElementTree compatible parser so that, e.g.,
        #   we ignore comments.
        kwargs['parser'] = etree_.ETCompatXMLParser()
    doc = etree_.parse(*args, **kwargs)
    return doc

#
# User methods
#
# Calls to the methods in these classes are generated by generateDS.py.
# You can replace these methods by re-implementing the following class
#   in a module named generatedssuper.py.

try:
    from generatedssuper import GeneratedsSuper
except ImportError, exp:

    class GeneratedsSuper(object):
        tzoff_pattern = re_.compile(r'(\+|-)((0\d|1[0-3]):[0-5]\d|14:00)$')
        class _FixedOffsetTZ(datetime_.tzinfo):
            def __init__(self, offset, name):
                self.__offset = datetime_.timedelta(minutes=offset)
                self.__name = name
            def utcoffset(self, dt):
                return self.__offset
            def tzname(self, dt):
                return self.__name
            def dst(self, dt):
                return None
        def gds_format_string(self, input_data, input_name=''):
            return input_data
        def gds_validate_string(self, input_data, node, input_name=''):
            if not input_data:
                return ''
            else:
                return input_data
        def gds_format_base64(self, input_data, input_name=''):
            return base64.b64encode(input_data)
        def gds_validate_base64(self, input_data, node, input_name=''):
            return input_data
        def gds_format_integer(self, input_data, input_name=''):
            return '%d' % input_data
        def gds_validate_integer(self, input_data, node, input_name=''):
            return input_data
        def gds_format_integer_list(self, input_data, input_name=''):
            return '%s' % input_data
        def gds_validate_integer_list(self, input_data, node, input_name=''):
            values = input_data.split()
            for value in values:
                try:
                    float(value)
                except (TypeError, ValueError):
                    raise_parse_error(node, 'Requires sequence of integers')
            return input_data
        def gds_format_float(self, input_data, input_name=''):
            return ('%.15f' % input_data).rstrip('0')
        def gds_validate_float(self, input_data, node, input_name=''):
            return input_data
        def gds_format_float_list(self, input_data, input_name=''):
            return '%s' % input_data
        def gds_validate_float_list(self, input_data, node, input_name=''):
            values = input_data.split()
            for value in values:
                try:
                    float(value)
                except (TypeError, ValueError):
                    raise_parse_error(node, 'Requires sequence of floats')
            return input_data
        def gds_format_double(self, input_data, input_name=''):
            return '%e' % input_data
        def gds_validate_double(self, input_data, node, input_name=''):
            return input_data
        def gds_format_double_list(self, input_data, input_name=''):
            return '%s' % input_data
        def gds_validate_double_list(self, input_data, node, input_name=''):
            values = input_data.split()
            for value in values:
                try:
                    float(value)
                except (TypeError, ValueError):
                    raise_parse_error(node, 'Requires sequence of doubles')
            return input_data
        def gds_format_boolean(self, input_data, input_name=''):
            return ('%s' % input_data).lower()
        def gds_validate_boolean(self, input_data, node, input_name=''):
            return input_data
        def gds_format_boolean_list(self, input_data, input_name=''):
            return '%s' % input_data
        def gds_validate_boolean_list(self, input_data, node, input_name=''):
            values = input_data.split()
            for value in values:
                if value not in ('true', '1', 'false', '0', ):
                    raise_parse_error(
                        node,
                        'Requires sequence of booleans '
                        '("true", "1", "false", "0")')
            return input_data
        def gds_validate_datetime(self, input_data, node, input_name=''):
            return input_data
        def gds_format_datetime(self, input_data, input_name=''):
            if input_data.microsecond == 0:
                _svalue = '%04d-%02d-%02dT%02d:%02d:%02d' % (
                    input_data.year,
                    input_data.month,
                    input_data.day,
                    input_data.hour,
                    input_data.minute,
                    input_data.second,
                )
            else:
                _svalue = '%04d-%02d-%02dT%02d:%02d:%02d.%s' % (
                    input_data.year,
                    input_data.month,
                    input_data.day,
                    input_data.hour,
                    input_data.minute,
                    input_data.second,
                    ('%f' % (float(input_data.microsecond) / 1000000))[2:],
                )
            if input_data.tzinfo is not None:
                tzoff = input_data.tzinfo.utcoffset(input_data)
                if tzoff is not None:
                    total_seconds = tzoff.seconds + (86400 * tzoff.days)
                    if total_seconds == 0:
                        _svalue += 'Z'
                    else:
                        if total_seconds < 0:
                            _svalue += '-'
                            total_seconds *= -1
                        else:
                            _svalue += '+'
                        hours = total_seconds // 3600
                        minutes = (total_seconds - (hours * 3600)) // 60
                        _svalue += '{0:02d}:{1:02d}'.format(hours, minutes)
            return _svalue
        @classmethod
        def gds_parse_datetime(cls, input_data):
            tz = None
            if input_data[-1] == 'Z':
                tz = GeneratedsSuper._FixedOffsetTZ(0, 'UTC')
                input_data = input_data[:-1]
            else:
                results = GeneratedsSuper.tzoff_pattern.search(input_data)
                if results is not None:
                    tzoff_parts = results.group(2).split(':')
                    tzoff = int(tzoff_parts[0]) * 60 + int(tzoff_parts[1])
                    if results.group(1) == '-':
                        tzoff *= -1
                    tz = GeneratedsSuper._FixedOffsetTZ(
                        tzoff, results.group(0))
                    input_data = input_data[:-6]
            if len(input_data.split('.')) > 1:
                dt = datetime_.datetime.strptime(
                    input_data, '%Y-%m-%dT%H:%M:%S.%f')
            else:
                dt = datetime_.datetime.strptime(
                    input_data, '%Y-%m-%dT%H:%M:%S')
            dt = dt.replace(tzinfo=tz)
            return dt
        def gds_validate_date(self, input_data, node, input_name=''):
            return input_data
        def gds_format_date(self, input_data, input_name=''):
            _svalue = '%04d-%02d-%02d' % (
                input_data.year,
                input_data.month,
                input_data.day,
            )
            try:
                if input_data.tzinfo is not None:
                    tzoff = input_data.tzinfo.utcoffset(input_data)
                    if tzoff is not None:
                        total_seconds = tzoff.seconds + (86400 * tzoff.days)
                        if total_seconds == 0:
                            _svalue += 'Z'
                        else:
                            if total_seconds < 0:
                                _svalue += '-'
                                total_seconds *= -1
                            else:
                                _svalue += '+'
                            hours = total_seconds // 3600
                            minutes = (total_seconds - (hours * 3600)) // 60
                            _svalue += '{0:02d}:{1:02d}'.format(hours, minutes)
            except AttributeError:
                pass
            return _svalue
        @classmethod
        def gds_parse_date(cls, input_data):
            tz = None
            if input_data[-1] == 'Z':
                tz = GeneratedsSuper._FixedOffsetTZ(0, 'UTC')
                input_data = input_data[:-1]
            else:
                results = GeneratedsSuper.tzoff_pattern.search(input_data)
                if results is not None:
                    tzoff_parts = results.group(2).split(':')
                    tzoff = int(tzoff_parts[0]) * 60 + int(tzoff_parts[1])
                    if results.group(1) == '-':
                        tzoff *= -1
                    tz = GeneratedsSuper._FixedOffsetTZ(
                        tzoff, results.group(0))
                    input_data = input_data[:-6]
            dt = datetime_.datetime.strptime(input_data, '%Y-%m-%d')
            dt = dt.replace(tzinfo=tz)
            return dt.date()
        def gds_validate_time(self, input_data, node, input_name=''):
            return input_data
        def gds_format_time(self, input_data, input_name=''):
            if input_data.microsecond == 0:
                _svalue = '%02d:%02d:%02d' % (
                    input_data.hour,
                    input_data.minute,
                    input_data.second,
                )
            else:
                _svalue = '%02d:%02d:%02d.%s' % (
                    input_data.hour,
                    input_data.minute,
                    input_data.second,
                    ('%f' % (float(input_data.microsecond) / 1000000))[2:],
                )
            if input_data.tzinfo is not None:
                tzoff = input_data.tzinfo.utcoffset(input_data)
                if tzoff is not None:
                    total_seconds = tzoff.seconds + (86400 * tzoff.days)
                    if total_seconds == 0:
                        _svalue += 'Z'
                    else:
                        if total_seconds < 0:
                            _svalue += '-'
                            total_seconds *= -1
                        else:
                            _svalue += '+'
                        hours = total_seconds // 3600
                        minutes = (total_seconds - (hours * 3600)) // 60
                        _svalue += '{0:02d}:{1:02d}'.format(hours, minutes)
            return _svalue
        @classmethod
        def gds_parse_time(cls, input_data):
            tz = None
            if input_data[-1] == 'Z':
                tz = GeneratedsSuper._FixedOffsetTZ(0, 'UTC')
                input_data = input_data[:-1]
            else:
                results = GeneratedsSuper.tzoff_pattern.search(input_data)
                if results is not None:
                    tzoff_parts = results.group(2).split(':')
                    tzoff = int(tzoff_parts[0]) * 60 + int(tzoff_parts[1])
                    if results.group(1) == '-':
                        tzoff *= -1
                    tz = GeneratedsSuper._FixedOffsetTZ(
                        tzoff, results.group(0))
                    input_data = input_data[:-6]
            if len(input_data.split('.')) > 1:
                dt = datetime_.datetime.strptime(input_data, '%H:%M:%S.%f')
            else:
                dt = datetime_.datetime.strptime(input_data, '%H:%M:%S')
            dt = dt.replace(tzinfo=tz)
            return dt.time()
        def gds_str_lower(self, instring):
            return instring.lower()
        def get_path_(self, node):
            path_list = []
            self.get_path_list_(node, path_list)
            path_list.reverse()
            path = '/'.join(path_list)
            return path
        Tag_strip_pattern_ = re_.compile(r'\{.*\}')
        def get_path_list_(self, node, path_list):
            if node is None:
                return
            tag = GeneratedsSuper.Tag_strip_pattern_.sub('', node.tag)
            if tag:
                path_list.append(tag)
            self.get_path_list_(node.getparent(), path_list)
        def get_class_obj_(self, node, default_class=None):
            class_obj1 = default_class
            if 'xsi' in node.nsmap:
                classname = node.get('{%s}type' % node.nsmap['xsi'])
                if classname is not None:
                    names = classname.split(':')
                    if len(names) == 2:
                        classname = names[1]
                    class_obj2 = globals().get(classname)
                    if class_obj2 is not None:
                        class_obj1 = class_obj2
            return class_obj1
        def gds_build_any(self, node, type_name=None):
            return None
        @classmethod
        def gds_reverse_node_mapping(cls, mapping):
            return dict(((v, k) for k, v in mapping.iteritems()))


#
# If you have installed IPython you can uncomment and use the following.
# IPython is available from http://ipython.scipy.org/.
#

## from IPython.Shell import IPShellEmbed
## args = ''
## ipshell = IPShellEmbed(args,
##     banner = 'Dropping into IPython',
##     exit_msg = 'Leaving Interpreter, back to program.')

# Then use the following line where and when you want to drop into the
# IPython shell:
#    ipshell('<some message> -- Entering ipshell.\nHit Ctrl-D to exit')

#
# Globals
#

ExternalEncoding = 'ascii'
Tag_pattern_ = re_.compile(r'({.*})?(.*)')
String_cleanup_pat_ = re_.compile(r"[\n\r\s]+")
Namespace_extract_pat_ = re_.compile(r'{(.*)}(.*)')

#
# Support/utility functions.
#


def showIndent(outfile, level, pretty_print=True):
    if pretty_print:
        for idx in range(level):
            outfile.write('    ')


def quote_xml(inStr):
    if not inStr:
        return ''
    s1 = (isinstance(inStr, basestring) and inStr or
          '%s' % inStr)
    s1 = s1.replace('&', '&amp;')
    s1 = s1.replace('<', '&lt;')
    s1 = s1.replace('>', '&gt;')
    return s1


def quote_attrib(inStr):
    s1 = (isinstance(inStr, basestring) and inStr or
          '%s' % inStr)
    s1 = s1.replace('&', '&amp;')
    s1 = s1.replace('<', '&lt;')
    s1 = s1.replace('>', '&gt;')
    if '"' in s1:
        if "'" in s1:
            s1 = '"%s"' % s1.replace('"', "&quot;")
        else:
            s1 = "'%s'" % s1
    else:
        s1 = '"%s"' % s1
    return s1


def quote_python(inStr):
    s1 = inStr
    if s1.find("'") == -1:
        if s1.find('\n') == -1:
            return "'%s'" % s1
        else:
            return "'''%s'''" % s1
    else:
        if s1.find('"') != -1:
            s1 = s1.replace('"', '\\"')
        if s1.find('\n') == -1:
            return '"%s"' % s1
        else:
            return '"""%s"""' % s1


def get_all_text_(node):
    if node.text is not None:
        text = node.text
    else:
        text = ''
    for child in node:
        if child.tail is not None:
            text += child.tail
    return text


def find_attr_value_(attr_name, node):
    attrs = node.attrib
    attr_parts = attr_name.split(':')
    value = None
    if len(attr_parts) == 1:
        value = attrs.get(attr_name)
    elif len(attr_parts) == 2:
        prefix, name = attr_parts
        namespace = node.nsmap.get(prefix)
        if namespace is not None:
            value = attrs.get('{%s}%s' % (namespace, name, ))
    return value


class GDSParseError(Exception):
    pass


def raise_parse_error(node, msg):
    if XMLParser_import_library == XMLParser_import_lxml:
        msg = '%s (element %s/line %d)' % (
            msg, node.tag, node.sourceline, )
    else:
        msg = '%s (element %s)' % (msg, node.tag, )
    raise GDSParseError(msg)


class MixedContainer:
    # Constants for category:
    CategoryNone = 0
    CategoryText = 1
    CategorySimple = 2
    CategoryComplex = 3
    # Constants for content_type:
    TypeNone = 0
    TypeText = 1
    TypeString = 2
    TypeInteger = 3
    TypeFloat = 4
    TypeDecimal = 5
    TypeDouble = 6
    TypeBoolean = 7
    TypeBase64 = 8
    def __init__(self, category, content_type, name, value):
        self.category = category
        self.content_type = content_type
        self.name = name
        self.value = value
    def getCategory(self):
        return self.category
    def getContenttype(self, content_type):
        return self.content_type
    def getValue(self):
        return self.value
    def getName(self):
        return self.name
    def export(self, outfile, level, name, namespace, pretty_print=True):
        if self.category == MixedContainer.CategoryText:
            # Prevent exporting empty content as empty lines.
            if self.value.strip():
                outfile.write(self.value)
        elif self.category == MixedContainer.CategorySimple:
            self.exportSimple(outfile, level, name)
        else:    # category == MixedContainer.CategoryComplex
            self.value.export(outfile, level, namespace, name, pretty_print)
    def exportSimple(self, outfile, level, name):
        if self.content_type == MixedContainer.TypeString:
            outfile.write('<%s>%s</%s>' % (
                self.name, self.value, self.name))
        elif self.content_type == MixedContainer.TypeInteger or \
                self.content_type == MixedContainer.TypeBoolean:
            outfile.write('<%s>%d</%s>' % (
                self.name, self.value, self.name))
        elif self.content_type == MixedContainer.TypeFloat or \
                self.content_type == MixedContainer.TypeDecimal:
            outfile.write('<%s>%f</%s>' % (
                self.name, self.value, self.name))
        elif self.content_type == MixedContainer.TypeDouble:
            outfile.write('<%s>%g</%s>' % (
                self.name, self.value, self.name))
        elif self.content_type == MixedContainer.TypeBase64:
            outfile.write('<%s>%s</%s>' % (
                self.name, base64.b64encode(self.value), self.name))
    def to_etree(self, element):
        if self.category == MixedContainer.CategoryText:
            # Prevent exporting empty content as empty lines.
            if self.value.strip():
                if len(element) > 0:
                    if element[-1].tail is None:
                        element[-1].tail = self.value
                    else:
                        element[-1].tail += self.value
                else:
                    if element.text is None:
                        element.text = self.value
                    else:
                        element.text += self.value
        elif self.category == MixedContainer.CategorySimple:
            subelement = etree_.SubElement(element, '%s' % self.name)
            subelement.text = self.to_etree_simple()
        else:    # category == MixedContainer.CategoryComplex
            self.value.to_etree(element)
    def to_etree_simple(self):
        if self.content_type == MixedContainer.TypeString:
            text = self.value
        elif (self.content_type == MixedContainer.TypeInteger or
                self.content_type == MixedContainer.TypeBoolean):
            text = '%d' % self.value
        elif (self.content_type == MixedContainer.TypeFloat or
                self.content_type == MixedContainer.TypeDecimal):
            text = '%f' % self.value
        elif self.content_type == MixedContainer.TypeDouble:
            text = '%g' % self.value
        elif self.content_type == MixedContainer.TypeBase64:
            text = '%s' % base64.b64encode(self.value)
        return text
    def exportLiteral(self, outfile, level, name):
        if self.category == MixedContainer.CategoryText:
            showIndent(outfile, level)
            outfile.write(
                'model_.MixedContainer(%d, %d, "%s", "%s"),\n' % (
                    self.category, self.content_type, self.name, self.value))
        elif self.category == MixedContainer.CategorySimple:
            showIndent(outfile, level)
            outfile.write(
                'model_.MixedContainer(%d, %d, "%s", "%s"),\n' % (
                    self.category, self.content_type, self.name, self.value))
        else:    # category == MixedContainer.CategoryComplex
            showIndent(outfile, level)
            outfile.write(
                'model_.MixedContainer(%d, %d, "%s",\n' % (
                    self.category, self.content_type, self.name,))
            self.value.exportLiteral(outfile, level + 1)
            showIndent(outfile, level)
            outfile.write(')\n')


class MemberSpec_(object):
    def __init__(self, name='', data_type='', container=0):
        self.name = name
        self.data_type = data_type
        self.container = container
    def set_name(self, name): self.name = name
    def get_name(self): return self.name
    def set_data_type(self, data_type): self.data_type = data_type
    def get_data_type_chain(self): return self.data_type
    def get_data_type(self):
        if isinstance(self.data_type, list):
            if len(self.data_type) > 0:
                return self.data_type[-1]
            else:
                return 'xs:string'
        else:
            return self.data_type
    def set_container(self, container): self.container = container
    def get_container(self): return self.container


def _cast(typ, value):
    if typ is None or value is None:
        return value
    return typ(value)

#
# Data representation classes.
#


class ComplexMetricType(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, _instances=None, _derived=None, _real_archetype=None, _desynched_atts=None, MetricID=None, _subtype=None, SubType=None, DataFormat=None, _archetype=None, _id=None, Type=None, Metric=None):
        self.original_tagname_ = None
        self._instances = _cast(None, _instances)
        self._derived = _cast(None, _derived)
        self._real_archetype = _cast(bool, _real_archetype)
        self._desynched_atts = _cast(None, _desynched_atts)
        self.MetricID = _cast(None, MetricID)
        self._subtype = _cast(bool, _subtype)
        self.SubType = _cast(None, SubType)
        self.DataFormat = _cast(None, DataFormat)
        self._archetype = _cast(None, _archetype)
        self._id = _cast(None, _id)
        self.Type = _cast(None, Type)
        if Metric is None:
            self.Metric = []
        else:
            self.Metric = Metric
    def factory(*args_, **kwargs_):
        if ComplexMetricType.subclass:
            return ComplexMetricType.subclass(*args_, **kwargs_)
        else:
            return ComplexMetricType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Metric(self): return self.Metric
    def set_Metric(self, Metric): self.Metric = Metric
    def add_Metric(self, value): self.Metric.append(value)
    def insert_Metric(self, index, value): self.Metric[index] = value
    def get__instances(self): return self._instances
    def set__instances(self, _instances): self._instances = _instances
    def get__derived(self): return self._derived
    def set__derived(self, _derived): self._derived = _derived
    def get__real_archetype(self): return self._real_archetype
    def set__real_archetype(self, _real_archetype): self._real_archetype = _real_archetype
    def get__desynched_atts(self): return self._desynched_atts
    def set__desynched_atts(self, _desynched_atts): self._desynched_atts = _desynched_atts
    def get_MetricID(self): return self.MetricID
    def set_MetricID(self, MetricID): self.MetricID = MetricID
    def get__subtype(self): return self._subtype
    def set__subtype(self, _subtype): self._subtype = _subtype
    def get_SubType(self): return self.SubType
    def set_SubType(self, SubType): self.SubType = SubType
    def get_DataFormat(self): return self.DataFormat
    def set_DataFormat(self, DataFormat): self.DataFormat = DataFormat
    def get__archetype(self): return self._archetype
    def set__archetype(self, _archetype): self._archetype = _archetype
    def get__id(self): return self._id
    def set__id(self, _id): self._id = _id
    def get_Type(self): return self.Type
    def set_Type(self, Type): self.Type = Type
    def hasContent_(self):
        if (
            self.Metric
        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='ComplexMetricType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='ComplexMetricType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='ComplexMetricType', pretty_print=pretty_print)
            showIndent(outfile, level, pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='ComplexMetricType'):
        if self._instances is not None and '_instances' not in already_processed:
            already_processed.add('_instances')
            outfile.write(' _instances=%s' % (self.gds_format_string(quote_attrib(self._instances).encode(ExternalEncoding), input_name='_instances'), ))
        if self._derived is not None and '_derived' not in already_processed:
            already_processed.add('_derived')
            outfile.write(' _derived=%s' % (self.gds_format_string(quote_attrib(self._derived).encode(ExternalEncoding), input_name='_derived'), ))
        if self._real_archetype is not None and '_real_archetype' not in already_processed:
            already_processed.add('_real_archetype')
            outfile.write(' _real_archetype="%s"' % self.gds_format_boolean(self._real_archetype, input_name='_real_archetype'))
        if self._desynched_atts is not None and '_desynched_atts' not in already_processed:
            already_processed.add('_desynched_atts')
            outfile.write(' _desynched_atts=%s' % (self.gds_format_string(quote_attrib(self._desynched_atts).encode(ExternalEncoding), input_name='_desynched_atts'), ))
        if self.MetricID is not None and 'MetricID' not in already_processed:
            already_processed.add('MetricID')
            outfile.write(' MetricID=%s' % (self.gds_format_string(quote_attrib(self.MetricID).encode(ExternalEncoding), input_name='MetricID'), ))
        if self._subtype is not None and '_subtype' not in already_processed:
            already_processed.add('_subtype')
            outfile.write(' _subtype="%s"' % self.gds_format_boolean(self._subtype, input_name='_subtype'))
        if self.SubType is not None and 'SubType' not in already_processed:
            already_processed.add('SubType')
            outfile.write(' SubType=%s' % (self.gds_format_string(quote_attrib(self.SubType).encode(ExternalEncoding), input_name='SubType'), ))
        if self.DataFormat is not None and 'DataFormat' not in already_processed:
            already_processed.add('DataFormat')
            outfile.write(' DataFormat=%s' % (self.gds_format_string(quote_attrib(self.DataFormat).encode(ExternalEncoding), input_name='DataFormat'), ))
        if self._archetype is not None and '_archetype' not in already_processed:
            already_processed.add('_archetype')
            outfile.write(' _archetype=%s' % (self.gds_format_string(quote_attrib(self._archetype).encode(ExternalEncoding), input_name='_archetype'), ))
        if self._id is not None and '_id' not in already_processed:
            already_processed.add('_id')
            outfile.write(' _id=%s' % (self.gds_format_string(quote_attrib(self._id).encode(ExternalEncoding), input_name='_id'), ))
        if self.Type is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            outfile.write(' Type=%s' % (self.gds_format_string(quote_attrib(self.Type).encode(ExternalEncoding), input_name='Type'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='ComplexMetricType', fromsubclass_=False, pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        for Metric_ in self.Metric:
            Metric_.export(outfile, level, namespace_, name_='Metric', pretty_print=pretty_print)
    def exportLiteral(self, outfile, level, name_='ComplexMetricType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self._instances is not None and '_instances' not in already_processed:
            already_processed.add('_instances')
            showIndent(outfile, level)
            outfile.write('_instances="%s",\n' % (self._instances,))
        if self._derived is not None and '_derived' not in already_processed:
            already_processed.add('_derived')
            showIndent(outfile, level)
            outfile.write('_derived="%s",\n' % (self._derived,))
        if self._real_archetype is not None and '_real_archetype' not in already_processed:
            already_processed.add('_real_archetype')
            showIndent(outfile, level)
            outfile.write('_real_archetype=%s,\n' % (self._real_archetype,))
        if self._desynched_atts is not None and '_desynched_atts' not in already_processed:
            already_processed.add('_desynched_atts')
            showIndent(outfile, level)
            outfile.write('_desynched_atts="%s",\n' % (self._desynched_atts,))
        if self.MetricID is not None and 'MetricID' not in already_processed:
            already_processed.add('MetricID')
            showIndent(outfile, level)
            outfile.write('MetricID="%s",\n' % (self.MetricID,))
        if self._subtype is not None and '_subtype' not in already_processed:
            already_processed.add('_subtype')
            showIndent(outfile, level)
            outfile.write('_subtype=%s,\n' % (self._subtype,))
        if self.SubType is not None and 'SubType' not in already_processed:
            already_processed.add('SubType')
            showIndent(outfile, level)
            outfile.write('SubType="%s",\n' % (self.SubType,))
        if self.DataFormat is not None and 'DataFormat' not in already_processed:
            already_processed.add('DataFormat')
            showIndent(outfile, level)
            outfile.write('DataFormat="%s",\n' % (self.DataFormat,))
        if self._archetype is not None and '_archetype' not in already_processed:
            already_processed.add('_archetype')
            showIndent(outfile, level)
            outfile.write('_archetype="%s",\n' % (self._archetype,))
        if self._id is not None and '_id' not in already_processed:
            already_processed.add('_id')
            showIndent(outfile, level)
            outfile.write('_id="%s",\n' % (self._id,))
        if self.Type is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            showIndent(outfile, level)
            outfile.write('Type="%s",\n' % (self.Type,))
    def exportLiteralChildren(self, outfile, level, name_):
        showIndent(outfile, level)
        outfile.write('Metric=[\n')
        level += 1
        for Metric_ in self.Metric:
            showIndent(outfile, level)
            outfile.write('model_.MetricType(\n')
            Metric_.exportLiteral(outfile, level, name_='MetricType')
            showIndent(outfile, level)
            outfile.write('),\n')
        level -= 1
        showIndent(outfile, level)
        outfile.write('],\n')
    def build(self, node):
        already_processed = set()
        self.buildAttributes(node, node.attrib, already_processed)
        for child in node:
            nodeName_ = Tag_pattern_.match(child.tag).groups()[-1]
            self.buildChildren(child, node, nodeName_)
        return self
    def buildAttributes(self, node, attrs, already_processed):
        value = find_attr_value_('_instances', node)
        if value is not None and '_instances' not in already_processed:
            already_processed.add('_instances')
            self._instances = value
        value = find_attr_value_('_derived', node)
        if value is not None and '_derived' not in already_processed:
            already_processed.add('_derived')
            self._derived = value
        value = find_attr_value_('_real_archetype', node)
        if value is not None and '_real_archetype' not in already_processed:
            already_processed.add('_real_archetype')
            if value in ('true', '1'):
                self._real_archetype = True
            elif value in ('false', '0'):
                self._real_archetype = False
            else:
                raise_parse_error(node, 'Bad boolean attribute')
        value = find_attr_value_('_desynched_atts', node)
        if value is not None and '_desynched_atts' not in already_processed:
            already_processed.add('_desynched_atts')
            self._desynched_atts = value
        value = find_attr_value_('MetricID', node)
        if value is not None and 'MetricID' not in already_processed:
            already_processed.add('MetricID')
            self.MetricID = value
        value = find_attr_value_('_subtype', node)
        if value is not None and '_subtype' not in already_processed:
            already_processed.add('_subtype')
            if value in ('true', '1'):
                self._subtype = True
            elif value in ('false', '0'):
                self._subtype = False
            else:
                raise_parse_error(node, 'Bad boolean attribute')
        value = find_attr_value_('SubType', node)
        if value is not None and 'SubType' not in already_processed:
            already_processed.add('SubType')
            self.SubType = value
        value = find_attr_value_('DataFormat', node)
        if value is not None and 'DataFormat' not in already_processed:
            already_processed.add('DataFormat')
            self.DataFormat = value
        value = find_attr_value_('_archetype', node)
        if value is not None and '_archetype' not in already_processed:
            already_processed.add('_archetype')
            self._archetype = value
        value = find_attr_value_('_id', node)
        if value is not None and '_id' not in already_processed:
            already_processed.add('_id')
            self._id = value
        value = find_attr_value_('Type', node)
        if value is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            self.Type = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        if nodeName_ == 'Metric':
            obj_ = MetricType.factory()
            obj_.build(child_)
            self.Metric.append(obj_)
            obj_.original_tagname_ = 'Metric'
# end class ComplexMetricType


class MetricType(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, _instances=None, _derived=None, _real_archetype=None, _desynched_atts=None, Type=None, MetricID=None, _subtype=None, DataFormat=None, _archetype=None, Units=None, _id=None, ArrayValue=None):
        self.original_tagname_ = None
        self._instances = _cast(None, _instances)
        self._derived = _cast(None, _derived)
        self._real_archetype = _cast(bool, _real_archetype)
        self._desynched_atts = _cast(None, _desynched_atts)
        self.Type = _cast(None, Type)
        self.MetricID = _cast(None, MetricID)
        self._subtype = _cast(bool, _subtype)
        self.DataFormat = _cast(None, DataFormat)
        self._archetype = _cast(None, _archetype)
        self.Units = _cast(None, Units)
        self._id = _cast(None, _id)
        self.ArrayValue = _cast(None, ArrayValue)
    def factory(*args_, **kwargs_):
        if MetricType.subclass:
            return MetricType.subclass(*args_, **kwargs_)
        else:
            return MetricType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get__instances(self): return self._instances
    def set__instances(self, _instances): self._instances = _instances
    def get__derived(self): return self._derived
    def set__derived(self, _derived): self._derived = _derived
    def get__real_archetype(self): return self._real_archetype
    def set__real_archetype(self, _real_archetype): self._real_archetype = _real_archetype
    def get__desynched_atts(self): return self._desynched_atts
    def set__desynched_atts(self, _desynched_atts): self._desynched_atts = _desynched_atts
    def get_Type(self): return self.Type
    def set_Type(self, Type): self.Type = Type
    def get_MetricID(self): return self.MetricID
    def set_MetricID(self, MetricID): self.MetricID = MetricID
    def get__subtype(self): return self._subtype
    def set__subtype(self, _subtype): self._subtype = _subtype
    def get_DataFormat(self): return self.DataFormat
    def set_DataFormat(self, DataFormat): self.DataFormat = DataFormat
    def get__archetype(self): return self._archetype
    def set__archetype(self, _archetype): self._archetype = _archetype
    def get_Units(self): return self.Units
    def set_Units(self, Units): self.Units = Units
    def get__id(self): return self._id
    def set__id(self, _id): self._id = _id
    def get_ArrayValue(self): return self.ArrayValue
    def set_ArrayValue(self, ArrayValue): self.ArrayValue = ArrayValue
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='MetricType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='MetricType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='MetricType', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='MetricType'):
        if self._instances is not None and '_instances' not in already_processed:
            already_processed.add('_instances')
            outfile.write(' _instances=%s' % (self.gds_format_string(quote_attrib(self._instances).encode(ExternalEncoding), input_name='_instances'), ))
        if self._derived is not None and '_derived' not in already_processed:
            already_processed.add('_derived')
            outfile.write(' _derived=%s' % (self.gds_format_string(quote_attrib(self._derived).encode(ExternalEncoding), input_name='_derived'), ))
        if self._real_archetype is not None and '_real_archetype' not in already_processed:
            already_processed.add('_real_archetype')
            outfile.write(' _real_archetype="%s"' % self.gds_format_boolean(self._real_archetype, input_name='_real_archetype'))
        if self._desynched_atts is not None and '_desynched_atts' not in already_processed:
            already_processed.add('_desynched_atts')
            outfile.write(' _desynched_atts=%s' % (self.gds_format_string(quote_attrib(self._desynched_atts).encode(ExternalEncoding), input_name='_desynched_atts'), ))
        if self.Type is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            outfile.write(' Type=%s' % (self.gds_format_string(quote_attrib(self.Type).encode(ExternalEncoding), input_name='Type'), ))
        if self.MetricID is not None and 'MetricID' not in already_processed:
            already_processed.add('MetricID')
            outfile.write(' MetricID=%s' % (self.gds_format_string(quote_attrib(self.MetricID).encode(ExternalEncoding), input_name='MetricID'), ))
        if self._subtype is not None and '_subtype' not in already_processed:
            already_processed.add('_subtype')
            outfile.write(' _subtype="%s"' % self.gds_format_boolean(self._subtype, input_name='_subtype'))
        if self.DataFormat is not None and 'DataFormat' not in already_processed:
            already_processed.add('DataFormat')
            outfile.write(' DataFormat=%s' % (self.gds_format_string(quote_attrib(self.DataFormat).encode(ExternalEncoding), input_name='DataFormat'), ))
        if self._archetype is not None and '_archetype' not in already_processed:
            already_processed.add('_archetype')
            outfile.write(' _archetype=%s' % (self.gds_format_string(quote_attrib(self._archetype).encode(ExternalEncoding), input_name='_archetype'), ))
        if self.Units is not None and 'Units' not in already_processed:
            already_processed.add('Units')
            outfile.write(' Units=%s' % (self.gds_format_string(quote_attrib(self.Units).encode(ExternalEncoding), input_name='Units'), ))
        if self._id is not None and '_id' not in already_processed:
            already_processed.add('_id')
            outfile.write(' _id=%s' % (self.gds_format_string(quote_attrib(self._id).encode(ExternalEncoding), input_name='_id'), ))
        if self.ArrayValue is not None and 'ArrayValue' not in already_processed:
            already_processed.add('ArrayValue')
            outfile.write(' ArrayValue=%s' % (self.gds_format_string(quote_attrib(self.ArrayValue).encode(ExternalEncoding), input_name='ArrayValue'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='MetricType', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='MetricType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self._instances is not None and '_instances' not in already_processed:
            already_processed.add('_instances')
            showIndent(outfile, level)
            outfile.write('_instances="%s",\n' % (self._instances,))
        if self._derived is not None and '_derived' not in already_processed:
            already_processed.add('_derived')
            showIndent(outfile, level)
            outfile.write('_derived="%s",\n' % (self._derived,))
        if self._real_archetype is not None and '_real_archetype' not in already_processed:
            already_processed.add('_real_archetype')
            showIndent(outfile, level)
            outfile.write('_real_archetype=%s,\n' % (self._real_archetype,))
        if self._desynched_atts is not None and '_desynched_atts' not in already_processed:
            already_processed.add('_desynched_atts')
            showIndent(outfile, level)
            outfile.write('_desynched_atts="%s",\n' % (self._desynched_atts,))
        if self.Type is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            showIndent(outfile, level)
            outfile.write('Type="%s",\n' % (self.Type,))
        if self.MetricID is not None and 'MetricID' not in already_processed:
            already_processed.add('MetricID')
            showIndent(outfile, level)
            outfile.write('MetricID="%s",\n' % (self.MetricID,))
        if self._subtype is not None and '_subtype' not in already_processed:
            already_processed.add('_subtype')
            showIndent(outfile, level)
            outfile.write('_subtype=%s,\n' % (self._subtype,))
        if self.DataFormat is not None and 'DataFormat' not in already_processed:
            already_processed.add('DataFormat')
            showIndent(outfile, level)
            outfile.write('DataFormat="%s",\n' % (self.DataFormat,))
        if self._archetype is not None and '_archetype' not in already_processed:
            already_processed.add('_archetype')
            showIndent(outfile, level)
            outfile.write('_archetype="%s",\n' % (self._archetype,))
        if self.Units is not None and 'Units' not in already_processed:
            already_processed.add('Units')
            showIndent(outfile, level)
            outfile.write('Units="%s",\n' % (self.Units,))
        if self._id is not None and '_id' not in already_processed:
            already_processed.add('_id')
            showIndent(outfile, level)
            outfile.write('_id="%s",\n' % (self._id,))
        if self.ArrayValue is not None and 'ArrayValue' not in already_processed:
            already_processed.add('ArrayValue')
            showIndent(outfile, level)
            outfile.write('ArrayValue="%s",\n' % (self.ArrayValue,))
    def exportLiteralChildren(self, outfile, level, name_):
        pass
    def build(self, node):
        already_processed = set()
        self.buildAttributes(node, node.attrib, already_processed)
        for child in node:
            nodeName_ = Tag_pattern_.match(child.tag).groups()[-1]
            self.buildChildren(child, node, nodeName_)
        return self
    def buildAttributes(self, node, attrs, already_processed):
        value = find_attr_value_('_instances', node)
        if value is not None and '_instances' not in already_processed:
            already_processed.add('_instances')
            self._instances = value
        value = find_attr_value_('_derived', node)
        if value is not None and '_derived' not in already_processed:
            already_processed.add('_derived')
            self._derived = value
        value = find_attr_value_('_real_archetype', node)
        if value is not None and '_real_archetype' not in already_processed:
            already_processed.add('_real_archetype')
            if value in ('true', '1'):
                self._real_archetype = True
            elif value in ('false', '0'):
                self._real_archetype = False
            else:
                raise_parse_error(node, 'Bad boolean attribute')
        value = find_attr_value_('_desynched_atts', node)
        if value is not None and '_desynched_atts' not in already_processed:
            already_processed.add('_desynched_atts')
            self._desynched_atts = value
        value = find_attr_value_('Type', node)
        if value is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            self.Type = value
        value = find_attr_value_('MetricID', node)
        if value is not None and 'MetricID' not in already_processed:
            already_processed.add('MetricID')
            self.MetricID = value
        value = find_attr_value_('_subtype', node)
        if value is not None and '_subtype' not in already_processed:
            already_processed.add('_subtype')
            if value in ('true', '1'):
                self._subtype = True
            elif value in ('false', '0'):
                self._subtype = False
            else:
                raise_parse_error(node, 'Bad boolean attribute')
        value = find_attr_value_('DataFormat', node)
        if value is not None and 'DataFormat' not in already_processed:
            already_processed.add('DataFormat')
            self.DataFormat = value
        value = find_attr_value_('_archetype', node)
        if value is not None and '_archetype' not in already_processed:
            already_processed.add('_archetype')
            self._archetype = value
        value = find_attr_value_('Units', node)
        if value is not None and 'Units' not in already_processed:
            already_processed.add('Units')
            self.Units = value
        value = find_attr_value_('_id', node)
        if value is not None and '_id' not in already_processed:
            already_processed.add('_id')
            self._id = value
        value = find_attr_value_('ArrayValue', node)
        if value is not None and 'ArrayValue' not in already_processed:
            already_processed.add('ArrayValue')
            self.ArrayValue = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class MetricType


class MetricsType(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, _derived=None, _real_archetype=None, _archetype=None, _subtype=None, _instances=None, _desynched_atts=None, _id=None, ComplexMetric=None, Metric=None):
        self.original_tagname_ = None
        self._derived = _cast(None, _derived)
        self._real_archetype = _cast(bool, _real_archetype)
        self._archetype = _cast(None, _archetype)
        self._subtype = _cast(bool, _subtype)
        self._instances = _cast(None, _instances)
        self._desynched_atts = _cast(None, _desynched_atts)
        self._id = _cast(None, _id)
        if ComplexMetric is None:
            self.ComplexMetric = []
        else:
            self.ComplexMetric = ComplexMetric
        if Metric is None:
            self.Metric = []
        else:
            self.Metric = Metric
    def factory(*args_, **kwargs_):
        if MetricsType.subclass:
            return MetricsType.subclass(*args_, **kwargs_)
        else:
            return MetricsType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_ComplexMetric(self): return self.ComplexMetric
    def set_ComplexMetric(self, ComplexMetric): self.ComplexMetric = ComplexMetric
    def add_ComplexMetric(self, value): self.ComplexMetric.append(value)
    def insert_ComplexMetric(self, index, value): self.ComplexMetric[index] = value
    def get_Metric(self): return self.Metric
    def set_Metric(self, Metric): self.Metric = Metric
    def add_Metric(self, value): self.Metric.append(value)
    def insert_Metric(self, index, value): self.Metric[index] = value
    def get__derived(self): return self._derived
    def set__derived(self, _derived): self._derived = _derived
    def get__real_archetype(self): return self._real_archetype
    def set__real_archetype(self, _real_archetype): self._real_archetype = _real_archetype
    def get__archetype(self): return self._archetype
    def set__archetype(self, _archetype): self._archetype = _archetype
    def get__subtype(self): return self._subtype
    def set__subtype(self, _subtype): self._subtype = _subtype
    def get__instances(self): return self._instances
    def set__instances(self, _instances): self._instances = _instances
    def get__desynched_atts(self): return self._desynched_atts
    def set__desynched_atts(self, _desynched_atts): self._desynched_atts = _desynched_atts
    def get__id(self): return self._id
    def set__id(self, _id): self._id = _id
    def hasContent_(self):
        if (
            self.ComplexMetric or
            self.Metric
        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='MetricsType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='MetricsType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='MetricsType', pretty_print=pretty_print)
            showIndent(outfile, level, pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='MetricsType'):
        if self._derived is not None and '_derived' not in already_processed:
            already_processed.add('_derived')
            outfile.write(' _derived=%s' % (self.gds_format_string(quote_attrib(self._derived).encode(ExternalEncoding), input_name='_derived'), ))
        if self._real_archetype is not None and '_real_archetype' not in already_processed:
            already_processed.add('_real_archetype')
            outfile.write(' _real_archetype="%s"' % self.gds_format_boolean(self._real_archetype, input_name='_real_archetype'))
        if self._archetype is not None and '_archetype' not in already_processed:
            already_processed.add('_archetype')
            outfile.write(' _archetype=%s' % (self.gds_format_string(quote_attrib(self._archetype).encode(ExternalEncoding), input_name='_archetype'), ))
        if self._subtype is not None and '_subtype' not in already_processed:
            already_processed.add('_subtype')
            outfile.write(' _subtype="%s"' % self.gds_format_boolean(self._subtype, input_name='_subtype'))
        if self._instances is not None and '_instances' not in already_processed:
            already_processed.add('_instances')
            outfile.write(' _instances=%s' % (self.gds_format_string(quote_attrib(self._instances).encode(ExternalEncoding), input_name='_instances'), ))
        if self._desynched_atts is not None and '_desynched_atts' not in already_processed:
            already_processed.add('_desynched_atts')
            outfile.write(' _desynched_atts=%s' % (self.gds_format_string(quote_attrib(self._desynched_atts).encode(ExternalEncoding), input_name='_desynched_atts'), ))
        if self._id is not None and '_id' not in already_processed:
            already_processed.add('_id')
            outfile.write(' _id=%s' % (self.gds_format_string(quote_attrib(self._id).encode(ExternalEncoding), input_name='_id'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='MetricsType', fromsubclass_=False, pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        for ComplexMetric_ in self.ComplexMetric:
            ComplexMetric_.export(outfile, level, namespace_, name_='ComplexMetric', pretty_print=pretty_print)
        for Metric_ in self.Metric:
            Metric_.export(outfile, level, namespace_, name_='Metric', pretty_print=pretty_print)
    def exportLiteral(self, outfile, level, name_='MetricsType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self._derived is not None and '_derived' not in already_processed:
            already_processed.add('_derived')
            showIndent(outfile, level)
            outfile.write('_derived="%s",\n' % (self._derived,))
        if self._real_archetype is not None and '_real_archetype' not in already_processed:
            already_processed.add('_real_archetype')
            showIndent(outfile, level)
            outfile.write('_real_archetype=%s,\n' % (self._real_archetype,))
        if self._archetype is not None and '_archetype' not in already_processed:
            already_processed.add('_archetype')
            showIndent(outfile, level)
            outfile.write('_archetype="%s",\n' % (self._archetype,))
        if self._subtype is not None and '_subtype' not in already_processed:
            already_processed.add('_subtype')
            showIndent(outfile, level)
            outfile.write('_subtype=%s,\n' % (self._subtype,))
        if self._instances is not None and '_instances' not in already_processed:
            already_processed.add('_instances')
            showIndent(outfile, level)
            outfile.write('_instances="%s",\n' % (self._instances,))
        if self._desynched_atts is not None and '_desynched_atts' not in already_processed:
            already_processed.add('_desynched_atts')
            showIndent(outfile, level)
            outfile.write('_desynched_atts="%s",\n' % (self._desynched_atts,))
        if self._id is not None and '_id' not in already_processed:
            already_processed.add('_id')
            showIndent(outfile, level)
            outfile.write('_id="%s",\n' % (self._id,))
    def exportLiteralChildren(self, outfile, level, name_):
        showIndent(outfile, level)
        outfile.write('ComplexMetric=[\n')
        level += 1
        for ComplexMetric_ in self.ComplexMetric:
            showIndent(outfile, level)
            outfile.write('model_.ComplexMetricType(\n')
            ComplexMetric_.exportLiteral(outfile, level, name_='ComplexMetricType')
            showIndent(outfile, level)
            outfile.write('),\n')
        level -= 1
        showIndent(outfile, level)
        outfile.write('],\n')
        showIndent(outfile, level)
        outfile.write('Metric=[\n')
        level += 1
        for Metric_ in self.Metric:
            showIndent(outfile, level)
            outfile.write('model_.MetricType(\n')
            Metric_.exportLiteral(outfile, level, name_='MetricType')
            showIndent(outfile, level)
            outfile.write('),\n')
        level -= 1
        showIndent(outfile, level)
        outfile.write('],\n')
    def build(self, node):
        already_processed = set()
        self.buildAttributes(node, node.attrib, already_processed)
        for child in node:
            nodeName_ = Tag_pattern_.match(child.tag).groups()[-1]
            self.buildChildren(child, node, nodeName_)
        return self
    def buildAttributes(self, node, attrs, already_processed):
        value = find_attr_value_('_derived', node)
        if value is not None and '_derived' not in already_processed:
            already_processed.add('_derived')
            self._derived = value
        value = find_attr_value_('_real_archetype', node)
        if value is not None and '_real_archetype' not in already_processed:
            already_processed.add('_real_archetype')
            if value in ('true', '1'):
                self._real_archetype = True
            elif value in ('false', '0'):
                self._real_archetype = False
            else:
                raise_parse_error(node, 'Bad boolean attribute')
        value = find_attr_value_('_archetype', node)
        if value is not None and '_archetype' not in already_processed:
            already_processed.add('_archetype')
            self._archetype = value
        value = find_attr_value_('_subtype', node)
        if value is not None and '_subtype' not in already_processed:
            already_processed.add('_subtype')
            if value in ('true', '1'):
                self._subtype = True
            elif value in ('false', '0'):
                self._subtype = False
            else:
                raise_parse_error(node, 'Bad boolean attribute')
        value = find_attr_value_('_instances', node)
        if value is not None and '_instances' not in already_processed:
            already_processed.add('_instances')
            self._instances = value
        value = find_attr_value_('_desynched_atts', node)
        if value is not None and '_desynched_atts' not in already_processed:
            already_processed.add('_desynched_atts')
            self._desynched_atts = value
        value = find_attr_value_('_id', node)
        if value is not None and '_id' not in already_processed:
            already_processed.add('_id')
            self._id = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        if nodeName_ == 'ComplexMetric':
            obj_ = ComplexMetricType.factory()
            obj_.build(child_)
            self.ComplexMetric.append(obj_)
            obj_.original_tagname_ = 'ComplexMetric'
        elif nodeName_ == 'Metric':
            obj_ = MetricType.factory()
            obj_.build(child_)
            self.Metric.append(obj_)
            obj_.original_tagname_ = 'Metric'
# end class MetricsType


class MaterialType(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, Bearing=None, _derived=None, _real_archetype=None, _desynched_atts=None, _subtype=None, _instances=None, _archetype=None, Units=None, _id=None, Mises=None, Shear=None):
        self.original_tagname_ = None
        self.Bearing = _cast(float, Bearing)
        self._derived = _cast(None, _derived)
        self._real_archetype = _cast(bool, _real_archetype)
        self._desynched_atts = _cast(None, _desynched_atts)
        self._subtype = _cast(bool, _subtype)
        self._instances = _cast(None, _instances)
        self._archetype = _cast(None, _archetype)
        self.Units = _cast(None, Units)
        self._id = _cast(None, _id)
        self.Mises = _cast(float, Mises)
        self.Shear = _cast(float, Shear)
    def factory(*args_, **kwargs_):
        if MaterialType.subclass:
            return MaterialType.subclass(*args_, **kwargs_)
        else:
            return MaterialType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Bearing(self): return self.Bearing
    def set_Bearing(self, Bearing): self.Bearing = Bearing
    def get__derived(self): return self._derived
    def set__derived(self, _derived): self._derived = _derived
    def get__real_archetype(self): return self._real_archetype
    def set__real_archetype(self, _real_archetype): self._real_archetype = _real_archetype
    def get__desynched_atts(self): return self._desynched_atts
    def set__desynched_atts(self, _desynched_atts): self._desynched_atts = _desynched_atts
    def get__subtype(self): return self._subtype
    def set__subtype(self, _subtype): self._subtype = _subtype
    def get__instances(self): return self._instances
    def set__instances(self, _instances): self._instances = _instances
    def get__archetype(self): return self._archetype
    def set__archetype(self, _archetype): self._archetype = _archetype
    def get_Units(self): return self.Units
    def set_Units(self, Units): self.Units = Units
    def get__id(self): return self._id
    def set__id(self, _id): self._id = _id
    def get_Mises(self): return self.Mises
    def set_Mises(self, Mises): self.Mises = Mises
    def get_Shear(self): return self.Shear
    def set_Shear(self, Shear): self.Shear = Shear
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='MaterialType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='MaterialType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='MaterialType', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='MaterialType'):
        if self.Bearing is not None and 'Bearing' not in already_processed:
            already_processed.add('Bearing')
            outfile.write(' Bearing="%s"' % self.gds_format_double(self.Bearing, input_name='Bearing'))
        if self._derived is not None and '_derived' not in already_processed:
            already_processed.add('_derived')
            outfile.write(' _derived=%s' % (self.gds_format_string(quote_attrib(self._derived).encode(ExternalEncoding), input_name='_derived'), ))
        if self._real_archetype is not None and '_real_archetype' not in already_processed:
            already_processed.add('_real_archetype')
            outfile.write(' _real_archetype="%s"' % self.gds_format_boolean(self._real_archetype, input_name='_real_archetype'))
        if self._desynched_atts is not None and '_desynched_atts' not in already_processed:
            already_processed.add('_desynched_atts')
            outfile.write(' _desynched_atts=%s' % (self.gds_format_string(quote_attrib(self._desynched_atts).encode(ExternalEncoding), input_name='_desynched_atts'), ))
        if self._subtype is not None and '_subtype' not in already_processed:
            already_processed.add('_subtype')
            outfile.write(' _subtype="%s"' % self.gds_format_boolean(self._subtype, input_name='_subtype'))
        if self._instances is not None and '_instances' not in already_processed:
            already_processed.add('_instances')
            outfile.write(' _instances=%s' % (self.gds_format_string(quote_attrib(self._instances).encode(ExternalEncoding), input_name='_instances'), ))
        if self._archetype is not None and '_archetype' not in already_processed:
            already_processed.add('_archetype')
            outfile.write(' _archetype=%s' % (self.gds_format_string(quote_attrib(self._archetype).encode(ExternalEncoding), input_name='_archetype'), ))
        if self.Units is not None and 'Units' not in already_processed:
            already_processed.add('Units')
            outfile.write(' Units=%s' % (self.gds_format_string(quote_attrib(self.Units).encode(ExternalEncoding), input_name='Units'), ))
        if self._id is not None and '_id' not in already_processed:
            already_processed.add('_id')
            outfile.write(' _id=%s' % (self.gds_format_string(quote_attrib(self._id).encode(ExternalEncoding), input_name='_id'), ))
        if self.Mises is not None and 'Mises' not in already_processed:
            already_processed.add('Mises')
            outfile.write(' Mises="%s"' % self.gds_format_double(self.Mises, input_name='Mises'))
        if self.Shear is not None and 'Shear' not in already_processed:
            already_processed.add('Shear')
            outfile.write(' Shear="%s"' % self.gds_format_double(self.Shear, input_name='Shear'))
    def exportChildren(self, outfile, level, namespace_='', name_='MaterialType', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='MaterialType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.Bearing is not None and 'Bearing' not in already_processed:
            already_processed.add('Bearing')
            showIndent(outfile, level)
            outfile.write('Bearing=%e,\n' % (self.Bearing,))
        if self._derived is not None and '_derived' not in already_processed:
            already_processed.add('_derived')
            showIndent(outfile, level)
            outfile.write('_derived="%s",\n' % (self._derived,))
        if self._real_archetype is not None and '_real_archetype' not in already_processed:
            already_processed.add('_real_archetype')
            showIndent(outfile, level)
            outfile.write('_real_archetype=%s,\n' % (self._real_archetype,))
        if self._desynched_atts is not None and '_desynched_atts' not in already_processed:
            already_processed.add('_desynched_atts')
            showIndent(outfile, level)
            outfile.write('_desynched_atts="%s",\n' % (self._desynched_atts,))
        if self._subtype is not None and '_subtype' not in already_processed:
            already_processed.add('_subtype')
            showIndent(outfile, level)
            outfile.write('_subtype=%s,\n' % (self._subtype,))
        if self._instances is not None and '_instances' not in already_processed:
            already_processed.add('_instances')
            showIndent(outfile, level)
            outfile.write('_instances="%s",\n' % (self._instances,))
        if self._archetype is not None and '_archetype' not in already_processed:
            already_processed.add('_archetype')
            showIndent(outfile, level)
            outfile.write('_archetype="%s",\n' % (self._archetype,))
        if self.Units is not None and 'Units' not in already_processed:
            already_processed.add('Units')
            showIndent(outfile, level)
            outfile.write('Units="%s",\n' % (self.Units,))
        if self._id is not None and '_id' not in already_processed:
            already_processed.add('_id')
            showIndent(outfile, level)
            outfile.write('_id="%s",\n' % (self._id,))
        if self.Mises is not None and 'Mises' not in already_processed:
            already_processed.add('Mises')
            showIndent(outfile, level)
            outfile.write('Mises=%e,\n' % (self.Mises,))
        if self.Shear is not None and 'Shear' not in already_processed:
            already_processed.add('Shear')
            showIndent(outfile, level)
            outfile.write('Shear=%e,\n' % (self.Shear,))
    def exportLiteralChildren(self, outfile, level, name_):
        pass
    def build(self, node):
        already_processed = set()
        self.buildAttributes(node, node.attrib, already_processed)
        for child in node:
            nodeName_ = Tag_pattern_.match(child.tag).groups()[-1]
            self.buildChildren(child, node, nodeName_)
        return self
    def buildAttributes(self, node, attrs, already_processed):
        value = find_attr_value_('Bearing', node)
        if value is not None and 'Bearing' not in already_processed:
            already_processed.add('Bearing')
            try:
                self.Bearing = float(value)
            except ValueError, exp:
                raise ValueError('Bad float/double attribute (Bearing): %s' % exp)
        value = find_attr_value_('_derived', node)
        if value is not None and '_derived' not in already_processed:
            already_processed.add('_derived')
            self._derived = value
        value = find_attr_value_('_real_archetype', node)
        if value is not None and '_real_archetype' not in already_processed:
            already_processed.add('_real_archetype')
            if value in ('true', '1'):
                self._real_archetype = True
            elif value in ('false', '0'):
                self._real_archetype = False
            else:
                raise_parse_error(node, 'Bad boolean attribute')
        value = find_attr_value_('_desynched_atts', node)
        if value is not None and '_desynched_atts' not in already_processed:
            already_processed.add('_desynched_atts')
            self._desynched_atts = value
        value = find_attr_value_('_subtype', node)
        if value is not None and '_subtype' not in already_processed:
            already_processed.add('_subtype')
            if value in ('true', '1'):
                self._subtype = True
            elif value in ('false', '0'):
                self._subtype = False
            else:
                raise_parse_error(node, 'Bad boolean attribute')
        value = find_attr_value_('_instances', node)
        if value is not None and '_instances' not in already_processed:
            already_processed.add('_instances')
            self._instances = value
        value = find_attr_value_('_archetype', node)
        if value is not None and '_archetype' not in already_processed:
            already_processed.add('_archetype')
            self._archetype = value
        value = find_attr_value_('Units', node)
        if value is not None and 'Units' not in already_processed:
            already_processed.add('Units')
            self.Units = value
        value = find_attr_value_('_id', node)
        if value is not None and '_id' not in already_processed:
            already_processed.add('_id')
            self._id = value
        value = find_attr_value_('Mises', node)
        if value is not None and 'Mises' not in already_processed:
            already_processed.add('Mises')
            try:
                self.Mises = float(value)
            except ValueError, exp:
                raise ValueError('Bad float/double attribute (Mises): %s' % exp)
        value = find_attr_value_('Shear', node)
        if value is not None and 'Shear' not in already_processed:
            already_processed.add('Shear')
            try:
                self.Shear = float(value)
            except ValueError, exp:
                raise ValueError('Bad float/double attribute (Shear): %s' % exp)
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class MaterialType


class ComponentType(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, _derived=None, _real_archetype=None, _desynched_atts=None, ComponentInstanceID=None, FEAElementID=None, _instances=None, _archetype=None, _subtype=None, _id=None, Material=None, Metrics=None):
        self.original_tagname_ = None
        self._derived = _cast(None, _derived)
        self._real_archetype = _cast(bool, _real_archetype)
        self._desynched_atts = _cast(None, _desynched_atts)
        self.ComponentInstanceID = _cast(None, ComponentInstanceID)
        self.FEAElementID = _cast(None, FEAElementID)
        self._instances = _cast(None, _instances)
        self._archetype = _cast(None, _archetype)
        self._subtype = _cast(bool, _subtype)
        self._id = _cast(None, _id)
        self.Material = Material
        self.Metrics = Metrics
    def factory(*args_, **kwargs_):
        if ComponentType.subclass:
            return ComponentType.subclass(*args_, **kwargs_)
        else:
            return ComponentType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Material(self): return self.Material
    def set_Material(self, Material): self.Material = Material
    def get_Metrics(self): return self.Metrics
    def set_Metrics(self, Metrics): self.Metrics = Metrics
    def get__derived(self): return self._derived
    def set__derived(self, _derived): self._derived = _derived
    def get__real_archetype(self): return self._real_archetype
    def set__real_archetype(self, _real_archetype): self._real_archetype = _real_archetype
    def get__desynched_atts(self): return self._desynched_atts
    def set__desynched_atts(self, _desynched_atts): self._desynched_atts = _desynched_atts
    def get_ComponentInstanceID(self): return self.ComponentInstanceID
    def set_ComponentInstanceID(self, ComponentInstanceID): self.ComponentInstanceID = ComponentInstanceID
    def get_FEAElementID(self): return self.FEAElementID
    def set_FEAElementID(self, FEAElementID): self.FEAElementID = FEAElementID
    def get__instances(self): return self._instances
    def set__instances(self, _instances): self._instances = _instances
    def get__archetype(self): return self._archetype
    def set__archetype(self, _archetype): self._archetype = _archetype
    def get__subtype(self): return self._subtype
    def set__subtype(self, _subtype): self._subtype = _subtype
    def get__id(self): return self._id
    def set__id(self, _id): self._id = _id
    def hasContent_(self):
        if (
            self.Material is not None or
            self.Metrics is not None
        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='ComponentType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='ComponentType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='ComponentType', pretty_print=pretty_print)
            showIndent(outfile, level, pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='ComponentType'):
        if self._derived is not None and '_derived' not in already_processed:
            already_processed.add('_derived')
            outfile.write(' _derived=%s' % (self.gds_format_string(quote_attrib(self._derived).encode(ExternalEncoding), input_name='_derived'), ))
        if self._real_archetype is not None and '_real_archetype' not in already_processed:
            already_processed.add('_real_archetype')
            outfile.write(' _real_archetype="%s"' % self.gds_format_boolean(self._real_archetype, input_name='_real_archetype'))
        if self._desynched_atts is not None and '_desynched_atts' not in already_processed:
            already_processed.add('_desynched_atts')
            outfile.write(' _desynched_atts=%s' % (self.gds_format_string(quote_attrib(self._desynched_atts).encode(ExternalEncoding), input_name='_desynched_atts'), ))
        if self.ComponentInstanceID is not None and 'ComponentInstanceID' not in already_processed:
            already_processed.add('ComponentInstanceID')
            outfile.write(' ComponentInstanceID=%s' % (self.gds_format_string(quote_attrib(self.ComponentInstanceID).encode(ExternalEncoding), input_name='ComponentInstanceID'), ))
        if self.FEAElementID is not None and 'FEAElementID' not in already_processed:
            already_processed.add('FEAElementID')
            outfile.write(' FEAElementID=%s' % (self.gds_format_string(quote_attrib(self.FEAElementID).encode(ExternalEncoding), input_name='FEAElementID'), ))
        if self._instances is not None and '_instances' not in already_processed:
            already_processed.add('_instances')
            outfile.write(' _instances=%s' % (self.gds_format_string(quote_attrib(self._instances).encode(ExternalEncoding), input_name='_instances'), ))
        if self._archetype is not None and '_archetype' not in already_processed:
            already_processed.add('_archetype')
            outfile.write(' _archetype=%s' % (self.gds_format_string(quote_attrib(self._archetype).encode(ExternalEncoding), input_name='_archetype'), ))
        if self._subtype is not None and '_subtype' not in already_processed:
            already_processed.add('_subtype')
            outfile.write(' _subtype="%s"' % self.gds_format_boolean(self._subtype, input_name='_subtype'))
        if self._id is not None and '_id' not in already_processed:
            already_processed.add('_id')
            outfile.write(' _id=%s' % (self.gds_format_string(quote_attrib(self._id).encode(ExternalEncoding), input_name='_id'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='ComponentType', fromsubclass_=False, pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.Material is not None:
            self.Material.export(outfile, level, namespace_, name_='Material', pretty_print=pretty_print)
        if self.Metrics is not None:
            self.Metrics.export(outfile, level, namespace_, name_='Metrics', pretty_print=pretty_print)
    def exportLiteral(self, outfile, level, name_='ComponentType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self._derived is not None and '_derived' not in already_processed:
            already_processed.add('_derived')
            showIndent(outfile, level)
            outfile.write('_derived="%s",\n' % (self._derived,))
        if self._real_archetype is not None and '_real_archetype' not in already_processed:
            already_processed.add('_real_archetype')
            showIndent(outfile, level)
            outfile.write('_real_archetype=%s,\n' % (self._real_archetype,))
        if self._desynched_atts is not None and '_desynched_atts' not in already_processed:
            already_processed.add('_desynched_atts')
            showIndent(outfile, level)
            outfile.write('_desynched_atts="%s",\n' % (self._desynched_atts,))
        if self.ComponentInstanceID is not None and 'ComponentInstanceID' not in already_processed:
            already_processed.add('ComponentInstanceID')
            showIndent(outfile, level)
            outfile.write('ComponentInstanceID="%s",\n' % (self.ComponentInstanceID,))
        if self.FEAElementID is not None and 'FEAElementID' not in already_processed:
            already_processed.add('FEAElementID')
            showIndent(outfile, level)
            outfile.write('FEAElementID="%s",\n' % (self.FEAElementID,))
        if self._instances is not None and '_instances' not in already_processed:
            already_processed.add('_instances')
            showIndent(outfile, level)
            outfile.write('_instances="%s",\n' % (self._instances,))
        if self._archetype is not None and '_archetype' not in already_processed:
            already_processed.add('_archetype')
            showIndent(outfile, level)
            outfile.write('_archetype="%s",\n' % (self._archetype,))
        if self._subtype is not None and '_subtype' not in already_processed:
            already_processed.add('_subtype')
            showIndent(outfile, level)
            outfile.write('_subtype=%s,\n' % (self._subtype,))
        if self._id is not None and '_id' not in already_processed:
            already_processed.add('_id')
            showIndent(outfile, level)
            outfile.write('_id="%s",\n' % (self._id,))
    def exportLiteralChildren(self, outfile, level, name_):
        if self.Material is not None:
            showIndent(outfile, level)
            outfile.write('Material=model_.MaterialType(\n')
            self.Material.exportLiteral(outfile, level, name_='Material')
            showIndent(outfile, level)
            outfile.write('),\n')
        if self.Metrics is not None:
            showIndent(outfile, level)
            outfile.write('Metrics=model_.MetricsType(\n')
            self.Metrics.exportLiteral(outfile, level, name_='Metrics')
            showIndent(outfile, level)
            outfile.write('),\n')
    def build(self, node):
        already_processed = set()
        self.buildAttributes(node, node.attrib, already_processed)
        for child in node:
            nodeName_ = Tag_pattern_.match(child.tag).groups()[-1]
            self.buildChildren(child, node, nodeName_)
        return self
    def buildAttributes(self, node, attrs, already_processed):
        value = find_attr_value_('_derived', node)
        if value is not None and '_derived' not in already_processed:
            already_processed.add('_derived')
            self._derived = value
        value = find_attr_value_('_real_archetype', node)
        if value is not None and '_real_archetype' not in already_processed:
            already_processed.add('_real_archetype')
            if value in ('true', '1'):
                self._real_archetype = True
            elif value in ('false', '0'):
                self._real_archetype = False
            else:
                raise_parse_error(node, 'Bad boolean attribute')
        value = find_attr_value_('_desynched_atts', node)
        if value is not None and '_desynched_atts' not in already_processed:
            already_processed.add('_desynched_atts')
            self._desynched_atts = value
        value = find_attr_value_('ComponentInstanceID', node)
        if value is not None and 'ComponentInstanceID' not in already_processed:
            already_processed.add('ComponentInstanceID')
            self.ComponentInstanceID = value
        value = find_attr_value_('FEAElementID', node)
        if value is not None and 'FEAElementID' not in already_processed:
            already_processed.add('FEAElementID')
            self.FEAElementID = value
        value = find_attr_value_('_instances', node)
        if value is not None and '_instances' not in already_processed:
            already_processed.add('_instances')
            self._instances = value
        value = find_attr_value_('_archetype', node)
        if value is not None and '_archetype' not in already_processed:
            already_processed.add('_archetype')
            self._archetype = value
        value = find_attr_value_('_subtype', node)
        if value is not None and '_subtype' not in already_processed:
            already_processed.add('_subtype')
            if value in ('true', '1'):
                self._subtype = True
            elif value in ('false', '0'):
                self._subtype = False
            else:
                raise_parse_error(node, 'Bad boolean attribute')
        value = find_attr_value_('_id', node)
        if value is not None and '_id' not in already_processed:
            already_processed.add('_id')
            self._id = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        if nodeName_ == 'Material':
            obj_ = MaterialType.factory()
            obj_.build(child_)
            self.Material = obj_
            obj_.original_tagname_ = 'Material'
        elif nodeName_ == 'Metrics':
            obj_ = MetricsType.factory()
            obj_.build(child_)
            self.Metrics = obj_
            obj_.original_tagname_ = 'Metrics'
# end class ComponentType


class ComponentsType(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, _derived=None, _real_archetype=None, _archetype=None, ConfigurationID=None, _subtype=None, _instances=None, _desynched_atts=None, _id=None, _libname=None, Component=None, Components=None):
        self.original_tagname_ = None
        self._derived = _cast(None, _derived)
        self._real_archetype = _cast(bool, _real_archetype)
        self._archetype = _cast(None, _archetype)
        self.ConfigurationID = _cast(None, ConfigurationID)
        self._subtype = _cast(bool, _subtype)
        self._instances = _cast(None, _instances)
        self._desynched_atts = _cast(None, _desynched_atts)
        self._id = _cast(None, _id)
        self._libname = _cast(None, _libname)
        if Component is None:
            self.Component = []
        else:
            self.Component = Component
        if Components is None:
            self.Components = []
        else:
            self.Components = Components
    def factory(*args_, **kwargs_):
        if ComponentsType.subclass:
            return ComponentsType.subclass(*args_, **kwargs_)
        else:
            return ComponentsType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Component(self): return self.Component
    def set_Component(self, Component): self.Component = Component
    def add_Component(self, value): self.Component.append(value)
    def insert_Component(self, index, value): self.Component[index] = value
    def get_Components(self): return self.Components
    def set_Components(self, Components): self.Components = Components
    def add_Components(self, value): self.Components.append(value)
    def insert_Components(self, index, value): self.Components[index] = value
    def get__derived(self): return self._derived
    def set__derived(self, _derived): self._derived = _derived
    def get__real_archetype(self): return self._real_archetype
    def set__real_archetype(self, _real_archetype): self._real_archetype = _real_archetype
    def get__archetype(self): return self._archetype
    def set__archetype(self, _archetype): self._archetype = _archetype
    def get_ConfigurationID(self): return self.ConfigurationID
    def set_ConfigurationID(self, ConfigurationID): self.ConfigurationID = ConfigurationID
    def get__subtype(self): return self._subtype
    def set__subtype(self, _subtype): self._subtype = _subtype
    def get__instances(self): return self._instances
    def set__instances(self, _instances): self._instances = _instances
    def get__desynched_atts(self): return self._desynched_atts
    def set__desynched_atts(self, _desynched_atts): self._desynched_atts = _desynched_atts
    def get__id(self): return self._id
    def set__id(self, _id): self._id = _id
    def get__libname(self): return self._libname
    def set__libname(self, _libname): self._libname = _libname
    def hasContent_(self):
        if (
            self.Component or
            self.Components
        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='ComponentsType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='ComponentsType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='ComponentsType', pretty_print=pretty_print)
            showIndent(outfile, level, pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='ComponentsType'):
        if self._derived is not None and '_derived' not in already_processed:
            already_processed.add('_derived')
            outfile.write(' _derived=%s' % (self.gds_format_string(quote_attrib(self._derived).encode(ExternalEncoding), input_name='_derived'), ))
        if self._real_archetype is not None and '_real_archetype' not in already_processed:
            already_processed.add('_real_archetype')
            outfile.write(' _real_archetype="%s"' % self.gds_format_boolean(self._real_archetype, input_name='_real_archetype'))
        if self._archetype is not None and '_archetype' not in already_processed:
            already_processed.add('_archetype')
            outfile.write(' _archetype=%s' % (self.gds_format_string(quote_attrib(self._archetype).encode(ExternalEncoding), input_name='_archetype'), ))
        if self.ConfigurationID is not None and 'ConfigurationID' not in already_processed:
            already_processed.add('ConfigurationID')
            outfile.write(' ConfigurationID=%s' % (self.gds_format_string(quote_attrib(self.ConfigurationID).encode(ExternalEncoding), input_name='ConfigurationID'), ))
        if self._subtype is not None and '_subtype' not in already_processed:
            already_processed.add('_subtype')
            outfile.write(' _subtype="%s"' % self.gds_format_boolean(self._subtype, input_name='_subtype'))
        if self._instances is not None and '_instances' not in already_processed:
            already_processed.add('_instances')
            outfile.write(' _instances=%s' % (self.gds_format_string(quote_attrib(self._instances).encode(ExternalEncoding), input_name='_instances'), ))
        if self._desynched_atts is not None and '_desynched_atts' not in already_processed:
            already_processed.add('_desynched_atts')
            outfile.write(' _desynched_atts=%s' % (self.gds_format_string(quote_attrib(self._desynched_atts).encode(ExternalEncoding), input_name='_desynched_atts'), ))
        if self._id is not None and '_id' not in already_processed:
            already_processed.add('_id')
            outfile.write(' _id=%s' % (self.gds_format_string(quote_attrib(self._id).encode(ExternalEncoding), input_name='_id'), ))
        if self._libname is not None and '_libname' not in already_processed:
            already_processed.add('_libname')
            outfile.write(' _libname=%s' % (self.gds_format_string(quote_attrib(self._libname).encode(ExternalEncoding), input_name='_libname'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='ComponentsType', fromsubclass_=False, pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        for Component_ in self.Component:
            Component_.export(outfile, level, namespace_, name_='Component', pretty_print=pretty_print)
        for Components_ in self.Components:
            Components_.export(outfile, level, namespace_, name_='Components', pretty_print=pretty_print)
    def exportLiteral(self, outfile, level, name_='ComponentsType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self._derived is not None and '_derived' not in already_processed:
            already_processed.add('_derived')
            showIndent(outfile, level)
            outfile.write('_derived="%s",\n' % (self._derived,))
        if self._real_archetype is not None and '_real_archetype' not in already_processed:
            already_processed.add('_real_archetype')
            showIndent(outfile, level)
            outfile.write('_real_archetype=%s,\n' % (self._real_archetype,))
        if self._archetype is not None and '_archetype' not in already_processed:
            already_processed.add('_archetype')
            showIndent(outfile, level)
            outfile.write('_archetype="%s",\n' % (self._archetype,))
        if self.ConfigurationID is not None and 'ConfigurationID' not in already_processed:
            already_processed.add('ConfigurationID')
            showIndent(outfile, level)
            outfile.write('ConfigurationID="%s",\n' % (self.ConfigurationID,))
        if self._subtype is not None and '_subtype' not in already_processed:
            already_processed.add('_subtype')
            showIndent(outfile, level)
            outfile.write('_subtype=%s,\n' % (self._subtype,))
        if self._instances is not None and '_instances' not in already_processed:
            already_processed.add('_instances')
            showIndent(outfile, level)
            outfile.write('_instances="%s",\n' % (self._instances,))
        if self._desynched_atts is not None and '_desynched_atts' not in already_processed:
            already_processed.add('_desynched_atts')
            showIndent(outfile, level)
            outfile.write('_desynched_atts="%s",\n' % (self._desynched_atts,))
        if self._id is not None and '_id' not in already_processed:
            already_processed.add('_id')
            showIndent(outfile, level)
            outfile.write('_id="%s",\n' % (self._id,))
        if self._libname is not None and '_libname' not in already_processed:
            already_processed.add('_libname')
            showIndent(outfile, level)
            outfile.write('_libname="%s",\n' % (self._libname,))
    def exportLiteralChildren(self, outfile, level, name_):
        showIndent(outfile, level)
        outfile.write('Component=[\n')
        level += 1
        for Component_ in self.Component:
            showIndent(outfile, level)
            outfile.write('model_.ComponentType(\n')
            Component_.exportLiteral(outfile, level, name_='ComponentType')
            showIndent(outfile, level)
            outfile.write('),\n')
        level -= 1
        showIndent(outfile, level)
        outfile.write('],\n')
        showIndent(outfile, level)
        outfile.write('Components=[\n')
        level += 1
        for Components_ in self.Components:
            showIndent(outfile, level)
            outfile.write('model_.ComponentsType(\n')
            Components_.exportLiteral(outfile, level, name_='ComponentsType')
            showIndent(outfile, level)
            outfile.write('),\n')
        level -= 1
        showIndent(outfile, level)
        outfile.write('],\n')
    def build(self, node):
        already_processed = set()
        self.buildAttributes(node, node.attrib, already_processed)
        for child in node:
            nodeName_ = Tag_pattern_.match(child.tag).groups()[-1]
            self.buildChildren(child, node, nodeName_)
        return self
    def buildAttributes(self, node, attrs, already_processed):
        value = find_attr_value_('_derived', node)
        if value is not None and '_derived' not in already_processed:
            already_processed.add('_derived')
            self._derived = value
        value = find_attr_value_('_real_archetype', node)
        if value is not None and '_real_archetype' not in already_processed:
            already_processed.add('_real_archetype')
            if value in ('true', '1'):
                self._real_archetype = True
            elif value in ('false', '0'):
                self._real_archetype = False
            else:
                raise_parse_error(node, 'Bad boolean attribute')
        value = find_attr_value_('_archetype', node)
        if value is not None and '_archetype' not in already_processed:
            already_processed.add('_archetype')
            self._archetype = value
        value = find_attr_value_('ConfigurationID', node)
        if value is not None and 'ConfigurationID' not in already_processed:
            already_processed.add('ConfigurationID')
            self.ConfigurationID = value
        value = find_attr_value_('_subtype', node)
        if value is not None and '_subtype' not in already_processed:
            already_processed.add('_subtype')
            if value in ('true', '1'):
                self._subtype = True
            elif value in ('false', '0'):
                self._subtype = False
            else:
                raise_parse_error(node, 'Bad boolean attribute')
        value = find_attr_value_('_instances', node)
        if value is not None and '_instances' not in already_processed:
            already_processed.add('_instances')
            self._instances = value
        value = find_attr_value_('_desynched_atts', node)
        if value is not None and '_desynched_atts' not in already_processed:
            already_processed.add('_desynched_atts')
            self._desynched_atts = value
        value = find_attr_value_('_id', node)
        if value is not None and '_id' not in already_processed:
            already_processed.add('_id')
            self._id = value
        value = find_attr_value_('_libname', node)
        if value is not None and '_libname' not in already_processed:
            already_processed.add('_libname')
            self._libname = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        if nodeName_ == 'Component':
            obj_ = ComponentType.factory()
            obj_.build(child_)
            self.Component.append(obj_)
            obj_.original_tagname_ = 'Component'
        elif nodeName_ == 'Components':
            obj_ = ComponentsType.factory()
            obj_.build(child_)
            self.Components.append(obj_)
            obj_.original_tagname_ = 'Components'
# end class ComponentsType


GDSClassesMapping = {
    'ComplexMetric': ComplexMetricType,
    'Metric': MetricType,
    'Material': MaterialType,
    'Component': ComponentType,
    'Metrics': MetricsType,
    'Components': ComponentsType,
}


USAGE_TEXT = """
Usage: python <Parser>.py [ -s ] <in_xml_file>
"""


def usage():
    print USAGE_TEXT
    sys.exit(1)


def get_root_tag(node):
    tag = Tag_pattern_.match(node.tag).groups()[-1]
    rootClass = GDSClassesMapping.get(tag)
    if rootClass is None:
        rootClass = globals().get(tag)
    return tag, rootClass


def parse(inFileName, silence=False):
    doc = parsexml_(inFileName)
    rootNode = doc.getroot()
    rootTag, rootClass = get_root_tag(rootNode)
    if rootClass is None:
        rootTag = 'ComplexMetricType'
        rootClass = ComplexMetricType
    rootObj = rootClass.factory()
    rootObj.build(rootNode)
    # Enable Python to collect the space used by the DOM.
    doc = None
    if not silence:
        sys.stdout.write('<?xml version="1.0" ?>\n')
        rootObj.export(
            sys.stdout, 0, name_=rootTag,
            namespacedef_='',
            pretty_print=True)
    return rootObj


def parseEtree(inFileName, silence=False):
    doc = parsexml_(inFileName)
    rootNode = doc.getroot()
    rootTag, rootClass = get_root_tag(rootNode)
    if rootClass is None:
        rootTag = 'ComplexMetricType'
        rootClass = ComplexMetricType
    rootObj = rootClass.factory()
    rootObj.build(rootNode)
    # Enable Python to collect the space used by the DOM.
    doc = None
    mapping = {}
    rootElement = rootObj.to_etree(None, name_=rootTag, mapping_=mapping)
    reverse_mapping = rootObj.gds_reverse_node_mapping(mapping)
    if not silence:
        content = etree_.tostring(
            rootElement, pretty_print=True,
            xml_declaration=True, encoding="utf-8")
        sys.stdout.write(content)
        sys.stdout.write('\n')
    return rootObj, rootElement, mapping, reverse_mapping


def parseString(inString, silence=False):
    from StringIO import StringIO
    doc = parsexml_(StringIO(inString))
    rootNode = doc.getroot()
    rootTag, rootClass = get_root_tag(rootNode)
    if rootClass is None:
        rootTag = 'ComplexMetricType'
        rootClass = ComplexMetricType
    rootObj = rootClass.factory()
    rootObj.build(rootNode)
    # Enable Python to collect the space used by the DOM.
    doc = None
    if not silence:
        sys.stdout.write('<?xml version="1.0" ?>\n')
        rootObj.export(
            sys.stdout, 0, name_=rootTag,
            namespacedef_='')
    return rootObj


def parseLiteral(inFileName, silence=False):
    doc = parsexml_(inFileName)
    rootNode = doc.getroot()
    rootTag, rootClass = get_root_tag(rootNode)
    if rootClass is None:
        rootTag = 'ComplexMetricType'
        rootClass = ComplexMetricType
    rootObj = rootClass.factory()
    rootObj.build(rootNode)
    # Enable Python to collect the space used by the DOM.
    doc = None
    if not silence:
        sys.stdout.write('#from a import *\n\n')
        sys.stdout.write('import a as model_\n\n')
        sys.stdout.write('rootObj = model_.rootClass(\n')
        rootObj.exportLiteral(sys.stdout, 0, name_=rootTag)
        sys.stdout.write(')\n')
    return rootObj


def main():
    args = sys.argv[1:]
    if len(args) == 1:
        parse(args[0])
    else:
        usage()


if __name__ == '__main__':
    #import pdb; pdb.set_trace()
    main()


__all__ = [
    "ComplexMetricType",
    "ComponentType",
    "ComponentsType",
    "MaterialType",
    "MetricType",
    "MetricsType"
]
