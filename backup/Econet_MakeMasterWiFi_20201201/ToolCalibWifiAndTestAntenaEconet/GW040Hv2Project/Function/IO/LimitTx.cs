﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW040Hv2Project.Function {
    public class LimitTx {

        static string filePath = string.Format("{0}Config\\", System.AppDomain.CurrentDomain.BaseDirectory);
        public static string fileName = "";

        //Load toàn bộ giá trị tên limit TX từ file vào listLimitWifiTX
        public static bool readFromFile() {
            try {
                // fileName = GlobalData.initSetting.ENWRITEBIN == false ? string.Format("{0}TxLimit-Config.csv", filePath) : string.Format("{0}TxLimit-Config-NewBIN.csv", filePath);
                fileName = string.Format("{0}TxLimit-Config-NewBIN.csv", filePath);
                fileName = GlobalData.initSetting.STATION == "Trước đóng vỏ" ? fileName : string.Format("{0}TxLimit-Config-Anten.csv", filePath);

                GlobalData.listLimitWifiTX = new List<limittx>();
                if (File.Exists(fileName) == false) return false;
                var lines = File.ReadLines(fileName);
                foreach (var line in lines) {
                    if (!line.Contains("RangeFrequency")) {
                        string[] buffer = line.Split(',');
                        limittx lt = new limittx() {
                            rangefreq = buffer[0],
                            wifi = buffer[1],
                            mcs = buffer[2],
                            power_MAX = buffer[3],
                            power_MIN = buffer[4],
                            evm_MAX = buffer[5],
                            evm_MIN = buffer[6],
                            freqError_MAX = buffer[7],
                            freqError_MIN = buffer[8],
                            symclock_MAX = buffer[9],
                            symclock_MIN = buffer[10]
                        };
                        GlobalData.listLimitWifiTX.Add(lt);
                    }
                }
                return true;
            }
            catch {
                return false;
            }
        }


        //Search tên limit TX từ listLimitWifiTX
        public static bool getData(string _rangefreq, string _wifi, string _mcs, out limittx _limit) {
            _limit = new limittx();
            try {
                if (GlobalData.listLimitWifiTX.Count == 0) return false;
                foreach (var item in GlobalData.listLimitWifiTX) {
                    if (item.rangefreq == _rangefreq && item.wifi == _wifi && item.mcs == _mcs) {
                        _limit = item;
                        break;
                    }
                }
                return true;
            }
            catch {
                return false;
            }
        }

    }
}
