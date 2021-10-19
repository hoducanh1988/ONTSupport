using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ConnectWiFi {

    class Program {

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static void Main(string[] args) {
            //hide window
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);

            string[] data = File.ReadAllLines($"{AppDomain.CurrentDomain.BaseDirectory}connectin.txt");
            string ssid_name = data[0].Split('=')[1];
            string wifi_pass = data[1].Split('=')[1];
            string sleep = data[2].Split('=')[1];

            bool r = connectToSSID(ssid_name, wifi_pass, 5000, 3);
            if (r) Thread.Sleep(int.Parse(sleep));

            File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}connectout.txt", r ? "passed" : "failed", Encoding.Unicode);
        }


        static bool connectToSSID(string ssid_name, string password, int timeout_miliseconds, int retry_time) {
            int retry = 0;
        STA:
            retry++;
            bool kq = false;
            Thread t = new Thread(new ThreadStart(() => {
                kq = Connect_Wifi(ssid_name, password);
            }));
            t.IsBackground = true;
            t.Start();

            int count = 0;
            int max = timeout_miliseconds / 100;
        RE:
            count++;
            if (count < max) {
                if (t.IsAlive == true) {
                    Thread.Sleep(100);
                    goto RE;
                }
                else {
                    if (!kq) {
                        if (retry < retry_time) {
                            Thread.Sleep(1000);
                            goto STA;
                        }
                    }
                }
            }
            else {
                t.Abort();
                if (retry < retry_time) {
                    Thread.Sleep(1000);
                    goto STA;
                }
            }
            return kq;
        }


        static bool Connect_Wifi(string ssid_name, string password) {
            try {
                //wifi object
                SimpleWifi.Wifi wifi = new SimpleWifi.Wifi();

                //disconnect wifi
                wifi.Disconnect();

                //delay
                Thread.Sleep(250);

                //get list of access points
                IEnumerable<SimpleWifi.AccessPoint> accessPoints = wifi.GetAccessPoints();

                //for each access point from list
                foreach (var ap in accessPoints) {
                    if (ap.Name.ToLower().Contains(ssid_name.ToLower())) {
                        if (!ap.IsConnected) {

                            //connect if not connected
                            SimpleWifi.AuthRequest authRequest = new SimpleWifi.AuthRequest(ap);
                            if (authRequest.IsUsernameRequired == true) authRequest.Username = "user";
                            if (authRequest.IsPasswordRequired == true) authRequest.Password = password;

                            ap.Connect(authRequest);
                        }
                        return true;
                    }
                }
                return false;
            }
            catch {
                return false;
            }

        }
    }
}
