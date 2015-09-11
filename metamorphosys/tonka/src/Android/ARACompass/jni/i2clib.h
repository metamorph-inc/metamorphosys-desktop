#include <jni.h>

//should be modified according your package name

#ifndef _i2clib_h_
#define _i2clib_h_
#ifdef __cplusplus
extern "C" {
#endif

//should be modified according your package name
JNIEXPORT jint JNICALL Java_com_metamorphsoftware_i2c_I2CLib_writeI2C(JNIEnv *, jclass, jbyte slaveAddr, jbyteArray data);
JNIEXPORT jint JNICALL Java_com_metamorphsoftware_i2c_I2CLib_readI2C(JNIEnv *, jclass, jbyte slaveAddr, jbyteArray data);

#ifdef __cplusplus
}
#endif
#endif
