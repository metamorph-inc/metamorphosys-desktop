#ifndef MODULE_H
#define MODULE_H

#include <string.h>
#include <stdlib.h>
#include <errno.h>
#include <stdio.h>
#include <cstdint>
#include <systemc>
#include <vector>

#include "tlm.h"
#include "tlm_utils/simple_initiator_socket.h"
#include "tlm_utils/simple_target_socket.h"

#include "greybus_protocols.h"
#include "greybus_manifest.h"

#include "compat.h"

using namespace std;
using namespace sc_core;

#define FILE_SIZE 64;

#define _ALIGNBYTES (sizeof(uint32_t) - 1)
#define ALIGN(p) ((decltype(p))(((unsigned)(p) + _ALIGNBYTES) & ~_ALIGNBYTES))

extern int verbose;
extern char* basedir;

extern int64_t time_start;

// data out of controller
struct gbsim_cport {

    uint16_t id;
    uint16_t hd_cport_id;
    int protocol;
};

struct gbsim_info {
    void* manifest;
    size_t manifest_size;
};

extern std::vector<gbsim_cport> cport_list;

inline void gbsim_dump(void* data, size_t size)
{
    char* buf = static_cast<char*>(data);
    unsigned int i;

    for (i = 0; i < size; i++)
        fprintf(stdout, "%02x ", buf[i]);

    fprintf(stdout, "\n");
}

inline void displaytime()
{
    int64_t now = nanosec_timer();
    cout << "in function " << sc_time_stamp() << endl;
    cout << "function time diff " << now - time_start << "ns" << endl;
}

#endif //MODULE_H