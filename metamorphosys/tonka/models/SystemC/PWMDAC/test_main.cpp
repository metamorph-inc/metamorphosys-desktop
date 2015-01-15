/////////////////////////////////////////////////////////////////////
////                                                             ////
////  Simple PWM as DAC Model Test Bench					     ////
////                                                             ////
////  SystemC Version: 2.3.0                                     ////
////  Author: Peter Volgyesi, MetaMorph, Inc.                    ////
////          pvolgyesi@metamorphsoftware.com                    ////
////                                                             ////
////                                                             ////
/////////////////////////////////////////////////////////////////////
#include <stdlib.h>
#include <time.h>
#include <systemc.h>
#include <PWMDAC/pwmdac.h>
#include "test_pwmdac.h"


#define VCD_OUTPUT_ENABLE

int sc_main(int argc, char *argv[]) {

	sc_set_time_resolution(1.0, SC_US);

	sc_clock clk("clock", 1, SC_MS);
	sc_signal<sc_logic> pwm;
	sc_signal<sc_uint<10>>	analog;

	pwmdac		i_pwmdac("PWMDAC");
	test_pwmdac i_test("TEST");

	i_pwmdac.analog(analog);
	i_pwmdac.pwm(pwm);

	i_test.clk(clk);
	i_test.analog(analog);
	i_test.pwm(pwm);

#ifdef VCD_OUTPUT_ENABLE
	sc_trace_file *vcd_log = sc_create_vcd_trace_file("TEST_PWMDAC");
	vcd_log->set_time_unit(1.0, SC_MS);
	sc_trace(vcd_log, pwm, "PWM");
	sc_trace(vcd_log, analog, "Analog");

#endif

	srand((unsigned int)(time(NULL) & 0xffffffff));
	sc_start();

#ifdef VCD_OUTPUT_ENABLE
	sc_close_vcd_trace_file(vcd_log);
#endif

	return i_test.error_cnt;
}

