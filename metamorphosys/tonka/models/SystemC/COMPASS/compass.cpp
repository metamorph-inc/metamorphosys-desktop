#include "compass.h"
#include <bitset>

void sensor_compass::initiate() {
	while (1) {
		SDA_OUT.write(1);
		if (!Start_det) {
			if (SDA_IN.read()) {
				wait();
				if (!SDA_IN.read()) {
					Start_det = 1;
					
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
						
						wait(5, SC_US);
						
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
				if (adr_rev != 0x03)
				{
					Start_det = 0;
					Rd_W = false;
					
					cout << "Not my address" << endl;
					goto newlife;

				}
				else {
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
				cout << "SlAVE:will write " << hex << mem[reg_addr] << " at  " << sc_time_stamp() << "\n";
				wait();
				
				for (int i = 0; i <= 7; i++) {
					wait();

					wait(4, SC_US);
					
					SDA_OUT.write(mem[reg_addr].bit(i));//1st bit //change to reg_addr;
					wait();
				}
				adr_reg = false;
				//wait for ack
				
				wait(2);
				

				if (SDA_IN){
					cout << "ACK Recieved\n";
					wait();
					
				}
				if (reg_addr <= 11)
					reg_addr++;
				else
					reg_addr = 3;
				

				
				wait(SCL.posedge_event());
			
				temp1 = SDA_IN.read();
				wait(SCL.negedge_event());
			
				temp2 = SDA_IN.read();
				if ((temp1 == 0) & (temp2 == 1)) {
						
						Start_det=0;
						Rd_W=0;
						
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
						mem[reg_addr] = temp_rev_buf; //replace with reg_addr

						adr_reg = false;
						if((reg_addr==0) | (reg_addr==2))
							e1.notify();
					}
				}
			}
			newlife: ;

		}
	}

}



inline int sensor_compass::read_a_bit() {
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


void sensor_compass::common_clk_gen() {
	/*if (rst.read()) {

		common_clk.write(false);
		common_clk_div = 0;
	}*/
	//else
	 {
	 	
		common_clk.write(false);
		if (common_clk_div == 1000)
		 {
		 	
			common_clk.write(true);
			common_clk_div = 0;
		}
		else {

			
			common_clk_div += 1;
		
		}
	}
}


void sensor_compass::process() {
	*REG_DATA_X_MSB = 0x10;
	*REG_DATA_X_LSB = 0x11;
	*REG_DATA_Y_MSB = 0x12;
	*REG_DATA_Y_LSB = 0x13;
	*REG_DATA_Z_MSB = 0x14;
	*REG_DATA_Z_LSB = 0x15;

	while (1) {

		
		/*if (rst.read()) {

			*CONFIG_REG_A = 0x10;
			*MODE_REG = 0x03;
			*REG_DATA_X_MSB += 0x00;
			*REG_DATA_X_LSB += 0x00;
			*REG_DATA_Y_MSB += 0x00;
			*REG_DATA_Y_LSB += 0x00;
			*REG_DATA_Z_MSB += 0x00;
			*REG_DATA_Z_LSB += 0x00;

		}*/

			//else
		{

			if (MODE == SINGLE) {
				
				//ask should I set and reset as done in chip done to reduce error
				data_rdy = false;
				*REG_DATA_X_MSB += 0x01;
				*REG_DATA_X_LSB += 0x02;
				*REG_DATA_Y_MSB += 0x03;
				*REG_DATA_Y_LSB += 0x04;
				*REG_DATA_Z_MSB += 0x05;
				*REG_DATA_Z_LSB += 0x06;
				MODE = IDLE;
				data_rdy = true;



			}
			if (MODE == CONTINUOUS) {
				
				*REG_DATA_X_MSB += 0x1;
				*REG_DATA_X_LSB += 0x1;
				*REG_DATA_Y_MSB += 0x1;
				*REG_DATA_Y_LSB += 0x1;
				*REG_DATA_Z_MSB += 0x1;
				*REG_DATA_Z_LSB += 0x1;
				data_rdy = true;
				wait(250, SC_US);
				data_rdy = false;
			}
		}

		wait();
	}
}

void sensor_compass::update_global() {

	uint8_t temp;
	/*if (!rst.read())*/ {
		temp = *MODE_REG & (0x03);
		if (temp == CONT)
			MODE = CONTINUOUS;
		else if (temp == SIGL)
			MODE = SINGLE;
		else
			MODE = IDLE;

		temp = 0;
		temp = ((*CONFIG_REG_A & 0x1C) >> 2);

		if (temp == 0)
			DATA_OUTPUT = speed_0_75;
		else if (temp == 1)
			DATA_OUTPUT = speed_1_5;
		else if (temp == 2)
			DATA_OUTPUT = speed_3;
		else if (temp == 3)
			DATA_OUTPUT = speed_7_5;
		else if (temp == 4)
			DATA_OUTPUT = speed_15;
		else if (temp == 5)
			DATA_OUTPUT = speed_30;
		else if (temp == 6)
			DATA_OUTPUT = speed_75;
		else if (temp == 7)
			cout << "Invalid Settings\n";


		cout << "MODE is " << MODE << "(1 for single and 0 for Contd)  " << "Speed is   " << DATA_OUTPUT << endl;
	}





}
