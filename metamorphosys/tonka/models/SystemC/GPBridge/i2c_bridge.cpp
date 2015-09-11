#include "i2c_bridge.h"
#include <bitset>

void i2c_bridge::initiate(){
while (1) {
		SDA_OUT.write(1);
		if (!Start_det) {
			if (SDA_IN.read()) {
				wait();
				if (!SDA_IN.read()) {
					Start_det = 1;
					cout<<"start detetcted"<<endl;
					addr=0;
				}
			}
			else
			wait();
		}
	
		if (Start_det & !Rd_W) {
			for (int i = 0; i <= 7; i++) {
				int ret = read_a_bit();
				
				if (ret == 1) { 
					temp_rev_buf.bit(i) = temp1;

					if (i == 7) {
						SDA_OUT.write(0);
						wait(SCL.negedge_event());
						SDA_OUT.write(1);
						//ack
					}
				}
				else if (ret == 2) { //stop bit
					cout << "SLAVE:stop bit\n";
					break;
				}
				else {
					cout << "corrupted byte quitting\n";
					Start_det = false;
					break;
				}
			}
		}
		if (Start_det) {
			if (!addr) {
				adr_rev = temp_rev_buf;
				bool receiveOperation = adr_rev.bit(0);
				adr_rev >>= 1;
				cout << "SLAVE:Recieved Address" << hex << adr_rev << " at " << sc_time_stamp() << "\n ";
				/*if (adr_rev != 0x03)
				{
					Start_det = 0;
					Rd_W = false;
					
					cout << "Not my address" << endl;
					goto newlife;

				}*/
				/*else*/ {
					temp_rev_buf = 0x00;
					addr = true;
					data_received = false;
					if (receiveOperation) {
						Rd_W = true;
					}
					else {
						Rd_W = false;
						adr_reg = false;
					}
				}
			}

			if (Rd_W) { //read from master
				send_tlm(true);
				cout << "SlAVE:will write " << hex << tx_reg << " at  " << sc_time_stamp() << "\n";
				
				for (int i = 0; i <= 7; i++) {
					wait(SCL.negedge_event());
					wait(STD, SC_US);//waiting for time according to i2c clock of 100khz
					
					SDA_OUT.write(tx_reg.bit(i));//1st bit //change to reg_addr;
				}
				adr_reg = false;
				//wait for ack
				wait(2);
				if (SDA_IN){
					cout << "ACK Recieved\n";
					wait();
				}
				wait(SCL.posedge_event());
				
				temp1 = SDA_IN.read();
				
				wait(SCL.negedge_event());
				
				
				temp2 = SDA_IN.read();
				if ((temp1 == 0) & (temp2 == 1)) {
						Start_det=0;
						Rd_W=0;
						cout<<"STOP BIT RECIVED SLAVE\n";
					}
				}
			
			else if (!Rd_W) {
				if (!data_received) {
					data_received = true;
				}
				else if (data_received) {
					cout << "SLAVE: Recieved data " << hex << temp_rev_buf << " at " << sc_time_stamp();
					if (!adr_reg) {
						cout << "  This is Register address\n";
						reg_addr = temp_rev_buf;
						adr_reg = true;
					}
					else if (adr_reg) {
						cout << "  This data is data to be put in previously specified register\n";
						rx_reg=temp_rev_buf;
						
						send_tlm(false);
						rx_reg=0x00;


						//mem[1] = temp_rev_buf; //replace with reg_addr

						adr_reg = false;
						
							
					}
				}
			}
			// newlife: ;

		}
	}

}


inline int i2c_bridge::read_a_bit() {
	wait(SCL.posedge_event());
	temp1 = SDA_IN.read();
	wait(SCL.negedge_event());
	temp2 = SDA_IN.read();
	if (temp1 == temp2) {
		return 1;
	}
	else if (temp1 != temp2) {
		Start_det = 0;
		addr = false;
		Rd_W = false;
		if ((temp1 == 0) & (temp2 == 1)) {
			return 2;
		}
		else {
			cout << "SLAVE:corrupted byte\n";
			return -1;
		}
	}

}

void i2c_bridge::send_tlm(bool read){
	
	send_trans->set_data_length(1);
    send_trans->set_streaming_width(1); // = data_length to indicate no streaming
    send_trans->set_byte_enable_ptr(0); // 0 indicates unused
    send_trans->set_dmi_allowed(false); // Mandatory initial value
    send_trans->set_response_status(tlm::TLM_INCOMPLETE_RESPONSE);

	if(read){ 
    send_trans->set_command(static_cast<tlm::tlm_command>(tlm::TLM_READ_COMMAND));
			uint8_t tmp_tx_reg;
			send_trans->set_address(reg_addr);
		 	send_trans->set_data_ptr(reinterpret_cast<unsigned char*>(&tmp_tx_reg));
		 	i2c_data_socket->b_transport(*send_trans, delay_send);
		 	 tx_reg=tmp_tx_reg;

		 	

	}
	else{ 
    send_trans->set_command(static_cast<tlm::tlm_command>(tlm::TLM_WRITE_COMMAND));
		
		send_trans->set_address(reg_addr);
		uint8_t tmp_rx_reg=rx_reg;
		
		send_trans->set_data_ptr(reinterpret_cast<unsigned char*>(&tmp_rx_reg));
		i2c_data_socket->b_transport(*send_trans, delay_send);

	}
	
	
   
    
   
	


}