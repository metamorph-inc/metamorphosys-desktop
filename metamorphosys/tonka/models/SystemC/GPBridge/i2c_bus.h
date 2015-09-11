#include <systemc.h>
#include <tlm.h>
#include <tlm_utils/simple_initiator_socket.h>
#include <tlm_utils/simple_target_socket.h>



SC_MODULE(i2c_bus) {

	sc_in<bool> SDA_FROM_MASTER;
	sc_in<bool>SDA_FROM_SLAVE;
	sc_out<bool> SDA;
	SC_CTOR(i2c_bus)  {
		

		SC_METHOD(anding);
		sensitive<<SDA_FROM_MASTER<<SDA_FROM_SLAVE;
		dont_initialize();

	}

	
	
	void anding(){

		SDA.write((SDA_FROM_MASTER.read())&&(SDA_FROM_SLAVE.read()));

	}

 
};



