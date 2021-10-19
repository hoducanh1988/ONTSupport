using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ONTWiFiMaster.Function.Instrument;
using ONTWiFiMaster.Function.Global;
using ONTWiFiMaster.Function.Custom;
using System.Collections.ObjectModel;

namespace ONTWiFiMaster.Function.Excute {
    public class MeasureAttenuator {

        public bool Excute(ObservableCollection<AttenuatorInfo> attenuators, string antenna, string transmitPort, string receivePort, string power) {
            bool ret = false, isConnected = false;
            IInstrument equipment = null;

            try {
                //kiem tra du lieu dau vao
                if (attenuators == null || attenuators.Count == 0) goto END;
                //Kết nối máy đo wifi
                myGlobal.myCable.logSystem += "> Kết nối máy đo wifi ++++++++++++++++++++++++++++++++\n";
                myGlobal.myCable.logSystem += $"... {myGlobal.mySetting.instrumentType}, {myGlobal.mySetting.gpibAddress}\n";
                switch (myGlobal.mySetting.instrumentType.ToLower()) {
                    case "e6640a": { equipment = new E6640A<CableDataBinding>(myGlobal.myCable, myGlobal.mySetting.gpibAddress, out ret); break; }
                    case "mt8870a": { equipment = new MT8870A<CableDataBinding>(myGlobal.myCable, myGlobal.mySetting.gpibAddress, out ret); break; }
                }
                isConnected = ret;
                myGlobal.myCable.logSystem += $"... result = {isConnected}\n";
                if (!ret) goto END;

                //Chuyển máy đo sang mode đo suy hao
                if (myGlobal.mySetting.instrumentType.ToLower() == "mt8870a") {
                    equipment.config_Attenuator_Total(transmitPort, receivePort, power);
                    Thread.Sleep(1000);
                }

                switch (antenna) {
                    case "1": { myGlobal.myCable.progressMax1 = attenuators.Count; myGlobal.myCable.progressValue1 = 0; break; }
                    case "2": { myGlobal.myCable.progressMax2 = attenuators.Count; myGlobal.myCable.progressValue2 = 0; break; }
                }

                myGlobal.myCable.logSystem += "> Kết quả đo suy hao ++++++++++++++++++++++++++++++++\n";
                foreach (var att in attenuators) {
                    //Yêu cầu máy đo phát tín hiệu wifi
                    equipment.config_Attenuator_Transmitter(att.Frequency, power, transmitPort);
                    myGlobal.myCable.logSystem += $"... Antenna= {antenna} - Channel= {att.Channel} - Frequency= {att.Frequency} - Transmission port= {transmitPort} - Receiver port= {receivePort}\n";
                    myGlobal.myCable.logSystem += $"...power transmit: {power} dBm\n";

                    //Đọc giá trị công suất wifi thu được (tính trung bình 3 lần)
                    int count = 0;
                    List<double> listValue = new List<double>();
                RE:
                    count++;
                    string data = equipment.config_Attenuator_Receiver(att.Frequency, receivePort);
                    bool r = false;
                    double value;
                    r = double.TryParse(data, out value);
                    if (!r) {
                        if (count < 3) goto RE;
                    }
                    else listValue.Add(value);
                    myGlobal.myCable.logSystem += $"...result[{count}]: {value} dBm\n";
                    if (count < 3) goto RE;

                    if (listValue.Count == 0) data = double.MaxValue.ToString();
                    else data = Math.Round(double.Parse(power) - listValue.Average(), 2).ToString();

                    myGlobal.myCable.logSystem += string.Format("...Connector: {0} dBm\n", antenna.Equals("1") ? myGlobal.mySetting.Connector1 : myGlobal.mySetting.Connector2);
                    switch (antenna) {
                        case "1": {
                                att.Antenna1 = Math.Round(double.Parse(data) - double.Parse(myGlobal.mySetting.Connector1), 3).ToString();
                                myGlobal.myCable.logSystem += $"...Attenuation: {att.Antenna1} dBm\n...\n";
                                myGlobal.myCable.progressValue1++;
                                break; 
                            }
                        case "2": { 
                                att.Antenna2 = Math.Round(double.Parse(data) - double.Parse(myGlobal.mySetting.Connector2), 3).ToString();
                                myGlobal.myCable.logSystem += $"...Attenuation: {att.Antenna2} dBm\n...\n";
                                myGlobal.myCable.progressValue2++; 
                                break; 
                            }
                    }

                    bool x1 = string.IsNullOrEmpty(att.Antenna1) || string.IsNullOrWhiteSpace(att.Antenna1);
                    bool x2 = string.IsNullOrEmpty(att.Antenna2) || string.IsNullOrWhiteSpace(att.Antenna2);
                    if (x1 == false && x2 == false) {
                        bool y1 = (double.Parse(att.Antenna1) > double.Parse(myGlobal.mySetting.rf1Lower)) && (double.Parse(att.Antenna1) <= double.Parse(myGlobal.mySetting.rf1Upper));
                        bool y2 = (double.Parse(att.Antenna2) > double.Parse(myGlobal.mySetting.rf2Lower)) && (double.Parse(att.Antenna2) <= double.Parse(myGlobal.mySetting.rf2Upper));
                        att.Result = y1 && y2 ? "Passed" : "Failed";
                    }

                    myGlobal.myCable.currentIndex++;
                }

                ret = true;
                goto END;
            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show(ex.ToString(), "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                ret = false; 
                goto END; 
            }

        END:
            if (equipment != null && isConnected == true) equipment.Close();
            return ret;
        }

    }
}
