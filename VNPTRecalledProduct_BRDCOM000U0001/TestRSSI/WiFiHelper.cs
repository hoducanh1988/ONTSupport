using NativeWifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRSSI {
    public class WiFiHelper {

        string ssid_Name = "";
        WlanClient wifi_Client = null;

        public WiFiHelper(WlanClient client, string ssid_name) {
            this.ssid_Name = ssid_name;
            this.wifi_Client = client;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string getRSSID() {
            string rssi = null;
            try {
                bool r = false;
                foreach (WlanClient.WlanInterface wlanIface in wifi_Client.Interfaces) {
                    Wlan.WlanBssEntry[] wlanBssEntries = wlanIface.GetNetworkBssList();
                    foreach (Wlan.WlanBssEntry network in wlanBssEntries) {
                        int rss = network.rssi;
                        string act_ssid = System.Text.ASCIIEncoding.ASCII.GetString(network.dot11Ssid.SSID).ToString();
                        if (act_ssid.ToLower().Contains(ssid_Name.ToLower())) {
                            rssi = rss.ToString();
                            r = true;
                        }
                        if (r) break;
                    }
                    if (r) break;
                }
            }
            catch {
                rssi = null;
            }
            return rssi;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool refreshWiFi() {
            return true;
        }

    }
}
