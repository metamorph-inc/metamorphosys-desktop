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

import os
import sys
import logging
from CAD_assembler import CADException

ROOT_DIR = os.getcwd()  # make sure to change back to this when exiting

try:
    import py_modelica as pym
    print 'Found py_modelica in virtual python environment'
except ImportError as err:
    print err.message
    print 'Use META virtual python environment'

from optparse import OptionParser

parser = OptionParser()
parser.add_option("-d", "--driver", dest="driver",
                  help="Name of the driver to be executed")


def _exit_on_failure(error_message):
    """ Function for exiting after a failure.
    """
    os.chdir(ROOT_DIR)
    print error_message
    import traceback
    with open('_FAILED.txt', 'wb') as f_out:
        f_out.writelines(error_message)
        f_out.writelines(traceback.format_exc())
    pym.update_analysis_status('FAILED', error_message)
    log = logging.getLogger()
    log.error(error_message)
    log.error("Exception was raised : {0}".format(traceback.format_exc()))
    sys.exit(1)


def main():
    if not os.path.isdir('log'):
        os.mkdir('log')
    pym.instantiate_logger(os.path.join('log', 'CAD_PET.log'))
    (options, args) = parser.parse_args()
    driver_name = options.driver
    driver = __import__(driver_name)
    try:
        driver.main()
    except CADException as e:
        _exit_on_failure(e.message)
    pym.check_for_invalid_numbers()
    pym.update_analysis_status('OK')


if __name__ == '__main__':
    try:
        main()
    except Exception as err:
        try:
            _exit_on_failure('{0} (not a CADException)'.format(err.message))
        except Exception:
            sys.exit(13)  # Just in case something should fail in the exit function