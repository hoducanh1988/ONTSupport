using ONTWiFiMaster.Function.Custom;
using ONTWiFiMaster.Function.Global;
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
using UtilityPack.IO;

namespace ONTWiFiMaster.uCtrl {
    /// <summary>
    /// Interaction logic for ucSetting.xaml
    /// </summary>
    public partial class ucSetting : UserControl {

        public ucSetting() {
            InitializeComponent();
            setComboboxItemSource();

            //load setting file
            //if (File.Exists(myGlobal.settingFileFullName)) myGlobal.mySetting = XmlHelper<SettingDataBinding>.FromXmlFile(myGlobal.settingFileFullName);
            //else myGlobal.mySetting = new SettingDataBinding();

            this.DataContext = myGlobal.mySetting;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            string b_content = (string)b.Content;

            switch ((b_content as string).ToLower()) {
                case "save setting": {
                        XmlHelper<SettingDataBinding>.ToXmlFile(myGlobal.mySetting, myGlobal.settingFileFullName); //save setting to xml file
                        MessageBox.Show("Success.", "Save Setting", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
            }
        }

        private void setComboboxItemSource() {
            this.cbb_InstrumentType.ItemsSource = myParameter.listInstrumentType;
            this.cbb_LoginPassword.ItemsSource = myParameter.listPassword;
            this.cbb_SerialPort.ItemsSource = myParameter.listSerialPort;
        }

    }

}
