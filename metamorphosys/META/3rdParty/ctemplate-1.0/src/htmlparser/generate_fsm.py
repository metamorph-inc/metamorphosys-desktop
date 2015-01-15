#!/usr/bin/env python
#
# Copyright (c) 2008, Google Inc.
# All rights reserved.
#
# Redistribution and use in source and binary forms, with or without
# modification, are permitted provided that the following conditions are
# met:
#
#     * Redistributions of source code must retain the above copyright
# notice, this list of conditions and the following disclaimer.
#     * Redistributions in binary form must reproduce the above
# copyright notice, this list of conditions and the following disclaimer
# in the documentation and/or other materials provided with the
# distribution.
#     * Neither the name of Google Inc. nor the names of its
# contributors may be used to endorse or promote products derived from
# this software without specific prior written permission.
#
# THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
# "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
# LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
# A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
# OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
# SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
# LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
# DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
# THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
# (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
# OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
# ---
#
# Generate a C include file from a finite state machine definition.
#
# Right now the form is the one expected by htmlparser.c so this file is pretty
# tightly coupled with htmlparser.c.
#

__author__ = 'falmeida@google.com (Filipe Almeida)'

import sys

from fsm_config import FSMConfig


class FSMGenerateAbstract(object):

  def __init__(self, config):
    self._config = config

  def Generate(self):
    """Returns the generated FSM description for the specified language.

    Raises a TypeError, because abstract methods can not be called.

    Raises:
      TypeError
    """
    raise TypeError('Abstract method %s.%s called' % (self._class.__name__,
                                                      self._function))


class FSMGenerateC(FSMGenerateAbstract):
  """Generate the C definition from a statemachien configuration object."""

  TABSTOP_ = 2

  def _Prefix(self):
    """Return a c declaration prefix."""

    return self._config.name.lower() + '_'

  def _StateInternalC(self, st):
    """Return the internal name of the state."""

    return '%sSTATE_INT_%s' % (self._Prefix().upper(), st.upper())

  def _StateExternalC(self, st):
    """Return the external name of the state."""

    return '%sSTATE_%s' % (self._Prefix().upper(), st.upper())

  def _MakeTuple(self, data):
    """Converts data to a string representation of a C tuple."""

    return '{ %s }' % ', '.join(data)

  def _CreateHeader(self):
    """Print the include file header."""

    out = []

    if self._config.comment:
      out.append('/* ' + self._config.comment)
    else:
      out.append('/* State machine definition for ' + self._config.name)
    out.append(' * Auto generated by generate_fsm.py. Please do not edit.')
    out.append(' */')

    return '\n'.join(out)

  def _ListToIndentedString(self, list):
    indented_list = ['  ' + e for e in list]
    return ',\n'.join(indented_list)

  def _CreateEnum(self, name, data):
    """Print a c enum definition."""

    return 'enum %s {\n%s\n};\n' % (name,
                                    self._ListToIndentedString(data))

  def _CreateStructList(self, name, type, data):
    """Print a c flat list.

    Generic function to print list in c in the form of a struct.

    Args:
      name: name of the structure.
      type: type of the struct.
      data: contents of the struct as a list of elements

    Returns:
      String with the generated list.
    """

    return "static const %s %s[] = {\n%s\n};\n" % (
        type,
        name,
        self._ListToIndentedString(data))

  def _CreateStatesEnum(self):
    """Print the internal states enum.

    Prints an enum containing all the valid states.

    Returns:
      String containing a C enumeration of the states.
    """
    list = []  # output list

    for state in self._config.states:
      list.append(self._StateInternalC(state))
    return self._CreateEnum(self._Prefix() + 'state_internal_enum', list)

  def _CreateStatesExternal(self):
    """Print a struct with a mapping from internal to external states."""
    list = []  # output list

    for state_name in self._config.states:
      list.append(self._StateExternalC(
                                self._config.states[state_name].external_name))

    return self._CreateStructList(self._Prefix() + 'states_external',
                                  'int',
                                  list)

  def _CreateStatesInternalNames(self):
    """Return a struct mapping internal states to a strings."""
    out = []  # output list

    for state_name in self._config.states:
      out.append('"' + state_name + '"')

    return self._CreateStructList(self._Prefix() + 'states_internal_names',
                                  'char *',
                                  out)

  def _CreateNumStates(self):
    """Print a Macro defining the number of states."""

    return "#define %s_NUM_STATES %s" % (self._config.name.upper(),
                                         str(len(self._config.states) + 1))

  def _ExpandBracketExpression(self, expression):
    """Expand ranges in a regexp bracket expression.

    Returns a string with the ranges in a bracket expression expanded.

    The bracket expression is similar to grep(1) or regular expression bracket
    expressions but it does not support the negation (^) modifier or named
    character classes like [:alpha:] or [:alnum:].

    The especial character class [:default:] will expand to all elements in the
    ascii range.

    For example, the expression 'a-c13A-D' will expand to 'abc13ABCD'.

    Args:
      expression: A regexp bracket expression. Ie: 'A-Z0-9'.

    Returns:
      A string with the ranges in the bracket expression expanded.
    """

    def ExpandRange(start, end):
      """Return a sequence of characters between start and end.

      Args:
        start: first character of the sequence.
        end: last character of the sequence.

      Returns:
        string containing the sequence of characters between start and end.
      """
      return [chr(c) for c in range(ord(start), ord(end) + 1)]

    def ListNext(input_list):
      """Pop the first element of a list.

      Args:
        input_list: python list object.

      Returns:
        First element of the list or None if the list is empty.
      """
      if input_list:
        return input_list.pop(0)
      else:
        return None

    out = []  # List containing the output

    # Special case for the character class [:default:]
    if expression == '[:default:]':
      out = [chr(c) for c in range(0, 255)]
      return ''.join(out)

    chars = [c for c in expression]  # list o characters in the expression.

    current = ListNext(chars)
    while current:
      next = ListNext(chars)
      if next == '-':
        next = ListNext(chars)
        if next:
          out.extend(ExpandRange(current, next))
        else:
          out.append(current)
          out.append('-')
        current = ListNext(chars)
      else:
        out.append(current)
        current = next

    return ''.join(out)

  def _CreateTransitionTable(self):
    """Print the state transition list.

    Returns a set of C structures that define the transition table for the state
    machine. This structure is a list of lists of ints (int **). The outer list
    indexes the source state and the inner list contains the destination state
    for each of the possible input characters:

    const int * const* transitions[source][input] == destination.

    The conditions are mapped from the conditions variable.

    Returns:
      String containing the generated transition table in a C struct.
    """
    out = []          # output list
    default_state = 'STATEMACHINE_ERROR'
    state_table = {}

    for state in self._config.states:
      state_table[state] = [default_state for col in xrange(255)]

    # We process the transition in reverse order while updating the table.
    for i_transition in range(len(self._config.transitions) - 1, -1, -1):
      transition = self._config.transitions[i_transition]
      (condition_name, src, dst) = (transition.condition,
                                    transition.source,
                                    transition.destination)
      condition = self._config.conditions[condition_name]
      char_list = self._ExpandBracketExpression(condition)

      for c in char_list:
        state_table[src][ord(c)] = self._StateInternalC(dst)

    # Create the inner lists which map input characters to destination states.
    for state in self._config.states:
      transition_row = []
      for c in xrange(0, 255):
        transition_row.append('    /* %06s */ %s' % (repr(chr(c)),
                                                     state_table[state][c]))

      out.append(self._CreateStructList('%stransition_row_%s' %
                                        (self._Prefix(),
                                         state),
                                        'int',
                                        transition_row))
      out.append('\n')

    # Create the outer list, which map source states to input characters.
    out.append('static const %s %s[] = {\n' % ('int *', self._Prefix() +
                                               'state_transitions'))

    row_list = ['  %stransition_row_%s' %
                (self._Prefix(), row) for row in self._config.states]
    out.append(',\n'.join(row_list))
    out.append('\n};\n')

    return ''.join(out)

  def Generate(self):
    """Returns the generated the C include statements for the statemachine."""

    print '\n'.join((self._CreateHeader(),
                     self._CreateNumStates(),
                     self._CreateStatesEnum(),
                     self._CreateStatesExternal(),
                     self._CreateStatesInternalNames(),
                     self._CreateTransitionTable()))


def main():
  if len(sys.argv) != 2:
    print "usage: generate_fsm.py config_file"
    sys.exit(1)

  config = FSMConfig()
  config.Load(sys.argv[1])

  gen = FSMGenerateC(config)
  gen.Generate()


if __name__ == "__main__":
  main()
