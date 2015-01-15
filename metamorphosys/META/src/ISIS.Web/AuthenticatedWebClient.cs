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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace ISIS.Web
{
    public abstract class AuthenticatedWebClient : WebClient, IAuthentication
    {
        protected TimeSpan m_SessionTimeOut { get; set; }
        protected Credentials m_credentials { get; set; }
        protected DateTime m_lastLogin = DateTime.MinValue;
        protected bool m_loggedIn { get; set; }

        public AuthenticatedWebClient(string serverUrl, Credentials credentials)
            : base(serverUrl, loggingEnabled: true)
        {
            this.m_SessionTimeOut = TimeSpan.FromMinutes(9);
            this.m_credentials = credentials;
            this.m_loggedIn = false;
        }

        #region IAuthentication

        public abstract void Login();

        public virtual void Logout()
        {
            if (this.m_loggedIn)
            {
                base.SendGetRequest(this.LogoutUrl);
                this.m_loggedIn = false;
            }
        }

        public void Dispose()
        {
            this.Logout();
        }

        public abstract string LoginUrl { get; }

        public abstract string LogoutUrl { get; }

        #endregion

        public override bool DownloadFile(string fileName, string url, WebClient.DownloadPercent callback)
        {
            this.Login();
            return base.DownloadFile(fileName, url, callback);
        }

        public override bool UploadFile(string fileName, string url, WebClient.UploadPercent callback)
        {
            this.Login();
            return base.UploadFile(fileName, url, callback);
        }

        public override string SendDeleteRequest(string url)
        {
            this.Login();
            return base.SendDeleteRequest(url);
        }

        public override T SendGetRequest<T>(string url, Dictionary<string, object> query = null)
        {
            this.Login();
            return base.SendGetRequest<T>(url, query);
        }

        public override string SendGetRequest(string url, Dictionary<string, object> query = null)
        {
            this.Login();
            return base.SendGetRequest(url, query);
        }

        public override T SendPostRequest<T>(string url, Dictionary<string, object> query = null, string content = null, string contentType = null)
        {
            this.Login();
            return base.SendPostRequest<T>(url, query, content, contentType);
        }

        public override string SendPostRequest(string url, Dictionary<string, object> query = null, string content = null, string contentType = null)
        {
            this.Login();
            return base.SendPostRequest(url, query, content, contentType);
        }

        public override string SendPutRequest(string url)
        {
            this.Login();
            return base.SendPutRequest(url);
        }

        public TimeSpan SessionTimeOut
        {
            get { return this.m_SessionTimeOut; }
        }

        public abstract bool TryPing();

    }
}
