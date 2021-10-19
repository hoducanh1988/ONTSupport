using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ONTResetCalibWifi.Function {
    public class LogFile {

        string dir_log = AppDomain.CurrentDomain.BaseDirectory;
        string dir_system = "";
        string dir_uart = "";

        public LogFile() {
            dir_system = $"{dir_log}LogSystem\\{DateTime.Now.ToString("yyyy-MM-dd")}";
            dir_uart = $"{dir_log}LogUart\\{DateTime.Now.ToString("yyyy-MM-dd")}";

            if (Directory.Exists(dir_system) == false) Directory.CreateDirectory(dir_system);
            if (Directory.Exists(dir_uart) == false) Directory.CreateDirectory(dir_uart);
        }

        public bool SaveLogSystem(string data, string mac, string result) {
            try {
                string f = $"{dir_system}\\{mac}_{DateTime.Now.ToString("HHmmss")}_system_{result}.txt";
                File.WriteAllText(f, data);
                return true;
            } catch { return false; }
            
        }

        public bool SaveLogUart(string data, string mac, string result) {
            try {
                string f = $"{dir_uart}\\{mac}_{DateTime.Now.ToString("HHmmss")}_uart_{result}.txt";
                File.WriteAllText(f, data);
                return true;
            }
            catch { return false; }
        }



    }
}
