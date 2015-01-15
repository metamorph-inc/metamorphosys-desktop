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

"""
This package provides a common API for dealing with Modelica models in an automated
way using different Modelica tools. Currently Dymola and OpenModelica are supported.
DymolaDemo has not been maintained and the user might experience inconveniences
when using this.

Future tool support include JModelica.org.
"""

__authors__ = ["Patrik Meijer", "Zsolt Lattmann"]
__author__ = __authors__[0]

__credits__ = __authors__

__version__ = "14.10" #Versioning: http://www.python.org/dev/peps/pep-0386/

__maintainer__ = __author__
__contact__ = __author__
__email__ = "patrik85@isis.vanderbilt.edu"

__status__ = "Production"
__url__ = 'https://svn.isis.vanderbilt.edu/META/trunk/src/Python27Packages/' + __name__

__copyright__ = "Copyright (C) 2011-2013, Vanderbilt University"
#__license__ = "MIT"

from py_modelica.exception_classes import ModelicaError
from py_modelica.modelica_simulation_tools.om_class import OpenModelica
from py_modelica.modelica_simulation_tools.dymola_class import Dymola
from py_modelica.modelica_simulation_tools.jmodelica import JModelica
from py_modelica.utility_functions import instantiate_logger, run_post_scripts, \
    get_simscript_opts_and_args, write_out_tool_statistics
from py_modelica.report_functions import get_parameters_from_report_json, \
    update_analysis_status, update_metrics_and_check_limits, check_for_invalid_numbers