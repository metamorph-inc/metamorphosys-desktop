

#include "module.h"
#include "gpio_handler.h"

#include <bitset>

void GPIO::gpio_type_protocol_version(struct protocol_version_rsp* pv_rsp)
{
    printf("in protocl version\n");
    //GPIO::rst.write(true);

    pv_rsp->version_major = 0x00;
    pv_rsp->version_minor = 0x01;
}

void GPIO::gpio_type_line_count(struct gb_gpio_line_count_response* gpio_lc_rsp)
{
    printf("in line count\n ");
    gpio_lc_rsp->count = SIZE;
}

void GPIO::gpio_type_activate(uint8_t which)
{
    printf("in activate stuff\n");
    cout << "\033[1;31mactivated\033[0m\n";
    //data_out_wr.write(false);
    //wait(clk.posedge_event());
    //sc_uint<8> temp = 0;
    //temp |= 1 << (which - 1);
    //enable_reg = enable_reg | (temp);
    //cout << "bitset" << std::bitset<8>(enable_reg) << endl;
    //data_out.write(enable_reg);
    //wait(clk.posedge_event());
}

void GPIO::gpio_type_deactivate(uint8_t which)
{
    printf("in deaticate stuff\n");
    // data_out_wr.write(false);
    // wait(clk.posedge_event());
    // sc_uint<8> temp = 0;
    // temp &= ~(1 << (which - 1));
    // enable_reg = enable_reg | (temp);
    // cout << "bitset" << std::bitset<8>(enable_reg) << endl;
    // data_out.write(enable_reg);
    // wait(clk.posedge_event());
}

void GPIO::gpio_get_direction(uint8_t which, struct gb_gpio_get_direction_response* gpio_get_dir_rsp)
{ // 1 for input in gb and 0 input in  ctrl( corection pending) so if we recive 0 put 1
    printf("get directiond\n");
    uint32_t temp = 0;
    send_req_trans->set_command(static_cast<tlm::tlm_command>(tlm::TLM_READ_COMMAND));
    send_req_trans->set_address(GPIO_DIR);
    send_req_trans->set_data_ptr(reinterpret_cast<unsigned char*>(&temp));
    send_req_trans->set_data_length(4);
    send_req_trans->set_streaming_width(4); // = data_length to indicate no streaming
    send_req_trans->set_byte_enable_ptr(0); // 0 indicates unused
    send_req_trans->set_dmi_allowed(false); // Mandatory initial value
    send_req_trans->set_response_status(tlm::TLM_INCOMPLETE_RESPONSE); // Mandatory initial value
    request_sending_socket->b_transport(*send_req_trans, delay_send); // Blocking send_req_transport call

    if (temp & (1 << (which - 1))) {
        gpio_get_dir_rsp->direction = 0;
        if (verbose)
            cout << "direction=out\n";
    }
    else {
        gpio_get_dir_rsp->direction = 1;
        if (verbose)
            cout << "direction=in\n";
    }
}

void GPIO::gpio_direction_in(uint8_t which)
{
    printf("set direction in\n");
    uint32_t temp = 0;
    //restrucure to call get direction later if youcan

    temp |= (1 << (which - 1));

    send_req_trans->set_command(static_cast<tlm::tlm_command>(tlm::TLM_WRITE_COMMAND));
    send_req_trans->set_address(GPIO_DIRIN);
    send_req_trans->set_data_ptr(reinterpret_cast<unsigned char*>(&temp));
    send_req_trans->set_data_length(4);
    send_req_trans->set_streaming_width(4); // = data_length to indicate no streaming
    send_req_trans->set_byte_enable_ptr(0); // 0 indicates unused
    send_req_trans->set_dmi_allowed(false); // Mandatory initial value
    send_req_trans->set_response_status(tlm::TLM_INCOMPLETE_RESPONSE); // Mandatory initial value
    request_sending_socket->b_transport(*send_req_trans, delay_send); // Blocking send_req_transport call
}

void GPIO::gpio_direction_out(uint8_t which, uint8_t value)
{
    printf("set direction out\n");
    uint32_t temp = 0;
    // restructure to call get direction instead later

    temp |= (1 << (which - 1));

    send_req_trans->set_command(static_cast<tlm::tlm_command>(tlm::TLM_WRITE_COMMAND));
    send_req_trans->set_address(GPIO_DIROUT);
    send_req_trans->set_data_ptr(reinterpret_cast<unsigned char*>(&temp));
    send_req_trans->set_data_length(4);
    send_req_trans->set_streaming_width(4); // = data_length to indicate no streaming
    send_req_trans->set_byte_enable_ptr(0); // 0 indicates unused
    send_req_trans->set_dmi_allowed(false); // Mandatory initial value
    send_req_trans->set_response_status(tlm::TLM_INCOMPLETE_RESPONSE); // Mandatory initial value
    request_sending_socket->b_transport(*send_req_trans, delay_send);
}

void GPIO::gpio_set_value(uint8_t which, uint8_t value)
{
    printf("set value\n");
    uint32_t temp = 0;
    temp |= (1 << (which - 1));

    if (value)
        send_req_trans->set_address(GPIO_ODATASET);

    else
        send_req_trans->set_address(GPIO_ODATACLR);

    send_req_trans->set_command(static_cast<tlm::tlm_command>(tlm::TLM_WRITE_COMMAND));

    send_req_trans->set_data_ptr(reinterpret_cast<unsigned char*>(&temp));
    send_req_trans->set_data_length(4);
    send_req_trans->set_streaming_width(4); // = data_length to indicate no streaming
    send_req_trans->set_byte_enable_ptr(0); // 0 indicates unused
    send_req_trans->set_dmi_allowed(false); // Mandatory initial value
    send_req_trans->set_response_status(tlm::TLM_INCOMPLETE_RESPONSE); // Mandatory initial value
    request_sending_socket->b_transport(*send_req_trans, delay_send);
}

void GPIO::gpio_get_value(uint8_t which, struct gb_gpio_get_value_response* gpio_get_val_rsp)
{
    printf("get value\n");
    uint32_t temp = 0;
    send_req_trans->set_command(static_cast<tlm::tlm_command>(tlm::TLM_READ_COMMAND));
    send_req_trans->set_address(GPIO_DATA);
    send_req_trans->set_data_ptr(reinterpret_cast<unsigned char*>(&temp));
    send_req_trans->set_data_length(4);
    send_req_trans->set_streaming_width(4); // = data_length to indicate no streaming
    send_req_trans->set_byte_enable_ptr(0); // 0 indicates unused
    send_req_trans->set_dmi_allowed(false); // Mandatory initial value
    send_req_trans->set_response_status(tlm::TLM_INCOMPLETE_RESPONSE); // Mandatory initial value
    request_sending_socket->b_transport(*send_req_trans, delay_send);

    if (temp & (1 << (which - 1))) {
        gpio_get_val_rsp->value = 1;
        //if(verbose)
        cout << "value =1\n";
    }
    else {
        gpio_get_val_rsp->value = 0;
        //if(verbose)
        cout << "value=0\n";
    }
}

void GPIO::gpio_set_debounce(uint8_t which, uint16_t usec)
{
    printf("set debounce\n");
}

void GPIO::gpio_irq_type(uint8_t which, uint8_t type)
{
    printf("gpio_irq_type\n");
}

void GPIO::gpio_irq_mask(uint8_t which)
{
    printf("gpio irq mask\n");
}

void GPIO::gpio_irq_unmask(uint8_t which)
{
    printf("gpio irq unmask\n");
}
