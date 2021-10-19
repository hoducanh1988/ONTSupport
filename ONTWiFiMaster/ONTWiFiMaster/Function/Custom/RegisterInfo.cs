using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.Custom {

    public class RegisterInfo {
        public string macaddress { get; set; }
        public string totalresult { get; set; }

        //2G - Register
        public string _0x59 { get; set; }
        public string _0x5A { get; set; }
        public string _0x5B { get; set; }
        public string _0x5F { get; set; }
        public string _0x60 { get; set; }
        public string _0x61 { get; set; }

        //5G - Register
        public string _0x143 { get; set; }
        public string _0x144 { get; set; }
        public string _0x148 { get; set; }
        public string _0x149 { get; set; }
        public string _0x14D { get; set; }
        public string _0x14E { get; set; }
        public string _0x157 { get; set; }
        public string _0x158 { get; set; }
        public string _0x15C { get; set; }
        public string _0x15D { get; set; }
        public string _0x161 { get; set; }
        public string _0x162 { get; set; }
        public string _0x166 { get; set; }
        public string _0x167 { get; set; }
        public string _0x16B { get; set; }
        public string _0x16C { get; set; }
        public string _0x170 { get; set; }
        public string _0x171 { get; set; }
        public string _0x175 { get; set; }
        public string _0x176 { get; set; }
        public string _0x17F { get; set; }
        public string _0x180 { get; set; }
        public string _0x184 { get; set; }
        public string _0x185 { get; set; }
        public string _0x189 { get; set; }
        public string _0x18A { get; set; }
        public string _0x18E { get; set; }
        public string _0x18F { get; set; }

        public override string ToString() {
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36}",
                                 DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                                 macaddress,
                                 _0x59, _0x5A, _0x5B,
                                 _0x5F, _0x60, _0x61,
                                 _0x143, _0x144, _0x148, _0x149, _0x14D, _0x14E, _0x157, _0x158, _0x15C, _0x15D, _0x161, _0x162, _0x166, _0x167,
                                 _0x16B, _0x16C, _0x170, _0x171, _0x175, _0x176, _0x17F, _0x180, _0x184, _0x185, _0x189, _0x18A, _0x18E, _0x18F,
                                 totalresult
                                 );
        }
    }
}
