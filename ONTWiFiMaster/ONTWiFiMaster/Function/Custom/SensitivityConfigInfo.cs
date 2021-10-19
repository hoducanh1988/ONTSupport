using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Custom {
    public class SensitivityConfigInfo {
        public string wifi { get; set; } //0=b, 1=g, 3=n
        public string rate { get; set; }
        public string bandwidth { get; set; } //0=20M, 1=40M
        public string anten { get; set; }
        public string channelfreq { get; set; }
        public double powertransmit { get; set; }
        public int packet { get; set; } //so goi tin 

        public override string ToString() {
            return string.Format("{0}|{1}|{2}|{3}|{4}|{5}", wifi, rate, bandwidth, anten, channelfreq, packet);
        }

    }
}
