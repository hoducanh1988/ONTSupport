using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace HEC {
    public class Converter {

        public static string ToMacWlan2G(string mac_ethernet) {
            string hexMAC = "FAIL";
            try {
                int num = Int32.Parse(mac_ethernet, System.Globalization.NumberStyles.HexNumber);
                num = num + 1;
                hexMAC = num.ToString("X").Trim();
                int hexMAC_len = hexMAC.Length;
                if (hexMAC_len < 6) {
                    for (int i = 0; i < 6 - hexMAC_len; i++) {
                        hexMAC = "0" + hexMAC;
                    }
                }
                else
                    if (hexMAC_len == 7) {
                    hexMAC = hexMAC.Substring(0, 6);
                }
            }
            catch { }
            return hexMAC;
        }

        public static string intToTimeSpan(int time_ms) {
            TimeSpan result = TimeSpan.FromMilliseconds(time_ms);
            return result.ToString("hh':'mm':'ss");
        }


    }
}
