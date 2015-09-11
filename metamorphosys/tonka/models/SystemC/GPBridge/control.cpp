
#include "control.h"
#include "compat.h"

void Control::b_transport(tlm::tlm_generic_payload& trans, sc_time& delay)
{
    tlm::tlm_generic_payload* send_resp_trans = new tlm::tlm_generic_payload;
    tlm::tlm_command cmd = trans.get_command();
    unsigned char* ptr = trans.get_data_ptr();
    unsigned int len = trans.get_data_length();

    if (cmd == tlm::TLM_WRITE_COMMAND) {
        memset(mem, 0, sizeof(mem));
        memcpy(&mem, ptr, len);
    }

    struct op_msg* op_req = reinterpret_cast<struct op_msg*>(mem);
    struct op_header* oph;
    oph = (struct op_header*)&op_req->header;
    struct op_msg* op_rsp = (struct op_msg*)&rsp_mem;
    int payload_size;
    uint16_t message_size;
    uint8_t result = PROTOCOL_STATUS_SUCCESS;

    payload_size = control_handler(op_req, op_rsp);
    if (payload_size == -1)
        cout << "error in control_handler\n";

    message_size = sizeof(struct op_header) + payload_size;
    op_rsp->header.size = htole16(message_size);
    op_rsp->header.id = oph->id;
    op_rsp->header.type = OP_RESPONSE | oph->type;
    op_rsp->header.result = result;
    op_rsp->header.pad[0] = oph->pad[0];
    op_rsp->header.pad[1] = oph->pad[1];
    if (verbose) {
        cout << "\033[1;31m control module response\033[0m";
        gbsim_dump(op_rsp, message_size);
    }
    trans.set_response_status(tlm::TLM_OK_RESPONSE);

    send_resp_trans->set_command(static_cast<tlm::tlm_command>(tlm::TLM_WRITE_COMMAND));
    send_resp_trans->set_data_ptr(reinterpret_cast<unsigned char*>(op_rsp));
    send_resp_trans->set_data_length(message_size);
    send_resp_trans->set_streaming_width(message_size); // = data_length to indicate no streaming
    send_resp_trans->set_byte_enable_ptr(0); // 0 indicates unused
    send_resp_trans->set_dmi_allowed(false); // Mandatory initial value
    send_resp_trans->set_response_status(tlm::TLM_INCOMPLETE_RESPONSE);
    rsp_sending_socket->b_transport(*send_resp_trans, delay_send);
}

size_t Control::control_handler(struct op_msg* op_req, struct op_msg* op_rsp)
{

    struct op_header* oph;
    size_t payload_size;
    oph = (struct op_header*)&op_req->header;

    switch (oph->type) {
    case GB_CONTROL_TYPE_PROTOCOL_VERSION:
        payload_size = sizeof(op_rsp->pv_rsp);

        op_rsp->pv_rsp.version_major = GB_CONTROL_VERSION_MAJOR;
        op_rsp->pv_rsp.version_minor = GB_CONTROL_VERSION_MINOR;

        break;
    case GB_CONTROL_TYPE_GET_MANIFEST_SIZE:
        payload_size = sizeof(op_rsp->control_msize_rsp);
        op_rsp->control_msize_rsp.size = htole16(manifest_info->manifest_size);

        break;
    case GB_CONTROL_TYPE_GET_MANIFEST:
        payload_size = manifest_info->manifest_size;
        memcpy(&op_rsp->control_manifest_rsp.data, manifest_info->manifest,
            payload_size);

        break;
    case GB_CONTROL_TYPE_CONNECTED:
        payload_size = 0;

        break;
    case GB_CONTROL_TYPE_DISCONNECTED:
        payload_size = 0;

        break;
    default:
        printf("control operation type %02x not supported\n", oph->type);
    }

    return payload_size;
}
