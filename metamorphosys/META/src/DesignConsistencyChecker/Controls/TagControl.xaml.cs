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

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using DesignConsistencyChecker.Framework.Command;

namespace DesignConsistencyChecker.Controls
{
    /// <summary>
    /// Interaction logic for TagControl.xaml
    /// </summary>
    internal partial class TagControl
    {

        #region Commands

        private DelegateCommand _addTagCommand;
        public DelegateCommand AddTagCommand
        {
            get { return _addTagCommand; }
            set
            {
                _addTagCommand = value;
                OnPropertyChanged(() => AddTagCommand);
            }
        }

        private DelegateCommand _removeTagCommand;
        public DelegateCommand RemoveTagCommand
        {
            get { return _removeTagCommand; }
            set
            {
                _removeTagCommand = value;
                OnPropertyChanged(() => RemoveTagCommand);
            }
        }

        private DelegateCommand _selectAllTagCommand;
        public DelegateCommand SelectAllTagCommand
        {
            get { return _selectAllTagCommand; }
            set
            {
                _selectAllTagCommand = value;
                OnPropertyChanged(() => SelectAllTagCommand);
            }
        }

        private DelegateCommand _clearTagSelectionCommand;
        public DelegateCommand ClearTagSelectionCommand
        {
            get { return _clearTagSelectionCommand; }
            set
            {
                _clearTagSelectionCommand = value;
                OnPropertyChanged(() => ClearTagSelectionCommand);
            }
        }

        #endregion

        #region Properties
        
        private string _textAdd;
        public string TextAdd
        {
            get { return _textAdd; }
            set
            {
                _textAdd = value;
                OnPropertyChanged(() => TextAdd);
            }
        }

        #region Dependency properties

        public static readonly DependencyProperty AllTagsProperty =
            DependencyProperty.Register("AllTags", typeof(ObservableCollection<string>), typeof(TagControl), new PropertyMetadata(default(ObservableCollection<string>)));

        public ObservableCollection<string> AllTags
        {
            get { return (ObservableCollection<string>)GetValue(AllTagsProperty); }
            set { SetValue(AllTagsProperty, value); }
        }

        public static readonly DependencyProperty SelectedTagsProperty =
            DependencyProperty.Register("SelectedTags", typeof (ObservableCollection<string>), typeof (TagControl), new PropertyMetadata(default(ObservableCollection<string>)));

        public ObservableCollection<string> SelectedTags
        {
            get { return (ObservableCollection<string>) GetValue(SelectedTagsProperty); }
            set { SetValue(SelectedTagsProperty, value); }
        }

        //private ObservableCollection<string> _selectedTags;
        //public ObservableCollection<string> SelectedTags
        //{
        //    get { return _selectedTags; }
        //    set
        //    {
        //        _selectedTags = value;
        //        OnPropertyChanged(() => SelectedTags);
        //    }
        //}

        #endregion

        #endregion

        public TagControl()
        {
            InitializeComponent();
            rGrid.DataContext = this;
            SelectedTags = new ObservableCollection<string>();
            AllTags = new ObservableCollection<string>();


            AddTagCommand = new DelegateCommand(o =>
                                                    {
                                                        var s = (string)o;
                                                        if (SelectedTags == null || AllTags == null) return;
                                                        SelectedTags.Add(s);
                                                        AllTags.Remove(s);
                                                    }, o => true);

            RemoveTagCommand = new DelegateCommand(o =>
            {
                var s = (string)o;
                if (SelectedTags == null || AllTags == null) return;
                SelectedTags.Remove(s);
                AllTags.Add(s);
            }, o => true);

            ClearTagSelectionCommand = new DelegateCommand(o =>
                                                               {
                                                                   if (SelectedTags == null || AllTags == null) return;
                                                                   foreach (var selectedTag in SelectedTags)
                                                                   {
                                                                       AllTags.Add(selectedTag);
                                                                   }

                                                                   SelectedTags.Clear();
                                                               }, o => true);

            SelectAllTagCommand = new DelegateCommand(o =>{
                                                              if (SelectedTags == null || AllTags == null) return;
                                                              foreach (var tag in AllTags)
                                                              {
                                                                  SelectedTags.Add(tag);
                                                              }
                                                              AllTags.Clear();
            }, o=>true);

        }
    }
}
