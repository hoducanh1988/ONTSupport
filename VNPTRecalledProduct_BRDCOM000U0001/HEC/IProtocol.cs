using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEC {

    public interface IProtocol {

        bool Open(string port_name, string baud_rate, string data_bit); //uart
        bool Open(string visa_address); // visa
        bool Open(string ip_address, string port); //telnet
        bool Open(string ip_address, string port, string width, string height); //ssh
        bool isConnected();
        bool Write(string cmd);
        bool WriteLine(string cmd);
        bool WriteBreak();
        string Read();
        string Query(string cmd);
        bool Query(string cmd, string timeout_sec, string end_swith);
        bool Close();

    }

}
