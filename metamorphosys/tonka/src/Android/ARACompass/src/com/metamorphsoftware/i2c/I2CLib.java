package com.metamorphsoftware.i2c;

public class I2CLib {
	public native static int writeI2C(byte slaveAddr, byte[] data);
	public native static int readI2C(byte slaveAddr, byte[] data);
	static{
            System.loadLibrary("I2C");
    }
}
