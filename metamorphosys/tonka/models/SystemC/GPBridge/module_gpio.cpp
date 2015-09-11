/////////////////////////////////////////////////////////////////////
////                                                             ////
////  Simple GPIO Model										     ////
////                                                             ////
////  SystemC Version: 2.3.0                                     ////
////  Author: Peter Volgyesi, MetaMorph, Inc.                    ////
////          pvolgyesi@metamorphsoftware.com                    ////
////                                                             ////
////                                                             ////
/////////////////////////////////////////////////////////////////////
#include "module_gpio.h"

#include <systemc>
#include <bitset>

void module_gpio::input_sync()
{
    //if (!rst.read()) {
    //	sync1_reg={0};
    //sync2_reg={0};-	//}
    //else {

    if (clk.posedge()) {
        sc_lv<32> tmp = pin.read();

        for (int i = 0; i < tmp.length(); i++) {
            if (tmp.get_bit(i) == sc_dt::Log_X) {
                tmp.set_bit(i, sc_dt::Log_1); // Pullup logic
            }
        }
        sync1_reg = (tmp);
        sync2_reg = sync1_reg;

        //}
        *REG_GPIO_DATA = sync2_reg;
    }
}

void module_gpio::output_update()
{
    //if (!rst.read()) {
    //}
    //else if (clk.posedge()) {
    //if (data_out_wr.read()) {
    //data_reg=(*data_out);
    //}
    //if (oe_wr.read()) {
    //oe_reg=(*oe);
    //}
    //}
    uint32_t tmp = *REG_GPIO_ODATA;

    tmp = (tmp | *REG_GPIO_ODATASET);
    tmp &= (~(*REG_GPIO_ODATACLR));
    *REG_GPIO_ODATA = tmp;

    //*REG_GPIO_ODATASET=0;
    //*REG_GPIO_ODATACLR=0;
    memset(REG_GPIO_ODATASET, 0, 4);
    memset(REG_GPIO_ODATACLR, 0, 4);

    //cout<<"tmp"<<std::bitset<32> (*tmp)<<endl;

    //for (int i = 0; i < tmp.length(); i++) {
    //if (!oe_reg[i]) {
    ///tmp.set_bit(i, sc_dt::Log_Z);
    //}
    //}
    pin.write(tmp);
}

void module_gpio::b_transport(tlm::tlm_generic_payload& trans, sc_time& delay)
{

    tlm::tlm_command cmd = trans.get_command();
    sc_dt::uint64 adr = trans.get_address();
    unsigned char* ptr = trans.get_data_ptr();
    unsigned int len = trans.get_data_length();

    //if (adr >= sc_dt::uint64(24) || byt != 0 || len > 8 || wid < len)
    // SC_REPORT_ERROR("TLM-2", "Target does not support given generic payload transaction");

    // Obliged to implement read and write commands
    e1.notify();
   wait(SC_ZERO_TIME);
    if (cmd == tlm::TLM_READ_COMMAND)
        memcpy(ptr, &mem[adr], len);

    else if (cmd == tlm::TLM_WRITE_COMMAND)
        memcpy(&mem[adr], ptr, len);
    cout << "bitset in btransport" << std::bitset<32>((int32_t)mem[adr]) << endl;
    // Obliged to set response status to indicate successful completion
    trans.set_response_status(tlm::TLM_OK_RESPONSE);
}
