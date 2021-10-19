using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GW040Hv2Project.Function
{
    public class baseFunction
    {
        //Kết nối tới ONT
        static bool Connect_ONT(string ip, string user, string pass, string _serial)
        {
            try
            {
                GlobalData.MODEM = new ModemTelnet(ip, 23, _serial);

                if (!GlobalData.MODEM.Login(user, pass, 400))
                {
                    GlobalData.MODEM.Close();
                    return false;
                }
                else
                {
                    GlobalData.testingData.MACADDRESS = GlobalData.MODEM.getMAC();
                    return true;
                }
            }
            catch (Exception Ex)
            {
                Ex.ToString();
                return false;
            }
        }

        /// <summary>
        /// LIỆT KÊ TÊN TẤT CẢ CÁC CỔNG SERIAL PORT ĐANG KẾT NỐI VÀO MÁY TÍNH
        /// </summary>
        /// <returns></returns>
        public static List<string> get_Array_Of_SerialPort()
        {
            try
            {
                // Get a list of serial port names.
                //string[] ports = SerialPort.GetPortNames();
                List<string> list = new List<string>();
                list.Add("-");
                for (int i = 1; i < 100; i++)
                {
                    list.Add(string.Format("COM{0}", i));
                }
                //foreach (var item in ports) {
                //    list.Add(item);
                //}
                return list;
            }
            catch
            {
                return null;
            }
        }

        //Ket noi toi thiet bi do
        public static bool Connect_Function()
        {
            bool ret = connect_Ont();
            if (ret == false) return false;
            ret = connect_Instrument();
            return ret;
        }


        static bool connect_Ont() {
            bool ret = false;
            int count = 0;
        RE:
            count++;
            ret = Connect_ONT(GlobalData.initSetting.ONTIP, GlobalData.initSetting.ONTUSER, GlobalData.initSetting.ONTPASS, GlobalData.initSetting.SERIALPORT);
            if (ret == false) {
                GlobalData.testingData.LOGSYSTEM += string.Format("[FAIL] Connect to ONT FAIL => Retry {0}\r\n", count);
                if (count < 20) goto RE;
            }
            else {
                GlobalData.testingData.LOGSYSTEM += "[OK] Connect to ONT Successful.\r\n";
                GlobalData.testingData.LOGSYSTEM += string.Format("ONT MAC Address: {0}\r\n", GlobalData.testingData.MACADDRESS);
                if (GlobalData.logManager != null)
                    GlobalData.logManager.mac = GlobalData.testingData.MACADDRESS;
            }

            return ret;
        }

        static bool connect_Instrument() {
            bool ret = true;
            if (GlobalData.initSetting.INSTRUMENT == "E6640A") {
                GlobalData.INSTRUMENT = new E6640A_VISA(GlobalData.initSetting.VISAADDRESS, out ret);

            }
            else if (GlobalData.initSetting.INSTRUMENT == "MT8870A") {
                GlobalData.INSTRUMENT = new MT8870A_VISA(GlobalData.initSetting.VISAADDRESS, out ret);

            }
            if (!ret) {
                GlobalData.testingData.LOGSYSTEM += "FAIL: Lỗi kết nối máy đo";
                GlobalData.testingData.LOGERROR += string.Format("FAIL: Lỗi kết nối máy đo \n");
            }
            else {
                GlobalData.testingData.LOGSYSTEM += "[OK] Kết nối tới máy đo thành công.\r\n";
            }
            return ret;
        }


        public static bool rejectDifferenceValueFromList(List<double> list_in, out List<double> list_out) {
            list_out = new List<double>();
            if (list_in.Count == 0) return false;
            if (list_in.Count == 1) {
                foreach (var i in list_in) list_out.Add(i);
            }
            else if (list_in.Count == 2) {
                foreach (var i in list_in) list_out.Add(i);
            }
            else if (list_in.Count == 3) {
                double a = Math.Abs(list_in[0] - list_in[1]);
                double b = Math.Abs(list_in[0] - list_in[2]);
                double c = Math.Abs(list_in[1] - list_in[2]);

                List<double> list_tmp = new List<double>() { a, b, c };
                double min = list_tmp.Min();
                int rej_idx = -1;

                if (min == a) rej_idx = 2;
                if (min == b) rej_idx = 1;
                if (min == c) rej_idx = 0;

                for (int i = 0; i < 3; i++) {
                    if (i != rej_idx) {
                        list_out.Add(list_in[i]);
                    }
                }
            }
            return true;
        }

    }
}
