/////////////////////////////////////////////////////////////////////
////                                                             ////
////  Simple GPIO Model											 ////
////                                                             ////
////  SystemC Version: 2.3.0                                     ////
////  Author: Peter Volgyesi, MetaMorph, Inc.                    ////
////          pvolgyesi@metamorphsoftware.com                    ////
////                                                             ////
////                                                             ////
/////////////////////////////////////////////////////////////////////

#ifndef MODULE_GPIO_H
#define MODULE_GPIO_H
#include <cstdint>
#include <systemc.h>
#include "tlm.h"
#include "tlm_utils/simple_initiator_socket.h"
#include "tlm_utils/simple_target_socket.h"

SC_MODULE(module_gpio)
{

    // ports
    sc_in<bool> clk;
    sc_in<bool> rst;

    sc_inout<sc_lv<32> > pin;
    sc_in<bool> data_out_wr;
    sc_in<bool> oe_wr;

    uint32_t mem[16];

    uint32_t* REG_GPIO_DATA;
    uint32_t* REG_GPIO_ODATA;
    uint32_t* REG_GPIO_ODATASET;
    uint32_t* REG_GPIO_ODATACLR;
    uint32_t* REG_GPIO_DIR;
    uint32_t* REG_GPIO_DIROUT;
    uint32_t* REG_GPIO_DIRIN;
    uint32_t* REG_GPIO_INTMASK;
    uint32_t* REG_GPIO_INTMASKSET;
    uint32_t* REG_GPIO_INTMASKCLR;
    uint32_t* REG_GPIO_RAWINTSTAT;
    uint32_t* REG_GPIO_INTSTAT;
    uint32_t* REG_GPIO_INTCTRL0;
    uint32_t* REG_GPIO_INTCTRL1;
    uint32_t* REG_GPIO_INTCTRL2;
    uint32_t* REG_GPIO_INTCTRL3;

    tlm::tlm_generic_payload* trans;
    tlm_utils::simple_target_socket<module_gpio> recieving_socket;
    //private
    sc_uint<32> data_reg;
    sc_uint<32> oe_reg;
    sc_uint<32> sync1_reg, sync2_reg; // double synchronizer

	sc_event e1;
    SC_CTOR(module_gpio)
        : recieving_socket("recieving_socket")
    {

        REG_GPIO_DATA = &mem[0];
        REG_GPIO_ODATA = &mem[1];
        REG_GPIO_ODATASET = &mem[2];
        REG_GPIO_ODATACLR = &mem[3];
        REG_GPIO_DIR = &mem[4];
        REG_GPIO_DIROUT = &mem[5];
        REG_GPIO_DIRIN = &mem[6];
        REG_GPIO_INTMASK = &mem[7];
        REG_GPIO_INTMASKSET = &mem[8];
        REG_GPIO_INTMASKCLR = &mem[9];
        REG_GPIO_RAWINTSTAT = &mem[10];
        REG_GPIO_INTSTAT = &mem[11];
        REG_GPIO_INTCTRL0 = &mem[12];
        REG_GPIO_INTCTRL1 = &mem[13];
        REG_GPIO_INTCTRL2 = &mem[14];
        REG_GPIO_INTCTRL3 = &mem[15];

        SC_METHOD(input_sync);
		sensitive << e1 << rst;
        SC_METHOD(output_update);
		sensitive <<e1 << rst;

        data_reg = sc_uint<32>(0);
        oe_reg = sc_uint<32>(0);
        trans = new tlm::tlm_generic_payload;
        memset(mem, 0, sizeof(mem));

        recieving_socket.register_b_transport(this, &module_gpio::b_transport);
    }

    void input_sync();
    void output_update();

    virtual void b_transport(tlm::tlm_generic_payload&, sc_time&);
};

#endif