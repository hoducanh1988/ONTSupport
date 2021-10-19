using ONTWiFiMaster.Function.Custom;
using ONTWiFiMaster.Function.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTWiFiMaster.Function.IO {
    public class VerifyLogFile {

        string dir = $"{AppDomain.CurrentDomain.BaseDirectory}Log\\{myGlobal.myMainWindowDataBinding.productName}\\{DateTime.Now.ToString("yyyy-MM-dd")}";

        public VerifyLogFile() {
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);
        }

        public bool SaveToFile(VerifyDataBinding vdb) {
            string file = $"{dir}\\{vdb.macAddress}_{DateTime.Now.ToString("HHmmss")}_logverify.txt";
            StreamWriter sw = new StreamWriter(file, true, UnicodeEncoding.Unicode);
            sw.WriteLine(getVerifySetting());
            sw.WriteLine(getLogItem(vdb));
            sw.WriteLine(getLogTable());
            sw.WriteLine(getLogSystem(vdb));
            sw.WriteLine(getLogUart(vdb));
            sw.WriteLine(getLogInstrument(vdb));
            sw.Close();
            return true;
        }

        string getVerifySetting() {
            string data = "";
            data += $"{myGlobal.myMainWindowDataBinding.appInfo}\n";
            data += $"ONT type: {myGlobal.myMainWindowDataBinding.productName}\n";
            data += $"Serial port: {myGlobal.mySetting.serialPortName}\n";
            data += $"Login user: {myGlobal.mySetting.loginUser}\n";
            data += $"Login password: {myGlobal.mySetting.loginPassword}\n";
            data += $"Instrument type: {myGlobal.mySetting.instrumentType}\n";
            data += $"GPIB address: {myGlobal.mySetting.gpibAddress}\n";
            data += $"Antenna1: {myGlobal.mySetting.Port1}\n";
            data += $"Antenna2: {myGlobal.mySetting.Port2}\n";
            data += $"Test PER 2G: {myGlobal.mySetting.testPer2G}\n";
            data += $"Test PER 5G: {myGlobal.mySetting.testPer5G}\n";
            data += $"Test signal 2G: {myGlobal.mySetting.testSignal2G}\n";
            data += $"Test signal 5G: {myGlobal.mySetting.testSignal5G}\n";
            return data;
        }


        string getLogItem(VerifyDataBinding vdb) {
            string data = "";
            data += "******************************************\n";
            data += "*** LOG ITEM ***\n";
            data += "******************************************\n";
            data += $"Mac address: {vdb.macAddress}\n";
            data += "------------------------------------------\n";
            data += "Test PER 2G".PadLeft(20, ' ') + $"{vdb.per2GResult}".PadLeft(10, ' ') + "\n";
            data += "Test PER 5G".PadLeft(20, ' ') + $"{vdb.per5GResult}".PadLeft(10, ' ') + "\n";
            data += "Test tín hiệu 2G".PadLeft(20, ' ') + $"{vdb.signal2GResult}".PadLeft(10, ' ') + "\n";
            data += "Test tín hiệu 5G".PadLeft(20, ' ') + $"{vdb.signal5GResult}".PadLeft(10, ' ') + "\n";
            data += "------------------------------------------\n";
            data += $"Total time: {vdb.totalTime}\n";
            data += $"Total result: {vdb.totalResult}\n\n";
            return data;
        }

        string getLogTable() {
            string data = "";
            data += "******************************************\n";
            data += "*** LOG TABLE ***\n";
            data += "******************************************\n";
            data += $"Kết quả test per 2G:\n";
            data += $"{"RANGE".PadLeft(15, ' ')}{"ANTENNA".PadLeft(15, ' ')}{"WIFI".PadLeft(15, ' ')}{"MCS".PadLeft(15, ' ')}{"CHANNEL".PadLeft(15, ' ')}{"ATTENUATOR".PadLeft(15, ' ')}{"POWER".PadLeft(15, ' ')}{"SENT".PadLeft(15, ' ')}{"RECEIVED".PadLeft(15, ' ')}{"PER".PadLeft(15, ' ')}{"RESULT".PadLeft(15, ' ')}\n";
            foreach (var item in myGlobal.collectionPerResult2G) {
                data += item.ToString() + "\n";
            }
            data += $"Kết quả test per 5G:\n";
            data += $"{"RANGE".PadLeft(15, ' ')}{"ANTENNA".PadLeft(15, ' ')}{"WIFI".PadLeft(15, ' ')}{"MCS".PadLeft(15, ' ')}{"CHANNEL".PadLeft(15, ' ')}{"ATTENUATOR".PadLeft(15, ' ')}{"POWER".PadLeft(15, ' ')}{"SENT".PadLeft(15, ' ')}{"RECEIVED".PadLeft(15, ' ')}{"PER".PadLeft(15, ' ')}{"RESULT".PadLeft(15, ' ')}\n";
            foreach (var item in myGlobal.collectionPerResult5G) {
                data += item.ToString() + "\n";
            }
            data += $"\nKết quả test tín hiệu 2G:\n";
            data += $"{"RANGE".PadLeft(15, ' ')}{"ANTENNA".PadLeft(15, ' ')}{"WIFI".PadLeft(15, ' ')}{"MCS".PadLeft(15, ' ')}{"CHANNEL".PadLeft(15, ' ')}{"ATTENUATOR".PadLeft(15, ' ')}{"POWER".PadLeft(15, ' ')}{"EVM".PadLeft(15, ' ')}{"FREQERR".PadLeft(15, ' ')}{"RESULT".PadLeft(15, ' ')}\n";
            foreach (var item in myGlobal.collectionSignalResult2G) {
                data += item.ToString() + "\n";
            }
            data += $"\nKết quả test tín hiệu 5G:\n";
            data += $"{"RANGE".PadLeft(15, ' ')}{"ANTENNA".PadLeft(15, ' ')}{"WIFI".PadLeft(15, ' ')}{"MCS".PadLeft(15, ' ')}{"CHANNEL".PadLeft(15, ' ')}{"ATTENUATOR".PadLeft(15, ' ')}{"POWER".PadLeft(15, ' ')}{"EVM".PadLeft(15, ' ')}{"FREQERR".PadLeft(15, ' ')}{"RESULT".PadLeft(15, ' ')}\n";
            foreach (var item in myGlobal.collectionSignalResult5G) {
                data += item.ToString() + "\n";
            }
            data += "\n\n";
            return data;
        }

        string getLogSystem(VerifyDataBinding vdb) {
            string data = "";
            data += "******************************************\n";
            data += "*** LOG SYSTEM ***\n";
            data += "******************************************\n";
            data += vdb.logSystem;
            data += "\n\n";
            return data;
        }

        string getLogUart(VerifyDataBinding vdb) {
            string data = "";
            data += "******************************************\n";
            data += "*** LOG UART ***\n";
            data += "******************************************\n";
            data += vdb.logUart;
            data += "\n\n";
            return data;
        }

        string getLogInstrument(VerifyDataBinding vdb) {
            string data = "";
            data += "******************************************\n";
            data += "*** LOG INSTRUMENT ***\n";
            data += "******************************************\n";
            data += vdb.logInstrument;
            data += "\n\n";
            return data;
        }

    }
}
