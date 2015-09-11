
#include <cstdint>
#include <systemc.h>
using namespace sc_core;
using namespace sc_dt;
using namespace std;

#include "tlm.h"
#include "tlm_utils/simple_initiator_socket.h"
#include "tlm_utils/simple_target_socket.h"
#include <bitset>




SC_MODULE(data_bank){

	tlm_utils::simple_target_socket<data_bank> req_recieving_socket;


	sc_uint<8> mem[13];

	sc_uint<8> *CONFIG_REG_A;
	sc_uint<8> *CONFIG_REG_B;
	sc_uint<8> *MODE_REG;
	sc_uint<8> *REG_DATA_X_MSB;
	sc_uint<8> *REG_DATA_X_LSB;
	sc_uint<8> *REG_DATA_Y_MSB;
	sc_uint<8> *REG_DATA_Y_LSB;
	sc_uint<8> *REG_DATA_Z_MSB;
	sc_uint<8> *REG_DATA_Z_LSB;
	sc_uint<8> *REG_STATUS;
	sc_uint<8>*REG_ID_A;
	sc_uint<8>*REG_ID_B;
	sc_uint<8>*REG_ID_C;
	

	SC_CTOR(data_bank){
		

		CONFIG_REG_A = &mem[0];
		CONFIG_REG_B = &mem[1];
		MODE_REG = &mem[2];
		REG_DATA_X_MSB = &mem[3];
		REG_DATA_X_LSB = &mem[4];
		REG_DATA_Y_MSB = &mem[5];
		REG_DATA_Y_LSB = &mem[6];
		REG_DATA_Z_MSB = &mem[7];
		REG_DATA_Z_LSB = &mem[8];
		REG_STATUS = &mem[9];
		REG_ID_A = &mem[10];
		REG_ID_B = &mem[11];
		REG_ID_C = &mem[12];

		mem[1]=0xAA;

		req_recieving_socket.register_b_transport(this, &data_bank::b_transport);



		
	}

	 virtual void b_transport(tlm::tlm_generic_payload & trans, sc_time & delay){

	 	tlm::tlm_command cmd = trans.get_command();
    sc_dt::uint64    adr = trans.get_address();
    unsigned char*   ptr = trans.get_data_ptr();
    unsigned int     len = trans.get_data_length();
   
    

    
     if ( cmd == tlm::TLM_READ_COMMAND ){
      //memcpy(ptr, &mem[1], len);

     	*ptr=0xAA;
     	cout<<"value in ptr "<<bitset<8>(*ptr)<<endl;

  	}
    else if ( cmd == tlm::TLM_WRITE_COMMAND ){
    	uint8_t temp=*ptr;
    	 mem[1]=temp;
     	cout<<"IN bank  "<<mem[1]<<endl;
    }

    // Obliged to set response status to indicate successful completion
    trans.set_response_status( tlm::TLM_OK_RESPONSE );
	 }



};