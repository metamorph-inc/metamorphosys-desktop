#include "i2clib.h"
#include <termios.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <string.h>
#include <jni.h>
#include <unistd.h>
#include <stdio.h>
#include <errno.h>

#include <linux/i2c.h>
#include <memory.h>
#include <malloc.h>

//library for log
#include <android/log.h>
#define APPNAME "ARACompass"
#define I2C_DEV_FILE "/dev/i2c-6"

static const char *TAG="I2C";

#define LOGI(fmt, args...) __android_log_print(ANDROID_LOG_INFO,  TAG, fmt, ##args)
#define LOGD(fmt, args...) __android_log_print(ANDROID_LOG_DEBUG, TAG, fmt, ##args)
#define LOGE(fmt, args...) __android_log_print(ANDROID_LOG_ERROR, TAG, fmt, ##args)

JNIEXPORT jint JNICALL Java_com_metamorphsoftware_i2c_I2CLib_writeI2C(JNIEnv * env, jclass clazz,
		jbyte slaveAddr, jbyteArray data)
{

    int res = 0, i = 0, j = 0;


    //open the file that corresponds to the i2c on the Ltouch board
    int fd = open(I2C_DEV_FILE, O_RDWR);

    if (fd < 0) {
        __android_log_print(ANDROID_LOG_VERBOSE, APPNAME, "could not open device file: %s", strerror(errno));
        return -1;
    }

    //specify the Address of the slave device (in my project the Arduino)
    res = ioctl(fd, I2C_SLAVE, (int)slaveAddr);
    if (res != 0) {
        __android_log_print(ANDROID_LOG_VERBOSE, APPNAME, "can't set slave address: %s", strerror(errno));
        return -2;
    }

    jbyte* bufferPtr = (*env)->GetByteArrayElements(env, data, NULL);
    jsize bufferLength = (*env)->GetArrayLength(env, data);

    if (bufferPtr == NULL) {
        __android_log_print(ANDROID_LOG_VERBOSE, APPNAME, "no valid data to be written");
        return -3;
    }

    //write data
    if ((j = write(fd, (char*)bufferPtr, bufferLength)) != bufferLength) {
        __android_log_print(ANDROID_LOG_VERBOSE, APPNAME, "write fail i2c (%d): %s", j, strerror(errno));
    	(*env)->ReleaseByteArrayElements(env, data, bufferPtr, 0);
        return -4;
    }

    __android_log_print(ANDROID_LOG_VERBOSE, APPNAME, "I2C: %d byte(s) written", j);

    close(fd);

	(*env)->ReleaseByteArrayElements(env, data, bufferPtr, 0);
    return 0;
}

JNIEXPORT jint JNICALL Java_com_metamorphsoftware_i2c_I2CLib_readI2C(JNIEnv * env, jclass clazz,
		jbyte slaveAddr, jbyteArray data)
{

    int res = 0, i = 0, j = 0;


    //open the file that corresponds to the i2c on the Ltouch board
    int fd = open(I2C_DEV_FILE, O_RDONLY);

    if (fd < 0) {
        __android_log_print(ANDROID_LOG_VERBOSE, APPNAME, "could not open device file: %s", strerror(errno));
        return -1;
    }

    //specify the Address of the slave device (in my project the Arduino)
    res = ioctl(fd, I2C_SLAVE, (int)slaveAddr);
    if (res != 0) {
        __android_log_print(ANDROID_LOG_VERBOSE, APPNAME, "can't set slave address: %s", strerror(errno));
        return -2;
    }

    jbyte* bufferPtr = (*env)->GetByteArrayElements(env, data, NULL);
    jsize bufferLength = (*env)->GetArrayLength(env, data);

    if (bufferPtr == NULL) {
        __android_log_print(ANDROID_LOG_VERBOSE, APPNAME, "no valid data buffer is provided");
        return -3;
    }

    //write data
    if ((j = read(fd, (char*)bufferPtr, bufferLength)) != bufferLength) {
        __android_log_print(ANDROID_LOG_VERBOSE, APPNAME, "read fail i2c (%d): %s", j, strerror(errno));
    	(*env)->ReleaseByteArrayElements(env, data, bufferPtr, 0);
        return -4;
    }

    __android_log_print(ANDROID_LOG_VERBOSE, APPNAME, "I2C: %d byte(s) read", j);

    close(fd);

	(*env)->ReleaseByteArrayElements(env, data, bufferPtr, 0);
    return 0;
}

