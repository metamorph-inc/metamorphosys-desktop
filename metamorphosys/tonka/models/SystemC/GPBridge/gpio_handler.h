#define GB_GPIO_TYPE_INVALID 0x00
#define GB_GPIO_TYPE_PROTOCOL_VERSION 0x01
#define GB_GPIO_TYPE_LINE_COUNT 0x02
#define GB_GPIO_TYPE_ACTIVATE 0x03
#define GB_GPIO_TYPE_DEACTIVATE 0x04
#define GB_GPIO_TYPE_GET_DIRECTION 0x05
#define GB_GPIO_TYPE_DIRECTION_IN 0x06
#define GB_GPIO_TYPE_DIRECTION_OUT 0x07
#define GB_GPIO_TYPE_GET_VALUE 0x08
#define GB_GPIO_TYPE_SET_VALUE 0x09
#define GB_GPIO_TYPE_SET_DEBOUNCE 0x0a
#define GB_GPIO_TYPE_IRQ_TYPE 0x0b
#define GB_GPIO_TYPE_IRQ_MASK 0x0c
#define GB_GPIO_TYPE_IRQ_UNMASK 0x0d
#define GB_GPIO_TYPE_IRQ_EVENT 0x0e

#define GB_GPIO_IRQ_TYPE_NONE 0x00
#define GB_GPIO_IRQ_TYPE_EDGE_RISING 0x01
#define GB_GPIO_IRQ_TYPE_EDGE_FALLING 0x02
#define GB_GPIO_IRQ_TYPE_EDGE_BOTH 0x03
#define GB_GPIO_IRQ_TYPE_LEVEL_HIGH 0x04
#define GB_GPIO_IRQ_TYPE_LEVEL_LOW 0x08

#define GPIO_BASE 0x00
#define GPIO_DATA 0
#define GPIO_ODATA 1
#define GPIO_ODATASET 2
#define GPIO_ODATACLR 3
#define GPIO_DIR 4
#define GPIO_DIROUT 5
#define GPIO_DIRIN 6
#define GPIO_INTMASK 7
#define GPIO_INTMASKSET 8
#define GPIO_INTMASKCLR 9
#define GPIO_RAWINTSTAT 10
#define GPIO_INTSTAT 11
#define GPIO_INTCTRL0 12
#define GPIO_INTCTRL1 13
#define GPIO_INTCTRL2 14
#define GPIO_INTCTRL3 15

#define SIZE 32

using namespace sc_core;
using namespace sc_dt;
using namespace std;

#include "module.h"
#include "compat.h"

size_t gpio_handler(struct op_msg*, struct op_msg*);

SC_MODULE(GPIO)
{
    // TLM-2 socket, defaults to 32-bits wide, base protocol
    tlm_utils::simple_target_socket<GPIO> recieving_socket;
    tlm_utils::simple_initiator_socket<GPIO> request_sending_socket;
    tlm_utils::simple_initiator_socket<GPIO> response_sending_socket;
    char mem[4 * 1024];
    char rsp_mem[4 * 1024];
    char read_data;
    int i;

    tlm::tlm_generic_payload* send_resp_trans;
    tlm::tlm_generic_payload* send_req_trans;

    sc_time delay_send;

    SC_CTOR(GPIO)
        : recieving_socket("recieving_socket")
        , request_sending_socket("request_sending_socket")
        , response_sending_socket("response_sending_socket")
    {
        // Register callback for incoming b_transport interface method call
        recieving_socket.register_b_transport(this, &GPIO::b_transport);
        delay_send = sc_time(10, SC_NS);

        send_resp_trans = new tlm::tlm_generic_payload;
        send_req_trans = new tlm::tlm_generic_payload;
        read_data = 100;
        i = 2;
    }

    void gpio_type_protocol_version(struct protocol_version_rsp*);
    void gpio_type_line_count(struct gb_gpio_line_count_response*);
    void gpio_type_activate(uint8_t);
    void gpio_type_deactivate(uint8_t);
    void gpio_get_direction(uint8_t, struct gb_gpio_get_direction_response*);
    void gpio_direction_in(uint8_t);
    void gpio_direction_out(uint8_t, uint8_t);
    void gpio_set_value(uint8_t, uint8_t);
    void gpio_get_value(uint8_t, struct gb_gpio_get_value_response*);
    void gpio_set_debounce(uint8_t, uint16_t);
    void gpio_irq_type(uint8_t, uint8_t);
    void gpio_irq_mask(uint8_t);
    void gpio_irq_unmask(uint8_t);

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
            cout << "\033[1;31m gpio module request\033[0m";

            gbsim_dump(static_cast<void*>(mem), (size_t)len);
        }

        struct op_msg* op_req = reinterpret_cast<struct op_msg*>(mem);

        struct op_header* oph;

        oph = (struct op_header*)&op_req->header;

        struct op_msg* op_rsp = (struct op_msg*)&rsp_mem;
        int payload_size;
        uint16_t message_size;

        uint8_t result = PROTOCOL_STATUS_SUCCESS;

        payload_size = gpio_handler(op_req, op_rsp);

        if (payload_size == -1)
            cout << "error in gpio_handler\n";

        cout << "returned\n";

        cout << "paylaod size" << payload_size << endl;
        message_size = sizeof(struct op_header) + payload_size;
        op_rsp->header.size = htole16(message_size);
        op_rsp->header.id = oph->id;
        op_rsp->header.type = OP_RESPONSE | oph->type;
        op_rsp->header.result = result;

        op_rsp->header.pad[0] = oph->pad[0];
        op_rsp->header.pad[1] = oph->pad[1];

        if (verbose) {
            cout << "\033[1;31m gpio module response\033[0m";
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

    size_t gpio_handler(struct op_msg * op_req, struct op_msg * op_rsp)
    {

        struct op_header* oph;
        size_t payload_size;
        oph = (struct op_header*)&op_req->header;

        switch (oph->type) {
        case GB_GPIO_TYPE_PROTOCOL_VERSION:
            if (verbose)
                displaytime();
            payload_size = sizeof(struct protocol_version_rsp);
            gpio_type_protocol_version(&(op_rsp->pv_rsp));
            break;

        case GB_GPIO_TYPE_LINE_COUNT:
            if (verbose)
                displaytime();
            payload_size = sizeof(struct gb_gpio_line_count_response);
            gpio_type_line_count(&(op_rsp->gpio_lc_rsp));
            break;

        case GB_GPIO_TYPE_ACTIVATE:
            if (verbose)
                displaytime();
            payload_size = 0;
            gpio_type_activate(op_req->gpio_act_req.which);
            break;

        case GB_GPIO_TYPE_DEACTIVATE:
            if (verbose)
                displaytime();
            payload_size = 0;
            gpio_type_deactivate(op_req->gpio_deact_req.which);
            break;

        case GB_GPIO_TYPE_GET_DIRECTION:
            if (verbose)
                displaytime();

            payload_size = sizeof(struct gb_gpio_get_direction_response);
            gpio_get_direction(op_req->gpio_get_dir_req.which, (&(op_rsp->gpio_get_dir_rsp)));
            break;

        case GB_GPIO_TYPE_DIRECTION_IN:
            if (verbose)
                displaytime();
            payload_size = 0;
            gpio_direction_in(op_req->gpio_dir_input_req.which);
            break;

        case GB_GPIO_TYPE_DIRECTION_OUT:
            if (verbose)
                displaytime();
            payload_size = 0;
            gpio_direction_out(op_req->gpio_dir_output_req.which, op_req->gpio_dir_output_req.value);
            break;

        case GB_GPIO_TYPE_GET_VALUE:
            if (verbose)
                displaytime();

            payload_size = sizeof(struct gb_gpio_get_value_response);
            gpio_get_value(op_req->gpio_get_val_req.which, &(op_rsp->gpio_get_val_rsp));
            break;

        case GB_GPIO_TYPE_SET_VALUE:
            if (verbose)
                displaytime();
            payload_size = 0;

            gpio_set_value(op_req->gpio_set_val_req.which, op_req->gpio_set_val_req.value);

            break;

        case GB_GPIO_TYPE_SET_DEBOUNCE:
            if (verbose)
                displaytime();
            payload_size = 0;
            gpio_set_debounce(op_req->gpio_set_db_req.which, op_req->gpio_set_db_req.usec);
            break;

        case GB_GPIO_TYPE_IRQ_TYPE:
            if (verbose)
                displaytime();
            payload_size = 0;
            gpio_irq_type(op_req->gpio_irq_type_req.which, op_req->gpio_irq_type_req.type);
            break;

        case GB_GPIO_TYPE_IRQ_MASK:
            if (verbose)
                displaytime();
            payload_size = 0;
            gpio_irq_mask(op_req->gpio_irq_mask_req.which);
            break;

        case GB_GPIO_TYPE_IRQ_UNMASK:
            if (verbose)
                displaytime();
            payload_size = 0;
            gpio_irq_unmask(op_req->gpio_irq_unmask_req.which);
            break;

        default:
            payload_size = -1;
            break;
        }
        return payload_size;
    }
};
