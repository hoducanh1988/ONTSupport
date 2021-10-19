using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HEC {
    public class Olt : IDisposable {

        IProtocol olt = null;
        string address = "", user = "", pass = "", port = "";
        public string logData { get; set; }
        public bool isConnected = false;

        public Olt(string _address, string _user, string _pass, string _port) {
            address = _address;
            user = _user;
            pass = _pass;
            logData = "";

            isConnected = loginDut();
        }

        bool loginDut() {
            try {
                bool r = false;

                r = address.ToLower().Contains("com");

                if (r) { olt = new UART(); r = olt.Open(address, "115200", "8"); }
                else { olt = new TELNET(); r = olt.Open(address, "23"); }
                if (!r) return false;

                string msg = "";
                r = olt is TELNET ? this.LoginTelnet(user, pass, out msg) : this.LoginUart(user, pass, out msg);
                logData += msg;
                return r;
            }
            catch { return false; }
        }


        bool LoginTelnet(string _userName, string _passWord, out string msg) {
            msg = olt.Read();

            if (!msg.TrimEnd().EndsWith(":")) {
                msg = "Failed to connect : no login prompt";
                return false;
            }
            olt.WriteLine(_userName);
            Thread.Sleep(300);

            msg += olt.Read();
            if (!msg.TrimEnd().EndsWith(":")) {
                msg = "Failed to connect : no password prompt";
                return false;
            }
            olt.WriteLine(_passWord);
            Thread.Sleep(300);

            msg += olt.Read();

            return true;
        }

        bool LoginUart(string _userName, string _passWord, out string msg) {
            msg = "";
            return false;
        }


        public bool WriteLine(string cmd) {
            if (isConnected == false) return false;
            return olt.WriteLine(cmd);
        }

        public string Read() {
            if (isConnected == false) return "";
            return olt.Read();
        }

        public string aluShowPonUnprovisionOnu() {
            if (isConnected == false) return "";
            bool r = false;
            int count = 0;

        RE:
            count++;
            olt.WriteLine("environment inhibit-alarms");
            Thread.Sleep(300);
            olt.WriteLine($"show pon unprovision-onu | match exact:{port}");
            Thread.Sleep(1000);
            string data = olt.Read().Trim();
            logData += data;
            r = string.IsNullOrEmpty(data) == false && string.IsNullOrWhiteSpace(data) == false;
            if (!r) {
                if (count < 3) goto RE;
                else return "";
            }

            return data;
        }


        public string sanetShowOnuUnconfig() {
            if (isConnected == false) return "";
            bool r = false;
            int count = 0;

        RE:
            count++;
            olt.WriteLine("enable");
            Thread.Sleep(300);
            olt.WriteLine("con t");
            Thread.Sleep(300);
            olt.WriteLine($"show onu unconfig");
            Thread.Sleep(1000);
            string data = olt.Read().Trim();
            logData += data;
            r = string.IsNullOrEmpty(data) == false && string.IsNullOrWhiteSpace(data) == false;
            if (!r) {
                if (count < 3) goto RE;
                else return "";
            }

            return data;
        }

        public bool Close() {
            try {
                if (olt != null) olt.Close();
                return true;
            }
            catch { return false; }
        }

        public bool pingNetwork(string ip) {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            // Use the default Ttl value which is 128, 
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted. 
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 1000;

            try {
                PingReply reply = pingSender.Send(ip, timeout, buffer, options);
                if (reply.Status == IPStatus.Success) {
                    return true;
                }
                else {
                    return false;
                }
            }
            catch {
                return false;
            }
        }

        public void Dispose() {
            this.Close();
        }
    }
}
