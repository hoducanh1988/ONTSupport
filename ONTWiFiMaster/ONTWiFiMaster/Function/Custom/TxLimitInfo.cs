using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Custom {
    public class TxLimitInfo {

        public string rangefreq { get; set; }
        public string wifi { get; set; }
        public string mcs { get; set; }
        public string power_MAX { get; set; }
        public string power_MIN { get; set; }
        public string evm_MAX { get; set; }
        public string evm_MIN { get; set; }
        public string freqError_MAX { get; set; }
        public string freqError_MIN { get; set; }
        public string symclock_MAX { get; set; }
        public string symclock_MIN { get; set; }

    }
}
