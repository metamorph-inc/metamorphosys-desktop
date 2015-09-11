package com.metamorphsoftware.aracompass;

import android.app.Activity;
import android.os.Bundle;
import android.text.method.ScrollingMovementMethod;
import android.view.animation.Animation;
import android.view.animation.RotateAnimation;
import android.widget.ImageView;
import android.widget.TextView;
import java.util.Timer;
import java.util.TimerTask;
import android.os.Handler;

import com.metamorphsoftware.i2c.I2CLib;

public class MainActivity extends Activity {

	final byte COMPASS_SLAVE_ADDR = 0x03;
	final byte COMPASS_REG_ADDR = 0x05;
	
	Timer timer;
	TimerTask timerTask;
	
	//we are going to use a handler to be able to run in our TimerTask
	final Handler handler = new Handler();
    
	// define the display assembly compass picture
    private ImageView image;

    // record the compass picture angle turned
    private float currentDegree = 0f;


    TextView tvHeading;
    TextView tvLog;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        image = (ImageView) findViewById(R.id.imageViewCompass);
        tvHeading = (TextView) findViewById(R.id.tvHeading);
        tvLog = (TextView) findViewById(R.id.tvLog);
        tvLog.setMovementMethod(new ScrollingMovementMethod());
    }

    @Override
    protected void onResume() {
        super.onResume();
        //onResume we start our timer so it can start when the app comes from the background
      	startTimer();
    }

    @Override
    protected void onPause() {
        super.onPause();
        // to stop the listener and save battery
        stopTimer();
    }
    
    public void startTimer() {
		//set a new Timer
		timer = new Timer();
		
		//initialize the TimerTask's job
		initializeTimerTask();
		
		//schedule the timer, after the first 100ms the TimerTask will run every 100ms
		timer.schedule(timerTask, 1000, 1000); //
	}

	public void stopTimer() {
		//stop the timer, if it's not already null
		if (timer != null) {
			timer.cancel();
			timer = null;
		}
	}

	public void initializeTimerTask() {
		
		timerTask = new TimerTask() {
			public void run() {
				//use a handler to run a toast that shows the current timestamp
				handler.post(new Runnable() {
					public void run() {
						processCompass();
					}
				});
			}
		};
	}

    public void processCompass() {

        byte[] data = new byte[1];
        data[0] = COMPASS_REG_ADDR; 
        I2CLib.writeI2C((byte)COMPASS_SLAVE_ADDR, data);
        I2CLib.readI2C((byte)COMPASS_SLAVE_ADDR, data);

        // get the angle around the z-axis rotated
        double degree = 360.0*(data[0] & 0xFF)/255.0;
        String degreeStr = String.format("Heading %.2f degrees", degree);
        tvHeading.setText(degreeStr);
        tvLog.append(degreeStr + "\n");
        final int scrollAmount = tvLog.getLayout().getLineTop(tvLog.getLineCount()) - tvLog.getHeight();
        if (scrollAmount > 0) {
        	tvLog.scrollTo(0, scrollAmount);
        }
        else {
        	tvLog.scrollTo(0, 0);
        }
        // create a rotation animation (reverse turn degree degrees)
        RotateAnimation ra = new RotateAnimation(
                currentDegree, 
                (float)-degree,
                Animation.RELATIVE_TO_SELF, 0.5f, 
                Animation.RELATIVE_TO_SELF,
                0.5f);

        // how long the animation will take place
        ra.setDuration(210);

        // set the animation after the end of the reservation status
        ra.setFillAfter(true);

        // Start the animation
        image.startAnimation(ra);
        currentDegree = (float)-degree;

    }
}