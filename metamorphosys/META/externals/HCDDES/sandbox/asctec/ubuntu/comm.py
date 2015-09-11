import math
import serial
import struct
import time
import ctypes as CT
import numpy as NU
import scipy as SC
from array import array

def gen_thrust_cmd(m1,m2,m3,m4):
	ctrl = 0x08;
	csum = CT.c_short(m1+m2+m3+m4+ctrl+0xAAAA).value;
	cmd = struct.pack('<5c6h','>','*','>','d','i',m1,m2,m3,m4,ctrl,csum);

def gen_attitude_cmd(pitch,roll,yaw,thrust):
	ctrl = 0x08;
	csum = CT.c_short(pitch+roll+yaw+thrust+ctrl+0xAAAA).value;
	print csum;
	cmd = struct.pack('<5c6h','>','*','>','d','i',pitch,roll,yaw,thrust,ctrl,csum);
	ser.write(cmd);



#format='<HBhhhhBBBBhhH'
#format='B'
#msg_size=struct.calcsize(format)
msg_size=1

def read_msg():
	st = 0;
	rt = 0;
	while(True):
		#print "st = "+str(st);
		x=ser.read(1)
		if (st == 0) and (x == '>') :
			st = 1;
		elif (st == 1) and (x == '*'):
			st = 2;
		elif (st == 1) and (x != '*'):
			rt = rt + 1;
			print "(st,rt,x) = "+str((st,rt,x));
			st = 0;
		elif (st == 2) and (x == '>'):
			break;
		else:
			rt = rt + 1;
			print "(st,rt,x) = "+str((st,rt,x));
			st = 0;
		if rt > 20:
			return -1;
						
	x=ser.read(2) #read length
	length=struct.unpack('<H',x)
	#print "length = " + str(length);
	x=ser.read(1) #read descriptor
	descriptor=struct.unpack('<B',x)
	#print "descriptor = " + str(descriptor);
	if descriptor == (1,):
		format = 'ihhhhhhhhhHIH' #IMURAWDATA
	elif descriptor == (2,):
		format = 'hhhhBBBBhh' #LLSTATUS
	elif descriptor == (3,):
		format = 'iiiiiihhhhhhiiiiiiiiiiiiiiH' #IMUCALCDATA
	elif descriptor == (11,):
		format = 'iiiiH' #CTRLOUT
	elif descriptor == (15,):
		format = 'HHBH' #RCDATA
	elif descriptor == (20,):
		format = 'BBHBBHHhiiiiH' #WAYPOINT
	elif descriptor == (23,):
		format = 'iiiiiiIIIIiH' #GPSDATA
	elif descriptor == (29,):
		format = 'iiiiiiIIIIiiiiiH' #GPSDATAADVANCED
	else:
		print "Invalid decriptor"
		return -1;
	msg_size=struct.calcsize(format)
	x=ser.read(msg_size)
	z=ser.read(5);
	if len(x) == msg_size:
		y=struct.unpack(format,x)
		print y
		return 0;
	print "Invalid message size"
	return -1;
		
#main
ser = serial.Serial('/dev/ttyUSB0',57600)
result = ser.isOpen()
ser.setTimeout(1)
CT.cdll.LoadLibrary("libc.so.6")
print "Serial port opened:", result
f=open('/home/hosm/test.txt', 'w');
#gen_attitude_cmd(0,0,0,0)
#ser.write(struct.pack('<cccch','>','*','>','m',1))

#gen_attitude_cmd(0,0,0,0)
ser.write(struct.pack('<cccch','>','*','>','m',1))
#ser.write(struct.pack('<ccccH','>','*','>','p',1)) #status request
motor = 1;
dTv = NU.zeros((1000));
i = 0;
try:	
	while(True):
		dt = time.time();
		ser.write(struct.pack('<ccccH','>','*','>','m',motor))
		if(motor == 1):
			motor = 0;
		else:
			motor = 1;
		ser.write(struct.pack('<ccccH','>','*','>','p',1)) #status request
		if read_msg():
			break;
		dT = time.time() - dt;
		dTv[i] = dT;
		i = i + 1;
		print "dT = " + str(dT);
		if i == 1000:
			break;
except:
	#gen_attitude_cmd(0,0,0,0)
	ser.write(struct.pack('<cccch','>','*','>','m',0))	
ser.write(struct.pack('<cccch','>','*','>','m',0))	
mean=SC.mean(dTv)
variance=SC.var(dTv)
stdev=SC.std(dTv)
print "Mean is "+str(mean)
print "Variance is "+str(variance)
print "Standard deviation is "+str(stdev)
i = 0;
sum = 0;
while(True):
	#if dTv[i] > 0.032:
	f.write(str(dTv[i])+'\n');
	sum = dTv[i] + sum;
	i = i + 1;
	if i == 1000:
		break;
#avg = sum / 1000;
#diff_sum = 0;
#i = 0;
#while(True):
#	diff_sum = diff_sum + ((dTv[i] - avg)*(dTv[i]-avg))
#	i=i+1;
#	if i == 1000:
#		break;
#variance = diff_sum / 999;
#stddev = math.sqrt(variance);
#print "average is "+str(avg);
#print "standard deviation is "+str(stddev);
ser.write(struct.pack('<cccch','>','*','>','m',0))	
ser.close()
print "closing"
