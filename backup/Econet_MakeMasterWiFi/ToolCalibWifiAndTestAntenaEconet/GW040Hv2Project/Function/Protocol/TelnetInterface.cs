using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace GW040Hv2Project.Function {

    enum Verbs {
        WILL = 251,
        WONT = 252,
        DO = 253,
        DONT = 254,
        IAC = 255
    }

    enum Options {
        SGA = 3
    }

    public class TelnetConnection : IProtocol {
        TcpClient tcpSocket;

        int TimeOutMs = 400;

        public TelnetConnection(string Hostname, int Port) {
            tcpSocket = new TcpClient(Hostname, Port);
            this.configTCP();
        }

        private void configTCP() {
            // Don't allow another process to bind to this port.
            //this.tcpSocket.ExclusiveAddressUse = false;
            // sets the amount of time to linger after closing, using the LingerOption public property.
            this.tcpSocket.LingerState = new LingerOption(false, 0);
            // Sends data immediately upon calling NetworkStream.Write.
            this.tcpSocket.NoDelay = false;
            // Sets the receive buffer size using the ReceiveBufferSize public property.
            this.tcpSocket.ReceiveBufferSize = 8096;
            // Sets the receive time out using the ReceiveTimeout public property.
            this.tcpSocket.ReceiveTimeout = 5000;
            // Sets the send buffer size using the SendBufferSize public property.
            this.tcpSocket.SendBufferSize = 8096;
            // sets the send time out using the SendTimeout public property.
            this.tcpSocket.SendTimeout = 5000;
        }

        public string Login(string Username, string Password, int LoginTimeOutMs) {
            int oldTimeOutMs = TimeOutMs;
            TimeOutMs = LoginTimeOutMs;
            string s = Read();
            if (!s.TrimEnd().EndsWith(":"))
                throw new Exception("Failed to connect : no login prompt");
            WriteLine(Username);

            s += Read();
            if (!s.TrimEnd().EndsWith(":"))
                throw new Exception("Failed to connect : no password prompt");
            WriteLine(Password);

            s += Read();
            TimeOutMs = oldTimeOutMs;
            return s;
        }

        public bool WriteLine(string cmd) {
            try {
                Write(cmd + "\n");
                return true;
            }
            catch {
                return false;
            }
        }

        public bool Write(string cmd) {
            try {
                if (!tcpSocket.Connected) return false;
                byte[] buf = System.Text.ASCIIEncoding.ASCII.GetBytes(cmd.Replace("\0xFF", "\0xFF\0xFF"));
                tcpSocket.GetStream().Write(buf, 0, buf.Length);
                return true;
            } catch {
                return false;
            }
        }

        public bool Query(string cmd, string patern, int timeout_sec) {
            string value = "";
            Write(cmd + "\n");
            int tout = 0;
            int delay_time = 100;
            int max_count = (timeout_sec * 1000) / delay_time;
            REP:
            try {
                Thread.Sleep(delay_time);
                value = Read();
                value = value.Replace("\r", "").Replace("\n", "").Trim();
                if (value.Substring(value.Length - 1, 1) == "#") {
                    return true;
                }
                else {
                    tout++;
                    if (tout < max_count) goto REP;
                    else return false;
                }
            }
            catch {
                tout++;
                if (tout < max_count) goto REP;
                else return false;
            }
        }

        public bool Query(string cmd, string patern,out string data, int timeout_sec) {
            Write(cmd + "\n");
            int tout = 0;
            int delay_time = 100;
            int max_count = (timeout_sec * 1000) / delay_time;
            data = "";

        REP:
            try {
                Thread.Sleep(delay_time);
                data += Read();
                string value = data.Replace("\r", "").Replace("\n", "").Trim();
                if (value.Substring(value.Length - 1, 1) == "#") {
                    return true;
                }
                else {
                    tout++;
                    if (tout < max_count) goto REP;
                    else return false;
                }
            }
            catch {
                tout++;
                if (tout < max_count) goto REP;
                else return false;
            }
        }

        public string Read() {
            if (!tcpSocket.Connected) return null;
            StringBuilder sb = new StringBuilder();
            do {
                ParseTelnet(sb);
                System.Threading.Thread.Sleep(TimeOutMs);
            } while (tcpSocket.Available > 0);
            string data = sb.ToString();
            GlobalData.testingData.LOGTELNET += data;
            return data;
        }

        public bool Close() {
            try {
                tcpSocket.Close();
                return true;
            } catch {
                return false;
            }
        }

        public bool IsConnected() {
            return tcpSocket.Connected;
        }

        void ParseTelnet(StringBuilder sb) {
            try {
                while (tcpSocket.Available > 0) {
                    int input = tcpSocket.GetStream().ReadByte();
                    switch (input) {
                        case -1:
                            break;
                        case (int)Verbs.IAC:
                            // interpret as command
                            int inputverb = tcpSocket.GetStream().ReadByte();
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
                                    int inputoption = tcpSocket.GetStream().ReadByte();
                                    if (inputoption == -1) break;
                                    tcpSocket.GetStream().WriteByte((byte)Verbs.IAC);
                                    if (inputoption == (int)Options.SGA)
                                        tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WILL : (byte)Verbs.DO);
                                    else
                                        tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WONT : (byte)Verbs.DONT);
                                    tcpSocket.GetStream().WriteByte((byte)inputoption);
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
            catch {
                return;
            }
        }

        public bool Query(string cmd, int timeout_sec, params string[] paterns) {
            throw new NotImplementedException();
        }
    }
}

