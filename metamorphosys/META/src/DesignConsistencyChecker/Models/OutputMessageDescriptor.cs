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
using System.Windows.Media.Imaging;
using DesignConsistencyChecker.Framework.Notification;
using DesignConsistencyChecker.DesignRule;
using System.Windows.Media;

namespace DesignConsistencyChecker.Models
{
    internal class OutputMessageDescriptor : NotifyBase
    {
        private RuleFeedbackBase _feedback;
        public RuleFeedbackBase Feedback
        {
            get { return _feedback; }
            set
            {
                _feedback = value;
                OnPropertyChanged(() => Feedback);
                OnPropertyChanged(() => Icon);
            }
        }

        public ImageSource Icon
        {
            get {
                if (Feedback == null) return null;
                if (Feedback.FeedbackType == FeedbackTypes.Error) return new BitmapImage(new Uri("/DesignConsistencyChecker;component/Images/CriticalError.png", UriKind.Relative));
                if (Feedback.FeedbackType == FeedbackTypes.Warning) return new BitmapImage(new Uri("/DesignConsistencyChecker;component/Images/Warning.png", UriKind.Relative));
                return null;
            }
        }

        //private string _message;
        //public string Message
        //{
        //    get { return _message; }
        //    set
        //    {
        //        _message = value;
        //        OnPropertyChanged(() => Message);
        //    }
        //}

        //private string _elementName;
        //public string ElementName
        //{
        //    get { return _elementName; }
        //    set
        //    {
        //        _elementName = value;
        //        OnPropertyChanged(() => ElementName);
        //    }
        //}

        //private string _elementId;
        //public string ElementId
        //{
        //    get { return _elementId; }
        //    set
        //    {
        //        _elementId = value;
        //        OnPropertyChanged(() => ElementId);
        //    }
        //}

        public OutputMessageDescriptor()
        {
            //Icon = @"\Images\CriticalError.png";
        }
    }
}