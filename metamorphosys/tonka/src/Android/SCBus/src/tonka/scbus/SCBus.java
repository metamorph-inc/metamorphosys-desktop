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

import android.os.Handler;
import android.os.Message;
import android.util.Log;

public class SCBus {
	public final static String TAG = "SCBus";
	public final static String SYSTEMC_SERVER_DEFAULT = "10.0.2.2";
	public final static int SYSTEMC_PORT_DEFAULT = 7674;
	protected final static int KEEPALIVE_INTERVAL_MS = 1000;
	protected final static int MAX_SYNC_SLACK_US = 5000;

	private SCBusCommThread commThread;
	private SCBusListenerHandler listenerHandler;
	
	public void open(SCBusListener l) {
		open(l, SYSTEMC_SERVER_DEFAULT, SYSTEMC_PORT_DEFAULT);
	}
	
	public void open(SCBusListener l, String serverName) {
		open(l, serverName, SYSTEMC_PORT_DEFAULT);
	}
		
	public void open(SCBusListener l, String serverName, int serverPort) {
		if (commThread != null) {
			Log.w(SCBus.TAG, "Multiple/repeated bus open attempts");
			return;
		}
		listenerHandler = new SCBusListenerHandler(l);
		commThread = new SCBusCommThread(listenerHandler, serverName, serverPort);
		commThread.start();
	}
	
	public void close() {
		if (commThread == null) {
			Log.w(SCBus.TAG, "Bus already closed");
			return;
		}
		
		commThread.reqStop();
		try {
			commThread.join(1000); // Wait first gracefully
			if (commThread.isAlive()) {
				Log.w(SCBus.TAG, "Cannot stop heartbeat thread peacefully, trying brute force");
				commThread.interrupt();
			}
		} catch (InterruptedException e) {
			Log.w(SCBus.TAG, "Interrupted while waiting for heartbeat thread" + e.getMessage());
		}
		listenerHandler.removeMessages(0);

		commThread = null;
		listenerHandler = null;
	}

	public void finalize() {
		close();
	}

	public boolean send(byte[] data) {
		return (commThread != null) && commThread.send(data);
	}

}

class SCBusListenerHandler extends Handler {
	private SCBusListener listener;
	public SCBusListenerHandler(SCBusListener l) {
		listener = l;
	}
	
	public void handleMessage(Message msg) {
		assert msg != null;
		if (listener != null) {
			SCBusPacket packet = (SCBusPacket)msg.obj;
			listener.dataReceived(packet.getData());
		}
		super.handleMessage(msg);
	}
}
