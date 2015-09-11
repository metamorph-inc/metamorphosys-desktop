
#include <cstdint>
#include <systemc.h>

#include "tlm.h"
#include "tlm_utils/simple_initiator_socket.h"
#include "tlm_utils/simple_target_socket.h"

using namespace sc_core;
using namespace sc_dt;
using namespace std;




//===i2c constants=======

#define STD 2//(US) //100KHZ scl line(10US)
#define FAST 750//(NS)//400KHZ SCl line(2.5us)


SC_MODULE(i2c_bridge){

	sc_in<bool> SDA_IN;
	sc_out<bool>SDA_OUT;
	sc_in <bool> SCL;
	
	
	sc_bit temp1, temp2;
	sc_time delay_send;





 	tlm_utils::simple_initiator_socket<i2c_bridge> i2c_data_socket;
 	 tlm::tlm_generic_payload* send_trans = new tlm::tlm_generic_payload;

	sc_uint<8> mem[13];

	

	sc_uint<8> rx_reg,tx_reg;
	sc_uint<8> temp_adr_reg;
	sc_uint<8> adr_rev;
	sc_uint<8> my_addr;
	sc_uint<8> temp_rev_buf,temp_send_buf;

	bool addr;
	bool Rd_W;
	sc_uint<8> reg_addr;
	bool Start_det;
	bool flag;
	bool data_received;
	bool adr_reg;

	SC_CTOR(i2c_bridge): i2c_data_socket("i2c_data_socket"){
		rx_reg=0xFF;
		adr_rev=0xAA;
		addr=false;
		my_addr=0xAA;
		reg_addr=0x03;
		Start_det=false;
		data_received=false;
		adr_reg=false;
		Rd_W=false;

		mem[1]=0xAA;


		SC_THREAD(initiate);
		dont_initialize();
		sensitive<<SCL;
	}
	void initiate();
	inline int read_a_bit();
	void send_tlm(bool);
};
