#define VCD_OUTPUT_ENABLE

#include <iostream>

#include <systemc.h>
#include "compat.h"
#include "module.h"

//#include "Compass/compass.h"

//#include "compass.h"
#include "test_randomreg.h"

#include "GPBridge.h"
#include "i2c_bus.h"
#include "i2c_bridge.h"


using namespace std;


int64_t time_start;
int64_t time_finish;



int sc_main(int argc, char* argv[])
{
    
    GPBridge GPBridge("GPBridge");
   
   i2c_bridge compass_model("compass_model");
   data_bank data_bank1("data_bank1");

    //DUT port binding
    sc_clock clk("clock", 10, SC_US);
    //sc_clock clk_1("clock_1", 300, SC_US);
    sc_signal<bool> sda_to_slave;
    sc_signal<bool> scl;
    sc_signal<bool> sda_to_master;

    

    GPBridge.i2c_controller->SDA_OUT(sda_to_slave);
    GPBridge.i2c_controller->SDA_IN(sda_to_master);
    GPBridge.i2c_controller->SCL(scl);
    GPBridge.i2c_controller->clk(clk);

    compass_model.SDA_IN(sda_to_slave);
    compass_model.SDA_OUT(sda_to_master);
    compass_model.SCL(scl);
    //compass_model.clk(clk_1);

    compass_model.i2c_data_socket(data_bank1.req_recieving_socket);

    /*sc_signal<bool> data_rdy;
    sc_signal<bool> rst_1;
    compass_model.rst(rst_1);
    compass_model.data_rdy(data_rdy);*/
		



#ifdef VCD_OUTPUT_ENABLE
    sc_trace_file* vcd_log = sc_create_vcd_trace_file("TEST_GPIO");
    vcd_log->set_time_unit(1, SC_US);

   
    sc_trace(vcd_log, sda_to_slave, "sda_to_slave");
    sc_trace(vcd_log, sda_to_master, "sda_to_master");
    sc_trace(vcd_log, scl, "scl");



#endif

    time_start = nanosec_timer();
    sc_start(10,SC_SEC);
    time_finish = nanosec_timer();

    cout << "simulation actualtime diff " << time_finish - time_start << " ns" << endl
         << endl;
    return (0);
}



	

/*gpio_controller.clk(clk);
    gpio_controller.rst(gpio_rst);
    gpio_controller.data_out_wr(data_out_wr);
    gpio_controller.oe_wr(oe_wr);
    gpio_controller.pin(pins);*/

    /*sc_signal<bool>   uart_rst;
    sc_signal<bool> tx_data_wr;
    sc_signal<bool> tx_empty;
    sc_signal<bool> rx_data_rd;
    sc_signal<bool> rx_avail;
    sc_signal<bool> txd, rxd;

    sc_signal<bool>   gpio_rst;
    sc_signal<bool> data_out_wr, oe_wr;
    sc_signal<sc_uint<32> > data_out, oe, data_in;
    sc_signal_rv<32> pins;*/


   /* uart_controller.clk(clk);
    uart_controller.rst(uart_rst);
    uart_controller.tx_data_wr(tx_data_wr);
    uart_controller.tx_empty(tx_empty);
    uart_controller.rx_data_rd(rx_data_rd);
    uart_controller.rx_avail(rx_avail);
    uart_controller.rxd(rxd);
    uart_controller.txd(txd);

    Uart_Handler.clk(clk);
    Uart_Handler.rst(uart_rst);
    Uart_Handler.tx_data_wr(tx_data_wr);
    Uart_Handler.tx_empty(tx_empty);
    Uart_Handler.rx_data_rd(rx_data_rd);
    Uart_Handler.rx_avail(rx_avail);*/

		

     //sc_trace(vcd_log, clk, "Clk");
    /*sc_trace(vcd_log, gpio_rst, "Reset_GPIO");
        sc_trace(vcd_log, pins, "Pins");*/
    /*sc_trace(vcd_log,uart_rst,"uart_rst");
    sc_trace(vcd_log, tx_data_wr, "tx_data_wr");
    sc_trace(vcd_log, tx_empty, "tx_empty");
    sc_trace(vcd_log, rxd, "rxd");
    sc_trace(vcd_log,txd,"txd");*/