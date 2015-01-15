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
*/

var username = 'kevin';
var password = 'asdfff';

var config = require('./config.json');
var passwords = require('./passwords.json');

var bcrypt = require('bcrypt');
var fs = require('fs');

bcrypt.genSalt(config.bcrypt_rounds, function(err, salt) {
    bcrypt.hash(password, salt, function(err, hash) {
        var ws = fs.createWriteStream('passwords_new.json');
        passwords[username] = { salt: salt, hash: hash, rounds: config.bcrypt_rounds };
        ws.end(JSON.stringify(passwords, null, 4), function(err) {
            if (err) {
                console.log(err);
            } else {
                fs.renameSync('passwords_new.json', 'passwords.json')
                console.log('success: ' + JSON.stringify(passwords[username]));
            }
        });
    });
});

