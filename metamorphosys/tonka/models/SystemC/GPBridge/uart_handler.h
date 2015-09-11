#define GB_UART_MESSAGE_SIZE_MAX GB_OPERATION_DATA_SIZE_MAX
#define GB_UART_DATA_SIZE_MAX \
    (GB_UART_MESSAGE_SIZE_MAX - sizeof(struct gb_uart_send_data_request))
#define BREAK_DURATION_MS 300 /* break duration tcsendbreak() */

/* greybus-spec/build/html/bridged_phy.html#uart-protocol */
#define GB_UART_TYPE_INVALID 0x00
#define GB_UART_TYPE_PROTOCOL_VERSION 0x01
#define GB_UART_TYPE_SEND_DATA 0x02
#define GB_UART_TYPE_RECIEVE_DATA 0x03
#define GB_UART_TYPE_SET_LINE_CODING 0x04
#define GB_UART_TYPE_SET_CONTROL_LINE_STATE 0x05
#define GB_UART_TYPE_SEND_BREAK 0x06
#define GB_UART_TYPE_SERIAL_STATE 0x07
#define GB_UART_TYPE_RESPONSE 0x80
#define GB_UART_MAX 255
#define GB_OPERATION_DATA_SIZE_MAX 0x400 /* TODO: BOD */

#define UART_MAXNAME 20
#define UART_IDX_TX 1
#define UART_IDX_RX 0
#define UART_IDX_COUNT 2

using namespace sc_core;
using namespace sc_dt;
using namespace std;
#include "module.h"

size_t uart_handler(struct op_msg*, struct op_msg*);

/* for now I have just defined 6 registers*/

#define UART_TXD_DATA 0
#define UART_RXD_DATA 1
#define UART_LINE_CONTROL_REG 2
#define UART_MODEM_CTRL_REG 3
#define UART_LINE_STATUS_REG 4
#define UART_MODEM_STATUS_REGISTER 5

#define GB_UART_MESSAGE_SIZE_MAX GB_OPERATION_DATA_SIZE_MAX
#define GB_UART_DATA_SIZE_MAX \
    (GB_UART_MESSAGE_SIZE_MAX - sizeof(struct gb_uart_send_data_request))

SC_MODULE(UART)
{
    // TLM-2 socket, defaults to 32-bits wide, base protocol
    tlm_utils::simple_target_socket<UART> recieving_socket;
    tlm_utils::simple_initiator_socket<UART> request_sending_socket;
    tlm_utils::simple_initiator_socket<UART> response_sending_socket;
    char mem[4 * 1024];
    char rsp_mem[4 * 1024];
    char read_data;
    int i;
    int local_counter;
    uint8_t cport[2]; //save cport ids for use later

    tlm::tlm_generic_payload* send_resp_trans;

    tlm::tlm_generic_payload* send_req_trans;

    sc_time delay_send;

    sc_in<bool> clk;
    sc_out<bool> rst;
    sc_out<bool> tx_data_wr;
    sc_in<bool> tx_empty;
    sc_out<bool> rx_data_rd;
    sc_in<bool> rx_avail;

    SC_CTOR(UART)
        : recieving_socket("recieving_socket")
        , request_sending_socket("request_sending_socket")
        , response_sending_socket("response_sending_socket")
        , local_counter(0)
    {
        // Register callback for incoming b_transport interface method call
        recieving_socket.register_b_transport(this, &UART::b_transport);
        delay_send = sc_time(10, SC_NS);
        SC_THREAD(uart_type_recieve_data);
        dont_initialize();
        sensitive << rx_avail.pos();

        send_resp_trans = new tlm::tlm_generic_payload;
        send_req_trans = new tlm::tlm_generic_payload;
    }

    void uart_type_protocol_version(struct protocol_version_rsp*);
    void uart_type_send_data(uint8_t);
    void uart_type_recieve_data();
    void uart_type_set_line_coding(uint32_t, uint8_t, uint8_t, uint8_t);
    void uart_control_line_state(uint8_t);
    void uart_send_break(uint8_t);
    void uart_serial_state(uint8_t);

    // TLM-2 blocking transport method
    virtual void b_transport(tlm::tlm_generic_payload & trans, sc_time & delay)
    {

        tlm::tlm_command cmd = trans.get_command();

        unsigned char* ptr = trans.get_data_ptr();
        unsigned int len = trans.get_data_length();

        if (cmd == tlm::TLM_READ_COMMAND) {
            cout << "\033[1;31mreading\033[0m\n";
            *ptr = read_data;
        }
        else if (cmd == tlm::TLM_WRITE_COMMAND) {

            memset(mem, 0, sizeof(mem));

            memcpy(&mem, ptr, len);
        }
        if (verbose) {
            cout << "\033[1;31m uart module request\033[0m";

            gbsim_dump(static_cast<void*>(mem), (size_t)len);
        }

        struct op_msg* op_req = reinterpret_cast<struct op_msg*>(mem);

        struct op_header* oph;

        oph = (struct op_header*)&op_req->header;

        struct op_msg* op_rsp = (struct op_msg*)&rsp_mem;
        int payload_size;
        uint16_t message_size;

        uint8_t result = PROTOCOL_STATUS_SUCCESS;
        cout << "calling uart handler\n";
        payload_size = uart_handler(op_req, op_rsp);

        if (payload_size == -1)
            cout << "error in uart_handler\n";

        message_size = sizeof(struct op_header) + payload_size;
        op_rsp->header.size = htole16(message_size);
        op_rsp->header.id = oph->id;
        op_rsp->header.type = OP_RESPONSE | oph->type;
        op_rsp->header.result = result;
        op_rsp->header.pad[0] = oph->pad[0];
        op_rsp->header.pad[1] = oph->pad[1];

        if (verbose) {
            cout << "\033[1;31m uart module response\033[0m";
            gbsim_dump(op_rsp, message_size);
        }
        trans.set_response_status(tlm::TLM_OK_RESPONSE);

        send_resp_trans->set_command(static_cast<tlm::tlm_command>(tlm::TLM_WRITE_COMMAND));
        //trans->set_address(slave_address); need an interconnect part ot used right now
        //cout<<" recieved slave address:"<<hex<<slave_address<<"   length:"<<length<<"    data:"<<(char*)write_data<<endl;
        send_resp_trans->set_data_ptr(reinterpret_cast<unsigned char*>(op_rsp));
        send_resp_trans->set_data_length(message_size);
        send_resp_trans->set_streaming_width(message_size); // = data_length to indicate no streaming
        send_resp_trans->set_byte_enable_ptr(0); // 0 indicates unused
        send_resp_trans->set_dmi_allowed(false); // Mandatory initial value
        send_resp_trans->set_response_status(tlm::TLM_INCOMPLETE_RESPONSE);
        response_sending_socket->b_transport(*send_resp_trans, delay_send);
    }

    size_t uart_handler(struct op_msg * op_req, struct op_msg * op_rsp)
    {

        struct op_header* oph;
        size_t payload_size;
        oph = (struct op_header*)&op_req->header;

        switch (oph->type) {
        case GB_UART_TYPE_PROTOCOL_VERSION:
            if (verbose)
                displaytime();
            cout << "i am here\n";
            payload_size = sizeof(struct protocol_version_rsp);
            cport[0] = oph->pad[0];
            cport[1] = oph->pad[1];

            uart_type_protocol_version(&(op_rsp->pv_rsp));
            break;

        case GB_UART_TYPE_SEND_DATA:
            if (verbose)
                displaytime();
            struct gb_uart_send_data_request* send_data;
            send_data = &op_req->uart_send_data_req;
            for (unsigned int i = 0; i < send_data->size; i++) {
                uart_type_send_data(send_data->data[i]);
            }
            payload_size = 0;
            break;
        case GB_UART_TYPE_SET_LINE_CODING:
            if (verbose)
                displaytime();
            struct gb_uart_set_line_coding_request* line_coding;
            line_coding = &op_req->uart_slc_req;
            uart_type_set_line_coding(line_coding->rate, line_coding->format, line_coding->parity, line_coding->data_bits);
            payload_size = 0;
            break;

        case GB_UART_TYPE_SET_CONTROL_LINE_STATE:

            if (verbose)
                displaytime();
            struct gb_uart_set_control_line_state_request* line_state;
            line_state = &op_req->uart_sls_req;
            uart_control_line_state(line_state->control);
            payload_size = 0;
            break;

        case GB_UART_TYPE_SEND_BREAK:
            if (verbose)
                displaytime();
            struct gb_uart_set_break_request* set_break;
            set_break = &op_req->uart_sb_req;
            uart_send_break(set_break->state);
            payload_size = 0;
            break;

        case (GB_UART_TYPE_RESPONSE | GB_UART_TYPE_RECEIVE_DATA):

        //void uart_type_recieve_data(uint16_t, uint8_t , uint8_t *);
        case (GB_UART_TYPE_RESPONSE | GB_UART_TYPE_SERIAL_STATE):

            if (verbose)
                displaytime();
            // uart_serial_state(uint8_t);
            payload_size = 0;

        default:
            printf("UART operation type %02x not supported\n", oph->type);
        }

        return payload_size;
    }
};
