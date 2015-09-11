#ifndef CONTROL_H
#define CONTROL_H
#include "systemc"
using namespace sc_core;
using namespace sc_dt;
using namespace std;

#include "tlm.h"
#include "tlm_utils/simple_initiator_socket.h"
#include "tlm_utils/simple_target_socket.h"

#include "module.h"

SC_MODULE(Control)
{

    

    tlm_utils::simple_target_socket<Control> req_recieving_socket;
    tlm_utils::simple_initiator_socket<Control> rsp_sending_socket;

    struct gbsim_info *manifest_info;

    char mem[4 * 1024];
    char rsp_mem[4 * 1024];
    uint8_t cport[2]; //save cport ids for use later

    sc_time delay_send;

    SC_HAS_PROCESS(Control);
    Control(sc_module_name nm, struct gbsim_info *manifest_info) :
        req_recieving_socket("req_recieving_socket"), 
        rsp_sending_socket("rsp_sending_socket"),
        manifest_info(manifest_info)
    {
        delay_send = sc_time(10, SC_NS);
        req_recieving_socket.register_b_transport(this, &Control::b_transport);
    }

    virtual void b_transport(tlm::tlm_generic_payload & trans, sc_time & delay);

    size_t control_handler(struct op_msg * op_req, struct op_msg * op_rsp);
};
#endif