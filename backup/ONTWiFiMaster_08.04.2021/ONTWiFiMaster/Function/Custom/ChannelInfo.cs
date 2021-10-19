using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Custom {
    public class ChannelInfo {

        public ChannelInfo() {
            WiFi = Channel = Frequency = "";
        }

        public string WiFi { get; set; }
        public string Channel { get; set; }
        public string Frequency { get; set; }
    }
}
