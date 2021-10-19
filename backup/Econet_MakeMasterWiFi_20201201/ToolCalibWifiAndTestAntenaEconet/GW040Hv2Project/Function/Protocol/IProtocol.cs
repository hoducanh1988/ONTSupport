using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW040Hv2Project.Function {
    public interface IProtocol {

        string Login(string Username, string Password, int LoginTimeOutMs);
        bool WriteLine(string cmd);
        bool Write(string cmd);
        //bool WriteLineAndWaitComplete(string cmd);
        bool Query(string cmd, string patern, int timeout_sec);
        bool Query(string cmd, string patern, out string data, int timeout_sec);
        bool Query(string cmd, int timeout_sec, params string[] paterns);
        string Read();
        bool Close();
        bool IsConnected();
    }
}
