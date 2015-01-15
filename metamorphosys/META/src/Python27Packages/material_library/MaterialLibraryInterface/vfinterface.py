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

import urllib2
import urllib
import cookielib
import json
from contextlib import contextmanager
import re

UNAUTHENTICATED_RE = re.compile(r'/auth/?\?return_to=.*$')


class VFError(Exception):
    """
    Base VehicleForgeInterface Exception

    If this is raised, something unexpected has occurred, most likely due to
    a bug in VehicleForge or misuse of this interface
    """
    pass


class AuthenticationError(VFError):
    """Authentication unsuccessful -- credentials incorrect"""
    pass


def require_authenticated(func):
    def wrapper(interface, *args, **kwargs):
        if not interface._session_id:
            raise VFError('Must be logged in')
        return func(interface, *args, **kwargs)
    return wrapper


class VehicleForgeInterface(object):

    def __init__(self, domain, scheme='https', port=None):
        super(VehicleForgeInterface, self).__init__()

        # set base url
        self.domain = domain
        if port is None:
            port = 443 if scheme == 'https' else 80
        self.base_url = "{scheme}://{domain}".format(
            scheme=scheme, domain=domain)
        if port not in (80, 443):
            self.base_url += ':{}'.format(port)

        # build opener
        self._cookie_jar = cookielib.CookieJar()
        self._opener = urllib2.build_opener(
            urllib2.HTTPCookieProcessor(self._cookie_jar))

        # attributes
        self._username = None
        self._password = None
        self._session_id = None

    def _build_request(self, uri, params=None, method="GET", **kw):
        full_url = self.base_url + uri
        data = None
        if params:
            data = urllib.urlencode(params)
            if method.lower() == "get":
                url, query = urllib.splitquery(full_url)
                if query:
                    data += '&' + query
                full_url = url + '?' + data
                data = None
        return urllib2.Request(full_url, data=data, **kw)

    def open(self, uri, params=None, check_auth=True, method="GET", **kw):
        # do request
        request = self._build_request(uri, params=params, method=method, **kw)
        response = self._opener.open(request)

        # validate
        if check_auth and UNAUTHENTICATED_RE.search(response.url):
            self._login()
            return self.open(uri, params=params, check_auth=False, **kw)

        return response

    def _login(self, username=None, password=None):
        # collect params
        if username is None:
            username = self._username
            if not username:
                raise VFError('login attempted with no username')
        if password is None:
            password = self._password
            if not password:
                raise VFError('login attempted with no password')

        # gen session cookie
        self.open('/auth/', check_auth=False)

        # make login request
        params = {
            'username': username,
            'password': password
        }
        request = self._build_request(
            '/auth/do_login', params=params, method="POST")

        # collect session id
        # cookies = self._cookie_jar._cookies_for_domain(self.domain, request)
        for cookie in self._cookie_jar:
            if cookie.name == '_session_id':
                self._session_id = cookie.value
                break
        else:
            raise VFError('No Session Cookie Found')

        # do login
        request.data += '&_session_id={}'.format(self._session_id)
        response = self._opener.open(request)

        # validate
        if response.url.endswith('/auth/do_login'):
            raise AuthenticationError('Credentials incorrect')

        return response

    @require_authenticated
    def _logout(self):
        self.open('/auth/logout')
        self._cookie_jar.clear_session_cookies()
        self._session_id = None
        self._username = None
        self._password = None

    @contextmanager
    def login(self, username, password):
        self._login(username, password)
        try:
            yield
        finally:
            self._logout()

    @require_authenticated
    def get_api_token(self):
        fp = self.open(
            '/auth/prefs/gen_service_token',
            {'_session_id': self._session_id},
            method="POST"
        )
        rj = json.loads(fp.read())
        return rj['service_token']

    @require_authenticated
    def del_api_token(self):
        fp = self.open(
            '/auth/prefs/gen_service_token',
            {'_session_id': self._session_id},
            method="POST"
        )
        rj = json.loads(fp.read())
        if not rj['success']:
            raise VFError('Unsuccessful deletion')
