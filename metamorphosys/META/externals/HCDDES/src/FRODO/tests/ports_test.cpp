/*** Included Header Files ***/
// Example main.c - This should be generated by the FRODO code generator
#include <gtest/gtest.h>
#define _WINDOWS_
#include "main.h"
#undef _WINDOWS_
#include "logger.h"
#include "arch/highres_timing.h"
#include "error_handler.h"
#include "scheduler.h"
#include "udp.h"
#include "ports.h"


/*****************************************************************************/


namespace {

// The fixture for testing class Foo.
class SamplingPortTest : public ::testing::Test {
protected:
	SamplingPortTest() {
		// You can do set-up work for each test here.
	}
	virtual ~SamplingPortTest() {
		// You can do clean-up work that doesn't throw exceptions here.
	}
	// If the constructor and destructor are not enough for setting up
	// and cleaning up each test, you can define the following methods:
	virtual void SetUp() {
		// Code here will be called immediately after the constructor (right
		// before each test).
	}
	virtual void TearDown() {
		// Code here will be called immediately after each test (right
		// before the destructor).
	}
};


/*****************************************************************************/


/* Test the simple creation of a sampling port */
TEST_F( SamplingPortTest, SimpleCreateSamplingPort ) {
	// Set up all of the in/out parameters for the function call
	PortName_t					name = "SimpleCreateSamplingPort";
	MessageSize_t				maxMessageSizeB = 128;
	PortDirection_t				direction = Bidirectional;
	SystemTime_t				refreshPeriodQ = 0;
	PortId_t					samplingPortID = 21;
	ReturnCode_t				returnCode = InvalidConfig;
	// Call to create the sampling port
	CreateSamplingPort( name, maxMessageSizeB, direction, refreshPeriodQ, &samplingPortID, &returnCode );
	EXPECT_EQ( NoError, returnCode );
	EXPECT_EQ( 0, samplingPortID );
}


/* Test for enough memory to allocate space for sampling port *
TEST_F( SamplingPortTest, MemoryAllocCreateSamplingPort ) {
	// Set up all of the in/out parameters for the function call
	PortName_t					name = "MemoryAllocSamplingPort";
	// Request more space than should be available (POOLSIZE - 128 from previous test)
	MessageSize_t				maxMessageSizeB = FP_PORTMEMORYPOOLSIZE;
	PortDirection_t				direction = Destination;
	SystemTime_t				refreshPeriodQ = 32;
	PortId_t					samplingPortID = 128;
	ReturnCode_t				returnCode = NoError;
	// Call to create the sampling port
	CreateSamplingPort( name, maxMessageSizeB, direction, refreshPeriodQ, &samplingPortID, &returnCode );
	EXPECT_EQ( InvalidConfig, returnCode );
	EXPECT_EQ( 0, samplingPortID );
}


/* Test that sampling port names must be unique */
TEST_F( SamplingPortTest, UniqueNameCreateSamplingPort ) {
	// Set up all of the in/out parameters for the function call
	//		Make sure name is same as previous test
	PortName_t					name = "SimpleCreateSamplingPort";
	MessageSize_t				maxMessageSizeB = 512;
	PortDirection_t				direction = Source;
	SystemTime_t				refreshPeriodQ = 16;
	PortId_t					samplingPortID = 32;
	ReturnCode_t				returnCode = NoError;
	// Call to create the sampling port
	CreateSamplingPort( name, maxMessageSizeB, direction, refreshPeriodQ, &samplingPortID, &returnCode );
	EXPECT_EQ( NoAction, returnCode );
	EXPECT_EQ( 0, samplingPortID );
}


/* Test that sampling port names must be unique */
TEST_F( SamplingPortTest, MessageSizeBoundsCreateSamplingPort ) {
	// Set up all of the in/out parameters for the function call
	//		Make sure name is same as previous test
	char						name[FP_MAXPORTNAMESIZE];
	MessageSize_t				maxMessageSizeB = FP_MAXSAMPLINGMESSAGESIZE;
	PortDirection_t				direction = Source;
	SystemTime_t				refreshPeriodQ = 16;
	PortId_t					samplingPortID = 32;
	ReturnCode_t				returnCode = NoError;
	// Call to create the sampling port
	strncpy( name, "MaxMessageSizeCreateSamplingPort", FP_MAXPORTNAMESIZE );
	CreateSamplingPort( name, maxMessageSizeB, direction, refreshPeriodQ, &samplingPortID, &returnCode );
	EXPECT_EQ( NoError, returnCode );
	EXPECT_EQ( 1, samplingPortID );
	// Now make the message one byte bigger
	maxMessageSizeB = FP_MAXSAMPLINGMESSAGESIZE + 1;
	strncpy( name, "MaxPlusOneCreateSamplingPort", FP_MAXPORTNAMESIZE );
	CreateSamplingPort( name, maxMessageSizeB, direction, refreshPeriodQ, &samplingPortID, &returnCode );
	EXPECT_EQ( InvalidConfig, returnCode );
	EXPECT_EQ( 0, samplingPortID );
}



/* Test that sampling port refresh period bounds are correct */
TEST_F( SamplingPortTest, RefreshPeriodBoundsCreateSamplingPort ) {
	// Set up all of the in/out parameters for the function call
	//		Make sure name is same as previous test
	char						name[FP_MAXPORTNAMESIZE];
	MessageSize_t				maxMessageSizeB = 512;
	PortDirection_t				direction = Source;
	SystemTime_t				refreshPeriodQ = FP_MAXSAMPLINGREFRESHPERIOD;
	PortId_t					samplingPortID = 32;
	ReturnCode_t				returnCode = NoError;
	// Call to create the sampling port
	strncpy( name, "MaxRefreshPeriodCreateSamplingPort", FP_MAXPORTNAMESIZE );
	CreateSamplingPort( name, maxMessageSizeB, direction, refreshPeriodQ, &samplingPortID, &returnCode );
	EXPECT_EQ( NoError, returnCode );
	EXPECT_EQ( 2, samplingPortID );
	// Now make the refresh period one unit longer
	refreshPeriodQ = FP_MAXSAMPLINGREFRESHPERIOD + 1;
	strncpy( name, "MaxQPlusOneCreateSamplingPort", FP_MAXPORTNAMESIZE );
	CreateSamplingPort( name, maxMessageSizeB, direction, refreshPeriodQ, &samplingPortID, &returnCode );
	EXPECT_EQ( InvalidConfig, returnCode );
	EXPECT_EQ( 0, samplingPortID );
}


/* Test that up to FP_MAXSAMPLINGPORTS can be created */
TEST_F( SamplingPortTest, MaxNumberCreateSamplingPort ) {
	// Set up all of the in/out parameters for the function call
	char						name[FP_MAXPORTNAMESIZE];
	MessageSize_t				maxMessageSizeB = 128;
	PortDirection_t				direction = Bidirectional;
	SystemTime_t				refreshPeriodQ = 0;
	PortId_t					samplingPortID = 21;
	ReturnCode_t				returnCode = InvalidConfig;
	// Loop from 0 through max number of sampling ports
	for ( PortId_t i = 3; i < FP_MAXSAMPLINGPORTS; i++ ) {
		// Must permute the name for each port
		_itoa( i, name, 10 );
		// Call to create the sampling port
		CreateSamplingPort( name, maxMessageSizeB, direction, refreshPeriodQ, &samplingPortID, &returnCode );
		EXPECT_EQ( NoError, returnCode );
		EXPECT_EQ( i, samplingPortID );
	}
}


/* Test that more than FP_MAXSAMPLINGPORTS can not be created */
TEST_F( SamplingPortTest, TooManyCreateSamplingPort ) {
	// Set up all of the in/out parameters for the function call
	char						name[] = "TestTooManySamplingPorts";
	MessageSize_t				maxMessageSizeB = 128;
	PortDirection_t				direction = Bidirectional;
	SystemTime_t				refreshPeriodQ = 0;
	PortId_t					samplingPortID = 21;
	ReturnCode_t				returnCode = InvalidConfig;
	// Make sure we get error condition on one more
	CreateSamplingPort( name, maxMessageSizeB, direction, refreshPeriodQ, &samplingPortID, &returnCode );
	EXPECT_EQ( InvalidConfig, returnCode );
	EXPECT_EQ( 0, samplingPortID );
}


/* Make sure we can read and write to sampling port */
TEST_F( SamplingPortTest, SimpleWriteReadSamplingMessage ) {
	PortId_t					samplingPortID = 0;
	char						writeData[] = "1234567890";
	ReturnCode_t				returnCode = InvalidConfig;
	// Try a simple write to the sampling port
	WriteSamplingMessage( samplingPortID, writeData, strlen(writeData), &returnCode );
	EXPECT_EQ( NoError, returnCode );

	// Now try reading from that port
	char						readData[FP_MAXSAMPLINGMESSAGESIZE];
	MessageSize_t				lengthB;
	Validity_t					validity;
	ReadSamplingMessage( samplingPortID, readData, &lengthB, &validity, &returnCode );
	EXPECT_EQ( NoError, returnCode );
	EXPECT_EQ( 0, strncmp( writeData, readData, lengthB ) );
}


}  // namespace


/*****************************************************************************/

