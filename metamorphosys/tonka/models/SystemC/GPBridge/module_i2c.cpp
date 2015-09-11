// in b_ tranport
//5 is for this is last byte send stop after this
//6 is for this is adddr with read so notifu the read_data
//7 is for ths is second last byte in read donot call erad_data after this


#include "module_i2c.h"

#include <systemc>
#include <bitset>

void module_i2c::b_transport( tlm::tlm_generic_payload & trans, sc_time & delay ) {

  tlm::tlm_command cmd = trans.get_command();
  sc_dt::uint64    adr = trans.get_address();
 
  unsigned char*   ptr = trans.get_data_ptr();
 
  if (adr == 5) {
      send_stop = 1;
    }
    else
      send_stop = 0;

  if ( cmd == tlm::TLM_READ_COMMAND ) {
    read_data();
    rx_reg1 = rx_reg;
    memcpy(ptr, &rx_reg1, 1);
  }
  else if ( cmd == tlm::TLM_WRITE_COMMAND ) {
    memcpy(&tx_reg1, ptr, 1);
    tx_reg = tx_reg1;
    
    e1.notify();
  }
  trans.set_response_status( tlm::TLM_OK_RESPONSE );

}

void module_i2c::toggleSCL() {

  if (flag) {
    flag = 0;
    SCL.write(1);

  }
  else if (!flag) {
    flag = 1;
    SCL.write(0);
  }
}

void module_i2c::sendstart() {
  value = 1;
  e3.notify();
 
  //SDA_OUT.write(1);

  wait(clk.posedge_event());

   
  

  flag = 1;
  e2.notify();
  
  wait(clk.negedge_event());
  value = 0;
  e3.notify();
  
  //wait(2, SC_US);
  wait(clk.posedge_event());
  e2.notify();
 
  start_bit = 1;
}

void module_i2c::send_stop_bit() {

  wait(clk.negedge_event());
  value=0;
  e3.notify();
  wait(clk.posedge_event());
  e2.notify();
  wait(clk.negedge_event());
  value = 1;
  e3.notify();
  wait(clk.posedge_event());
  e2.notify();
  start_bit = 0;

}
void module_i2c::write_a_bit() { //sensitive to e3
  if (value)
    SDA_OUT.write(1);
  else
    SDA_OUT.write(0);

}



void module_i2c::send_data() {
  while (1) {
    for (int i = 0; i <= 7; i++) {
      if (!start_bit) {
      
        sendstart();
      }


      wait(clk.negedge_event());
      
      if (tx_reg.bit(i))value = 1;
      else value = 0;
      e3.notify();
       

      wait(clk.posedge_event());
      e2.notify();
       
      wait(clk.posedge_event());
      e2.notify();
       
    }
    wait(clk.negedge_event());
    value=1;
    e3.notify();//pulled up after sendng all 8 bit now wat for ack
    wait(clk.posedge_event());
    e2.notify();//by this time we should have the line pulled high by reciever as an ack
   
   
    if (!SDA_IN.read()) {
      cout << "MASTER acknowledge recieved at  "  << sc_time_stamp() << "\n";
    
      wait(clk.posedge_event());
      e2.notify();
      cout<<"end of ack waiting"<<sc_time_stamp()<<endl;
      if (send_stop) {
       /// cout << "sending STOP\n";
        send_stop_bit();
      }
    }
    else {
      cout << "MASTER NO ACK\n";
    }
    wait();
  }
}


void module_i2c::read_data() {
 
   wait(clk.posedge_event());
     cout<<sc_time_stamp()<<"just after first scl in read data"<<endl;
   flag=1;
  e2.notify();

  wait(clk.posedge_event());
  cout<<sc_time_stamp()<<"just after89 first scl in read data"<<endl;

  e2.notify();
  for (int i = 0; i <= 7; i++) {
    wait(clk.posedge_event());

    e2.notify();
    int ret = read_a_bit();
    if (ret == 1)
    {
      rx_reg.bit(i) = temp1;
    }
  }
 //ack sending
 wait(clk.posedge_event());
  e2.notify();
  value = 1;
  e3.notify();
  wait(clk.posedge_event());
  e2.notify();
  

  
  if(send_stop)
    send_stop_bit();
  else{
    wait(clk.posedge_event());
    cout<<"the wait bit "<<sc_time_stamp()<<endl;
    e2.notify();
    
    wait(clk.posedge_event());
    cout<<"the wait bit2 "<<sc_time_stamp()<<endl;
    
    e2.notify();
  }


}




int module_i2c::read_a_bit() {
  temp1 = SDA_IN.read();
  wait(clk.posedge_event());
  e2.notify();
  temp2 = SDA_IN.read();
  if (temp1 == temp2) {
    return 1;
  }
  else if (temp1 != temp2) {
    if ((temp1 == 0) & (temp2 == 1)) {
      cout << "SLAVE:stop condition recieved at" << sc_time_stamp() << endl;

      return 2;
    }
    else {
      cout << "SLAVE:corrupted byte\n";
      return -1;
    }
  }

}