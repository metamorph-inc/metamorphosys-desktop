﻿/*
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

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections.Concurrent;

using MetaLinkProtobuf = edu.vanderbilt.isis.meta;

namespace CyPhyMetaLinkBridgeClient
{
    public class SocketQueue
    {
        public static int port = 15150;
        private BlockingCollection<MetaLinkProtobuf.Edit> _messageQueue = new BlockingCollection<MetaLinkProtobuf.Edit>();

        private Socket _socket = null;
        private NetworkStream _networkStream = null;
        private BufferedStream _bufferedNetworkStream = null;

        //GMEConsole GMEConsole { get; set; }

        public SocketQueue(/*MgaProject mgaProject*/)
        {
            //GMEConsole = GMEConsole.CreateFromProject(mgaProject);
        }

        private Socket tryGetSocket()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //            socket.Blocking = false;

            byte[] byteAddress = new byte[] { 127, 0, 0, 1 };
            IPAddress ipAddress = new IPAddress(byteAddress);
            try
            {
                socket.Connect(ipAddress, port);
            }
            catch (SocketException socketException)
            {
                if (socketException.ErrorCode == 10035)
                { // WSAEWOULDBLOCK
                    bool socketGood = socket.Poll(10000000, SelectMode.SelectWrite);
                    if (!socketGood) return null;
                }
            }

            return socket;
        }

        private Socket getSocket()
        {
            Socket socket = null;

            try
            {
                socket = tryGetSocket();
                if (!socket.Connected)
                    socket = null;
            }
            catch (Exception)
            {
                socket = null;
            }
            //if (socket != null) return socket;

            //ProcessStartInfo processStartInfo = new ProcessStartInfo(@"C:\Path\to\server.exe");
            //processStartInfo.RedirectStandardOutput = true;
            //Process process = null;
            //try {
            //    process = Process.Start(processStartInfo);
            //} catch( Exception exception ) {
            //    terminate(exception.Message);
            //}

            //string status = process.StandardOutput.ReadToEnd();

            //try {
            //    socket = tryGetSocket();
            //} catch( Exception exception ) {
            //    terminate(exception.Message);
            //}

            return socket;
        }

        public bool establishSocket()
        {

            _socket = getSocket();
            if (_socket == null)
            {
                //GMEConsole.Error.WriteLine("CyPhyMLSync cannot establish a connection with the server.  Exiting.");
                return false;
            }

            _networkStream = new NetworkStream(_socket);
            _bufferedNetworkStream = new BufferedStream(_networkStream);
            return true;
        }

        public bool disconnectSocket()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();

            _socket = null;
            return true;
        }

        private void encodeInteger(Stream stream, int number)
        {
            byte[] byteArray = BitConverter.GetBytes(number);
            if (BitConverter.IsLittleEndian) Array.Reverse(byteArray);
            stream.Write(byteArray, 0, byteArray.Length);
        }

        private void encodeInteger(Stream stream, uint number)
        {
            encodeInteger(stream, (int)number);
        }

        private byte[] getCrc32(byte[] data, long dataLength)
        {
            CRC32 crc32 = new CRC32();
            crc32.Initialize();
            byte[] hash = crc32.ComputeHash(data, 0, (int)dataLength);
            Array.Reverse(hash);
            return hash;
        }

        public bool IsConnected()
        {
            bool status = false;
            if (_socket != null)
            {
                //status = _socket.Connected;
                try
                {
                    status = !(_socket.Poll(1, SelectMode.SelectRead) && _socket.Available == 0);
                }
                catch (SocketException)
                {
                    status = false;
                }
            }

            return status;
        }

        private void sendMessage(MetaLinkProtobuf.Edit message)
        {
            MemoryStream messageMemoryStream = new MemoryStream();
            ProtoBuf.Serializer.Serialize(messageMemoryStream, message);
            byte[] payload = messageMemoryStream.GetBuffer();
            int payloadLength = (int)messageMemoryStream.Length;

            MemoryStream frameMemoryStream = new MemoryStream();

            encodeInteger(frameMemoryStream, 0xDEADBEEF);
            encodeInteger(frameMemoryStream, (int)messageMemoryStream.Length);
            encodeInteger(frameMemoryStream, 0);

            byte[] payloadHash = getCrc32(payload, payloadLength);
            frameMemoryStream.Write(payloadHash, 0, 4);

            byte[] frame = frameMemoryStream.GetBuffer();
            int frameLength = (int)frameMemoryStream.Length;
            byte[] frameHash = getCrc32(frame, frameLength);
            frameMemoryStream.Write(frameHash, 0, 4);

            frameMemoryStream.Write(payload, 0, payloadLength);
            frameMemoryStream.Write(payloadHash, 0, 4);

            _networkStream.Write(frameMemoryStream.GetBuffer(), 0, (int)frameMemoryStream.Length);
            _networkStream.Flush();
            // test serialization:
            //  messageMemoryStream.Seek(0, SeekOrigin.Begin);
            //  MetaLinkProtobuf.Edit editdeserialized = ProtoBuf.Serializer.Deserialize<MetaLinkProtobuf.Edit>(messageMemoryStream);
        }

        public void enQueue(MetaLinkProtobuf.Edit message)
        {
            _messageQueue.Add(message);
        }

        public System.Threading.CancellationTokenSource sendThreadCancellation = new System.Threading.CancellationTokenSource();
        public void sendThread()
        {
            try
            {
                while (true)
                {
                    MetaLinkProtobuf.Edit message;
                    if (_messageQueue.TryTake(out message, 1000, sendThreadCancellation.Token))
                    {
                        sendMessage(message);
                    }
                }
            }
            catch (SocketException e)
            {
                ReceiveError(e);
            }
            catch (IOException e)
            {
                ReceiveError(e);
            }
            catch (ObjectDisposedException e)
            {
                ReceiveError(e);
            }
            catch (ProtoBuf.ProtoException e)
            {
                // e.InnerException may be System.Threading.ThreadAbortException
                ReceiveError(e);
            }
            catch (System.OperationCanceledException)
            {
            }
        }

        public event Action<MetaLinkProtobuf.Edit> EditMessageReceived;
        public event Action<Exception> ReceiveError;

        public void receiveThread()
        {
            FrameReader frameReader = new FrameReader(_socket);
            try
            {
                while (true)
                {
                    MetaLinkProtobuf.Edit message = frameReader.getMessage();
                    if (EditMessageReceived != null)
                    {
                        EditMessageReceived(message);
                    }
                }
            }
            catch (SocketException e)
            {
                ReceiveError(e);
            }
            catch (IOException e)
            {
                ReceiveError(e);
            }
            catch (ObjectDisposedException e)
            {
                ReceiveError(e);
            }
            catch (ProtoBuf.ProtoException e)
            {
                ReceiveError(e);
            }
        }

    }
}
