using ONTWiFiMaster.Function.Custom;
using ONTWiFiMaster.Function.Global;
using ONTWiFiMaster.uCtrl;
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

namespace ONTWiFiMaster {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        ucRunAll uc_runall = null;
        ucSetting uc_setting = null;
        ucLog uc_log = null;
        ucHelp uc_help = null;
        ucAbout uc_about = null;

        int mouse_counter = 0;

        public MainWindow() {
            InitializeComponent();
            
            //load setting file
            if (File.Exists(myGlobal.settingFileFullName)) myGlobal.mySetting = XmlHelper<SettingDataBinding>.FromXmlFile(myGlobal.settingFileFullName);
            else myGlobal.mySetting = new SettingDataBinding();
            myGlobal.myTesting = new TestingDataBinding();
            myGlobal.myCable = new CableDataBinding();
            myGlobal.myCalib = new CalibDataBinding();
            myGlobal.myVerify = new VerifyDataBinding();
            myGlobal.myMaster = new MasterDataBinding();

            uc_runall = new ucRunAll();
            uc_setting = new ucSetting();
            uc_log = new ucLog();
            uc_help = new ucHelp();
            uc_about = new ucAbout();

            //add control
            addControl(0);
            this.DataContext = myGlobal.myMainWindowDataBinding;
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
                        //add control
                        addControl(t);
                        break;
                    }
            }
        }

        private bool addControl(int idx) {
            this.grid_main.Children.Clear();
            switch (idx) {
                case 0: { this.grid_main.Children.Add(uc_runall); break; }
                case 1: { this.grid_main.Children.Add(uc_setting); break; }
                case 2: { this.grid_main.Children.Add(uc_log); break; }
                case 3: { this.grid_main.Children.Add(uc_help); break; }
                case 4: { this.grid_main.Children.Add(uc_about); break; }
            }
            return true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            XmlHelper<SettingDataBinding>.ToXmlFile(myGlobal.mySetting, myGlobal.settingFileFullName); //save setting to xml file
        }

        private void lbl_mode_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                mouse_counter++;
                if (mouse_counter % 10 == 0) {
                    myGlobal.myMainWindowDataBinding.modeRD = !myGlobal.myMainWindowDataBinding.modeRD;
                }
            }
        }
    }
}
