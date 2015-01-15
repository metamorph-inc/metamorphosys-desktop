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
# Generated Tue Jul 29 15:19:46 2014 by generateDS.py version 2.12d.
#
# Command line options:
#   ('-o', 'aa')
#
# Command line arguments:
#   ..\..\..\..\..\src\CyPhy2CAD_CSharp\OutputFormats\Analysis_MBD.xsd
#
# Command line:
#   C:\Users\snyako.ISIS\Desktop\generateDS-2.12d\generateDS.py -o "aa" ..\..\..\..\..\src\CyPhy2CAD_CSharp\OutputFormats\Analysis_MBD.xsd
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


class Model(GeneratedsSuper):
    """Model that contains information needed to create a multibody dynamic
    simulation model, where geometric information is referenced
    only."""
    subclass = None
    superclass = None
    def __init__(self, Name=None, Script=None, Contact=None, Terrain=None, Ground=None, Units=None, Assembly=None, Loads=None, Simulation=None, Results=None):
        self.original_tagname_ = None
        self.Name = _cast(None, Name)
        if Script is None:
            self.Script = []
        else:
            self.Script = Script
        if Contact is None:
            self.Contact = []
        else:
            self.Contact = Contact
        self.Terrain = Terrain
        self.Ground = Ground
        self.Units = Units
        self.Assembly = Assembly
        self.Loads = Loads
        self.Simulation = Simulation
        self.Results = Results
    def factory(*args_, **kwargs_):
        if Model.subclass:
            return Model.subclass(*args_, **kwargs_)
        else:
            return Model(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Script(self): return self.Script
    def set_Script(self, Script): self.Script = Script
    def add_Script(self, value): self.Script.append(value)
    def insert_Script(self, index, value): self.Script[index] = value
    def get_Contact(self): return self.Contact
    def set_Contact(self, Contact): self.Contact = Contact
    def add_Contact(self, value): self.Contact.append(value)
    def insert_Contact(self, index, value): self.Contact[index] = value
    def get_Terrain(self): return self.Terrain
    def set_Terrain(self, Terrain): self.Terrain = Terrain
    def get_Ground(self): return self.Ground
    def set_Ground(self, Ground): self.Ground = Ground
    def get_Units(self): return self.Units
    def set_Units(self, Units): self.Units = Units
    def get_Assembly(self): return self.Assembly
    def set_Assembly(self, Assembly): self.Assembly = Assembly
    def get_Loads(self): return self.Loads
    def set_Loads(self, Loads): self.Loads = Loads
    def get_Simulation(self): return self.Simulation
    def set_Simulation(self, Simulation): self.Simulation = Simulation
    def get_Results(self): return self.Results
    def set_Results(self, Results): self.Results = Results
    def get_Name(self): return self.Name
    def set_Name(self, Name): self.Name = Name
    def hasContent_(self):
        if (
            self.Script or
            self.Contact or
            self.Terrain is not None or
            self.Ground is not None or
            self.Units is not None or
            self.Assembly is not None or
            self.Loads is not None or
            self.Simulation is not None or
            self.Results is not None
        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='Model', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='Model')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='Model', pretty_print=pretty_print)
            showIndent(outfile, level, pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='Model'):
        if self.Name is not None and 'Name' not in already_processed:
            already_processed.add('Name')
            outfile.write(' Name=%s' % (self.gds_format_string(quote_attrib(self.Name).encode(ExternalEncoding), input_name='Name'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='Model', fromsubclass_=False, pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        for Script_ in self.Script:
            Script_.export(outfile, level, namespace_, name_='Script', pretty_print=pretty_print)
        for Contact_ in self.Contact:
            Contact_.export(outfile, level, namespace_, name_='Contact', pretty_print=pretty_print)
        if self.Terrain is not None:
            self.Terrain.export(outfile, level, namespace_, name_='Terrain', pretty_print=pretty_print)
        if self.Ground is not None:
            self.Ground.export(outfile, level, namespace_, name_='Ground', pretty_print=pretty_print)
        if self.Units is not None:
            self.Units.export(outfile, level, namespace_, name_='Units', pretty_print=pretty_print)
        if self.Assembly is not None:
            self.Assembly.export(outfile, level, namespace_, name_='Assembly', pretty_print=pretty_print)
        if self.Loads is not None:
            self.Loads.export(outfile, level, namespace_, name_='Loads', pretty_print=pretty_print)
        if self.Simulation is not None:
            self.Simulation.export(outfile, level, namespace_, name_='Simulation', pretty_print=pretty_print)
        if self.Results is not None:
            self.Results.export(outfile, level, namespace_, name_='Results', pretty_print=pretty_print)
    def exportLiteral(self, outfile, level, name_='Model'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.Name is not None and 'Name' not in already_processed:
            already_processed.add('Name')
            showIndent(outfile, level)
            outfile.write('Name="%s",\n' % (self.Name,))
    def exportLiteralChildren(self, outfile, level, name_):
        showIndent(outfile, level)
        outfile.write('Script=[\n')
        level += 1
        for Script_ in self.Script:
            showIndent(outfile, level)
            outfile.write('model_.ScriptType(\n')
            Script_.exportLiteral(outfile, level, name_='ScriptType')
            showIndent(outfile, level)
            outfile.write('),\n')
        level -= 1
        showIndent(outfile, level)
        outfile.write('],\n')
        showIndent(outfile, level)
        outfile.write('Contact=[\n')
        level += 1
        for Contact_ in self.Contact:
            showIndent(outfile, level)
            outfile.write('model_.ContactType(\n')
            Contact_.exportLiteral(outfile, level, name_='ContactType')
            showIndent(outfile, level)
            outfile.write('),\n')
        level -= 1
        showIndent(outfile, level)
        outfile.write('],\n')
        if self.Terrain is not None:
            showIndent(outfile, level)
            outfile.write('Terrain=model_.TerrainType(\n')
            self.Terrain.exportLiteral(outfile, level, name_='Terrain')
            showIndent(outfile, level)
            outfile.write('),\n')
        if self.Ground is not None:
            showIndent(outfile, level)
            outfile.write('Ground=model_.GroundType(\n')
            self.Ground.exportLiteral(outfile, level, name_='Ground')
            showIndent(outfile, level)
            outfile.write('),\n')
        if self.Units is not None:
            showIndent(outfile, level)
            outfile.write('Units=model_.UnitsType(\n')
            self.Units.exportLiteral(outfile, level, name_='Units')
            showIndent(outfile, level)
            outfile.write('),\n')
        if self.Assembly is not None:
            showIndent(outfile, level)
            outfile.write('Assembly=model_.AssemblyType(\n')
            self.Assembly.exportLiteral(outfile, level, name_='Assembly')
            showIndent(outfile, level)
            outfile.write('),\n')
        if self.Loads is not None:
            showIndent(outfile, level)
            outfile.write('Loads=model_.LoadsType(\n')
            self.Loads.exportLiteral(outfile, level, name_='Loads')
            showIndent(outfile, level)
            outfile.write('),\n')
        if self.Simulation is not None:
            showIndent(outfile, level)
            outfile.write('Simulation=model_.SimulationType(\n')
            self.Simulation.exportLiteral(outfile, level, name_='Simulation')
            showIndent(outfile, level)
            outfile.write('),\n')
        if self.Results is not None:
            showIndent(outfile, level)
            outfile.write('Results=model_.ResultsType(\n')
            self.Results.exportLiteral(outfile, level, name_='Results')
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
        value = find_attr_value_('Name', node)
        if value is not None and 'Name' not in already_processed:
            already_processed.add('Name')
            self.Name = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        if nodeName_ == 'Script':
            obj_ = ScriptType.factory()
            obj_.build(child_)
            self.Script.append(obj_)
            obj_.original_tagname_ = 'Script'
        elif nodeName_ == 'Contact':
            obj_ = ContactType.factory()
            obj_.build(child_)
            self.Contact.append(obj_)
            obj_.original_tagname_ = 'Contact'
        elif nodeName_ == 'Terrain':
            obj_ = TerrainType.factory()
            obj_.build(child_)
            self.Terrain = obj_
            obj_.original_tagname_ = 'Terrain'
        elif nodeName_ == 'Ground':
            obj_ = GroundType.factory()
            obj_.build(child_)
            self.Ground = obj_
            obj_.original_tagname_ = 'Ground'
        elif nodeName_ == 'Units':
            obj_ = UnitsType.factory()
            obj_.build(child_)
            self.Units = obj_
            obj_.original_tagname_ = 'Units'
        elif nodeName_ == 'Assembly':
            obj_ = AssemblyType.factory()
            obj_.build(child_)
            self.Assembly = obj_
            obj_.original_tagname_ = 'Assembly'
        elif nodeName_ == 'Loads':
            obj_ = LoadsType.factory()
            obj_.build(child_)
            self.Loads = obj_
            obj_.original_tagname_ = 'Loads'
        elif nodeName_ == 'Simulation':
            obj_ = SimulationType.factory()
            obj_.build(child_)
            self.Simulation = obj_
            obj_.original_tagname_ = 'Simulation'
        elif nodeName_ == 'Results':
            obj_ = ResultsType.factory()
            obj_.build(child_)
            self.Results = obj_
            obj_.original_tagname_ = 'Results'
# end class Model


class ScriptType(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, Path=None):
        self.original_tagname_ = None
        self.Path = _cast(None, Path)
    def factory(*args_, **kwargs_):
        if ScriptType.subclass:
            return ScriptType.subclass(*args_, **kwargs_)
        else:
            return ScriptType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Path(self): return self.Path
    def set_Path(self, Path): self.Path = Path
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='ScriptType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='ScriptType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='ScriptType', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='ScriptType'):
        if self.Path is not None and 'Path' not in already_processed:
            already_processed.add('Path')
            outfile.write(' Path=%s' % (self.gds_format_string(quote_attrib(self.Path).encode(ExternalEncoding), input_name='Path'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='ScriptType', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='ScriptType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.Path is not None and 'Path' not in already_processed:
            already_processed.add('Path')
            showIndent(outfile, level)
            outfile.write('Path="%s",\n' % (self.Path,))
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
        value = find_attr_value_('Path', node)
        if value is not None and 'Path' not in already_processed:
            already_processed.add('Path')
            self.Path = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class ScriptType


class ContactType(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, CyphyId2=None, CyphyId1=None):
        self.original_tagname_ = None
        self.CyphyId2 = _cast(None, CyphyId2)
        self.CyphyId1 = _cast(None, CyphyId1)
    def factory(*args_, **kwargs_):
        if ContactType.subclass:
            return ContactType.subclass(*args_, **kwargs_)
        else:
            return ContactType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_CyphyId2(self): return self.CyphyId2
    def set_CyphyId2(self, CyphyId2): self.CyphyId2 = CyphyId2
    def get_CyphyId1(self): return self.CyphyId1
    def set_CyphyId1(self, CyphyId1): self.CyphyId1 = CyphyId1
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='ContactType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='ContactType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='ContactType', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='ContactType'):
        if self.CyphyId2 is not None and 'CyphyId2' not in already_processed:
            already_processed.add('CyphyId2')
            outfile.write(' CyphyId2=%s' % (self.gds_format_string(quote_attrib(self.CyphyId2).encode(ExternalEncoding), input_name='CyphyId2'), ))
        if self.CyphyId1 is not None and 'CyphyId1' not in already_processed:
            already_processed.add('CyphyId1')
            outfile.write(' CyphyId1=%s' % (self.gds_format_string(quote_attrib(self.CyphyId1).encode(ExternalEncoding), input_name='CyphyId1'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='ContactType', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='ContactType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.CyphyId2 is not None and 'CyphyId2' not in already_processed:
            already_processed.add('CyphyId2')
            showIndent(outfile, level)
            outfile.write('CyphyId2="%s",\n' % (self.CyphyId2,))
        if self.CyphyId1 is not None and 'CyphyId1' not in already_processed:
            already_processed.add('CyphyId1')
            showIndent(outfile, level)
            outfile.write('CyphyId1="%s",\n' % (self.CyphyId1,))
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
        value = find_attr_value_('CyphyId2', node)
        if value is not None and 'CyphyId2' not in already_processed:
            already_processed.add('CyphyId2')
            self.CyphyId2 = value
        value = find_attr_value_('CyphyId1', node)
        if value is not None and 'CyphyId1' not in already_processed:
            already_processed.add('CyphyId1')
            self.CyphyId1 = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class ContactType


class TerrainType(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, FileName=None):
        self.original_tagname_ = None
        self.FileName = _cast(None, FileName)
    def factory(*args_, **kwargs_):
        if TerrainType.subclass:
            return TerrainType.subclass(*args_, **kwargs_)
        else:
            return TerrainType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_FileName(self): return self.FileName
    def set_FileName(self, FileName): self.FileName = FileName
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='TerrainType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='TerrainType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='TerrainType', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='TerrainType'):
        if self.FileName is not None and 'FileName' not in already_processed:
            already_processed.add('FileName')
            outfile.write(' FileName=%s' % (self.gds_format_string(quote_attrib(self.FileName).encode(ExternalEncoding), input_name='FileName'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='TerrainType', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='TerrainType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.FileName is not None and 'FileName' not in already_processed:
            already_processed.add('FileName')
            showIndent(outfile, level)
            outfile.write('FileName="%s",\n' % (self.FileName,))
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
        value = find_attr_value_('FileName', node)
        if value is not None and 'FileName' not in already_processed:
            already_processed.add('FileName')
            self.FileName = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class TerrainType


class GroundType(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, CyphyId=None):
        self.original_tagname_ = None
        self.CyphyId = _cast(None, CyphyId)
    def factory(*args_, **kwargs_):
        if GroundType.subclass:
            return GroundType.subclass(*args_, **kwargs_)
        else:
            return GroundType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_CyphyId(self): return self.CyphyId
    def set_CyphyId(self, CyphyId): self.CyphyId = CyphyId
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='GroundType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='GroundType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='GroundType', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='GroundType'):
        if self.CyphyId is not None and 'CyphyId' not in already_processed:
            already_processed.add('CyphyId')
            outfile.write(' CyphyId=%s' % (self.gds_format_string(quote_attrib(self.CyphyId).encode(ExternalEncoding), input_name='CyphyId'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='GroundType', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='GroundType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.CyphyId is not None and 'CyphyId' not in already_processed:
            already_processed.add('CyphyId')
            showIndent(outfile, level)
            outfile.write('CyphyId="%s",\n' % (self.CyphyId,))
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
        value = find_attr_value_('CyphyId', node)
        if value is not None and 'CyphyId' not in already_processed:
            already_processed.add('CyphyId')
            self.CyphyId = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class GroundType


class UnitsType(GeneratedsSuper):
    """mmdeg/radnewtonkgsec"""
    subclass = None
    superclass = None
    def __init__(self, Mass=None, Force=None, Length=None, Angle=None, Time=None):
        self.original_tagname_ = None
        self.Mass = _cast(None, Mass)
        self.Force = _cast(None, Force)
        self.Length = _cast(None, Length)
        self.Angle = _cast(None, Angle)
        self.Time = _cast(None, Time)
    def factory(*args_, **kwargs_):
        if UnitsType.subclass:
            return UnitsType.subclass(*args_, **kwargs_)
        else:
            return UnitsType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Mass(self): return self.Mass
    def set_Mass(self, Mass): self.Mass = Mass
    def get_Force(self): return self.Force
    def set_Force(self, Force): self.Force = Force
    def get_Length(self): return self.Length
    def set_Length(self, Length): self.Length = Length
    def get_Angle(self): return self.Angle
    def set_Angle(self, Angle): self.Angle = Angle
    def get_Time(self): return self.Time
    def set_Time(self, Time): self.Time = Time
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='UnitsType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='UnitsType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='UnitsType', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='UnitsType'):
        if self.Mass is not None and 'Mass' not in already_processed:
            already_processed.add('Mass')
            outfile.write(' Mass=%s' % (self.gds_format_string(quote_attrib(self.Mass).encode(ExternalEncoding), input_name='Mass'), ))
        if self.Force is not None and 'Force' not in already_processed:
            already_processed.add('Force')
            outfile.write(' Force=%s' % (self.gds_format_string(quote_attrib(self.Force).encode(ExternalEncoding), input_name='Force'), ))
        if self.Length is not None and 'Length' not in already_processed:
            already_processed.add('Length')
            outfile.write(' Length=%s' % (self.gds_format_string(quote_attrib(self.Length).encode(ExternalEncoding), input_name='Length'), ))
        if self.Angle is not None and 'Angle' not in already_processed:
            already_processed.add('Angle')
            outfile.write(' Angle=%s' % (self.gds_format_string(quote_attrib(self.Angle).encode(ExternalEncoding), input_name='Angle'), ))
        if self.Time is not None and 'Time' not in already_processed:
            already_processed.add('Time')
            outfile.write(' Time=%s' % (self.gds_format_string(quote_attrib(self.Time).encode(ExternalEncoding), input_name='Time'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='UnitsType', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='UnitsType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.Mass is not None and 'Mass' not in already_processed:
            already_processed.add('Mass')
            showIndent(outfile, level)
            outfile.write('Mass="%s",\n' % (self.Mass,))
        if self.Force is not None and 'Force' not in already_processed:
            already_processed.add('Force')
            showIndent(outfile, level)
            outfile.write('Force="%s",\n' % (self.Force,))
        if self.Length is not None and 'Length' not in already_processed:
            already_processed.add('Length')
            showIndent(outfile, level)
            outfile.write('Length="%s",\n' % (self.Length,))
        if self.Angle is not None and 'Angle' not in already_processed:
            already_processed.add('Angle')
            showIndent(outfile, level)
            outfile.write('Angle="%s",\n' % (self.Angle,))
        if self.Time is not None and 'Time' not in already_processed:
            already_processed.add('Time')
            showIndent(outfile, level)
            outfile.write('Time="%s",\n' % (self.Time,))
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
        value = find_attr_value_('Mass', node)
        if value is not None and 'Mass' not in already_processed:
            already_processed.add('Mass')
            self.Mass = value
        value = find_attr_value_('Force', node)
        if value is not None and 'Force' not in already_processed:
            already_processed.add('Force')
            self.Force = value
        value = find_attr_value_('Length', node)
        if value is not None and 'Length' not in already_processed:
            already_processed.add('Length')
            self.Length = value
        value = find_attr_value_('Angle', node)
        if value is not None and 'Angle' not in already_processed:
            already_processed.add('Angle')
            self.Angle = value
        value = find_attr_value_('Time', node)
        if value is not None and 'Time' not in already_processed:
            already_processed.add('Time')
            self.Time = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class UnitsType


class AssemblyType(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, Joints=None):
        self.original_tagname_ = None
        self.Joints = Joints
    def factory(*args_, **kwargs_):
        if AssemblyType.subclass:
            return AssemblyType.subclass(*args_, **kwargs_)
        else:
            return AssemblyType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Joints(self): return self.Joints
    def set_Joints(self, Joints): self.Joints = Joints
    def hasContent_(self):
        if (
            self.Joints is not None
        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='AssemblyType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='AssemblyType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='AssemblyType', pretty_print=pretty_print)
            showIndent(outfile, level, pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='AssemblyType'):
        pass
    def exportChildren(self, outfile, level, namespace_='', name_='AssemblyType', fromsubclass_=False, pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.Joints is not None:
            self.Joints.export(outfile, level, namespace_, name_='Joints', pretty_print=pretty_print)
    def exportLiteral(self, outfile, level, name_='AssemblyType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        pass
    def exportLiteralChildren(self, outfile, level, name_):
        if self.Joints is not None:
            showIndent(outfile, level)
            outfile.write('Joints=model_.JointsType(\n')
            self.Joints.exportLiteral(outfile, level, name_='Joints')
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
        pass
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        if nodeName_ == 'Joints':
            obj_ = JointsType.factory()
            obj_.build(child_)
            self.Joints = obj_
            obj_.original_tagname_ = 'Joints'
# end class AssemblyType


class JointsType(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, Joint_Locations_Only=None, Joint_One_Axis_Given=None):
        self.original_tagname_ = None
        if Joint_Locations_Only is None:
            self.Joint_Locations_Only = []
        else:
            self.Joint_Locations_Only = Joint_Locations_Only
        if Joint_One_Axis_Given is None:
            self.Joint_One_Axis_Given = []
        else:
            self.Joint_One_Axis_Given = Joint_One_Axis_Given
    def factory(*args_, **kwargs_):
        if JointsType.subclass:
            return JointsType.subclass(*args_, **kwargs_)
        else:
            return JointsType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Joint_Locations_Only(self): return self.Joint_Locations_Only
    def set_Joint_Locations_Only(self, Joint_Locations_Only): self.Joint_Locations_Only = Joint_Locations_Only
    def add_Joint_Locations_Only(self, value): self.Joint_Locations_Only.append(value)
    def insert_Joint_Locations_Only(self, index, value): self.Joint_Locations_Only[index] = value
    def get_Joint_One_Axis_Given(self): return self.Joint_One_Axis_Given
    def set_Joint_One_Axis_Given(self, Joint_One_Axis_Given): self.Joint_One_Axis_Given = Joint_One_Axis_Given
    def add_Joint_One_Axis_Given(self, value): self.Joint_One_Axis_Given.append(value)
    def insert_Joint_One_Axis_Given(self, index, value): self.Joint_One_Axis_Given[index] = value
    def hasContent_(self):
        if (
            self.Joint_Locations_Only or
            self.Joint_One_Axis_Given
        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='JointsType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='JointsType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='JointsType', pretty_print=pretty_print)
            showIndent(outfile, level, pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='JointsType'):
        pass
    def exportChildren(self, outfile, level, namespace_='', name_='JointsType', fromsubclass_=False, pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        for Joint_Locations_Only_ in self.Joint_Locations_Only:
            Joint_Locations_Only_.export(outfile, level, namespace_, name_='Joint_Locations_Only', pretty_print=pretty_print)
        for Joint_One_Axis_Given_ in self.Joint_One_Axis_Given:
            Joint_One_Axis_Given_.export(outfile, level, namespace_, name_='Joint_One_Axis_Given', pretty_print=pretty_print)
    def exportLiteral(self, outfile, level, name_='JointsType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        pass
    def exportLiteralChildren(self, outfile, level, name_):
        showIndent(outfile, level)
        outfile.write('Joint_Locations_Only=[\n')
        level += 1
        for Joint_Locations_Only_ in self.Joint_Locations_Only:
            showIndent(outfile, level)
            outfile.write('model_.Joint_Locations_OnlyType(\n')
            Joint_Locations_Only_.exportLiteral(outfile, level, name_='Joint_Locations_OnlyType')
            showIndent(outfile, level)
            outfile.write('),\n')
        level -= 1
        showIndent(outfile, level)
        outfile.write('],\n')
        showIndent(outfile, level)
        outfile.write('Joint_One_Axis_Given=[\n')
        level += 1
        for Joint_One_Axis_Given_ in self.Joint_One_Axis_Given:
            showIndent(outfile, level)
            outfile.write('model_.Joint_One_Axis_GivenType(\n')
            Joint_One_Axis_Given_.exportLiteral(outfile, level, name_='Joint_One_Axis_GivenType')
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
        pass
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        if nodeName_ == 'Joint_Locations_Only':
            obj_ = Joint_Locations_OnlyType.factory()
            obj_.build(child_)
            self.Joint_Locations_Only.append(obj_)
            obj_.original_tagname_ = 'Joint_Locations_Only'
        elif nodeName_ == 'Joint_One_Axis_Given':
            obj_ = Joint_One_Axis_GivenType.factory()
            obj_.build(child_)
            self.Joint_One_Axis_Given.append(obj_)
            obj_.original_tagname_ = 'Joint_One_Axis_Given'
# end class JointsType


class Joint_Locations_OnlyType(GeneratedsSuper):
    """Fixed/Spherical"""
    subclass = None
    superclass = None
    def __init__(self, Type=None, ID=None, ComponentA=None, ComponentB=None):
        self.original_tagname_ = None
        self.Type = _cast(None, Type)
        self.ID = _cast(None, ID)
        self.ComponentA = ComponentA
        self.ComponentB = ComponentB
    def factory(*args_, **kwargs_):
        if Joint_Locations_OnlyType.subclass:
            return Joint_Locations_OnlyType.subclass(*args_, **kwargs_)
        else:
            return Joint_Locations_OnlyType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_ComponentA(self): return self.ComponentA
    def set_ComponentA(self, ComponentA): self.ComponentA = ComponentA
    def get_ComponentB(self): return self.ComponentB
    def set_ComponentB(self, ComponentB): self.ComponentB = ComponentB
    def get_Type(self): return self.Type
    def set_Type(self, Type): self.Type = Type
    def get_ID(self): return self.ID
    def set_ID(self, ID): self.ID = ID
    def hasContent_(self):
        if (
            self.ComponentA is not None or
            self.ComponentB is not None
        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='Joint_Locations_OnlyType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='Joint_Locations_OnlyType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='Joint_Locations_OnlyType', pretty_print=pretty_print)
            showIndent(outfile, level, pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='Joint_Locations_OnlyType'):
        if self.Type is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            outfile.write(' Type=%s' % (self.gds_format_string(quote_attrib(self.Type).encode(ExternalEncoding), input_name='Type'), ))
        if self.ID is not None and 'ID' not in already_processed:
            already_processed.add('ID')
            outfile.write(' ID=%s' % (self.gds_format_string(quote_attrib(self.ID).encode(ExternalEncoding), input_name='ID'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='Joint_Locations_OnlyType', fromsubclass_=False, pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.ComponentA is not None:
            self.ComponentA.export(outfile, level, namespace_, name_='ComponentA', pretty_print=pretty_print)
        if self.ComponentB is not None:
            self.ComponentB.export(outfile, level, namespace_, name_='ComponentB', pretty_print=pretty_print)
    def exportLiteral(self, outfile, level, name_='Joint_Locations_OnlyType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.Type is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            showIndent(outfile, level)
            outfile.write('Type="%s",\n' % (self.Type,))
        if self.ID is not None and 'ID' not in already_processed:
            already_processed.add('ID')
            showIndent(outfile, level)
            outfile.write('ID="%s",\n' % (self.ID,))
    def exportLiteralChildren(self, outfile, level, name_):
        if self.ComponentA is not None:
            showIndent(outfile, level)
            outfile.write('ComponentA=model_.ComponentAType(\n')
            self.ComponentA.exportLiteral(outfile, level, name_='ComponentA')
            showIndent(outfile, level)
            outfile.write('),\n')
        if self.ComponentB is not None:
            showIndent(outfile, level)
            outfile.write('ComponentB=model_.ComponentBType(\n')
            self.ComponentB.exportLiteral(outfile, level, name_='ComponentB')
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
        value = find_attr_value_('Type', node)
        if value is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            self.Type = value
        value = find_attr_value_('ID', node)
        if value is not None and 'ID' not in already_processed:
            already_processed.add('ID')
            self.ID = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        if nodeName_ == 'ComponentA':
            obj_ = ComponentAType.factory()
            obj_.build(child_)
            self.ComponentA = obj_
            obj_.original_tagname_ = 'ComponentA'
        elif nodeName_ == 'ComponentB':
            obj_ = ComponentBType.factory()
            obj_.build(child_)
            self.ComponentB = obj_
            obj_.original_tagname_ = 'ComponentB'
# end class Joint_Locations_OnlyType


class ComponentAType(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, Location_MetricID=None, ComponentID=None):
        self.original_tagname_ = None
        self.Location_MetricID = _cast(None, Location_MetricID)
        self.ComponentID = _cast(None, ComponentID)
    def factory(*args_, **kwargs_):
        if ComponentAType.subclass:
            return ComponentAType.subclass(*args_, **kwargs_)
        else:
            return ComponentAType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Location_MetricID(self): return self.Location_MetricID
    def set_Location_MetricID(self, Location_MetricID): self.Location_MetricID = Location_MetricID
    def get_ComponentID(self): return self.ComponentID
    def set_ComponentID(self, ComponentID): self.ComponentID = ComponentID
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='ComponentAType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='ComponentAType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='ComponentAType', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='ComponentAType'):
        if self.Location_MetricID is not None and 'Location_MetricID' not in already_processed:
            already_processed.add('Location_MetricID')
            outfile.write(' Location_MetricID=%s' % (self.gds_format_string(quote_attrib(self.Location_MetricID).encode(ExternalEncoding), input_name='Location_MetricID'), ))
        if self.ComponentID is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            outfile.write(' ComponentID=%s' % (self.gds_format_string(quote_attrib(self.ComponentID).encode(ExternalEncoding), input_name='ComponentID'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='ComponentAType', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='ComponentAType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.Location_MetricID is not None and 'Location_MetricID' not in already_processed:
            already_processed.add('Location_MetricID')
            showIndent(outfile, level)
            outfile.write('Location_MetricID="%s",\n' % (self.Location_MetricID,))
        if self.ComponentID is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            showIndent(outfile, level)
            outfile.write('ComponentID="%s",\n' % (self.ComponentID,))
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
        value = find_attr_value_('Location_MetricID', node)
        if value is not None and 'Location_MetricID' not in already_processed:
            already_processed.add('Location_MetricID')
            self.Location_MetricID = value
        value = find_attr_value_('ComponentID', node)
        if value is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            self.ComponentID = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class ComponentAType


class ComponentBType(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, Location_MetricID=None, ComponentID=None):
        self.original_tagname_ = None
        self.Location_MetricID = _cast(None, Location_MetricID)
        self.ComponentID = _cast(None, ComponentID)
    def factory(*args_, **kwargs_):
        if ComponentBType.subclass:
            return ComponentBType.subclass(*args_, **kwargs_)
        else:
            return ComponentBType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Location_MetricID(self): return self.Location_MetricID
    def set_Location_MetricID(self, Location_MetricID): self.Location_MetricID = Location_MetricID
    def get_ComponentID(self): return self.ComponentID
    def set_ComponentID(self, ComponentID): self.ComponentID = ComponentID
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='ComponentBType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='ComponentBType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='ComponentBType', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='ComponentBType'):
        if self.Location_MetricID is not None and 'Location_MetricID' not in already_processed:
            already_processed.add('Location_MetricID')
            outfile.write(' Location_MetricID=%s' % (self.gds_format_string(quote_attrib(self.Location_MetricID).encode(ExternalEncoding), input_name='Location_MetricID'), ))
        if self.ComponentID is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            outfile.write(' ComponentID=%s' % (self.gds_format_string(quote_attrib(self.ComponentID).encode(ExternalEncoding), input_name='ComponentID'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='ComponentBType', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='ComponentBType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.Location_MetricID is not None and 'Location_MetricID' not in already_processed:
            already_processed.add('Location_MetricID')
            showIndent(outfile, level)
            outfile.write('Location_MetricID="%s",\n' % (self.Location_MetricID,))
        if self.ComponentID is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            showIndent(outfile, level)
            outfile.write('ComponentID="%s",\n' % (self.ComponentID,))
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
        value = find_attr_value_('Location_MetricID', node)
        if value is not None and 'Location_MetricID' not in already_processed:
            already_processed.add('Location_MetricID')
            self.Location_MetricID = value
        value = find_attr_value_('ComponentID', node)
        if value is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            self.ComponentID = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class ComponentBType


class Joint_One_Axis_GivenType(GeneratedsSuper):
    """Translational/Revolute/Cylindrical"""
    subclass = None
    superclass = None
    def __init__(self, Type=None, ID=None, ComponentA=None, ComponentB=None, Geometry=None):
        self.original_tagname_ = None
        self.Type = _cast(None, Type)
        self.ID = _cast(None, ID)
        self.ComponentA = ComponentA
        self.ComponentB = ComponentB
        self.Geometry = Geometry
    def factory(*args_, **kwargs_):
        if Joint_One_Axis_GivenType.subclass:
            return Joint_One_Axis_GivenType.subclass(*args_, **kwargs_)
        else:
            return Joint_One_Axis_GivenType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_ComponentA(self): return self.ComponentA
    def set_ComponentA(self, ComponentA): self.ComponentA = ComponentA
    def get_ComponentB(self): return self.ComponentB
    def set_ComponentB(self, ComponentB): self.ComponentB = ComponentB
    def get_Geometry(self): return self.Geometry
    def set_Geometry(self, Geometry): self.Geometry = Geometry
    def get_Type(self): return self.Type
    def set_Type(self, Type): self.Type = Type
    def get_ID(self): return self.ID
    def set_ID(self, ID): self.ID = ID
    def hasContent_(self):
        if (
            self.ComponentA is not None or
            self.ComponentB is not None or
            self.Geometry is not None
        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='Joint_One_Axis_GivenType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='Joint_One_Axis_GivenType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='Joint_One_Axis_GivenType', pretty_print=pretty_print)
            showIndent(outfile, level, pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='Joint_One_Axis_GivenType'):
        if self.Type is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            outfile.write(' Type=%s' % (self.gds_format_string(quote_attrib(self.Type).encode(ExternalEncoding), input_name='Type'), ))
        if self.ID is not None and 'ID' not in already_processed:
            already_processed.add('ID')
            outfile.write(' ID=%s' % (self.gds_format_string(quote_attrib(self.ID).encode(ExternalEncoding), input_name='ID'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='Joint_One_Axis_GivenType', fromsubclass_=False, pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.ComponentA is not None:
            self.ComponentA.export(outfile, level, namespace_, name_='ComponentA', pretty_print=pretty_print)
        if self.ComponentB is not None:
            self.ComponentB.export(outfile, level, namespace_, name_='ComponentB', pretty_print=pretty_print)
        if self.Geometry is not None:
            self.Geometry.export(outfile, level, namespace_, name_='Geometry', pretty_print=pretty_print)
    def exportLiteral(self, outfile, level, name_='Joint_One_Axis_GivenType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.Type is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            showIndent(outfile, level)
            outfile.write('Type="%s",\n' % (self.Type,))
        if self.ID is not None and 'ID' not in already_processed:
            already_processed.add('ID')
            showIndent(outfile, level)
            outfile.write('ID="%s",\n' % (self.ID,))
    def exportLiteralChildren(self, outfile, level, name_):
        if self.ComponentA is not None:
            showIndent(outfile, level)
            outfile.write('ComponentA=model_.ComponentAType1(\n')
            self.ComponentA.exportLiteral(outfile, level, name_='ComponentA')
            showIndent(outfile, level)
            outfile.write('),\n')
        if self.ComponentB is not None:
            showIndent(outfile, level)
            outfile.write('ComponentB=model_.ComponentBType2(\n')
            self.ComponentB.exportLiteral(outfile, level, name_='ComponentB')
            showIndent(outfile, level)
            outfile.write('),\n')
        if self.Geometry is not None:
            showIndent(outfile, level)
            outfile.write('Geometry=model_.GeometryType(\n')
            self.Geometry.exportLiteral(outfile, level, name_='Geometry')
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
        value = find_attr_value_('Type', node)
        if value is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            self.Type = value
        value = find_attr_value_('ID', node)
        if value is not None and 'ID' not in already_processed:
            already_processed.add('ID')
            self.ID = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        if nodeName_ == 'ComponentA':
            obj_ = ComponentAType1.factory()
            obj_.build(child_)
            self.ComponentA = obj_
            obj_.original_tagname_ = 'ComponentA'
        elif nodeName_ == 'ComponentB':
            obj_ = ComponentBType2.factory()
            obj_.build(child_)
            self.ComponentB = obj_
            obj_.original_tagname_ = 'ComponentB'
        elif nodeName_ == 'Geometry':
            obj_ = GeometryType.factory()
            obj_.build(child_)
            self.Geometry = obj_
            obj_.original_tagname_ = 'Geometry'
# end class Joint_One_Axis_GivenType


class ComponentAType1(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, Location_MetricID=None, ComponentID=None):
        self.original_tagname_ = None
        self.Location_MetricID = _cast(None, Location_MetricID)
        self.ComponentID = _cast(None, ComponentID)
    def factory(*args_, **kwargs_):
        if ComponentAType1.subclass:
            return ComponentAType1.subclass(*args_, **kwargs_)
        else:
            return ComponentAType1(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Location_MetricID(self): return self.Location_MetricID
    def set_Location_MetricID(self, Location_MetricID): self.Location_MetricID = Location_MetricID
    def get_ComponentID(self): return self.ComponentID
    def set_ComponentID(self, ComponentID): self.ComponentID = ComponentID
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='ComponentAType1', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='ComponentAType1')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='ComponentAType1', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='ComponentAType1'):
        if self.Location_MetricID is not None and 'Location_MetricID' not in already_processed:
            already_processed.add('Location_MetricID')
            outfile.write(' Location_MetricID=%s' % (self.gds_format_string(quote_attrib(self.Location_MetricID).encode(ExternalEncoding), input_name='Location_MetricID'), ))
        if self.ComponentID is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            outfile.write(' ComponentID=%s' % (self.gds_format_string(quote_attrib(self.ComponentID).encode(ExternalEncoding), input_name='ComponentID'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='ComponentAType1', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='ComponentAType1'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.Location_MetricID is not None and 'Location_MetricID' not in already_processed:
            already_processed.add('Location_MetricID')
            showIndent(outfile, level)
            outfile.write('Location_MetricID="%s",\n' % (self.Location_MetricID,))
        if self.ComponentID is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            showIndent(outfile, level)
            outfile.write('ComponentID="%s",\n' % (self.ComponentID,))
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
        value = find_attr_value_('Location_MetricID', node)
        if value is not None and 'Location_MetricID' not in already_processed:
            already_processed.add('Location_MetricID')
            self.Location_MetricID = value
        value = find_attr_value_('ComponentID', node)
        if value is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            self.ComponentID = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class ComponentAType1


class ComponentBType2(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, Location_MetricID=None, ComponentID=None):
        self.original_tagname_ = None
        self.Location_MetricID = _cast(None, Location_MetricID)
        self.ComponentID = _cast(None, ComponentID)
    def factory(*args_, **kwargs_):
        if ComponentBType2.subclass:
            return ComponentBType2.subclass(*args_, **kwargs_)
        else:
            return ComponentBType2(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Location_MetricID(self): return self.Location_MetricID
    def set_Location_MetricID(self, Location_MetricID): self.Location_MetricID = Location_MetricID
    def get_ComponentID(self): return self.ComponentID
    def set_ComponentID(self, ComponentID): self.ComponentID = ComponentID
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='ComponentBType2', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='ComponentBType2')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='ComponentBType2', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='ComponentBType2'):
        if self.Location_MetricID is not None and 'Location_MetricID' not in already_processed:
            already_processed.add('Location_MetricID')
            outfile.write(' Location_MetricID=%s' % (self.gds_format_string(quote_attrib(self.Location_MetricID).encode(ExternalEncoding), input_name='Location_MetricID'), ))
        if self.ComponentID is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            outfile.write(' ComponentID=%s' % (self.gds_format_string(quote_attrib(self.ComponentID).encode(ExternalEncoding), input_name='ComponentID'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='ComponentBType2', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='ComponentBType2'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.Location_MetricID is not None and 'Location_MetricID' not in already_processed:
            already_processed.add('Location_MetricID')
            showIndent(outfile, level)
            outfile.write('Location_MetricID="%s",\n' % (self.Location_MetricID,))
        if self.ComponentID is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            showIndent(outfile, level)
            outfile.write('ComponentID="%s",\n' % (self.ComponentID,))
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
        value = find_attr_value_('Location_MetricID', node)
        if value is not None and 'Location_MetricID' not in already_processed:
            already_processed.add('Location_MetricID')
            self.Location_MetricID = value
        value = find_attr_value_('ComponentID', node)
        if value is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            self.ComponentID = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class ComponentBType2


class GeometryType(GeneratedsSuper):
    """Vector"""
    subclass = None
    superclass = None
    def __init__(self, MetricID=None, Type=None):
        self.original_tagname_ = None
        self.MetricID = _cast(None, MetricID)
        self.Type = _cast(None, Type)
    def factory(*args_, **kwargs_):
        if GeometryType.subclass:
            return GeometryType.subclass(*args_, **kwargs_)
        else:
            return GeometryType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_MetricID(self): return self.MetricID
    def set_MetricID(self, MetricID): self.MetricID = MetricID
    def get_Type(self): return self.Type
    def set_Type(self, Type): self.Type = Type
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='GeometryType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='GeometryType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='GeometryType', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='GeometryType'):
        if self.MetricID is not None and 'MetricID' not in already_processed:
            already_processed.add('MetricID')
            outfile.write(' MetricID=%s' % (self.gds_format_string(quote_attrib(self.MetricID).encode(ExternalEncoding), input_name='MetricID'), ))
        if self.Type is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            outfile.write(' Type=%s' % (self.gds_format_string(quote_attrib(self.Type).encode(ExternalEncoding), input_name='Type'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='GeometryType', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='GeometryType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.MetricID is not None and 'MetricID' not in already_processed:
            already_processed.add('MetricID')
            showIndent(outfile, level)
            outfile.write('MetricID="%s",\n' % (self.MetricID,))
        if self.Type is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            showIndent(outfile, level)
            outfile.write('Type="%s",\n' % (self.Type,))
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
        value = find_attr_value_('MetricID', node)
        if value is not None and 'MetricID' not in already_processed:
            already_processed.add('MetricID')
            self.MetricID = value
        value = find_attr_value_('Type', node)
        if value is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            self.Type = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class GeometryType


class LoadsType(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, Motion=None, Gravity=None):
        self.original_tagname_ = None
        if Motion is None:
            self.Motion = []
        else:
            self.Motion = Motion
        self.Gravity = Gravity
    def factory(*args_, **kwargs_):
        if LoadsType.subclass:
            return LoadsType.subclass(*args_, **kwargs_)
        else:
            return LoadsType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Motion(self): return self.Motion
    def set_Motion(self, Motion): self.Motion = Motion
    def add_Motion(self, value): self.Motion.append(value)
    def insert_Motion(self, index, value): self.Motion[index] = value
    def get_Gravity(self): return self.Gravity
    def set_Gravity(self, Gravity): self.Gravity = Gravity
    def hasContent_(self):
        if (
            self.Motion or
            self.Gravity is not None
        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='LoadsType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='LoadsType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='LoadsType', pretty_print=pretty_print)
            showIndent(outfile, level, pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='LoadsType'):
        pass
    def exportChildren(self, outfile, level, namespace_='', name_='LoadsType', fromsubclass_=False, pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        for Motion_ in self.Motion:
            Motion_.export(outfile, level, namespace_, name_='Motion', pretty_print=pretty_print)
        if self.Gravity is not None:
            self.Gravity.export(outfile, level, namespace_, name_='Gravity', pretty_print=pretty_print)
    def exportLiteral(self, outfile, level, name_='LoadsType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        pass
    def exportLiteralChildren(self, outfile, level, name_):
        showIndent(outfile, level)
        outfile.write('Motion=[\n')
        level += 1
        for Motion_ in self.Motion:
            showIndent(outfile, level)
            outfile.write('model_.MotionType(\n')
            Motion_.exportLiteral(outfile, level, name_='MotionType')
            showIndent(outfile, level)
            outfile.write('),\n')
        level -= 1
        showIndent(outfile, level)
        outfile.write('],\n')
        if self.Gravity is not None:
            showIndent(outfile, level)
            outfile.write('Gravity=model_.GravityType(\n')
            self.Gravity.exportLiteral(outfile, level, name_='Gravity')
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
        pass
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        if nodeName_ == 'Motion':
            obj_ = MotionType.factory()
            obj_.build(child_)
            self.Motion.append(obj_)
            obj_.original_tagname_ = 'Motion'
        elif nodeName_ == 'Gravity':
            obj_ = GravityType.factory()
            obj_.build(child_)
            self.Gravity = obj_
            obj_.original_tagname_ = 'Gravity'
# end class LoadsType


class MotionType(GeneratedsSuper):
    """JointMotionTranslational/RotationalDisplacement/Velocity/Acceleratio
    non/off"""
    subclass = None
    superclass = None
    def __init__(self, Function=None, FreedomType=None, Type=None, MotionID=None, TimeDerivative=None, Active=None, JointID=None):
        self.original_tagname_ = None
        self.Function = _cast(None, Function)
        self.FreedomType = _cast(None, FreedomType)
        self.Type = _cast(None, Type)
        self.MotionID = _cast(None, MotionID)
        self.TimeDerivative = _cast(None, TimeDerivative)
        self.Active = _cast(None, Active)
        self.JointID = _cast(None, JointID)
    def factory(*args_, **kwargs_):
        if MotionType.subclass:
            return MotionType.subclass(*args_, **kwargs_)
        else:
            return MotionType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Function(self): return self.Function
    def set_Function(self, Function): self.Function = Function
    def get_FreedomType(self): return self.FreedomType
    def set_FreedomType(self, FreedomType): self.FreedomType = FreedomType
    def get_Type(self): return self.Type
    def set_Type(self, Type): self.Type = Type
    def get_MotionID(self): return self.MotionID
    def set_MotionID(self, MotionID): self.MotionID = MotionID
    def get_TimeDerivative(self): return self.TimeDerivative
    def set_TimeDerivative(self, TimeDerivative): self.TimeDerivative = TimeDerivative
    def get_Active(self): return self.Active
    def set_Active(self, Active): self.Active = Active
    def get_JointID(self): return self.JointID
    def set_JointID(self, JointID): self.JointID = JointID
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='MotionType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='MotionType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='MotionType', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='MotionType'):
        if self.Function is not None and 'Function' not in already_processed:
            already_processed.add('Function')
            outfile.write(' Function=%s' % (self.gds_format_string(quote_attrib(self.Function).encode(ExternalEncoding), input_name='Function'), ))
        if self.FreedomType is not None and 'FreedomType' not in already_processed:
            already_processed.add('FreedomType')
            outfile.write(' FreedomType=%s' % (self.gds_format_string(quote_attrib(self.FreedomType).encode(ExternalEncoding), input_name='FreedomType'), ))
        if self.Type is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            outfile.write(' Type=%s' % (self.gds_format_string(quote_attrib(self.Type).encode(ExternalEncoding), input_name='Type'), ))
        if self.MotionID is not None and 'MotionID' not in already_processed:
            already_processed.add('MotionID')
            outfile.write(' MotionID=%s' % (self.gds_format_string(quote_attrib(self.MotionID).encode(ExternalEncoding), input_name='MotionID'), ))
        if self.TimeDerivative is not None and 'TimeDerivative' not in already_processed:
            already_processed.add('TimeDerivative')
            outfile.write(' TimeDerivative=%s' % (self.gds_format_string(quote_attrib(self.TimeDerivative).encode(ExternalEncoding), input_name='TimeDerivative'), ))
        if self.Active is not None and 'Active' not in already_processed:
            already_processed.add('Active')
            outfile.write(' Active=%s' % (self.gds_format_string(quote_attrib(self.Active).encode(ExternalEncoding), input_name='Active'), ))
        if self.JointID is not None and 'JointID' not in already_processed:
            already_processed.add('JointID')
            outfile.write(' JointID=%s' % (self.gds_format_string(quote_attrib(self.JointID).encode(ExternalEncoding), input_name='JointID'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='MotionType', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='MotionType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.Function is not None and 'Function' not in already_processed:
            already_processed.add('Function')
            showIndent(outfile, level)
            outfile.write('Function="%s",\n' % (self.Function,))
        if self.FreedomType is not None and 'FreedomType' not in already_processed:
            already_processed.add('FreedomType')
            showIndent(outfile, level)
            outfile.write('FreedomType="%s",\n' % (self.FreedomType,))
        if self.Type is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            showIndent(outfile, level)
            outfile.write('Type="%s",\n' % (self.Type,))
        if self.MotionID is not None and 'MotionID' not in already_processed:
            already_processed.add('MotionID')
            showIndent(outfile, level)
            outfile.write('MotionID="%s",\n' % (self.MotionID,))
        if self.TimeDerivative is not None and 'TimeDerivative' not in already_processed:
            already_processed.add('TimeDerivative')
            showIndent(outfile, level)
            outfile.write('TimeDerivative="%s",\n' % (self.TimeDerivative,))
        if self.Active is not None and 'Active' not in already_processed:
            already_processed.add('Active')
            showIndent(outfile, level)
            outfile.write('Active="%s",\n' % (self.Active,))
        if self.JointID is not None and 'JointID' not in already_processed:
            already_processed.add('JointID')
            showIndent(outfile, level)
            outfile.write('JointID="%s",\n' % (self.JointID,))
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
        value = find_attr_value_('Function', node)
        if value is not None and 'Function' not in already_processed:
            already_processed.add('Function')
            self.Function = value
        value = find_attr_value_('FreedomType', node)
        if value is not None and 'FreedomType' not in already_processed:
            already_processed.add('FreedomType')
            self.FreedomType = value
        value = find_attr_value_('Type', node)
        if value is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            self.Type = value
        value = find_attr_value_('MotionID', node)
        if value is not None and 'MotionID' not in already_processed:
            already_processed.add('MotionID')
            self.MotionID = value
        value = find_attr_value_('TimeDerivative', node)
        if value is not None and 'TimeDerivative' not in already_processed:
            already_processed.add('TimeDerivative')
            self.TimeDerivative = value
        value = find_attr_value_('Active', node)
        if value is not None and 'Active' not in already_processed:
            already_processed.add('Active')
            self.Active = value
        value = find_attr_value_('JointID', node)
        if value is not None and 'JointID' not in already_processed:
            already_processed.add('JointID')
            self.JointID = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class MotionType


class GravityType(GeneratedsSuper):
    """=> gravity as a 3D vectoron/off"""
    subclass = None
    superclass = None
    def __init__(self, Active=None, Value=None):
        self.original_tagname_ = None
        self.Active = _cast(None, Active)
        self.Value = _cast(None, Value)
    def factory(*args_, **kwargs_):
        if GravityType.subclass:
            return GravityType.subclass(*args_, **kwargs_)
        else:
            return GravityType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Active(self): return self.Active
    def set_Active(self, Active): self.Active = Active
    def get_Value(self): return self.Value
    def set_Value(self, Value): self.Value = Value
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='GravityType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='GravityType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='GravityType', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='GravityType'):
        if self.Active is not None and 'Active' not in already_processed:
            already_processed.add('Active')
            outfile.write(' Active=%s' % (self.gds_format_string(quote_attrib(self.Active).encode(ExternalEncoding), input_name='Active'), ))
        if self.Value is not None and 'Value' not in already_processed:
            already_processed.add('Value')
            outfile.write(' Value=%s' % (self.gds_format_string(quote_attrib(self.Value).encode(ExternalEncoding), input_name='Value'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='GravityType', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='GravityType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.Active is not None and 'Active' not in already_processed:
            already_processed.add('Active')
            showIndent(outfile, level)
            outfile.write('Active="%s",\n' % (self.Active,))
        if self.Value is not None and 'Value' not in already_processed:
            already_processed.add('Value')
            showIndent(outfile, level)
            outfile.write('Value="%s",\n' % (self.Value,))
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
        value = find_attr_value_('Active', node)
        if value is not None and 'Active' not in already_processed:
            already_processed.add('Active')
            self.Active = value
        value = find_attr_value_('Value', node)
        if value is not None and 'Value' not in already_processed:
            already_processed.add('Value')
            self.Value = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class GravityType


class SimulationType(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, Steps=None, Time=None):
        self.original_tagname_ = None
        self.Steps = _cast(int, Steps)
        self.Time = _cast(float, Time)
    def factory(*args_, **kwargs_):
        if SimulationType.subclass:
            return SimulationType.subclass(*args_, **kwargs_)
        else:
            return SimulationType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Steps(self): return self.Steps
    def set_Steps(self, Steps): self.Steps = Steps
    def get_Time(self): return self.Time
    def set_Time(self, Time): self.Time = Time
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='SimulationType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='SimulationType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='SimulationType', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='SimulationType'):
        if self.Steps is not None and 'Steps' not in already_processed:
            already_processed.add('Steps')
            outfile.write(' Steps="%s"' % self.gds_format_integer(self.Steps, input_name='Steps'))
        if self.Time is not None and 'Time' not in already_processed:
            already_processed.add('Time')
            outfile.write(' Time="%s"' % self.gds_format_float(self.Time, input_name='Time'))
    def exportChildren(self, outfile, level, namespace_='', name_='SimulationType', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='SimulationType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.Steps is not None and 'Steps' not in already_processed:
            already_processed.add('Steps')
            showIndent(outfile, level)
            outfile.write('Steps=%d,\n' % (self.Steps,))
        if self.Time is not None and 'Time' not in already_processed:
            already_processed.add('Time')
            showIndent(outfile, level)
            outfile.write('Time=%f,\n' % (self.Time,))
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
        value = find_attr_value_('Steps', node)
        if value is not None and 'Steps' not in already_processed:
            already_processed.add('Steps')
            try:
                self.Steps = int(value)
            except ValueError, exp:
                raise_parse_error(node, 'Bad integer attribute: %s' % exp)
            if self.Steps <= 0:
                raise_parse_error(node, 'Invalid PositiveInteger')
        value = find_attr_value_('Time', node)
        if value is not None and 'Time' not in already_processed:
            already_processed.add('Time')
            try:
                self.Time = float(value)
            except ValueError, exp:
                raise ValueError('Bad float/double attribute (Time): %s' % exp)
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class SimulationType


class ResultsType(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, Metric_FEA=None, Metric_Component_XYZ_Mag=None, Metric_Component_Mag_Only=None, Metric_Point=None, Metric_Motion=None):
        self.original_tagname_ = None
        if Metric_FEA is None:
            self.Metric_FEA = []
        else:
            self.Metric_FEA = Metric_FEA
        if Metric_Component_XYZ_Mag is None:
            self.Metric_Component_XYZ_Mag = []
        else:
            self.Metric_Component_XYZ_Mag = Metric_Component_XYZ_Mag
        if Metric_Component_Mag_Only is None:
            self.Metric_Component_Mag_Only = []
        else:
            self.Metric_Component_Mag_Only = Metric_Component_Mag_Only
        if Metric_Point is None:
            self.Metric_Point = []
        else:
            self.Metric_Point = Metric_Point
        if Metric_Motion is None:
            self.Metric_Motion = []
        else:
            self.Metric_Motion = Metric_Motion
    def factory(*args_, **kwargs_):
        if ResultsType.subclass:
            return ResultsType.subclass(*args_, **kwargs_)
        else:
            return ResultsType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Metric_FEA(self): return self.Metric_FEA
    def set_Metric_FEA(self, Metric_FEA): self.Metric_FEA = Metric_FEA
    def add_Metric_FEA(self, value): self.Metric_FEA.append(value)
    def insert_Metric_FEA(self, index, value): self.Metric_FEA[index] = value
    def get_Metric_Component_XYZ_Mag(self): return self.Metric_Component_XYZ_Mag
    def set_Metric_Component_XYZ_Mag(self, Metric_Component_XYZ_Mag): self.Metric_Component_XYZ_Mag = Metric_Component_XYZ_Mag
    def add_Metric_Component_XYZ_Mag(self, value): self.Metric_Component_XYZ_Mag.append(value)
    def insert_Metric_Component_XYZ_Mag(self, index, value): self.Metric_Component_XYZ_Mag[index] = value
    def get_Metric_Component_Mag_Only(self): return self.Metric_Component_Mag_Only
    def set_Metric_Component_Mag_Only(self, Metric_Component_Mag_Only): self.Metric_Component_Mag_Only = Metric_Component_Mag_Only
    def add_Metric_Component_Mag_Only(self, value): self.Metric_Component_Mag_Only.append(value)
    def insert_Metric_Component_Mag_Only(self, index, value): self.Metric_Component_Mag_Only[index] = value
    def get_Metric_Point(self): return self.Metric_Point
    def set_Metric_Point(self, Metric_Point): self.Metric_Point = Metric_Point
    def add_Metric_Point(self, value): self.Metric_Point.append(value)
    def insert_Metric_Point(self, index, value): self.Metric_Point[index] = value
    def get_Metric_Motion(self): return self.Metric_Motion
    def set_Metric_Motion(self, Metric_Motion): self.Metric_Motion = Metric_Motion
    def add_Metric_Motion(self, value): self.Metric_Motion.append(value)
    def insert_Metric_Motion(self, index, value): self.Metric_Motion[index] = value
    def hasContent_(self):
        if (
            self.Metric_FEA or
            self.Metric_Component_XYZ_Mag or
            self.Metric_Component_Mag_Only or
            self.Metric_Point or
            self.Metric_Motion
        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='ResultsType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='ResultsType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='ResultsType', pretty_print=pretty_print)
            showIndent(outfile, level, pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='ResultsType'):
        pass
    def exportChildren(self, outfile, level, namespace_='', name_='ResultsType', fromsubclass_=False, pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        for Metric_FEA_ in self.Metric_FEA:
            Metric_FEA_.export(outfile, level, namespace_, name_='Metric_FEA', pretty_print=pretty_print)
        for Metric_Component_XYZ_Mag_ in self.Metric_Component_XYZ_Mag:
            Metric_Component_XYZ_Mag_.export(outfile, level, namespace_, name_='Metric_Component_XYZ_Mag', pretty_print=pretty_print)
        for Metric_Component_Mag_Only_ in self.Metric_Component_Mag_Only:
            Metric_Component_Mag_Only_.export(outfile, level, namespace_, name_='Metric_Component_Mag_Only', pretty_print=pretty_print)
        for Metric_Point_ in self.Metric_Point:
            Metric_Point_.export(outfile, level, namespace_, name_='Metric_Point', pretty_print=pretty_print)
        for Metric_Motion_ in self.Metric_Motion:
            Metric_Motion_.export(outfile, level, namespace_, name_='Metric_Motion', pretty_print=pretty_print)
    def exportLiteral(self, outfile, level, name_='ResultsType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        pass
    def exportLiteralChildren(self, outfile, level, name_):
        showIndent(outfile, level)
        outfile.write('Metric_FEA=[\n')
        level += 1
        for Metric_FEA_ in self.Metric_FEA:
            showIndent(outfile, level)
            outfile.write('model_.Metric_FEAType(\n')
            Metric_FEA_.exportLiteral(outfile, level, name_='Metric_FEAType')
            showIndent(outfile, level)
            outfile.write('),\n')
        level -= 1
        showIndent(outfile, level)
        outfile.write('],\n')
        showIndent(outfile, level)
        outfile.write('Metric_Component_XYZ_Mag=[\n')
        level += 1
        for Metric_Component_XYZ_Mag_ in self.Metric_Component_XYZ_Mag:
            showIndent(outfile, level)
            outfile.write('model_.Metric_Component_XYZ_MagType(\n')
            Metric_Component_XYZ_Mag_.exportLiteral(outfile, level, name_='Metric_Component_XYZ_MagType')
            showIndent(outfile, level)
            outfile.write('),\n')
        level -= 1
        showIndent(outfile, level)
        outfile.write('],\n')
        showIndent(outfile, level)
        outfile.write('Metric_Component_Mag_Only=[\n')
        level += 1
        for Metric_Component_Mag_Only_ in self.Metric_Component_Mag_Only:
            showIndent(outfile, level)
            outfile.write('model_.Metric_Component_Mag_OnlyType(\n')
            Metric_Component_Mag_Only_.exportLiteral(outfile, level, name_='Metric_Component_Mag_OnlyType')
            showIndent(outfile, level)
            outfile.write('),\n')
        level -= 1
        showIndent(outfile, level)
        outfile.write('],\n')
        showIndent(outfile, level)
        outfile.write('Metric_Point=[\n')
        level += 1
        for Metric_Point_ in self.Metric_Point:
            showIndent(outfile, level)
            outfile.write('model_.Metric_PointType(\n')
            Metric_Point_.exportLiteral(outfile, level, name_='Metric_PointType')
            showIndent(outfile, level)
            outfile.write('),\n')
        level -= 1
        showIndent(outfile, level)
        outfile.write('],\n')
        showIndent(outfile, level)
        outfile.write('Metric_Motion=[\n')
        level += 1
        for Metric_Motion_ in self.Metric_Motion:
            showIndent(outfile, level)
            outfile.write('model_.Metric_MotionType(\n')
            Metric_Motion_.exportLiteral(outfile, level, name_='Metric_MotionType')
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
        pass
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        if nodeName_ == 'Metric_FEA':
            obj_ = Metric_FEAType.factory()
            obj_.build(child_)
            self.Metric_FEA.append(obj_)
            obj_.original_tagname_ = 'Metric_FEA'
        elif nodeName_ == 'Metric_Component_XYZ_Mag':
            obj_ = Metric_Component_XYZ_MagType.factory()
            obj_.build(child_)
            self.Metric_Component_XYZ_Mag.append(obj_)
            obj_.original_tagname_ = 'Metric_Component_XYZ_Mag'
        elif nodeName_ == 'Metric_Component_Mag_Only':
            obj_ = Metric_Component_Mag_OnlyType.factory()
            obj_.build(child_)
            self.Metric_Component_Mag_Only.append(obj_)
            obj_.original_tagname_ = 'Metric_Component_Mag_Only'
        elif nodeName_ == 'Metric_Point':
            obj_ = Metric_PointType.factory()
            obj_.build(child_)
            self.Metric_Point.append(obj_)
            obj_.original_tagname_ = 'Metric_Point'
        elif nodeName_ == 'Metric_Motion':
            obj_ = Metric_MotionType.factory()
            obj_.build(child_)
            self.Metric_Motion.append(obj_)
            obj_.original_tagname_ = 'Metric_Motion'
# end class ResultsType


class Metric_FEAType(GeneratedsSuper):
    subclass = None
    superclass = None
    def __init__(self, FEA_Tool=None, Type=None, ComponentID=None):
        self.original_tagname_ = None
        self.FEA_Tool = _cast(None, FEA_Tool)
        self.Type = _cast(None, Type)
        self.ComponentID = _cast(None, ComponentID)
    def factory(*args_, **kwargs_):
        if Metric_FEAType.subclass:
            return Metric_FEAType.subclass(*args_, **kwargs_)
        else:
            return Metric_FEAType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_FEA_Tool(self): return self.FEA_Tool
    def set_FEA_Tool(self, FEA_Tool): self.FEA_Tool = FEA_Tool
    def get_Type(self): return self.Type
    def set_Type(self, Type): self.Type = Type
    def get_ComponentID(self): return self.ComponentID
    def set_ComponentID(self, ComponentID): self.ComponentID = ComponentID
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='Metric_FEAType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='Metric_FEAType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='Metric_FEAType', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='Metric_FEAType'):
        if self.FEA_Tool is not None and 'FEA_Tool' not in already_processed:
            already_processed.add('FEA_Tool')
            outfile.write(' FEA_Tool=%s' % (self.gds_format_string(quote_attrib(self.FEA_Tool).encode(ExternalEncoding), input_name='FEA_Tool'), ))
        if self.Type is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            outfile.write(' Type=%s' % (self.gds_format_string(quote_attrib(self.Type).encode(ExternalEncoding), input_name='Type'), ))
        if self.ComponentID is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            outfile.write(' ComponentID=%s' % (self.gds_format_string(quote_attrib(self.ComponentID).encode(ExternalEncoding), input_name='ComponentID'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='Metric_FEAType', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='Metric_FEAType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.FEA_Tool is not None and 'FEA_Tool' not in already_processed:
            already_processed.add('FEA_Tool')
            showIndent(outfile, level)
            outfile.write('FEA_Tool="%s",\n' % (self.FEA_Tool,))
        if self.Type is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            showIndent(outfile, level)
            outfile.write('Type="%s",\n' % (self.Type,))
        if self.ComponentID is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            showIndent(outfile, level)
            outfile.write('ComponentID="%s",\n' % (self.ComponentID,))
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
        value = find_attr_value_('FEA_Tool', node)
        if value is not None and 'FEA_Tool' not in already_processed:
            already_processed.add('FEA_Tool')
            self.FEA_Tool = value
        value = find_attr_value_('Type', node)
        if value is not None and 'Type' not in already_processed:
            already_processed.add('Type')
            self.Type = value
        value = find_attr_value_('ComponentID', node)
        if value is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            self.ComponentID = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class Metric_FEAType


class Metric_Component_XYZ_MagType(GeneratedsSuper):
    """CM_Position/Velocity/Acceleration, CM_Angular_Vel, CM_Angular_Acc,
    Trans_Momentum, Angular_Momentum_About_CMX/Y/Z/Mag"""
    subclass = None
    superclass = None
    def __init__(self, M_Component=None, ComponentID=None, M_Type=None):
        self.original_tagname_ = None
        self.M_Component = _cast(None, M_Component)
        self.ComponentID = _cast(None, ComponentID)
        self.M_Type = _cast(None, M_Type)
    def factory(*args_, **kwargs_):
        if Metric_Component_XYZ_MagType.subclass:
            return Metric_Component_XYZ_MagType.subclass(*args_, **kwargs_)
        else:
            return Metric_Component_XYZ_MagType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_M_Component(self): return self.M_Component
    def set_M_Component(self, M_Component): self.M_Component = M_Component
    def get_ComponentID(self): return self.ComponentID
    def set_ComponentID(self, ComponentID): self.ComponentID = ComponentID
    def get_M_Type(self): return self.M_Type
    def set_M_Type(self, M_Type): self.M_Type = M_Type
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='Metric_Component_XYZ_MagType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='Metric_Component_XYZ_MagType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='Metric_Component_XYZ_MagType', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='Metric_Component_XYZ_MagType'):
        if self.M_Component is not None and 'M_Component' not in already_processed:
            already_processed.add('M_Component')
            outfile.write(' M_Component=%s' % (self.gds_format_string(quote_attrib(self.M_Component).encode(ExternalEncoding), input_name='M_Component'), ))
        if self.ComponentID is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            outfile.write(' ComponentID=%s' % (self.gds_format_string(quote_attrib(self.ComponentID).encode(ExternalEncoding), input_name='ComponentID'), ))
        if self.M_Type is not None and 'M_Type' not in already_processed:
            already_processed.add('M_Type')
            outfile.write(' M_Type=%s' % (self.gds_format_string(quote_attrib(self.M_Type).encode(ExternalEncoding), input_name='M_Type'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='Metric_Component_XYZ_MagType', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='Metric_Component_XYZ_MagType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.M_Component is not None and 'M_Component' not in already_processed:
            already_processed.add('M_Component')
            showIndent(outfile, level)
            outfile.write('M_Component="%s",\n' % (self.M_Component,))
        if self.ComponentID is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            showIndent(outfile, level)
            outfile.write('ComponentID="%s",\n' % (self.ComponentID,))
        if self.M_Type is not None and 'M_Type' not in already_processed:
            already_processed.add('M_Type')
            showIndent(outfile, level)
            outfile.write('M_Type="%s",\n' % (self.M_Type,))
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
        value = find_attr_value_('M_Component', node)
        if value is not None and 'M_Component' not in already_processed:
            already_processed.add('M_Component')
            self.M_Component = value
        value = find_attr_value_('ComponentID', node)
        if value is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            self.ComponentID = value
        value = find_attr_value_('M_Type', node)
        if value is not None and 'M_Type' not in already_processed:
            already_processed.add('M_Type')
            self.M_Type = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class Metric_Component_XYZ_MagType


class Metric_Component_Mag_OnlyType(GeneratedsSuper):
    """Kinetic_Energy, Trans_Kinetic_Energy, Angular_Kinetic_Energy,
    Potential_Energy_DeltaMag"""
    subclass = None
    superclass = None
    def __init__(self, M_Component=None, ComponentID=None, M_Type=None):
        self.original_tagname_ = None
        self.M_Component = _cast(None, M_Component)
        self.ComponentID = _cast(None, ComponentID)
        self.M_Type = _cast(None, M_Type)
    def factory(*args_, **kwargs_):
        if Metric_Component_Mag_OnlyType.subclass:
            return Metric_Component_Mag_OnlyType.subclass(*args_, **kwargs_)
        else:
            return Metric_Component_Mag_OnlyType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_M_Component(self): return self.M_Component
    def set_M_Component(self, M_Component): self.M_Component = M_Component
    def get_ComponentID(self): return self.ComponentID
    def set_ComponentID(self, ComponentID): self.ComponentID = ComponentID
    def get_M_Type(self): return self.M_Type
    def set_M_Type(self, M_Type): self.M_Type = M_Type
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='Metric_Component_Mag_OnlyType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='Metric_Component_Mag_OnlyType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='Metric_Component_Mag_OnlyType', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='Metric_Component_Mag_OnlyType'):
        if self.M_Component is not None and 'M_Component' not in already_processed:
            already_processed.add('M_Component')
            outfile.write(' M_Component=%s' % (self.gds_format_string(quote_attrib(self.M_Component).encode(ExternalEncoding), input_name='M_Component'), ))
        if self.ComponentID is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            outfile.write(' ComponentID=%s' % (self.gds_format_string(quote_attrib(self.ComponentID).encode(ExternalEncoding), input_name='ComponentID'), ))
        if self.M_Type is not None and 'M_Type' not in already_processed:
            already_processed.add('M_Type')
            outfile.write(' M_Type=%s' % (self.gds_format_string(quote_attrib(self.M_Type).encode(ExternalEncoding), input_name='M_Type'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='Metric_Component_Mag_OnlyType', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='Metric_Component_Mag_OnlyType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.M_Component is not None and 'M_Component' not in already_processed:
            already_processed.add('M_Component')
            showIndent(outfile, level)
            outfile.write('M_Component="%s",\n' % (self.M_Component,))
        if self.ComponentID is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            showIndent(outfile, level)
            outfile.write('ComponentID="%s",\n' % (self.ComponentID,))
        if self.M_Type is not None and 'M_Type' not in already_processed:
            already_processed.add('M_Type')
            showIndent(outfile, level)
            outfile.write('M_Type="%s",\n' % (self.M_Type,))
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
        value = find_attr_value_('M_Component', node)
        if value is not None and 'M_Component' not in already_processed:
            already_processed.add('M_Component')
            self.M_Component = value
        value = find_attr_value_('ComponentID', node)
        if value is not None and 'ComponentID' not in already_processed:
            already_processed.add('ComponentID')
            self.ComponentID = value
        value = find_attr_value_('M_Type', node)
        if value is not None and 'M_Type' not in already_processed:
            already_processed.add('M_Type')
            self.M_Type = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class Metric_Component_Mag_OnlyType


class Metric_PointType(GeneratedsSuper):
    """Total_Force/Torque, Trans_Displacement, Trans_Velocity,
    Trans_Acceleration, Angular_Velocity,
    Angular_AccelerationX/Y/Z/Mag"""
    subclass = None
    superclass = None
    def __init__(self, Point_MetricID=None, M_Component=None, M_Type=None):
        self.original_tagname_ = None
        self.Point_MetricID = _cast(None, Point_MetricID)
        self.M_Component = _cast(None, M_Component)
        self.M_Type = _cast(None, M_Type)
    def factory(*args_, **kwargs_):
        if Metric_PointType.subclass:
            return Metric_PointType.subclass(*args_, **kwargs_)
        else:
            return Metric_PointType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_Point_MetricID(self): return self.Point_MetricID
    def set_Point_MetricID(self, Point_MetricID): self.Point_MetricID = Point_MetricID
    def get_M_Component(self): return self.M_Component
    def set_M_Component(self, M_Component): self.M_Component = M_Component
    def get_M_Type(self): return self.M_Type
    def set_M_Type(self, M_Type): self.M_Type = M_Type
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='Metric_PointType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='Metric_PointType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='Metric_PointType', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='Metric_PointType'):
        if self.Point_MetricID is not None and 'Point_MetricID' not in already_processed:
            already_processed.add('Point_MetricID')
            outfile.write(' Point_MetricID=%s' % (self.gds_format_string(quote_attrib(self.Point_MetricID).encode(ExternalEncoding), input_name='Point_MetricID'), ))
        if self.M_Component is not None and 'M_Component' not in already_processed:
            already_processed.add('M_Component')
            outfile.write(' M_Component=%s' % (self.gds_format_string(quote_attrib(self.M_Component).encode(ExternalEncoding), input_name='M_Component'), ))
        if self.M_Type is not None and 'M_Type' not in already_processed:
            already_processed.add('M_Type')
            outfile.write(' M_Type=%s' % (self.gds_format_string(quote_attrib(self.M_Type).encode(ExternalEncoding), input_name='M_Type'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='Metric_PointType', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='Metric_PointType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.Point_MetricID is not None and 'Point_MetricID' not in already_processed:
            already_processed.add('Point_MetricID')
            showIndent(outfile, level)
            outfile.write('Point_MetricID="%s",\n' % (self.Point_MetricID,))
        if self.M_Component is not None and 'M_Component' not in already_processed:
            already_processed.add('M_Component')
            showIndent(outfile, level)
            outfile.write('M_Component="%s",\n' % (self.M_Component,))
        if self.M_Type is not None and 'M_Type' not in already_processed:
            already_processed.add('M_Type')
            showIndent(outfile, level)
            outfile.write('M_Type="%s",\n' % (self.M_Type,))
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
        value = find_attr_value_('Point_MetricID', node)
        if value is not None and 'Point_MetricID' not in already_processed:
            already_processed.add('Point_MetricID')
            self.Point_MetricID = value
        value = find_attr_value_('M_Component', node)
        if value is not None and 'M_Component' not in already_processed:
            already_processed.add('M_Component')
            self.M_Component = value
        value = find_attr_value_('M_Type', node)
        if value is not None and 'M_Type' not in already_processed:
            already_processed.add('M_Type')
            self.M_Type = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class Metric_PointType


class Metric_MotionType(GeneratedsSuper):
    """Power_ConsumptionMag"""
    subclass = None
    superclass = None
    def __init__(self, M_Component=None, MotionID=None, M_Type=None):
        self.original_tagname_ = None
        self.M_Component = _cast(None, M_Component)
        self.MotionID = _cast(None, MotionID)
        self.M_Type = _cast(None, M_Type)
    def factory(*args_, **kwargs_):
        if Metric_MotionType.subclass:
            return Metric_MotionType.subclass(*args_, **kwargs_)
        else:
            return Metric_MotionType(*args_, **kwargs_)
    factory = staticmethod(factory)
    def get_M_Component(self): return self.M_Component
    def set_M_Component(self, M_Component): self.M_Component = M_Component
    def get_MotionID(self): return self.MotionID
    def set_MotionID(self, MotionID): self.MotionID = MotionID
    def get_M_Type(self): return self.M_Type
    def set_M_Type(self, M_Type): self.M_Type = M_Type
    def hasContent_(self):
        if (

        ):
            return True
        else:
            return False
    def export(self, outfile, level, namespace_='', name_='Metric_MotionType', namespacedef_='', pretty_print=True):
        if pretty_print:
            eol_ = '\n'
        else:
            eol_ = ''
        if self.original_tagname_ is not None:
            name_ = self.original_tagname_
        showIndent(outfile, level, pretty_print)
        outfile.write('<%s%s%s' % (namespace_, name_, namespacedef_ and ' ' + namespacedef_ or '', ))
        already_processed = set()
        self.exportAttributes(outfile, level, already_processed, namespace_, name_='Metric_MotionType')
        if self.hasContent_():
            outfile.write('>%s' % (eol_, ))
            self.exportChildren(outfile, level + 1, namespace_='', name_='Metric_MotionType', pretty_print=pretty_print)
            outfile.write('</%s%s>%s' % (namespace_, name_, eol_))
        else:
            outfile.write('/>%s' % (eol_, ))
    def exportAttributes(self, outfile, level, already_processed, namespace_='', name_='Metric_MotionType'):
        if self.M_Component is not None and 'M_Component' not in already_processed:
            already_processed.add('M_Component')
            outfile.write(' M_Component=%s' % (self.gds_format_string(quote_attrib(self.M_Component).encode(ExternalEncoding), input_name='M_Component'), ))
        if self.MotionID is not None and 'MotionID' not in already_processed:
            already_processed.add('MotionID')
            outfile.write(' MotionID=%s' % (self.gds_format_string(quote_attrib(self.MotionID).encode(ExternalEncoding), input_name='MotionID'), ))
        if self.M_Type is not None and 'M_Type' not in already_processed:
            already_processed.add('M_Type')
            outfile.write(' M_Type=%s' % (self.gds_format_string(quote_attrib(self.M_Type).encode(ExternalEncoding), input_name='M_Type'), ))
    def exportChildren(self, outfile, level, namespace_='', name_='Metric_MotionType', fromsubclass_=False, pretty_print=True):
        pass
    def exportLiteral(self, outfile, level, name_='Metric_MotionType'):
        level += 1
        already_processed = set()
        self.exportLiteralAttributes(outfile, level, already_processed, name_)
        if self.hasContent_():
            self.exportLiteralChildren(outfile, level, name_)
    def exportLiteralAttributes(self, outfile, level, already_processed, name_):
        if self.M_Component is not None and 'M_Component' not in already_processed:
            already_processed.add('M_Component')
            showIndent(outfile, level)
            outfile.write('M_Component="%s",\n' % (self.M_Component,))
        if self.MotionID is not None and 'MotionID' not in already_processed:
            already_processed.add('MotionID')
            showIndent(outfile, level)
            outfile.write('MotionID="%s",\n' % (self.MotionID,))
        if self.M_Type is not None and 'M_Type' not in already_processed:
            already_processed.add('M_Type')
            showIndent(outfile, level)
            outfile.write('M_Type="%s",\n' % (self.M_Type,))
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
        value = find_attr_value_('M_Component', node)
        if value is not None and 'M_Component' not in already_processed:
            already_processed.add('M_Component')
            self.M_Component = value
        value = find_attr_value_('MotionID', node)
        if value is not None and 'MotionID' not in already_processed:
            already_processed.add('MotionID')
            self.MotionID = value
        value = find_attr_value_('M_Type', node)
        if value is not None and 'M_Type' not in already_processed:
            already_processed.add('M_Type')
            self.M_Type = value
    def buildChildren(self, child_, node, nodeName_, fromsubclass_=False):
        pass
# end class Metric_MotionType


GDSClassesMapping = {
    'Script': ScriptType,
    'Metric_Component_XYZ_Mag': Metric_Component_XYZ_MagType,
    'Metric_Component_Mag_Only': Metric_Component_Mag_OnlyType,
    'Gravity': GravityType,
    'Units': UnitsType,
    'Assembly': AssemblyType,
    'Metric_Motion': Metric_MotionType,
    'Metric_Point': Metric_PointType,
    'Geometry': GeometryType,
    'Metric_FEA': Metric_FEAType,
    'Simulation': SimulationType,
    'Motion': MotionType,
    'Contact': ContactType,
    'Loads': LoadsType,
    'ComponentA': ComponentAType1,
    'ComponentB': ComponentBType2,
    'Joints': JointsType,
    'Terrain': TerrainType,
    'Results': ResultsType,
    'Joint_Locations_Only': Joint_Locations_OnlyType,
    'Joint_One_Axis_Given': Joint_One_Axis_GivenType,
    'Ground': GroundType,
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
        rootTag = 'Model'
        rootClass = Model
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
        rootTag = 'Model'
        rootClass = Model
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
        rootTag = 'Model'
        rootClass = Model
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
        rootTag = 'Model'
        rootClass = Model
    rootObj = rootClass.factory()
    rootObj.build(rootNode)
    # Enable Python to collect the space used by the DOM.
    doc = None
    if not silence:
        sys.stdout.write('#from aa import *\n\n')
        sys.stdout.write('import aa as model_\n\n')
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
    "AssemblyType",
    "ComponentAType",
    "ComponentAType1",
    "ComponentBType",
    "ComponentBType2",
    "ContactType",
    "GeometryType",
    "GravityType",
    "GroundType",
    "Joint_Locations_OnlyType",
    "Joint_One_Axis_GivenType",
    "JointsType",
    "LoadsType",
    "Metric_Component_Mag_OnlyType",
    "Metric_Component_XYZ_MagType",
    "Metric_FEAType",
    "Metric_MotionType",
    "Metric_PointType",
    "Model",
    "MotionType",
    "ResultsType",
    "ScriptType",
    "SimulationType",
    "TerrainType",
    "UnitsType"
]
