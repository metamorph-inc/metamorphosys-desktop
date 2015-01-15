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

package tonka.xmastree;

import tonka.xmastree.R;

import android.os.Bundle;
import android.preference.PreferenceManager;
import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.ToggleButton;
import tonka.scbus.SCBus;
import tonka.scbus.SCBusListener;

public class MainActivity extends Activity implements SCBusListener, LEDControlListener {

	private final String TAG = "XmasTree";
	private SCBus bus;
	private LEDControlGroup[] ledGroups;   
	
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        ledGroups = new LEDControlGroup[11];
        ledGroups[0] = new LEDControlGroup(this, 
        		(RadioButton) findViewById(R.id.LED0off), 
        		(RadioButton) findViewById(R.id.LED0on), 
        		(RadioButton) findViewById(R.id.LED0blink));
        ledGroups[1] = new LEDControlGroup(this, 
        		(RadioButton) findViewById(R.id.LED1off), 
        		(RadioButton) findViewById(R.id.LED1on), 
        		(RadioButton) findViewById(R.id.LED1blink));
        ledGroups[2] = new LEDControlGroup(this, 
        		(RadioButton) findViewById(R.id.LED2off), 
        		(RadioButton) findViewById(R.id.LED2on), 
        		(RadioButton) findViewById(R.id.LED2blink));
        ledGroups[3] = new LEDControlGroup(this, 
        		(RadioButton) findViewById(R.id.LED3off), 
        		(RadioButton) findViewById(R.id.LED3on), 
        		(RadioButton) findViewById(R.id.LED3blink));
        ledGroups[4] = new LEDControlGroup(this, 
        		(RadioButton) findViewById(R.id.LED4off), 
        		(RadioButton) findViewById(R.id.LED4on), 
        		(RadioButton) findViewById(R.id.LED4blink));
        ledGroups[5] = new LEDControlGroup(this, 
        		(RadioButton) findViewById(R.id.LED5off), 
        		(RadioButton) findViewById(R.id.LED5on), 
        		(RadioButton) findViewById(R.id.LED5blink));
        ledGroups[6] = new LEDControlGroup(this, 
        		(RadioButton) findViewById(R.id.LED6off), 
        		(RadioButton) findViewById(R.id.LED6on), 
        		(RadioButton) findViewById(R.id.LED6blink));
        ledGroups[7] = new LEDControlGroup(this, 
        		(RadioButton) findViewById(R.id.LED7off), 
        		(RadioButton) findViewById(R.id.LED7on), 
        		(RadioButton) findViewById(R.id.LED7blink));
        ledGroups[8] = new LEDControlGroup(this, 
        		(RadioButton) findViewById(R.id.LED8off), 
        		(RadioButton) findViewById(R.id.LED8on), 
        		(RadioButton) findViewById(R.id.LED8blink));
        ledGroups[9] = new LEDControlGroup(this, 
        		(RadioButton) findViewById(R.id.LED9off), 
        		(RadioButton) findViewById(R.id.LED9on), 
        		(RadioButton) findViewById(R.id.LED9blink));
        ledGroups[10] = new LEDControlGroup(this, 
        		(RadioButton) findViewById(R.id.LED10off), 
        		(RadioButton) findViewById(R.id.LED10on), 
        		(RadioButton) findViewById(R.id.LED10blink));
        for (LEDControlGroup lcg : ledGroups) {
    		lcg.setEnabled(false);
    	}
        bus = new SCBus();
    }

    public void onBusButton(View view) {
    	boolean on = ((ToggleButton) view).isChecked();
        if (on) {
        	SharedPreferences SP = PreferenceManager.getDefaultSharedPreferences(getBaseContext());
        	
        	if (SP.getBoolean("systemc_simulator_local", true)) {
        		bus.open(this);
        		Log.d(TAG, "Opening local bus connection");
        	}
        	else {
        		String systemcAddress = SP.getString("systemc_simulator_address", SCBus.SYSTEMC_SERVER_DEFAULT);
        		bus.open(this, systemcAddress);
        		Log.d(TAG, "Opening remote bus connection to " + systemcAddress);
        	}
        	
        	String initSeq = "";
        	for (LEDControlGroup lcg : ledGroups) {
        		lcg.setEnabled(true);
        		initSeq = initSeq + lcg.getCurrentTag() + "\0";
        	}
        	
        	Log.d(TAG, "initializing all leds");
    		bus.send((initSeq).getBytes());
        	
        } 
        else {
        	bus.close();
        	for (LEDControlGroup lcg : ledGroups) {
        		lcg.setEnabled(false);
        	}
        }
        //Button sendButton = (Button) findViewById(R.id.sendButton);
        //sendButton.setEnabled(on);
    }
    
    public void onSendButton(View view) {
    	//EditText text = (EditText) findViewById(R.id.sendText);
    	//bus.send(text.getText().toString().getBytes());
    	//text.setText("");
    }
    
    protected void onDestroy() {
    	Log.d(TAG, "onDestroy");
    	bus.close();
    	super.onDestroy();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.main, menu);
        return true;
    }
    
    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        if(item.getItemId()==R.id.action_settings)
        {
            startActivity(new Intent(this, AppPreferences.class));
            return true;
        }
        return false;
    }

	@Override
	public void dataReceived(byte[] data) {
		Log.d(TAG, "dataReceived :" + new String(data));
		//EditText text = (EditText) findViewById(R.id.receivedText);
		//text.append(new String(data));
	}

	@Override
	public void LEDSelected(String tag, RadioButton ledButton) {
		Log.d(TAG, "led command tag:" + tag);
		bus.send((tag + "\0").getBytes());
	}
    
	
    
}
