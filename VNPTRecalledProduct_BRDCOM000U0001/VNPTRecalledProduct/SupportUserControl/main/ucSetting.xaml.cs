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
using VNPTRecalledProduct.Function.Custom;
using VNPTRecalledProduct.Function.Global;

namespace VNPTRecalledProduct.SupportUserControl {
    /// <summary>
    /// Interaction logic for ucSetting.xaml
    /// </summary>
    public partial class ucSetting : UserControl {
        public ucSetting() {
            InitializeComponent();
            loadSettingItem();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            string b_content = (string)b.Content;

            switch (b_content) {
                case "Save Setting": {
                        using (var sw = new StreamWriter(myGlobal.myTesting.fileSetting)) {
                            foreach (var item in myGlobal.mySetting) {
                                string s = $"{item.Title.Replace(" ", "_")}={item.Content}";
                                sw.WriteLine(s);
                            }
                        }

                        string data = File.ReadAllText(myGlobal.myTesting.fileSetting);
                        HEC.Encryption encryption = new HEC.Encryption(myGlobal.myTesting.fileSetting);
                        encryption.saveSource(data);

                        MessageBox.Show("Success.", "Save Setting", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
            }
        }

        private bool loadSettingItem() {
            myGlobal.mySetting.Clear();
            HEC.Encryption encryption = new HEC.Encryption(myGlobal.myTesting.fileSetting);
            string data = encryption.readSource();
            string[] buffer = data.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            
            foreach (var b in buffer) {
                if (string.IsNullOrEmpty(b) == false) {
                    string title = b.Split('=')[0].Replace("_", " ").Trim();
                    string content = b.Split('=')[1];
                    SettingItemInfo item_info = new SettingItemInfo() { Title = title, Content = content };
                    myGlobal.mySetting.Add(item_info);

                    if (item_info.Title.ToLower().Contains("network speed")) {
                        ucItemComboboxSetting ucItem = new ucItemComboboxSetting(new List<string>() { "100", "1000" });
                        ucItem.DataContext = item_info;
                        this.sp_setting.Children.Add(ucItem);
                    }
                    else if (item_info.Title.ToLower().Contains("olt type")) {
                        ucItemComboboxSetting ucItem = new ucItemComboboxSetting(new List<string>() { "ALU", "SANET" });
                        ucItem.DataContext = item_info;
                        this.sp_setting.Children.Add(ucItem);
                    }
                    else if (item_info.Title.ToLower().Contains("ont ip") ||
                             item_info.Title.ToLower().Contains("ont telnet user") ||
                             item_info.Title.ToLower().Contains("ont telnet password") ||
                             item_info.Title.ToLower().Contains("firmware build time") ||
                             item_info.Title.ToLower().Contains("ecc error value") ||
                             item_info.Title.ToLower().Contains("usb plugin string")) { }
                    else {
                        ucItemTextBoxSetting ucItem = new ucItemTextBoxSetting();
                        ucItem.DataContext = item_info;
                        this.sp_setting.Children.Add(ucItem);
                    }
                }
            }

            return true;
        }
    }
}
