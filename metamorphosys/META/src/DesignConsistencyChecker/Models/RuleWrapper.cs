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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DesignConsistencyChecker.DesignRule;
using DesignConsistencyChecker.Framework.Notification;
namespace DesignConsistencyChecker.Models
{
    internal class RuleWrapper : NotifyBase
    {
        public RuleDescriptor Rule { get; private set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(() => IsSelected);
            }
        }

        private ResultTypes _resultType;

        public ResultTypes ResultType
        {
            get { return _resultType; }
            set
            {
                _resultType = value;
                OnPropertyChanged(() => ResultType);
                OnPropertyChanged(() => Icon);
                OnPropertyChanged(() => FeedbackResultMessage);
            }
        }

        private CheckerResultTypes _checkerResultType;
        public CheckerResultTypes CheckerResultType
        {
            get { return _checkerResultType; }
            set
            {
                _checkerResultType = value;
                OnPropertyChanged(() => CheckerResultType);
                OnPropertyChanged(() => CheckerIcon);
                OnPropertyChanged(() => CheckerResultMessage);
            }
        }

        public ImageSource Icon
        {
            get
            {
                if (ResultType == ResultTypes.Undefined) return null;
                if (ResultType == ResultTypes.Error) return new BitmapImage(new Uri("/DesignConsistencyChecker;component/Images/CriticalError.png", UriKind.Relative));
                if (ResultType == ResultTypes.Warning) return new BitmapImage(new Uri("/DesignConsistencyChecker;component/Images/Warning.png", UriKind.Relative));
                if (ResultType == ResultTypes.Ok) return new BitmapImage(new Uri("/DesignConsistencyChecker;component/Images/OK.png", UriKind.Relative));

                return null;
            }
        }

        public ImageSource CheckerIcon
        {
            get
            {
                if (CheckerResultType == CheckerResultTypes.InvalidContext) return new BitmapImage(new Uri("/DesignConsistencyChecker;component/Images/redwarning.png", UriKind.Relative));
                return null;
            }
        }

        public string FeedbackResultMessage
        {
            get
            {
                if (ResultType == ResultTypes.Undefined) return string.Empty;
                if (ResultType == ResultTypes.Error) return "The rule has finished with errors.";
                if (ResultType == ResultTypes.Warning) return "The rule has finished with warnings.";
                if (ResultType == ResultTypes.Ok) return "No errors or warnings.";

                return null;
            }
        }

        public string CheckerResultMessage
        {
            get
            {
                if (CheckerResultType == CheckerResultTypes.InvalidContext) return "Context is not valid for this rule!";
                return string.Empty;
            }
        }

        public RuleWrapper(RuleDescriptor rule)
        {
            Rule = rule;
        }
    }

    internal enum ResultTypes
    {
        Undefined,
        Ok,
        Warning,
        Error
    }

    
}