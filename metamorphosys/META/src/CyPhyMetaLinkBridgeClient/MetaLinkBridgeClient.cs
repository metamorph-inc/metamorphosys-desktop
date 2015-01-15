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
using System.Threading;

using MetaLinkProtobuf = edu.vanderbilt.isis.meta;

namespace CyPhyMetaLinkBridgeClient
{
    public sealed class MetaLinkBridgeClient
    {
        private const string Host = "127.0.0.1";
        private const string Port = "15150";
        private SocketQueue _socketQueue = null;
        public SocketQueue SocketQueue { get { return _socketQueue; } }
        private Thread _socketSendQueueThread = null;
        private Thread _receiveThread = null;
        private object lockObject = new Object();

        public bool ConnectionEnabled
        {
            get;
            private set;
        }

        public bool EstablishConnection(Action<MetaLinkProtobuf.Edit> EditMessageReceived, Action<Exception> connectionClosed)
        {
            lock (lockObject)
            {
                _socketQueue = new SocketQueue();
                if (!_socketQueue.establishSocket())
                {
                    _socketQueue = null;
                    return false;
                }
                _socketQueue.EditMessageReceived += EditMessageReceived;
                _socketQueue.ReceiveError += x =>
                {
                    CloseConnection();
                    connectionClosed(x);
                };

                _socketSendQueueThread = new Thread(new ThreadStart(_socketQueue.sendThread));
                _socketSendQueueThread.Name = "MetaLinkBridge Send";
                _socketSendQueueThread.Start();

                _receiveThread = new Thread(new ThreadStart(_socketQueue.receiveThread));
                _receiveThread.Name = "MetaLinkBridge Receive";
                _receiveThread.Start();

                ConnectionEnabled = true;
                closeRequested = false;

                return true;
            }
        }

        bool closeRequested = false;
        public bool CloseConnection()
        {
            lock (lockObject)
            {
                if (_socketQueue == null)
                    return true;
                if (!closeRequested)
                {
                    closeRequested = true;

                    if (_socketQueue == null)
                    {
                        return true;
                    }
                    _socketQueue.disconnectSocket();
                    if (Thread.CurrentThread != _receiveThread)
                    {
                        _receiveThread.Abort();
                    }
                    if (Thread.CurrentThread != _socketSendQueueThread)
                    {
                        _socketQueue.sendThreadCancellation.Cancel(true);
                        _socketSendQueueThread.Abort();
                    }
                    _socketQueue = null;
                    ConnectionEnabled = false;
                }
            }
            // If send or receive thread is calling CloseConnection, it will exit after this call
            if (Thread.CurrentThread != _receiveThread)
            {
                _receiveThread.Join();
            }
            if (Thread.CurrentThread != _socketSendQueueThread)
            {
                _socketSendQueueThread.Join();
            }
            return true;
        }

        public bool IsConnectedToBridge()
        {
            lock (lockObject)
            {
                return _socketQueue != null && _socketQueue.IsConnected();
            }
        }

        public bool SendToMetaLinkBridge(MetaLinkProtobuf.Edit message)
        {
            bool status = true;
            if (ConnectionEnabled)
            {
                _socketQueue.enQueue(message);
            }
            else
                status = false;

            return status; // FIXME: doesn't really mean anything, since the send thread could fail. Does the consumer want a callback?
        }
    }
}
