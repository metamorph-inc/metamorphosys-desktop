#include "test_compass.h"
void master_compass::test_case_1(){

	// set configuration A
	rst.write(false);
	wait(1,SC_SEC);
	cout<<"setting configuration to \n";
	set_configuration(0x10);
	wait(1,SC_SEC);
	wait();
	cout<<"setting mode to single  \n";
	set_mode(0x01);
	wait();
	cout<<"will try to read at "<<sc_time_stamp()<<"\n";
	try_read_all();
	cout<<"set mode to continuous"<<sc_time_stamp()<<"\n";
	cout<<"will try to read at "<<sc_time_stamp()<<"\n";
	try_read_all();
	wait(100,SC_US);
	cout<<"will try to read at "<<sc_time_stamp()<<"\n";
	if(data_rdy)
	try_read_all();
	else
	cout<<"data not available\n";
	

	

}
void master_compass::set_configuration(uint8_t data){

		trans->set_command(static_cast<tlm::tlm_command>(tlm::TLM_WRITE_COMMAND));
		trans->set_address(CONFIG_REG_A);
		trans->set_data_ptr( reinterpret_cast<unsigned char*>(&data) );
		trans->set_data_length(1);
		trans->set_streaming_width(1); 
		trans->set_byte_enable_ptr( 0 ); 
		trans->set_dmi_allowed( false ); 
		trans->set_response_status( tlm::TLM_INCOMPLETE_RESPONSE );
		sending_socket->b_transport(*trans,delay);

	}


void master_compass::set_mode(uint8_t mode){
	trans->set_command(static_cast<tlm::tlm_command>(tlm::TLM_WRITE_COMMAND));
		trans->set_address(MODE_REG);
		trans->set_data_ptr( reinterpret_cast<unsigned char*>(&mode) );
		trans->set_data_length(1);
		trans->set_streaming_width(1); 
		trans->set_byte_enable_ptr( 0 ); 
		trans->set_dmi_allowed( false ); 
		trans->set_response_status( tlm::TLM_INCOMPLETE_RESPONSE );
		sending_socket->b_transport(*trans,delay);

}

void master_compass::try_read(uint16_t addr){
	uint8_t buf;
	trans->set_command(static_cast<tlm::tlm_command>(tlm::TLM_READ_COMMAND));
		trans->set_address(addr);
		trans->set_data_ptr( reinterpret_cast<unsigned char*>(&buf) );
		trans->set_data_length(1);
		trans->set_streaming_width(1); 
		trans->set_byte_enable_ptr( 0 ); 
		trans->set_dmi_allowed( false ); 
		trans->set_response_status( tlm::TLM_INCOMPLETE_RESPONSE );
		sending_socket->b_transport(*trans,delay);
		cout<<"data recieved"<< std::bitset<8>((uint8_t)buf)<<endl;
		wait(delay);

}
void master_compass::try_read_all(){
	
	try_read(REG_DATA_X_MSB);
	try_read(REG_DATA_X_LSB);
	try_read(REG_DATA_Y_MSB);
	try_read(REG_DATA_Y_LSB);
	try_read (REG_DATA_Z_MSB);
	try_read (REG_DATA_Z_LSB);
	

}
