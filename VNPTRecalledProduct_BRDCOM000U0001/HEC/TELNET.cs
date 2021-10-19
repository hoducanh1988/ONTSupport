using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HEC {

    public class TELNET : IProtocol {

        TcpClient clients;
        enum Verbs { WILL = 251, WONT = 252, DO = 253, DONT = 254, IAC = 255 }
        enum Options { SGA = 3 }
        bool _isconnected = false;

        void configTCP() {
            // Don't allow another process to bind to this port.
            this.clients.ExclusiveAddressUse = false;
            // sets the amount of time to linger after closing, using the LingerOption public property.
            this.clients.LingerState = new LingerOption(false, 0);
            // Sends data immediately upon calling NetworkStream.Write.
            this.clients.NoDelay = true;
            // Sets the receive buffer size using the ReceiveBufferSize public property.
            this.clients.ReceiveBufferSize = 1024;
            // Sets the receive time out using the ReceiveTimeout public property.
            this.clients.ReceiveTimeout = 5000;
            // Sets the send buffer size using the SendBufferSize public property.
            this.clients.SendBufferSize = 1024;
            // sets the send time out using the SendTimeout public property.
            this.clients.SendTimeout = 5000;
        }

        public bool Close() {
            try {
                if (this.clients != null) this.clients.Close();
                return true;
            }
            catch { return false; }
        }

        public bool isConnected() {
            return _isconnected;
        }

        public bool Open(string port_name, string baud_rate, string data_bit) {
            throw new NotImplementedException();
        }

        public bool Open(string visa_address) {
            throw new NotImplementedException();
        }

        public bool Open(string ip_address, string port) {
            this.clients = new TcpClient();
            this.configTCP();

            bool r = false;
            try {
                r = this.clients.ConnectAsync(ip_address, int.Parse(port)).Wait(3000);
            }
            catch { r = false; }
            _isconnected = r;
            return r;
        }

        public bool Open(string ip_address, string port, string width, string height) {
            throw new NotImplementedException();
        }

        public string Query(string cmd) {
            if (!isConnected()) return null;
            this.WriteLine(cmd);
            Thread.Sleep(100);
            return this.Read();
        }

        public bool Query(string cmd, string timeout_sec, string end_swith) {
            if (!isConnected()) return false;
            this.WriteLine(cmd);
            int delay_ms = 200;
            int count = 0;
            int max_count = (int.Parse(timeout_sec) * 1000) / delay_ms;
            string data = "";

        RE:
            bool r = true;
            count++;
            data += this.Read();
            r = data.Replace("\n", "").Replace("\r", "").Trim().ToLower().EndsWith(end_swith.ToLower());
            if (!r) {
                if (count < max_count) {
                    Thread.Sleep(delay_ms);
                    goto RE;
                }
            }

            return r;
        }

        public string Read() {
            if (!isConnected()) return null;

            if (!this.clients.Connected) return null;
            StringBuilder sb = new StringBuilder();
            do {
                ParseTelnet(sb);
                System.Threading.Thread.Sleep(300);
            } while (this.clients.Available > 0);

            string data = sb.ToString();
            return data;
        }

        public bool Write(string cmd) {
            try {
                if (!isConnected()) return false;
                byte[] buf = System.Text.ASCIIEncoding.ASCII.GetBytes(cmd);
                this.clients.GetStream().Write(buf, 0, buf.Length);
                return true;
            }
            catch {
                return false;
            }
        }

        public bool WriteBreak() {
            try {
                if (!isConnected()) return false;
                byte[] buf = System.Text.ASCIIEncoding.ASCII.GetBytes(new char[] { Convert.ToChar(03) });
                this.clients.GetStream().Write(buf, 0, buf.Length);
                return true;
            }
            catch {
                return false;
            }
        }

        public bool WriteLine(string cmd) {
            return this.Write(cmd + "\n");
        }


        void ParseTelnet(StringBuilder sb) {

            while (this.clients.Available > 0) {
                int input = this.clients.GetStream().ReadByte();
                switch (input) {
                    case -1:
                        break;
                    case (int)Verbs.IAC:
                        // interpret as command
                        int inputverb = this.clients.GetStream().ReadByte();
                        if (inputverb == -1) break;
                        switch (inputverb) {
                            case (int)Verbs.IAC:
                                //literal IAC = 255 escaped, so append char 255 to string
                                sb.Append(inputverb);
                                break;
                            case (int)Verbs.DO:
                            case (int)Verbs.DONT:
                            case (int)Verbs.WILL:
                            case (int)Verbs.WONT:
                                // reply to all commands with "WONT", unless it is SGA (suppres go ahead)
                                int inputoption = this.clients.GetStream().ReadByte();
                                if (inputoption == -1) break;
                                this.clients.GetStream().WriteByte((byte)Verbs.IAC);
                                if (inputoption == (int)Options.SGA)
                                    this.clients.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WILL : (byte)Verbs.DO);
                                else
                                    this.clients.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WONT : (byte)Verbs.DONT);
                                this.clients.GetStream().WriteByte((byte)inputoption);
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        sb.Append((char)input);
                        break;
                }
            }
        }
    }


}
