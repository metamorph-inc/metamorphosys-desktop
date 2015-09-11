

#include "module.h"
#include "i2c_handler.h"

#include "greybus_protocols.h"
#include <bitset>
#include <systemc>
#include "i2c-linux.h"

void I2C::i2c_type_protocol_version(struct protocol_version_rsp* pv_rsp)
{
    pv_rsp->version_major = 0x00;
    pv_rsp->version_minor = 0x01;
}
void I2C::i2c_type_get_functionality(struct gb_i2c_functionality_response* i2c_fcn_rsp ){
	i2c_fcn_rsp->functionality = htole32(I2C_FUNC_I2C|I2C_FUNC_SMBUS_EMUL);
}

void I2C::i2c_type_transfer(struct gb_i2c_transfer_request* i2c_xfer_req,
											struct gb_i2c_transfer_response* i2c_xfer_rsp, size_t* payload_size){

	bool read_op;

	uint16_t op_count = le16toh(i2c_xfer_req->op_count);

	
	int read_count=0;
	for(int i=0;i<op_count;i++){
		struct gb_i2c_transfer_op *op;
			uint16_t addr;
			

			uint16_t flags;
			uint16_t size;
			
			op = &i2c_xfer_req->ops[i];
			addr = le16toh(op->addr);
			
			


			flags = le16toh(op->flags);
			size = le16toh(op->size);
			read_op = (flags & I2C_M_RD) ? true : false;

			read_op=true;///JUST a test delete later

			//need to modify address with read or write accordingly
			uint8_t actual_addr = (addr & 0xff); //ask peter about it coverting 16 bit address to 7 bit address
			
			if (read_op) {
				actual_addr <<= 1;
				actual_addr|=(1<<0);
			}
			else {
				actual_addr <<= 1;
				actual_addr&=~(1<<0);
			}

			


			 //Send address
			cout<<" MASTER: Writing Address"<<bitset<8>(actual_addr)<<endl;
			send_req_trans->set_command( static_cast<tlm::tlm_command>(tlm::TLM_WRITE_COMMAND) );
			send_req_trans->set_address(1);// 1 for tx_reg
			send_req_trans->set_data_ptr( reinterpret_cast<unsigned char*>(&actual_addr) );
			send_req_trans->set_data_length( 1 );
			send_req_trans->set_streaming_width( 1 ); // = data_length to indicate no streaming
			send_req_trans->set_byte_enable_ptr( 0 ); // 0 indicates unused
			send_req_trans->set_dmi_allowed( false ); // Mandatory initial value
			send_req_trans->set_response_status( tlm::TLM_INCOMPLETE_RESPONSE ); // Mandatory initial value
			request_sending_socket->b_transport( *send_req_trans, delay_send);
			wait(200,SC_US);
			//cout<<"returned from sending data at "<<sc_time_stamp()<<endl;
			

			/* FIXME: need some error handling */
			
			if (read_op) {
				for(int y=0;y<size;y++){

					uint8_t temp_recv_buf;
					send_req_trans->set_data_ptr( reinterpret_cast<unsigned char*>(&temp_recv_buf) );
					send_req_trans->set_command( static_cast<tlm::tlm_command>(tlm::TLM_READ_COMMAND) );
					if (y==size-1)
						send_req_trans->set_address(5);
					else
						send_req_trans->set_address(1);
					send_req_trans->set_data_length( 1);
					//cout<<"sending read data at "<<sc_time_stamp()<<endl;

					request_sending_socket->b_transport( *send_req_trans, delay_send);
					cout<<"MASTER: data recieved "<<std::bitset<8>(temp_recv_buf)<<"at "<<sc_time_stamp()<<endl;
					i2c_xfer_rsp->data[read_count]=temp_recv_buf;
					read_count++;
					*payload_size=read_count;
					temp_recv_buf=0;

					wait(200,SC_US);
				}
				

			}
			else {
				
				uint8_t* write_data = (uint8_t *)&i2c_xfer_req->ops[op_count];
				for(int y=0;y<size;y++){
					
					cout<<"MASTER: Writing data"<<bitset<8>(*write_data)<<endl;
					send_req_trans->set_data_ptr( reinterpret_cast<unsigned char*>(write_data) );
					send_req_trans->set_command( static_cast<tlm::tlm_command>(tlm::TLM_WRITE_COMMAND) );
					if (y==size-1)
						send_req_trans->set_address(5);
					else
						send_req_trans->set_address(1);

					send_req_trans->set_data_length( size );
					write_data++;
					request_sending_socket->b_transport( *send_req_trans, delay_send);
					wait(200,SC_US);
				}


				
			}
				
			}
		}
