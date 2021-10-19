using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using VNPTRecalledProduct.Function.Global;
using VNPTRecalledProduct.SupportUserControl;

namespace VNPTRecalledProduct {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        ucRunAll uc_runall = new ucRunAll();
        ucSetting uc_setting = new ucSetting();
        ucLog uc_log = new ucLog();
        ucHelp uc_help = new ucHelp();
        ucAbout uc_about = new ucAbout();
        ucLogin uc_login = new ucLogin();
        DispatcherTimer timer = null;

        public MainWindow() {
            InitializeComponent();
            this.SetStartupLocation();
            this.DataContext = myGlobal.myTesting;
            addControl(0);
            myGlobal.myAuthorization.Clear();

            timer = new DispatcherTimer();
            int timer_interval = 250;
            timer.Interval = TimeSpan.FromMilliseconds(timer_interval);
            timer.Tick += (s, e) => {
              if (myGlobal.myAuthorization.User.Equals("administrator") && myGlobal.myAuthorization.Password.Equals("12345678")) {
                    this.grid_main.Children.Clear();
                    this.grid_main.Children.Add(uc_setting);
                    myGlobal.myAuthorization.User = "";
                    myGlobal.myAuthorization.Password = "";
                }
            };
            timer.Start();
        }


        private void SetStartupLocation() {
            //double scaleX = 1;
            //double scaleY = 1;
            double scaleX = 0.85;
            double scaleY = 0.95;
            this.Height = SystemParameters.WorkArea.Height * scaleY;
            this.Width = SystemParameters.WorkArea.Width * scaleX;
            this.Top = (SystemParameters.WorkArea.Height * (1 - scaleY)) / 2;
            this.Left = (SystemParameters.WorkArea.Width * (1 - scaleX)) / 2;
        }


        private void Label_MouseDown(object sender, MouseButtonEventArgs e) {
            Label l = sender as Label;
            switch (l.Content.ToString()) {
                case "X": {
                        this.Close();
                        break;
                    }
                default: {
                        Dictionary<string, int> dictionary = new Dictionary<string, int>() { { "RUN ALL", 0 }, { "SETTING", 1 }, { "LOG", 2 }, { "HELP", 3 }, { "ABOUT", 4 } };
                        int t = 0;
                        bool ret = dictionary.TryGetValue(l.Content.ToString(), out t);
                        this.lblMinus.Margin = new Thickness(t * 100, 0, 0, 0);
                        addControl(t);
                        break;
                    }
            }
        }

        private bool addControl(int idx) {
            this.grid_main.Children.Clear();
            switch (idx) {
                case 0: { this.grid_main.Children.Add(uc_runall); break; }
                case 1: {
                        this.grid_main.Children.Add(uc_login); break;
                    }
                case 2: { this.grid_main.Children.Add(uc_log); break; }
                case 3: { this.grid_main.Children.Add(uc_help); break; }
                case 4: { this.grid_main.Children.Add(uc_about); break; }
            }
            return true;
        }

        private void WrapPanel_MouseDown(object sender, MouseButtonEventArgs e) {
            if (MessageBox.Show("Bạn muốn quay trở lại form login phải không?\nChọn 'YES' = đồng ý, 'NO' = thoát.", "Back to login form", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                LoginWindow window = new LoginWindow();
                window.Show();
                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            string s = File.ReadAllText(myGlobal.myTesting.fileProduct);
            if (s.Contains("using")) {
                string[] ds = File.ReadAllLines(myGlobal.myTesting.fileProduct);

                foreach (var item in myGlobal.datagridResult) {
                    string name = item.itemName;
                    string ischeck = item.isCheck;
                    int idx = ds.ToList().FindIndex(x => x.ToLower().Contains(name.ToLower()));
                    ds[idx - 1] = $"		//[{ischeck}]";
                }
                File.WriteAllLines(myGlobal.myTesting.fileProduct, ds);
            }
            else {
                HEC.Encryption encryption = new HEC.Encryption(myGlobal.myTesting.fileProduct);
                string data = encryption.readSource();
                string[] ds = data.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                foreach (var item in myGlobal.datagridResult) {
                    string name = item.itemName;
                    string ischeck = item.isCheck;
                    int idx = ds.ToList().FindIndex(x => x.ToLower().Contains(name.ToLower()));
                    ds[idx - 1] = $"		//[{ischeck}]";
                }

                File.WriteAllLines(myGlobal.myTesting.fileProduct, ds);
                data = File.ReadAllText(myGlobal.myTesting.fileProduct);
                encryption.saveSource(data);
            }



            
            
            this.timer.Stop();
        }

    }

}
