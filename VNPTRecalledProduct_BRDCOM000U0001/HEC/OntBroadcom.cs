using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HEC {

    public class OntBroadcom {

        IProtocol ont = null;
        string address = "", user = "", pass = "";
        public string logData { get; set; }
        public bool isConnected = false;

        public OntBroadcom(string _address, string _user, string _pass) {
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

                if (r) { ont = new UART(); r = ont.Open(address, "115200", "8"); }
                else { ont = new TELNET(); r = ont.Open(address, "23"); }
                if (!r) return false;

                string msg = "";
                r = ont is TELNET ? this.LoginTelnet(user, pass, out msg) : this.LoginUart(user, pass, out msg);
                logData += msg;
                return r;
            }
            catch { return false; }
        }


        bool LoginTelnet(string _userName, string _passWord, out string msg) {
            msg = ont.Read();

            if (!msg.TrimEnd().EndsWith(":")) {
                msg = "Failed to connect : no login prompt";
                return false;
            }
            ont.WriteLine(_userName);
            Thread.Sleep(300);

            msg += ont.Read();
            if (!msg.TrimEnd().EndsWith(":")) {
                msg = "Failed to connect : no password prompt";
                return false;
            }
            ont.WriteLine(_passWord);
            Thread.Sleep(300);

            msg += ont.Read();

            return true;
        }

        bool LoginUart(string _userName, string _passWord, out string msg) {
            msg = "";
            try {
                bool _flag = false;
                int index = 0;
                int max = 20;
                while (!_flag) {
                    //Gửi lệnh Enter để ONT về trạng thái đăng nhập
                    msg += "Gửi lệnh Enter để truy nhập vào login...\r\n";
                    ont.WriteLine("");
                    Thread.Sleep(200);
                    string data = "";
                    data = ont.Read();
                    msg += string.Format("Feedback:=> {0}\r\n", data);
                    if (data.Replace("\r", "").Replace("\n", "").Trim().Contains(">")) return true;
                    while (!data.Contains("Login:")) {
                        data += ont.Read();
                        msg += string.Format("Feedback:=> {0}\r\n", data);
                        Thread.Sleep(200);
                        if (index >= max) break;
                        else index++;
                    }
                    if (index >= max) break;

                    //Gửi thông tin User
                    ont.Write(_userName + "\n");
                    msg += "Gửi thông tin user: " + _userName + "...\r\n";

                    //Chờ ONT xác nhận User
                    data = "";
                    while (!data.Contains("Password:")) {
                        data += ont.Read();
                        msg += string.Format("Feedback:=> {0}\r\n", data);
                        Thread.Sleep(100);
                        if (index >= max) break;
                        else index++;
                    }
                    if (index >= max) break;

                    //Gửi thông tin Password
                    ont.Write(_passWord + "\n");
                    msg += "Gửi thông tin password: " + _passWord + "...\r\n";

                    //Chờ ONT xác nhận Password
                    while (!data.Contains(">")) {
                        data += ont.Read();
                        msg += string.Format("Feedback:=> {0}\r\n", data);
                        Thread.Sleep(500);
                        if (index >= max) break;
                        else index++;
                    }
                    if (index >= max) break;
                    else _flag = true;
                }
                return _flag;
            }
            catch (Exception ex) {
                msg += ex.ToString() + "\r\n";
                return false;
            }
        }


        public bool WriteLine(string cmd) {
            if (isConnected == false) return false;
            return ont.WriteLine(cmd);
        }

        public string Read() {
            if (isConnected == false) return "";
            return ont.Read();
        }

        public string getMacEthernet() {
            if (isConnected == false) return "";
            bool r = false;
            int count = 0;

        RE:
            count++;
            ont.WriteLine("ifconfig eth0");
            Thread.Sleep(300);
            string data = ont.Read().Trim();
            logData += data;
            r = data.ToLower().Contains("link encap:ethernet") && data.ToLower().Contains("hwaddr");
            if (!r) {
                if (count < 3) goto RE;
                else return "";
            }

            string mac = "";
            string[] buffer = data.Split('\n');
            foreach (var str in buffer) {
                if (str.ToLower().Contains("link encap:ethernet") && str.ToLower().Contains("hwaddr")) {
                    mac = str.ToLower().Split(new string[] { "hwaddr" }, StringSplitOptions.None)[1];
                }
            }
            mac = mac.Trim();
            mac = mac.Replace("\r", "").Replace("\n", "").Replace(":", "").Replace("-", "").Trim();

            return mac;
        }

        public string getFirmwareVersion() {
            if (isConnected == false) return "";
            bool r = false;
            int count = 0;

        RE:
            count++;
            ont.WriteLine("swversion");
            Thread.Sleep(300);
            string data = ont.Read().Trim();
            logData += data;
            r = data.ToLower().Contains("swversion") && data.EndsWith(">");
            if (!r) {
                if (count < 3) goto RE;
                else return "";
            }

            string fw_ver = "";
            string[] buffer = data.Split('\n');
            fw_ver = buffer[buffer.Length - 2];
            fw_ver = fw_ver.Replace("\r", "").Replace("\n", "").Trim();

            return fw_ver;
        }

        public string getFirmwareBuildTime() {
            if (isConnected == false) return "";
            bool r = false;
            int count = 0;

        RE:
            count++;
            ont.WriteLine("swversion -b");
            Thread.Sleep(300);
            string data = ont.Read().Trim();
            logData += data;
            r = data.ToLower().Contains("swversion -b") && data.EndsWith(">");
            if (!r) {
                if (count < 3) goto RE;
                else return "";
            }

            string fw_bt = "";
            string[] buffer = data.Split('\n');
            fw_bt = buffer[buffer.Length - 2];
            fw_bt = fw_bt.Replace("\r", "").Replace("\n", "").Trim();

            return fw_bt;
        }

        public string getGponSerial() {
            if (isConnected == false) return "";
            bool r = false;
            int count = 0;

        RE:
            count++;
            ont.WriteLine("sh");
            Thread.Sleep(200);
            ont.WriteLine("gponctl getSnPwd");
            Thread.Sleep(500);
            ont.WriteLine("exit");
            Thread.Sleep(100);


            string data = ont.Read().Trim();
            logData += data;
            r = data.ToLower().Contains("serial number:") && data.EndsWith(">");
            if (!r) {
                if (count < 3) goto RE;
                else return "";
            }

            string gpon_sn = "";
            string[] buffer = data.Split('\n');

            foreach (var b in buffer) {
                if (b.ToLower().Contains("serial number:")) {
                    gpon_sn = b;
                    break;
                }
            }
            gpon_sn = gpon_sn.ToLower().Replace("serial number:", "").Replace("\n", "").Replace("\r", "").Replace("-", "").Trim().ToUpper();
            return gpon_sn;
        }


        public string getHardwareVersion() {
            if (isConnected == false) return "";
            bool r = false;
            int count = 0;

        RE:
            count++;
            ont.WriteLine("mdm getpv InternetGatewayDevice.DeviceInfo.HardwareVersion");
            Thread.Sleep(300);
            string data = ont.Read().Trim();
            logData += data;
            r = data.ToLower().Contains("mdm getpv InternetGatewayDevice.DeviceInfo.HardwareVersion".ToLower()) && data.EndsWith(">");
            if (!r) {
                if (count < 3) goto RE;
                else return "";
            }

            string hw_ver = "";
            string[] buffer = data.Split('\n');
            foreach (var b in buffer) {
                if (b.ToLower().Contains("param value=")) {
                    hw_ver = b;
                    break;
                }
            }
            hw_ver = hw_ver.ToLower().Replace("param value=", "").Replace("\r", "").Trim().Split('v')[1].Trim();
            return hw_ver;
        }

        public string getLanSpeed(string eth_index) {
            if (isConnected == false) return "";
            ont.WriteLine("sh");
            Thread.Sleep(100);
            ont.WriteLine(string.Format("ethctl eth{0} media-type", eth_index));
            Thread.Sleep(500);
            ont.WriteLine("exit");
            Thread.Sleep(100);
            string data = ont.Read().Trim();
            logData += data;

            return data;
        }

        public string getFactorySerial() {
            if (isConnected == false) return "";
            bool r = false;
            int count = 0;

        RE:
            count++;
            ont.WriteLine("mdm getpv InternetGatewayDevice.DeviceInfo.FactorySerialNumber");
            Thread.Sleep(500);
            string data = ont.Read().Trim();
            logData += data;
            r = data.ToLower().Contains("mdm getpv InternetGatewayDevice.DeviceInfo.FactorySerialNumber".ToLower()) && data.EndsWith(">");
            if (!r) {
                if (count < 3) goto RE;
                else return "";
            }

            string fac_sn = "";
            string[] buffer = data.Split('\n');
            foreach (var b in buffer) {
                if (b.ToLower().Contains("param value=")) {
                    fac_sn = b;
                    break;
                }
            }
            fac_sn = fac_sn.ToLower().Replace("param value=", "").Replace("\r", "").ToUpper().Trim();
            return fac_sn;
        }

        public bool factoryReset(string sucess_string) {
            if (isConnected == false) return false;
            string data = "";
            try {
                bool r = false;
                int count = 0;

            RE:
                count++;
                ont.WriteLine("restoredefault");
                Thread.Sleep(100);
                data = ont.Read().Trim();
                logData += data;
                r = data.ToLower().Contains(sucess_string.ToLower());
                if (!r) {
                    if (count < 3) goto RE;
                    else return false;
                }
                return r;

            }
            catch {
                return true;
            }
        }


        public string getTxPower(int retry) {
            bool r = false;
            int count = 0;

        RE:
            count++;
            ont.WriteLine("laser power --txread");
            Thread.Sleep(500);
            string data = ont.Read().Trim();
            logData += data;
            r = data.ToLower().Contains("tx laser power".ToLower()) && data.EndsWith(">");
            if (!r) {
                if (count < retry) goto RE;
                else return "";
            }

            string tx_pwr = "";
            string[] buffer = data.Split('\n');
            r = false;
            foreach (var b in buffer) {
                if (r) {
                    tx_pwr = b;
                    break;
                }
                if (b.ToLower().Contains("tx laser power")) r = true;
            }
            tx_pwr = tx_pwr.ToLower().Replace("=", "").Replace("dbm", "").Replace("\r", "").ToUpper().Trim();
            return tx_pwr;
        }


        public string getRxPower(int retry) {
            bool r = false;
            int count = 0;

        RE:
            count++;
            ont.WriteLine("laser power --rxread");
            Thread.Sleep(500);
            string data = ont.Read().Trim();
            logData += data;
            r = data.ToLower().Contains("rx laser power".ToLower()) && data.EndsWith(">");
            if (!r) {
                if (count < retry) goto RE;
                else return "";
            }

            string rx_pwr = "";
            string[] buffer = data.Split('\n');
            r = false;
            foreach (var b in buffer) {
                if (r) {
                    rx_pwr = b;
                    break;
                }
                if (b.ToLower().Contains("rx laser power")) r = true;
            }
            rx_pwr = rx_pwr.ToLower().Replace("=", "").Replace("dbm", "").Replace("\r", "").ToUpper().Trim();
            return rx_pwr;
        }

        public string getUSBStatus(int retry) {
            int count = 0;
        RE:
            count++;
            ont.WriteLine("sh");
            Thread.Sleep(100);
            ont.WriteLine("ls /mnt/");
            Thread.Sleep(200);
            ont.WriteLine("exit");
            Thread.Sleep(100);
            string data = ont.Read();
            logData += data;
            if (string.IsNullOrEmpty(data) || string.IsNullOrWhiteSpace(data)) {
                if (count < retry) goto RE;
                else return null;
            }
            return data;
        }

        public string getECCValue(int retry) {
            int count = 0;
            string ecc_value = "";

        RE:
            count++;
            ont.WriteLine("chkecc");
            Thread.Sleep(100);
            string data = ont.Read();
            if (data.ToLower().Contains("eccerror value:") == false) {
                if (count < retry) goto RE;
                else return null;
            }

            string[] buffer = data.Split('\n');
            foreach (var b in buffer) {
                if (b.ToLower().Contains("eccerror value:")) {
                    ecc_value = b.ToLower().Split(new string[] { "eccerror value:" }, StringSplitOptions.None)[1].Replace("\r", "").Replace("\n", "").Trim();
                    break;
                }
            }

            ont.WriteLine("rstecc");
            Thread.Sleep(100);
            data += ont.Read();
            logData += data;

            return ecc_value;
        }

        public bool startVoice() {
            string data = ont.Read();
            if (data.ToLower().Contains("voice start")) return true;

            ont.WriteLine("loglevel set vodsl Notice");
            Thread.Sleep(500);
            //ont.WriteLine("voice set defaults");
            //Thread.Sleep(500);
            ont.WriteLine("mdm setpv InternetGatewayDevice.Services.VoiceService.1.Enable TRUE");
            Thread.Sleep(100);
            ont.WriteLine("voice set boundIfname LAN");
            Thread.Sleep(100);
            //ont.WriteLine("voice set reg 0 0.0.0.0");
            //Thread.Sleep(100);
            //ont.WriteLine("voice set proxy 0 0.0.0.0");
            //Thread.Sleep(100);
            //ont.WriteLine("voice set obProx 0 0.0.0.0");
            //Thread.Sleep(100);
            ont.WriteLine("voice stop");
            Thread.Sleep(500);
            ont.WriteLine("voice start");
            Thread.Sleep(10000);
            data += ont.Read();
            logData += data;
            return true;
            
        }


        public string getVoiceValue() {
            //bool r = false;
            string data = "";
            int count = 0;

        //RE:
            count++;
            ont.WriteLine("sh");
            Thread.Sleep(100);
            ont.WriteLine("dmesg|grep eCMGREVT_LINE_");
            Thread.Sleep(500);
            ont.WriteLine("exit");
            Thread.Sleep(100);

            data = ont.Read();
            logData += data;
            return data;
        }
        

        public bool Close() {
            try {
                if (ont != null) ont.Close();
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


        public byte[] HexToBin(string pHexString) {
            if (String.IsNullOrEmpty(pHexString))
                return new Byte[0];

            if (pHexString.Length % 2 != 0)
                throw new Exception("Hexstring must have an even length");

            Byte[] bin = new Byte[pHexString.Length / 2];
            int o = 0;
            int i = 0;
            for (; i < pHexString.Length; i += 2, o++) {
                switch (pHexString[i]) {
                    case '0': bin[o] = 0x00; break;
                    case '1': bin[o] = 0x10; break;
                    case '2': bin[o] = 0x20; break;
                    case '3': bin[o] = 0x30; break;
                    case '4': bin[o] = 0x40; break;
                    case '5': bin[o] = 0x50; break;
                    case '6': bin[o] = 0x60; break;
                    case '7': bin[o] = 0x70; break;
                    case '8': bin[o] = 0x80; break;
                    case '9': bin[o] = 0x90; break;
                    case 'A': bin[o] = 0xa0; break;
                    case 'a': bin[o] = 0xa0; break;
                    case 'B': bin[o] = 0xb0; break;
                    case 'b': bin[o] = 0xb0; break;
                    case 'C': bin[o] = 0xc0; break;
                    case 'c': bin[o] = 0xc0; break;
                    case 'D': bin[o] = 0xd0; break;
                    case 'd': bin[o] = 0xd0; break;
                    case 'E': bin[o] = 0xe0; break;
                    case 'e': bin[o] = 0xe0; break;
                    case 'F': bin[o] = 0xf0; break;
                    case 'f': bin[o] = 0xf0; break;
                    default: throw new Exception("Invalid character found during hex decode");
                }

                switch (pHexString[i + 1]) {
                    case '0': bin[o] |= 0x00; break;
                    case '1': bin[o] |= 0x01; break;
                    case '2': bin[o] |= 0x02; break;
                    case '3': bin[o] |= 0x03; break;
                    case '4': bin[o] |= 0x04; break;
                    case '5': bin[o] |= 0x05; break;
                    case '6': bin[o] |= 0x06; break;
                    case '7': bin[o] |= 0x07; break;
                    case '8': bin[o] |= 0x08; break;
                    case '9': bin[o] |= 0x09; break;
                    case 'A': bin[o] |= 0x0a; break;
                    case 'a': bin[o] |= 0x0a; break;
                    case 'B': bin[o] |= 0x0b; break;
                    case 'b': bin[o] |= 0x0b; break;
                    case 'C': bin[o] |= 0x0c; break;
                    case 'c': bin[o] |= 0x0c; break;
                    case 'D': bin[o] |= 0x0d; break;
                    case 'd': bin[o] |= 0x0d; break;
                    case 'E': bin[o] |= 0x0e; break;
                    case 'e': bin[o] |= 0x0e; break;
                    case 'F': bin[o] |= 0x0f; break;
                    case 'f': bin[o] |= 0x0f; break;
                    default: throw new Exception("Invalid character found during hex decode");
                }
            }
            return bin;
        }


        public string BinToHex(string bin) {
            string output = "";
            try {
                int rest = bin.Length % 4;
                bin = bin.PadLeft(rest, '0'); //pad the length out to by divideable by 4

                for (int i = 0; i <= bin.Length - 4; i += 4) {
                    output += string.Format("{0:X}", Convert.ToByte(bin.Substring(i, 4), 2));
                }

                return output;
            }
            catch {
                return "ERROR";
            }
        }

        public string genGponSerial(string mac) {
            try {
                string mac_Header = mac.Substring(0, 6);
                string low_MAC = mac.Substring(6, 6);
                string origalByteString = Convert.ToString(HexToBin(low_MAC)[0], 2).PadLeft(8, '0');
                string VNPT_SERIAL_ONT = null;

                origalByteString = origalByteString + "" + Convert.ToString(HexToBin(low_MAC)[1], 2).PadLeft(8, '0');
                origalByteString = origalByteString + "" + Convert.ToString(HexToBin(low_MAC)[2], 2).PadLeft(8, '0');
                //----HEX to BIN Cach 2-------
                string value = low_MAC;
                var s = String.Join("", low_MAC.Select(x => Convert.ToString(Convert.ToInt32(x + "", 16), 2).PadLeft(4, '0')));
                //----HEX to BIN Cach 2-------
                string shiftByteString = "";
                shiftByteString = origalByteString.Substring(1, origalByteString.Length - 1) + origalByteString[0];

                string[] lines = System.IO.File.ReadAllLines(string.Format("{0}GponFormat.dll", AppDomain.CurrentDomain.BaseDirectory));
                foreach (var line in lines) {
                    if (line.ToUpper().Contains(mac_Header.ToUpper())) {
                        VNPT_SERIAL_ONT = line.Split('=')[1].ToUpper() + BinToHex(shiftByteString);
                        break;
                    }
                }
                return VNPT_SERIAL_ONT;
            }
            catch {
                return "ERROR";
            }
        }


    }

}
