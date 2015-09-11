
#ifndef MODULE_GPIO_H
#define MODULE_GPIO_H

#include <systemc.h>
#include <tlm.h>
#include <tlm_utils/simple_initiator_socket.h>
#include <tlm_utils/simple_target_socket.h>

#include "compat.h"

SC_MODULE(module_i2c)
{
	sc_out<bool> SDA_OUT;
	sc_in<bool>SDA_IN;
	sc_out <bool> SCL;
	sc_in<bool> clk;
			
	sc_bit temp1, temp2;


	
	sc_event e1,e2,e3;
    tlm::tlm_generic_payload* trans;
    tlm_utils::simple_target_socket<module_i2c> recieving_socket;

	sc_uint<8>  rx_reg,tx_reg;
	uint8_t tx_reg1 ,rx_reg1;

	bool flag,flag_wait,thread_running,sending,start_bit,send_stop,value;
	int tx_reg_ptr_write,rx_reg_ptr_read;
	SC_CTOR(module_i2c){
        trans = new tlm::tlm_generic_payload;
		flag=0;
		flag_wait=1;
		send_stop=0;
		thread_running=false;
		tx_reg_ptr_write=0;rx_reg_ptr_read=0;
		start_bit=0;
		

        recieving_socket.register_b_transport(this, &module_i2c::b_transport);

		SC_THREAD(send_data);
		sensitive<<e1;
		dont_initialize();

		SC_METHOD(toggleSCL);
		sensitive<<e2;
		dont_initialize();

		SC_METHOD(write_a_bit);
		sensitive<<e3;
		dont_initialize();
    }

    //private:

    virtual void b_transport(tlm::tlm_generic_payload&, sc_time&);
	void toggleSCL();
	void send_data();
	void sendstart();
	void send_stop_bit();
	void read_data();
	int read_a_bit();
	void write_a_bit();

 
};


#endif