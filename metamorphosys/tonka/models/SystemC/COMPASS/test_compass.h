#ifndef MASTER_COMPASS_H
#define MASTER_COMPASS_H
#include <cstdint>
#include <systemc.h>
#include <bitset>
#include "tlm.h"
#include "tlm_utils/simple_initiator_socket.h"
#include "tlm_utils/simple_target_socket.h"

SC_MODULE(master_compass) {

	// ports
	sc_in<bool> clk;
	sc_out<bool>   rst;
	sc_in<bool> data_rdy;

	sc_time delay;


	uint16_t CONFIG_REG_A;
	uint16_t CONFIG_REG_B;
	uint16_t MODE_REG;
	uint16_t REG_DATA_X_MSB;
	uint16_t REG_DATA_X_LSB;
	uint16_t REG_DATA_Y_MSB;
	uint16_t REG_DATA_Y_LSB;
	uint16_t REG_DATA_Z_MSB;
	uint16_t REG_DATA_Z_LSB;
	uint16_t REG_STATUS;
	uint16_t REG_ID_A;
	uint16_t REG_ID_B;
	uint16_t REG_ID_C;



	tlm::tlm_generic_payload* trans;
	tlm_utils::simple_initiator_socket<master_compass> sending_socket;

	SC_CTOR(master_compass) : sending_socket("sending_socket") {

		CONFIG_REG_A = 0;
		CONFIG_REG_B = 1;
		MODE_REG = 2;
		REG_DATA_X_MSB = 3;
		REG_DATA_X_LSB = 4;
		REG_DATA_Y_MSB = 5;
		REG_DATA_Y_LSB = 6;
		REG_DATA_Z_MSB = 7;
		REG_DATA_Z_LSB = 8;
		REG_STATUS = 9;
		REG_ID_A = 10;
		REG_ID_B = 11;
		REG_ID_C = 12;
		trans = new tlm::tlm_generic_payload;
		delay=sc_time(10,SC_US);
		SC_THREAD(test_case_1);
		sensitive<<clk;



	}
	void test_case_1();
	void set_configuration(uint8_t );
	void set_mode(uint8_t);
	void try_read(uint16_t );
	void try_read_all();


};

#endif // COMPASS_H