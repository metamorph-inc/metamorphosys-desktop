/////////////////////////////////////////////////////////////////////
////                                                             ////
////  Simple UART Model	                             ////
////                                                             ////
////  SystemC Version: 2.3.0                                     ////
////  Author: Peter Volgyesi, MetaMorph, Inc.                    ////
////          pvolgyesi@metamorphsoftware.com                    ////
////                                                             ////
////                                                             ////
/////////////////////////////////////////////////////////////////////

#ifndef UART_H
#define UART_H

#include <systemc.h>
#include <tlm.h>
#include <tlm_utils/simple_initiator_socket.h>
#include <tlm_utils/simple_target_socket.h>
#include <bitset>

#include "compat.h"

SC_MODULE(module_uart)
{

    // ports
    sc_in<bool> clk;
    sc_in<bool> rst;
    sc_out<bool> txd;
    sc_in<bool> rxd;
    sc_in<bool> tx_data_wr;

    sc_out<bool> tx_empty;
    sc_in<bool> rx_data_rd;
    sc_out<bool> rx_avail;

    tlm_utils::simple_target_socket<module_uart> recieving_socket;

    uint8_t mem[16];

    uint8_t* REG_TXD;
    uint8_t* REG_RXD;
    uint8_t* REG_LINE_CTRL;
    uint8_t* REG_MOD_CTRL;
    uint8_t* REG_LINE_STATUS;
    uint8_t* REG_MODEM_STATUS;

    //private:
    void common_clk_gen();
    void rx_clk_gen();
    void tx_clk_gen();
    void transmit_data();
    void receive_data();
    virtual void b_transport(tlm::tlm_generic_payload&, sc_time&);

    const sc_uint<16> divisor;

    sc_signal<bool> common_clk;
    sc_uint<16> common_clk_div;

    sc_signal<bool> tx_clk;
    sc_uint<16> tx_clk_div;

    sc_signal<bool> rx_clk;
    sc_uint<16> rx_clk_div;

    sc_signal<bool> rx_clear_clk_div;

    sc_uint<10> tx_reg;
    //sc_uint<8> tx_reg_in;
    sc_uint<16> tx_bit_cnt;
    enum tx_state_t { IDLE_TX,
        LOAD_TX,
        SHIFT_TX,
        STOP_TX };
    tx_state_t tx_FSM;

    sc_uint<8> rx_reg;

    sc_uint<16> rx_bit_cnt;
    enum rx_state_t { IDLE_RX,
        START_RX,
        EDGE_RX,
        SHIFT_RX,
        STOP_RX,
        DONE_RX,
        RX_OVF };
    rx_state_t rx_FSM;

	sc_event e1;
 SC_CTOR(module_uart)
        : recieving_socket("recieving_socket")
        , divisor(10)
    {
        REG_TXD = &mem[0];
        REG_RXD = &mem[1];
        REG_LINE_CTRL = &mem[2];
        REG_MOD_CTRL = &mem[3];
        REG_LINE_STATUS = &mem[4];
        REG_MODEM_STATUS = &mem[5];

        SC_METHOD(common_clk_gen);
        dont_initialize();
		sensitive <<clk.pos() << rst;

        SC_METHOD(rx_clk_gen);
        dont_initialize();
        sensitive << clk.pos() << rst;

		

		SC_THREAD(transmit_data);
        dont_initialize();
		sensitive << e1 << rst;

        SC_METHOD(receive_data);
        dont_initialize();
        sensitive << clk.pos() << rst;


		recieving_socket.register_b_transport(this, &module_uart::b_transport);
	}


};

#endif // UART_H