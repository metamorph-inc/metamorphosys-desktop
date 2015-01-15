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

package tonka.scbus;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.PortUnreachableException;
import java.net.SocketAddress;
import java.util.concurrent.ArrayBlockingQueue;
import java.util.concurrent.BlockingQueue;
import java.util.concurrent.TimeUnit;

import android.os.Handler;
import android.os.Message;
import android.util.Log;

public class SCBusCommThread extends Thread {

	private volatile boolean running = true;
	private DatagramSocket udpSocket;
	private Handler listenerHandler;
	private BlockingQueue<byte[]>	txQueue;
	private SocketAddress serverSocketAddress;
	private SCBusCommReceiverThread receiverThread;
	private String serverName;
	private int serverPort;
	
	public SCBusCommThread(Handler listenerHandler, String serverName, int serverPort) {
		txQueue = new ArrayBlockingQueue<byte[]>(16);
		this.listenerHandler = listenerHandler;
		this.serverName = serverName;
		this.serverPort = serverPort;
	}
	
	public boolean send(byte[] data) {
		return txQueue.offer(data.clone());
	}
	
	private void disconnect() {
		Log.d(SCBus.TAG, "Closing SCBus connection");
		// TODO: might want to send a special "bye" packet here
		
		// Receiver (sub)thread will stop due to socket
		// close and running==false
		if (udpSocket != null) {
			udpSocket.close();
		}
		udpSocket = null;
	}
	
	private void connect() {
		
		assert udpSocket == null;
		
		Log.d(SCBus.TAG, "Connecting to SCBus");
		try {
			udpSocket = new DatagramSocket();
			InetAddress serverIP = InetAddress.getByName(serverName);
			serverSocketAddress = new InetSocketAddress(serverIP, serverPort);
			udpSocket.connect(serverSocketAddress);	// Only sets up outgoing /incoming filtering
			
			// Start receiver (sub) thread
			this.receiverThread = new SCBusCommReceiverThread();
			receiverThread.start();
			
			// TODO: might want to send a special "hello" packet here
			Log.d(SCBus.TAG, "SCBus connected successfully");

		} catch (Exception e) {
			Log.e(SCBus.TAG, "Error in connect: " + e.getMessage());
			disconnect();
		}
	}

	private void sendPacket(long timestamp, byte[] data) {
		
		assert udpSocket != null;
		
		int cid = 0; // TODO: CID assignment for multiple clients
		SCBusPacket packet = new SCBusPacket(
				SCBusPacket.Direction.AndroidToSystemC, timestamp, cid, data);

		Log.d(SCBus.TAG, String.format("Sending packet: " + packet));
		try {
			byte[] udpData = packet.serialize();
			DatagramPacket udpPacket = new DatagramPacket(udpData, udpData.length, serverSocketAddress);
			udpSocket.send(udpPacket);
		} catch (Exception e) {
			e.printStackTrace();
			Log.e(SCBus.TAG, "Unable to send packet: " + e.getMessage());
		}
	}
	
	
	public void reqStop () {
		running = false;
	}
	
	public void run() {
		Log.d(SCBus.TAG, "Communication thread starting");
		connect();
		while (running) {
			try {
				byte[] data = txQueue.poll(SCBus.KEEPALIVE_INTERVAL_MS, TimeUnit.MILLISECONDS);
				long timestamp = System.nanoTime()/1000;
				sendPacket(timestamp, data); // either user data or keepalive (data==null)
            } catch (InterruptedException e) {
            	Log.w(SCBus.TAG, "Hearbeat thread got interrupted");
            }
		}
		Log.d(SCBus.TAG, "Communication thread exiting");
		disconnect();
	}
	
	/////////////////////////////////////
	// Receiver Thread (inner class)
	private class SCBusCommReceiverThread extends Thread {
		private byte[] receiveBuffer = new byte[SCBusPacket.MAX_PACKET_LENGTH];
		
		public void run() {
			Log.d(SCBus.TAG, "Receiver thread starting");
			while (running) {
				try {
					receivePacket();
				} 
				catch (Exception e) {
					if (e instanceof PortUnreachableException) {
						Log.w(SCBus.TAG, "SystemC simulator is unreachable");
					}
					else {
						Log.e(SCBus.TAG, "Error in Receiver Thread: " + e.getMessage());
					}
					
					try {
						sleep(1000); // wait some before retrying
					} catch (InterruptedException e1) {
						Log.e(SCBus.TAG, "Receiver thread is giving up " + e1.getMessage());
						return;	// complete failure
					}
				}
			}
			Log.d(SCBus.TAG, "Receiver thread exiting");
		}
		
		private void checkSync(long local_timestamp, long sc_timestamp) {
			long dT = local_timestamp - sc_timestamp;
			
			if (dT > SCBus.MAX_SYNC_SLACK_US) {
				Log.w(SCBus.TAG, "Synchronization WARNING: SystemC is too slow, dT= " + dT + " us");
			}
			
			if (dT < -SCBus.MAX_SYNC_SLACK_US) {
				Log.w(SCBus.TAG, "Synchronization WARNING: Android is too slow, dT= " + -dT + " us");
			}
		}
		
		private void receivePacket() throws IOException {
			
			assert udpSocket != null;
			
			DatagramPacket udpPacket = new DatagramPacket(receiveBuffer, receiveBuffer.length);
			udpSocket.receive(udpPacket);
			long timestamp = System.nanoTime()/1000;
			
			SCBusPacket packet = SCBusPacket.deserialize(receiveBuffer);
			
			if (packet == null) {
				Log.w(SCBus.TAG, "Received invalid packet.");
				return;
			}
			
			checkSync(timestamp, packet.getTimeStamp());
			// TODO: Client demux beased on CID
			Message msg = listenerHandler.obtainMessage();
			msg.obj = packet;
			msg.sendToTarget();
		}			
	}
}
