using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BosaManagementSystem.IFunction.Custom {
    public class bosaInfo {

        public bosaInfo() {
            bosasn = "";
            ith = "";
            pf = "";
            vf = "";
            im = "";
            vbr = "";
            sen = "";
            psat = "";
            bosafile = "";
            bosamd5 = "";
        }

        public string bosasn { get; set; }
        public string ith { get; set; }
        public string pf { get; set; }
        public string vf { get; set; }
        public string im { get; set; }
        public string vbr { get; set; }
        public string sen { get; set; }
        public string psat { get; set; }
        public string bosafile { get; set; }
        public string bosamd5 { get; set; }
    }
}
