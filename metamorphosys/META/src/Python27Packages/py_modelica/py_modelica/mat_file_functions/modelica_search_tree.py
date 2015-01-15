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

class TreeNode():
    name = ''
    children = None

    def __init__(self, name):
        self.name = name
        self.children = []
    # end of __init__

    def nbr_of_children(self):
        if self.children:
            return len(self.children)
        else:
            return 0
    # end of nbr_of_children

    def add_child(self, new_node, i=0):
        self.children.insert(i, new_node)
    # end of add_child

    def populate_tree(self, node_list):
        """
        Called on the root_node 
        """
        for node in node_list:
            self.place_in_tree(node.split('.'))
    # end of populate_tree
   
    def place_in_tree(self, name_pieces):
        i = 0
        nbr_children = self.nbr_of_children()
        if nbr_children != 0:
            while name_pieces[0] >= self.children[i].name:
                if name_pieces[0] == self.children[i].name:
                    self.children[i].place_in_tree(name_pieces[1:])
                    return
                else:
                    i += 1
                    if i == nbr_children:
                        break
                
        new_node = TreeNode(name_pieces[0])
        self.add_child(new_node, i)
        node = new_node
        for name in name_pieces[1:]:
            new_node = TreeNode(name)
            node.add_child(new_node)
            node = new_node
    # end of place_in_tree

    def search_tree(self, search_name):
        """
        Called on root_node when search begins.

        """  
        name_pieces = search_name.split('.')
        for child in self.children:
            if name_pieces[0] < child.name:
                return False
            elif child.name == name_pieces[0]:
                if len(name_pieces) == 1:
                    return True
                else: 
                    return child.search_tree_rec(name_pieces[1:])
        return False
    # end of search_tree

    def search_tree_rec(self, name_pieces):
        nbr_children = self.nbr_of_children()
        if nbr_children == 0:
            if name_pieces:
                return name_pieces[0] == self.name
            else:
                return False
        else :
            if self.children[0].name == '':
                return True
            elif self.children[0].name == '*' and len(name_pieces) == 1:
                return True
            for child in self.children:
                if name_pieces[0] < child.name :
                    return False
                elif child.name == name_pieces[0]:
                    if len(name_pieces) == 1:
                        return True
                    else:
                        return child.search_tree_rec(name_pieces[1:])

            return False
        # end of search_tree_rec

# end of tree_node


  






