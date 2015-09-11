
#ifndef C0MPASS_H
#define COMPASS_H
#include <cstdint>
#include <systemc.h>


#define CONT 0
#define SIGL 1
#define clock_freq 3000 //HZ
#define speed_0_75 (clock_freq/0.75) 
#define speed_1_5  (clock_freq/1.5)
#define speed_3  (clock_freq/3)
#define speed_7_5 (clock_freq/7.5)
#define speed_15 (clock_freq/15)
#define speed_30 (clock_freq/30) 
#define speed_75 (clock_freq/75)


SC_MODULE(sensor_compass){

	sc_in<bool> SDA_IN;
	sc_out<bool>SDA_OUT;
	sc_in <bool> SCL;
	sc_in<bool> clk;
	sc_in<bool>   rst;
	
	sc_signal<bool> common_clk;
	//sc_in<bool>   rst;
	sc_out<bool> data_rdy;
	sc_uint<32> common_clk_div;


	sc_event e1;
	
	sc_bit temp1, temp2;

	enum MODES_t {CONTINUOUS, SINGLE, IDLE};
	

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
	MODES_t MODE;
	unsigned int  DATA_OUTPUT;

	sc_uint<8> rx_reg;
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

	SC_CTOR(sensor_compass){
		rx_reg=0xFF;
		adr_rev=0x55;
		addr=false;
		my_addr=0xAA;
		reg_addr=0x03;
		Start_det=false;
		data_received=false;
		adr_reg=false;

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

		*CONFIG_REG_B=0xb1;
		*MODE_REG=0X00;

		SC_THREAD(initiate);
		dont_initialize();
		sensitive<<SCL;

		SC_THREAD(process);
		
		sensitive << common_clk ;

		SC_METHOD(common_clk_gen);
		dont_initialize();
		sensitive << clk.pos() ;

		SC_METHOD(update_global);
		
		sensitive<<e1;
	}

	void initiate();
	inline int read_a_bit();
	void process();
	void common_clk_gen();
	void update_global();

};
#endif
