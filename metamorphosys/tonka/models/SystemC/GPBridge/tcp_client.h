#include <sys/types.h>
#include <fcntl.h>
#include <time.h>
#include <sys/stat.h>

#include "compat.h"

#include "manifest.h"

#include "module.h"

void gbsim_dump(void* data, size_t size);
void write_manifest();

SC_MODULE(tcp_client)
{
    tlm_utils::simple_initiator_socket<tcp_client> sending_socket;
    tlm_utils::simple_target_socket<tcp_client> response_recv_socket;

    const char *gbsim_servername;
    const char *manifest_filename;

    int sockfd_manifest, sockfd_gb;
    struct sockaddr_in serv_addr_manifest, serv_addr_gb;
    struct gbsim_info manifest_info;

    tlm::tlm_generic_payload* trans;
    sc_time delay;

    
    char mem[4 * 1024];
 
    SC_HAS_PROCESS(tcp_client);
    tcp_client(sc_module_name nm, 
        const char* gbsim_servername, 
        const char* manifest_filename) :
        sc_module(nm),
        sending_socket("sending_socket"), 
        response_recv_socket("response_recv_socket"),
        gbsim_servername(gbsim_servername),
        manifest_filename(manifest_filename)

    {
        delay = sc_time(10, SC_NS);
        sockfd_manifest = 0;
        sockfd_gb = 0;
        trans = new tlm::tlm_generic_payload;
        response_recv_socket.register_b_transport(this, &tcp_client::b_transport);

        SC_THREAD(send_manifest);
        SC_THREAD(recv_greybus);
    }

   

    virtual void b_transport(tlm::tlm_generic_payload & trans, sc_time & delay)
    {
        int size = 0;

        tlm::tlm_command cmd = trans.get_command();
        unsigned char* ptr = trans.get_data_ptr();
        unsigned int len = trans.get_data_length();

        if (cmd == tlm::TLM_WRITE_COMMAND) {
            memset(mem, 0, sizeof(mem));
            memcpy(&mem, ptr, len);
        }

        size = send(sockfd_manifest, mem, len, 0);

        if (size < 0) {
            if (size != EAGAIN)
                printf("error in writing %d", size);
        }
    }

    

    void send_manifest()
    {
        struct greybus_manifest_header* mh = get_manifest_blob(manifest_filename);

        if (mh) {

            manifest_info.manifest = mh;
            manifest_info.manifest_size = le16toh(mh->size);
            manifest_parse(mh, le16toh(mh->size));
        }



#ifdef _WIN32
		// init winsock
		WSADATA wsaData;
		int iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
		if (iResult != 0) {
			printf("WSAStartup failed: %d\n", iResult);
			return ;
		}
#endif

        if ((sockfd_manifest = socket(AF_INET, SOCK_STREAM, 0)) < 0) {
            printf("\n Error : Could not create socket \n");
            return;
        }

        /* Initialize sockaddr_in data structure */
        serv_addr_manifest.sin_family = AF_INET;
        serv_addr_manifest.sin_port = htons(5000); // port
        serv_addr_manifest.sin_addr.s_addr = inet_addr(gbsim_servername);

        /* Attempt a connection */
        if (connect(sockfd_manifest, (struct sockaddr*)&serv_addr_manifest, sizeof(serv_addr_manifest)) < 0) {
            printf("\n Error : Connect Failed \n");
            sc_stop();
            return;
        }

        write_manifest();
/* set the socket to non blocking*/
#ifdef _WIN32
        unsigned long iMode = 0;
        int status = ioctlsocket(sockfd_manifest, FIONBIO, &iMode);
#else
        int status = fcntl(sockfd_manifest, F_SETFL, fcntl(sockfd_manifest, F_GETFL, 0) | O_NONBLOCK);
#endif
        if (status == -1) {
            perror("calling fcntl");
        }
    }

    void recv_greybus()
    {
        int n, temp;
        int64_t time1, time2;

        unsigned char gb_message[4 * 1024]; //stores incoming values
        unsigned char gb_message_temp[4 * 1024]; // stores part of incoming value
        while (1) {
            time1 = nanosec_timer();

            memset(gb_message, 0, sizeof(gb_message));
            memset(gb_message_temp, 0, sizeof(gb_message_temp));
            n = 0;
            temp = 0;
            //cout << "time in recv handler before" << sc_time_stamp() << endl;
            if ((n = recv(sockfd_manifest, (char*)gb_message, sizeof(gb_message), 0)) > 0) {
                /*some times we recive more than one
                gb message in just one read ( non blocking read ) the following while loop
                breaks them according to the size mentioned
                in the first byte and then send them for further processing*/
                while (temp < n) {
                    std::copy(gb_message + temp, gb_message + (temp + gb_message[temp]), gb_message_temp);

                    trans->set_command(static_cast<tlm::tlm_command>(tlm::TLM_WRITE_COMMAND));
                    //trans->set_address(slave_address); need an interconnect part ot used right now
                    //cout<<" recieved slave address:"<<hex<<slave_address<<"   length:"<<length<<"    data:"<<(char*)write_data<<endl;
                    trans->set_data_ptr(reinterpret_cast<unsigned char*>(&gb_message_temp));
                    trans->set_data_length(gb_message[temp]);
                    trans->set_streaming_width(gb_message[temp]); // = data_length to indicate no streaming
                    trans->set_byte_enable_ptr(0); // 0 indicates unused
                    trans->set_dmi_allowed(false); // Mandatory initial value
                    trans->set_response_status(tlm::TLM_INCOMPLETE_RESPONSE);
                    sending_socket->b_transport(*trans, delay);

                    temp += gb_message[temp];
                }
            }

            wait(2, SC_MS);
            time2 = nanosec_timer();
            // cout <<"time in" << time2 - time1 << "ns" <<endl;
            if ((time2 - time1) >= 2000000)
                printf("systemc is too slow it took %ld \n", (time2-time1) );

            while ((time2 - time1) < 2000000) {
                time2 = nanosec_timer();
            }
        }
    }

    inline void write_manifest()
    {
        FILE* fp = fopen(manifest_filename, "rb");
        if (fp == NULL) {
            printf("File opern error");
            sc_stop();
            return;
        }
        if ((send(sockfd_manifest, manifest_filename, 64, 0)) < 0) {
            printf("error in writing file name");
            sc_stop();
        }
    }
};