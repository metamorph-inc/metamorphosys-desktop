#include "module.h"
#include "uart_handler.h"

#include <bitset>

void UART::uart_type_protocol_version(struct protocol_version_rsp* pv_rsp)
{

    pv_rsp->version_major = 0x00;
    pv_rsp->version_minor = 0x01;

    rst.write(true);
    cout << "set rst down" << sc_time_stamp() << endl;
}
void UART::uart_type_send_data(uint8_t data)
{
    cout << " in uart_type_send_data\n";

    if (!tx_empty.read()) {
        cout << "ERROR: UART TX register is not empty "
             << " (" << sc_time_stamp() << ")" << endl
             << endl;
    }
    else {
		cout<<"turned tx_data true "<<sc_time_stamp()<<endl;
		tx_data_wr.write(true);
		
        send_req_trans->set_command(static_cast<tlm::tlm_command>(tlm::TLM_WRITE_COMMAND));
        send_req_trans->set_address(UART_TXD_DATA);
        send_req_trans->set_data_ptr(reinterpret_cast<unsigned char*>(&data));
        send_req_trans->set_data_length(1);
        send_req_trans->set_streaming_width(1); // = data_length to indicate no streaming
        send_req_trans->set_byte_enable_ptr(0); // 0 indicates unused
        send_req_trans->set_dmi_allowed(false); // Mandatory initial value
        send_req_trans->set_response_status(tlm::TLM_INCOMPLETE_RESPONSE); // Mandatory initial value
        request_sending_socket->b_transport(*send_req_trans, delay_send);
		
		
        wait(clk.posedge_event());
		cout<<"turned tx_data false"<<sc_time_stamp()<<endl;

        tx_data_wr.write(false);
    }
}
void UART::uart_type_recieve_data()
{

    uint8_t data;
    char uart_buf[100];
    cout << " in uartuart_type_recieve_data\n";
    send_req_trans->set_command(static_cast<tlm::tlm_command>(tlm::TLM_READ_COMMAND));
    send_req_trans->set_address(UART_RXD_DATA);
    send_req_trans->set_data_ptr(reinterpret_cast<unsigned char*>(&data));
    send_req_trans->set_data_length(1);
    send_req_trans->set_streaming_width(1); // = data_length to indicate no streaming
    send_req_trans->set_byte_enable_ptr(0); // 0 indicates unused
    send_req_trans->set_dmi_allowed(false); // Mandatory initial value
    send_req_trans->set_response_status(tlm::TLM_INCOMPLETE_RESPONSE); // Mandatory initial value
    request_sending_socket->b_transport(*send_req_trans, delay_send);
    rx_data_rd.write(true);
    wait(clk.posedge_event());
    rx_data_rd.write(false);

    // make a response for greybus
    cout << "i reached here\n";
    struct op_msg* op_req_recv = (struct op_msg*)uart_buf;
    size_t payload_size = 0;
    uint16_t message_size;
    struct gb_uart_recv_data_request* rdr = (struct gb_uart_recv_data_request*)(uart_buf + sizeof(struct op_header));
    rdr->size = htole16(1); // one 1 byte at a time
    memcpy(&rdr->data, &data, 1);
    payload_size = sizeof(*rdr) + 1;

    /* Fill in the request header */
    message_size = sizeof(struct op_header) + payload_size;
    op_req_recv->header.size = htole16(message_size);

    op_req_recv->header.id = ++local_counter;
    op_req_recv->header.type = GB_UART_TYPE_RECEIVE_DATA;

    /* Store the cport id in the header pad bytes */

    op_req_recv->header.pad[0] = cport[0];
    op_req_recv->header.pad[1] = cport[1];
    cout << "reached here\n";
    //send it
    tlm::tlm_generic_payload* send_resp_trans_2 = new tlm::tlm_generic_payload;
    ;
    send_resp_trans_2->set_command(static_cast<tlm::tlm_command>(tlm::TLM_WRITE_COMMAND));
    send_resp_trans_2->set_data_ptr(reinterpret_cast<unsigned char*>(op_req_recv));
    send_resp_trans_2->set_data_length(message_size);
    send_resp_trans_2->set_streaming_width(message_size); // = data_length to indicate no streaming
    send_resp_trans_2->set_byte_enable_ptr(0); // 0 indicates unused
    send_resp_trans_2->set_dmi_allowed(false); // Mandatory initial value
    send_resp_trans_2->set_response_status(tlm::TLM_INCOMPLETE_RESPONSE);
    response_sending_socket->b_transport(*send_resp_trans_2, delay_send);
}
void UART::uart_type_set_line_coding(uint32_t, uint8_t, uint8_t, uint8_t)
{
    cout << " in uart_set_line_cosing\n";
}
void UART::uart_control_line_state(uint8_t)
{

    cout << " in uart_control_line_state\n";
}
void UART::uart_send_break(uint8_t)
{
    cout << " in uart_send_break\n";
}

void UART::uart_serial_state(uint8_t)
{
    cout << " in uart_serail_state\n";
}