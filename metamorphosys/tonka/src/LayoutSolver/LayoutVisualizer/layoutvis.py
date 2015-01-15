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

__author__ = 'Sandeep'

import Tkinter as Tk
from Tkinter import *
import json
from json import *

json_data = open('layout-output.json').read()
layout = json.loads(json_data)

master = Tk()

w = Canvas(master, width=400, height=400)
w.pack()

for p in layout["packages"]:
    rot = p["rotation"]
    x1 = p["x"]*10
    y1 = p["y"]*10
    if rot == 1:
        x2 = x1 + p["height"]*10
        y2 = y1 + p["width"]*10
    else:
        x2 = x1 + p["width"]*10
        y2 = y1 + p["height"]*10

    if p["layer"] == 1:
        w.create_rectangle(x1, y1, x2, y2, dash=(3,5))
    else:
        w.create_rectangle(x1, y1, x2, y2)

mainloop()


