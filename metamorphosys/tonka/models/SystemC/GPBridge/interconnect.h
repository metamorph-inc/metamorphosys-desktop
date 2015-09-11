
using namespace sc_core;
using namespace sc_dt;
using namespace std;
#include "module.h"
SC_MODULE(Interconnect)
{

    tlm_utils::simple_target_socket<Interconnect> gb_req_recieving_socket;
    tlm_utils::simple_initiator_socket<Interconnect> gb_rsp_sending_socket;

	//tlm_utils::simple_initiator_socket<Interconnect> gb_gpio_req_sending_socket;
	//tlm_utils::simple_target_socket_tagged<Interconnect> gb_gpio_resp_recieving_socket; //id =1

    tlm_utils::simple_initiator_socket<Interconnect> gb_control_req_sending_socket;
    tlm_utils::simple_target_socket_tagged<Interconnect> gb_control_resp_recieving_socket; //id =0

	//tlm_utils::simple_initiator_socket<Interconnect> gb_uart_req_sending_socket;
	//tlm_utils::simple_target_socket_tagged<Interconnect> gb_uart_resp_recieving_socket; //id =2

	tlm_utils::simple_initiator_socket<Interconnect> gb_i2c_req_sending_socket;
	tlm_utils::simple_target_socket_tagged<Interconnect> gb_i2c_resp_recieving_socket; //id =3



    SC_CTOR(Interconnect)
        : gb_req_recieving_socket("gb_req_recieving_socket")
        , gb_rsp_sending_socket("gb_rsp_sending_socket")
        /*, gb_gpio_req_sending_socket("gb_gpio_req_sending_socket")
        , gb_gpio_resp_recieving_socket("gb_gpio_resp_recieving_socket")*/
        , gb_control_req_sending_socket("gb_control_req_sending_socket")
        , gb_control_resp_recieving_socket("gb_control_resp_recieving_socket")
		//gb_gpio_req_sending_socket("gb_gpio_req_sending_socket"),
		//gb_gpio_resp_recieving_socket("gb_gpio_resp_recieving_socket"),
		//gb_uart_req_sending_socket("gb_uart_req_sending_socket"),
		//gb_uart_resp_recieving_socket("gb_uart_resp_recieving_socket"),
		,gb_i2c_req_sending_socket("gb_i2c_req_sending_socket")
		,gb_i2c_resp_recieving_socket("gb_i2c_resp_recieving_socket")

    {

        gb_req_recieving_socket.register_b_transport(this, &Interconnect::gb_b_transport);

        gb_control_resp_recieving_socket.register_b_transport(this, &Interconnect::resp_b_transport, 0);
		//gb_gpio_resp_recieving_socket.register_b_transport(this, &Interconnect::resp_b_transport, 1);
		//gb_uart_resp_recieving_socket.register_b_transport(this, &Interconnect::resp_b_transport, 2);
		gb_i2c_resp_recieving_socket.register_b_transport(this,&Interconnect::resp_b_transport,3);

    }

    virtual void gb_b_transport(tlm::tlm_generic_payload & trans, sc_time & delay)
    {

        uint16_t hd_cport_id;
        char mem[1024];

        tlm::tlm_command cmd = trans.get_command();
        unsigned char* ptr = trans.get_data_ptr();
        unsigned int len = trans.get_data_length();

        if (cmd == tlm::TLM_WRITE_COMMAND) {
            memset(mem, 0, sizeof(mem));
            memcpy(&mem, ptr, len);
        }

        tlm::tlm_generic_payload* send_trans = new tlm::tlm_generic_payload;
        send_trans->set_command(static_cast<tlm::tlm_command>(tlm::TLM_WRITE_COMMAND));
        send_trans->set_data_ptr(reinterpret_cast<unsigned char*>(&mem));
        send_trans->set_data_length(len);
        send_trans->set_streaming_width(len); // = data_length to indicate no streaming
        send_trans->set_byte_enable_ptr(0); // 0 indicates unused
        send_trans->set_dmi_allowed(false); // Mandatory initial value
        send_trans->set_response_status(tlm::TLM_INCOMPLETE_RESPONSE);

        gbsim_dump(mem, len);
        struct op_header* hdr = reinterpret_cast<struct op_header*>(mem);

        /*
		 * Retreive and clear the cport id stored in the header pad bytes.
		 */
        hd_cport_id = hdr->pad[1] << 8 | hdr->pad[0];

        int protocol = find_protocol(hd_cport_id);

        switch (protocol) {
        case GREYBUS_PROTOCOL_CONTROL:
            if (verbose) {
                cout << "control protocol detected\n";
            }
            gb_control_req_sending_socket->b_transport(*send_trans, delay);
            break;

		/*case GREYBUS_PROTOCOL_GPIO:
            if (verbose) {
                cout << "GPIO protocol detected\n";
            }
            gb_gpio_req_sending_socket->b_transport(*send_trans, delay);
			break;*/

		case GREYBUS_PROTOCOL_I2C:
			if (verbose){cout<<"I2c protocol detected\n";}
			gb_i2c_req_sending_socket->b_transport(*send_trans,delay);
			break;
		/*case GREYBUS_PROTOCOL_UART:
            if (verbose) {
                cout << "UART protocol detected\n";
            }
            gb_uart_req_sending_socket->b_transport(*send_trans, delay);
			break;*/
        /*
		case GREYBUS_PROTOCOL_PWM:
		case GREYBUS_PROTOCOL_I2S_MGMT:
		case GREYBUS_PROTOCOL_I2S_RECEIVER:
		case GREYBUS_PROTOCOL_I2S_TRANSMITTER:*/
        case GREYBUS_PROTOCOL_UNKNOWN:
            printf("handler not found for cport\n");
            break;
        default:
            printf("handler not found for cport\n");
        }
    }

    virtual void resp_b_transport(int id, tlm::tlm_generic_payload& trans, sc_time& delay)
    {

        tlm::tlm_command cmd = trans.get_command();
        unsigned char* ptr = trans.get_data_ptr();
        unsigned int len = trans.get_data_length();

        char resp_mem[4 * 1024];

        if (cmd == tlm::TLM_WRITE_COMMAND) {
            memset(resp_mem, 0, sizeof(resp_mem));
            memcpy(resp_mem, ptr, len);
        }

        tlm::tlm_generic_payload* send_trans = new tlm::tlm_generic_payload;
        send_trans->set_command(static_cast<tlm::tlm_command>(tlm::TLM_WRITE_COMMAND));
        send_trans->set_data_ptr(reinterpret_cast<unsigned char*>(&resp_mem));
        send_trans->set_data_length(len);
        send_trans->set_streaming_width(len); // = data_length to indicate no streaming
        send_trans->set_byte_enable_ptr(0); // 0 indicates unused
        send_trans->set_dmi_allowed(false); // Mandatory initial value
        send_trans->set_response_status(tlm::TLM_INCOMPLETE_RESPONSE);

        gb_rsp_sending_socket->b_transport(*send_trans, delay);
    }

    int find_protocol(uint16_t cportid)
    {

        // visual studio does not support range-based for-loop
        for (auto i = cport_list.begin(); i != cport_list.end(); i++) {

            if (i->hd_cport_id == cportid) {
                return i->protocol;
            }
        }
        return 0x37;
    }
};