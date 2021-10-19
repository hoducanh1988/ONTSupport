using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Global {
    public class myParameter {

        public static List<string> listPort = new List<string>() { "PORT1", "PORT2", "PORT3", "PORT4" };
        public static List<string> listPower = new List<string>() { "0", "-5", "-10", "-15", "-20", "-25", "-30", "-35", "-40", "-45", "-50" };
        public static List<string> listPassword = new List<string>() { "VnT3ch@dm1n", "ttcn@77CN" };
        public static List<string> listInstrumentType = new List<string>() { "E6640A", "MT8870A" };
        public static List<string> listSerialPort = null;

        static myParameter() {
            listSerialPort = new List<string>();
            for (int i = 1; i < 100; i++) {
                listSerialPort.Add($"COM{i}");
            }
        }

    }
}
