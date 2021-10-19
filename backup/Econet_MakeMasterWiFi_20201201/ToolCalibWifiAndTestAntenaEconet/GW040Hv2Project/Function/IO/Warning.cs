using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace GW040Hv2Project.Function {
    public class Warning {

        static string fileSetting = @"C:\SoftData\040H\CalibWIFI\Setting.dll";
        static string fileFlag = @"C:\SoftData\040H\CalibWIFI\Flag.ini";

        static Warning() {
            string _path = @"C:\SoftData";
            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
            Thread.Sleep(100);
            _path = @"C:\SoftData\040H";
            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
            Thread.Sleep(100);
            _path = @"C:\SoftData\040H\CalibWIFI";
            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
            Thread.Sleep(100);
        }

        public static bool SaveFileSetting() {
            try {
                List<string> contents = new List<string>() { "01:00", "08:00", "13:00", "21:00" };
                File.WriteAllLines(fileSetting, contents);
                return true;
            }
            catch {
                return false;
            }
        }

        public static bool LoadFileSetting(out List<string> list) {
            list = new List<string>();
            try {
                string[] temp = File.ReadAllLines(fileSetting);
                if (temp.Length == 0) return false;
                for (int i = 0; i < temp.Length; i++) list.Add(temp[i]);
                return true;
            }
            catch {
                return false;
            }
        }

        public static bool SaveFileFlag() {
            try {
                File.WriteAllText(fileFlag, DateTime.Now.ToString());
                return true;
            }
            catch {
                return false;
            }
        }

        public static bool IsShowWarning() {
            try {
                //get time standard
                List<string> _list = null;
                if (!Warning.LoadFileSetting(out _list) || _list.Count == 0) {
                    Warning.SaveFileSetting();
                    return false; //ko canh bao
                }

                //get time measure attenuation
                string data = File.ReadAllText(fileFlag);
                DateTime date = DateTime.Parse(data);

                return false;
            }
            catch {
                return true; //canh bao
            }
        }


    }
}
