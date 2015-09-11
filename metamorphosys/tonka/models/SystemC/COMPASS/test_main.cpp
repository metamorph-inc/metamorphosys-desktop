#define SC_INCLUDE_DYNAMIC_PROCESSES

#include "compass.h"
#include "test_compass.h"
#include "tlm.h"
#include "tlm_utils/simple_initiator_socket.h"
#include "tlm_utils/simple_target_socket.h"
#include <systemc.h>

int sc_main(int argc, char* argv[]) {


	sc_set_time_resolution(10, SC_NS);

	sc_clock clk("clock", 3, SC_US);
	sc_signal <bool> data_rdy;
	sc_signal <bool> rst;

	master_compass i_master("master");
	sensor_compass i_sensor("sensor");

	i_master.clk(clk);
	i_master.rst(rst);
	i_master.data_rdy(data_rdy);

	i_sensor.clk(clk);
	i_sensor.rst(rst);
	i_sensor.data_rdy(data_rdy);
	

	sc_start(25,SC_SEC);
	return 0;
}


