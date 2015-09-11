#include <systemc.h>
//#include "gpio_functions.h"
//#include "uart_functions.h"
//#include "module_gpio.h"
#include "module_i2c.h"
#include "interconnect.h"
#include "control.h"
#include "i2c_functions.h"
#include "tcp_client.h"
//#include "module_uart.h"

int verbose = 0;

SC_MODULE(GPBridge)
{
  tcp_client *Init;
  Interconnect *Router;
  Control *Control_Handler;
  I2C *I2c_Handler;
  module_i2c *i2c_controller;


  //all handler go below this
  const char *gbsim_servername;
  const char *manifest_filename;
  
SC_CTOR(GPBridge){

    accept_argument();

    Init = new tcp_client("tcp_client", gbsim_servername, manifest_filename);
    Router   = new Interconnect("Router");
    Control_Handler = new Control("Control_Handler", &Init->manifest_info);
    I2c_Handler = new I2C("i2c_handler");
    i2c_controller=new module_i2c ("i2c_controller");
   

    //BInding sockets
    Init->sending_socket.bind(Router->gb_req_recieving_socket);
    Router->gb_rsp_sending_socket.bind(Init->response_recv_socket);

    Router->gb_control_req_sending_socket.bind(Control_Handler->req_recieving_socket);
    Control_Handler->rsp_sending_socket.bind(Router->gb_control_resp_recieving_socket);

    /* Router->gb_gpio_req_sending_socket.bind(Gpio_Handler->recieving_socket);
     Gpio_Handler->response_sending_socket.bind(Router->gb_gpio_resp_recieving_socket);

     Gpio_Handler->request_sending_socket.bind(gpio_controller->recieving_socket);

     Router->gb_uart_req_sending_socket.bind(Uart_Handler->recieving_socket);
     Uart_Handler->response_sending_socket.bind(Router->gb_uart_resp_recieving_socket);

     Uart_Handler->request_sending_socket.bind(uart_controller->recieving_socket);*/

    Router->gb_i2c_req_sending_socket.bind(I2c_Handler->recieving_socket);
    I2c_Handler->response_sending_socket.bind(Router->gb_i2c_resp_recieving_socket);

    I2c_Handler->request_sending_socket.bind(i2c_controller->recieving_socket);


    
  }


void usage()
{
    cerr << "usage: " << sc_argv()[0] << " -i <gbsim server> -f <manifest file>" << endl;
}

  
int accept_argument(){
#ifdef _WIN32
    // poor man's argument processing
    for (int i = 1; i < sc_argc(); i++) {
        if (sc_argv()[i][0] != '-')
            continue;
        switch (sc_argv()[i][1]) {
        case 'i':
            gbsim_servername = strdup(sc_argv()[++i]);
            break;
        case 'f':
            manifest_filename = strdup(sc_argv()[++i]);
            
            break;
        case 'v':
            verbose = 1;
           
            break;
        default:
            usage();
            return -1;
        }
    }

#else
    int opt;
    while ((opt = getopt(sc_argc(), const_cast<char* const*>(sc_argv()), "i:f:v")) != -1) {
        switch (opt) {

        case 'i':
            gbsim_servername = strdup(optarg);
            break;
        case 'f':
            manifest_filename = strdup(optarg);
            break;
        case 'v':
            verbose = 1;
            break;
        default:
            usage();
            return -1;
        }
    }
#endif

    if (NULL == gbsim_servername) {
        cerr << "required parameter missing: -i <gbsim server> " << endl;
        usage();
        return -1;
    }

    if (NULL == manifest_filename) {
        cerr << "required parameter missing: -f <manifest file> " << endl;
        usage();
        return -1;
    }

    cout << endl;
    cout << "=============================" << endl;
    cout << "GPBridge simulation testbench" << endl;
    cout << "=============================" << endl;
    cout << " gbsim server:  " << gbsim_servername << endl;
    cout << " manifest file: " << manifest_filename << endl;
    if (verbose) {
        cout << " verbose mode is on" << endl;
    }
    cout << "=============================" << endl;


  }
};




