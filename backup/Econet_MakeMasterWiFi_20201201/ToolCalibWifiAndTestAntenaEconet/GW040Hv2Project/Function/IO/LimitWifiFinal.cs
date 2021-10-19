using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW040Hv2Project.Function.IO
{
    class LimitWifiFinal
    {
        static string fileName = string.Format("{0}Config\\LimitWifiFinal-Config.csv", System.AppDomain.CurrentDomain.BaseDirectory);

        static LimitWifiFinal()
        {           
        }
        public static bool readFromFile()
        {
            try
            {
                GlobalData.lstWifiTestingInfor.LstWifiTesting = new List<WifiTestingInfor>();
                if (File.Exists(fileName) == false) return false;
                var lines = File.ReadLines(fileName);
                foreach (var line in lines)
                {
                    if (!line.Contains("Frequency") || line!="")
                    {
                        string[] buffer = line.Split(',');
                        WifiTestingInfor wf = new WifiTestingInfor { Frequency = buffer[0].Trim(), Antena = buffer[1].Trim(), Attenuator = buffer[2].Trim(), UpperLimit = buffer[3].Trim(), LowerLimit = buffer[4].Trim()};
                        GlobalData.lstWifiTestingInfor.LstWifiTesting.Add(wf);
                    }
                }
                //var a = GlobalData.lstWifiTestingInfor.LstWifiTesting;
                //var b = a[0].Attenuator;
                
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool SaveFile()
        {
            string data = "Frequency,Antena,Attenuator,UpperLimit,LowerLimit\n";
            try
            {
                if (File.Exists(fileName) == false) return false;
                foreach(var tmp in GlobalData.lstWifiTestingInfor.LstWifiTesting)
                {
                    data += tmp.Frequency + "," + tmp.Antena + "," + tmp.Attenuator + "," + tmp.UpperLimit + "," + tmp.LowerLimit + "\n"; 
                }
                data = data.Trim();
                File.WriteAllText(fileName, data);
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}

