

#define OP_I2C_PROTOCOL_VERSION 0x01
#define OP_I2C_PROTOCOL_FUNCTIONALITY 0x02
#define OP_I2C_PROTOCOL_TIMEOUT 0x03
#define OP_I2C_PROTOCOL_RETRIES 0x04
#define OP_I2C_PROTOCOL_TRANSFER 0x05

#define SIZE 32

#include "systemc"
using namespace sc_core;
using namespace sc_dt;
using namespace std;

#include <time.h>
#include "tlm.h"
#include "tlm_utils/simple_initiator_socket.h"
#include "tlm_utils/simple_target_socket.h"

#include "module.h"

size_t i2c_handler(struct op_msg*, struct op_msg*);

SC_MODULE(I2C)
{
    // TLM-2 socket, defaults to 32-bits wide, base protocol
    tlm_utils::simple_target_socket<I2C> recieving_socket;
 tlm_utils::simple_initiator_socket<I2C> request_sending_socket;
    tlm_utils::simple_initiator_socket<I2C> response_sending_socket;
    char mem[4 * 1024];
    char rsp_mem[4 * 1024];
    char read_data;
    int i;


    tlm::tlm_generic_payload* send_resp_trans;
    tlm::tlm_generic_payload* send_req_trans;

    sc_time delay_send;

  SC_CTOR(I2C)
    : recieving_socket("recieving_socket"), request_sending_socket("request_sending_socket"), 
    response_sending_socket("response_sending_socket") {
		
		send_resp_trans = new tlm::tlm_generic_payload;
		send_req_trans = new tlm::tlm_generic_payload;
		delay_send = sc_time(10, SC_NS);
    // Register callback for incoming b_transport interface method call
       recieving_socket.register_b_transport(this, &I2C::b_transport);


         read_data = 100;
          i = 2;

  }

    void i2c_type_protocol_version(struct protocol_version_rsp*);
    void i2c_type_get_functionality(struct gb_i2c_functionality_response*);
    void i2c_type_timeout(uint16_t);
    void i2c_type_retries(uint8_t);
void i2c_type_transfer(struct gb_i2c_transfer_request*,struct gb_i2c_transfer_response*, size_t*);


  // TLM-2 blocking transport method
  virtual void b_transport( tlm::tlm_generic_payload & trans, sc_time & delay ) {

        tlm::tlm_command cmd = trans.get_command();

        unsigned char* ptr = trans.get_data_ptr();
        unsigned int len = trans.get_data_length();
    


    // if (adr >= sc_dt::uint64(SIZE) || byt != 0 || len > 4 || wid < len)
    // SC_REPORT_ERROR("TLM-2", "Target does not support given generic payload transaction");

        if (cmd == tlm::TLM_READ_COMMAND) {
            cout << "\033[1;31mreading\033[0m\n";
            *ptr = read_data;
        }
        else if (cmd == tlm::TLM_WRITE_COMMAND) {

            memset(mem, 0, sizeof(mem));

            memcpy(&mem, ptr, len);
        }
        if (verbose) {
            cout << "\033[1;31m I2C module request\033[0m";

            gbsim_dump(static_cast<void*>(mem), (size_t)len);
        }

        struct op_msg* op_req = reinterpret_cast<struct op_msg*>(mem);

        struct op_header* oph;

        oph = (struct op_header*)&op_req->header;

        struct op_msg* op_rsp = (struct op_msg*)&rsp_mem;
        size_t payload_size;
        uint16_t message_size;

        uint8_t result = PROTOCOL_STATUS_SUCCESS;

        payload_size = i2c_handler(op_req, op_rsp);

        if (payload_size == -1)
            cout << "error in gpio_handler\n";

   



    message_size = sizeof(struct op_header) + payload_size;
    op_rsp->header.size = htole16(message_size);
    op_rsp->header.id = oph->id;
    op_rsp->header.type = OP_RESPONSE | oph->type;
    op_rsp->header.result = result;

    op_rsp->header.pad[0] = oph->pad[0];
    op_rsp->header.pad[1] = oph->pad[1];


        if (verbose) {
            cout << "\033[1;31m I2C module response\033[0m";
            gbsim_dump(op_rsp, message_size);
        }
        trans.set_response_status(tlm::TLM_OK_RESPONSE);

    send_resp_trans->set_command(static_cast<tlm::tlm_command>(tlm::TLM_WRITE_COMMAND));
    //trans->set_address(slave_address); need an interconnect part ot used right now
    //cout<<" recieved slave address:"<<hex<<slave_address<<"   length:"<<length<<"    data:"<<(char*)write_data<<endl;
    send_resp_trans->set_data_ptr( reinterpret_cast<unsigned char*>(op_rsp) );
    send_resp_trans->set_data_length(message_size);
    send_resp_trans->set_streaming_width(message_size); // = data_length to indicate no streaming
    send_resp_trans->set_byte_enable_ptr( 0 ); // 0 indicates unused
    send_resp_trans->set_dmi_allowed( false ); // Mandatory initial value
    send_resp_trans->set_response_status( tlm::TLM_INCOMPLETE_RESPONSE );
    response_sending_socket->b_transport( *send_resp_trans, delay_send );

  }


  size_t i2c_handler(struct op_msg * op_req, struct op_msg * op_rsp) {

        struct op_header* oph;
        size_t payload_size;
        oph = (struct op_header*)&op_req->header;

        switch (oph->type) {
        case OP_I2C_PROTOCOL_VERSION:
      //if (verbose) displaytime();
                displaytime();
            payload_size = sizeof(struct protocol_version_rsp);
            i2c_type_protocol_version(&(op_rsp->pv_rsp));
            break;

        case OP_I2C_PROTOCOL_FUNCTIONALITY:
     // if (verbose) displaytime();
                displaytime();
            payload_size = sizeof(struct gb_i2c_functionality_response);
            i2c_type_get_functionality(&(op_rsp->i2c_fcn_rsp));
            break;

        case OP_I2C_PROTOCOL_TIMEOUT:
     // if (verbose) displaytime();
                displaytime();
            payload_size = 0;
       
            break;

        case OP_I2C_PROTOCOL_RETRIES:
     // if (verbose) displaytime();
                displaytime();
            payload_size = 0;
      
            break;

        case OP_I2C_PROTOCOL_TRANSFER:
     // if (verbose) displaytime();
                displaytime();

      i2c_type_transfer(&(op_req->i2c_xfer_req),&(op_rsp->i2c_xfer_rsp),&payload_size);
            break;

        default:
            payload_size = -1;
            break;
        }
        return payload_size;
    }
};
