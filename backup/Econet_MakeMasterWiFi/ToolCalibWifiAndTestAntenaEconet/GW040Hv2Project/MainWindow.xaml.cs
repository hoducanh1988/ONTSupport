using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Xml;
using GW040Hv2Project.Function;
using GW040Hv2Project.Function.IO;
using GW040Hv2Project.SubWindow;

namespace GW040Hv2Project {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        DispatcherTimer timer = null;
        bool _isScroll = false;
        public MainWindow() {
            InitializeComponent();
            // read Station config // 
            txtVersion.Content = $"{GlobalData.version} - Copyright of VNPT Technology 2020";
            XmlDocument doc = new XmlDocument();
            doc.Load(string.Format("{0}setting.xml", AppDomain.CurrentDomain.BaseDirectory));
            foreach (XmlNode node in doc.DocumentElement.ChildNodes) {
                if (node.Name == "StationName") {
                    GlobalData.initSetting.STATION = node.InnerText.Trim();
                    break;
                }
            }

            this.datagridTX.ItemsSource = GlobalData.datagridlogTX;
            this.datagridRX.ItemsSource = GlobalData.datagridlogRX;

            GlobalData.testingData.RDContent = Properties.Settings.Default.RDMODE ? "MODE Chỉ Dành Cho Nghiên Cứu Phát Triển" : "";
            this.DataContext = GlobalData.testingData;
            this.spBefore.Visibility = GlobalData.initSetting.STATION == "Trước đóng vỏ" ? Visibility.Visible : Visibility.Collapsed;
            this.spAfter.Visibility = GlobalData.initSetting.STATION == "Trước đóng vỏ" ? Visibility.Collapsed : Visibility.Visible;
            GlobalData.testingData.APPTITLE = GlobalData.initSetting.STATION == "Trước đóng vỏ" ? "PHẦN MỀM CALIBRATION + LÀM MẠCH MASTER ONT " : "PHẦN MỀM TEST ANTEN WIFI ONT ";
            GlobalData.testingData.APPTITLE += GlobalData.initSetting.ONTTYPE;
            this.miMaster.Visibility = GlobalData.initSetting.STATION == "Trước đóng vỏ" ? Visibility.Visible : Visibility.Collapsed;
            //this.miAtten.Visibility = GlobalData.initSetting.STATION == "Trước đóng vỏ" ? Visibility.Visible : Visibility.Collapsed;
            //this.miReset.Visibility = GlobalData.initSetting.STATION == "Trước đóng vỏ" ? Visibility.Visible : Visibility.Collapsed;
            //this.miMode.Visibility = GlobalData.initSetting.STATION == "Trước đóng vỏ" ? Visibility.Visible : Visibility.Collapsed;
            this.lbl_counted.Visibility = GlobalData.initSetting.STATION == "Trước đóng vỏ" ? Visibility.Visible : Visibility.Collapsed;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            timer.Tick += ((sender, e) => {

                if (_isScroll == true) {
                    GlobalData.testingData.TimeCount++;
                    _scrollViewer.ScrollToEnd();
                    _scrollViewer1.ScrollToEnd();
                    _scrollViewer2.ScrollToEnd();
                    _scrollViewer3.ScrollToEnd();
                    _scrollViewer4.ScrollToEnd();
                    _scrollViewer5.ScrollToEnd();
                    if (GlobalData.datagridlogTX.Count > 0) {
                        this.datagridTX.ScrollIntoView(this.datagridTX.Items.GetItemAt(GlobalData.datagridlogTX.Count - 1));
                    }
                }
            });
            timer.Start();

        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e) {
            Label l = sender as Label;
            switch (l.Content) {
                case "X": { this.Close(); break; }
            }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            MenuItem menu = sender as MenuItem;
            string _text = menu.Header.ToString();
            string path = AppDomain.CurrentDomain.BaseDirectory.Replace($"{GlobalData.initSetting.ONTTYPE}\\", "").Replace($"{GlobalData.initSetting.ONTTYPE.ToLower()}\\", "");

            switch (_text) {
                case "Exit": { this.Close(); break; }
                case "open Log Folder": {
                        string root_path = AppDomain.CurrentDomain.BaseDirectory.Replace($"{GlobalData.initSetting.ONTTYPE}\\", "").Replace($"{GlobalData.initSetting.ONTTYPE.ToLower()}\\", "");
                        string dir_total = string.Format("{0}LogData\\{1}\\{2}\\{3}",
                                                  root_path,
                                                  GlobalData.initSetting.STATION == "Trước đóng vỏ" ? "CalibWiFi" : "TestAntenna",
                                                  GlobalData.initSetting.ONTTYPE,
                                                  DateTime.Now.ToString("yyyyMMdd"));

                        if (Directory.Exists(dir_total) == false) Directory.CreateDirectory(dir_total);
                        Process.Start(dir_total);

                        break;
                    }
                case "Cấu hình ONT": {
                        configWindow cfg = new configWindow();
                        cfg.ShowDialog();
                        break;
                    }
                case "Cài đặt máy đo wifi": {
                        instrumentWindow inst = new instrumentWindow();
                        inst.ShowDialog();
                        break;
                    }
                case "Thiết lập tiêu chuẩn": {
                        if (GlobalData.initSetting.STATION == "Trước đóng vỏ") {
                            limitWindow lim = new limitWindow();
                            lim.ShowDialog();
                        }
                        else {
                            wifiTestingConfigWindow frm = new wifiTestingConfigWindow();
                            frm.ShowDialog();
                        }
                        break;
                    }
                case "Thiết lập bài test": {
                        testcaseWindow tcw = new testcaseWindow();
                        tcw.ShowDialog();
                        break;
                    }
                case "Tính suy hao trạm": {
                        attenuatorWindow att = new attenuatorWindow();
                        att.ShowDialog();
                        break;
                    }
                case "Tính suy hao dây cable RF": {
                        Process.Start($"{AppDomain.CurrentDomain.BaseDirectory}TestSuyHaoXML\\TestSuyHaoXML.exe");
                        this.Close();
                        break;
                    }
                case "Thiết lập master": {
                        masterWindow mw = new masterWindow();
                        mw.ShowDialog();
                        break;
                    }
                case "Phân tích kết quả test": {
                        analyzerWindow anal = new analyzerWindow();
                        anal.ShowDialog();
                        break;
                    }
                case "Thiết lập giá trị thanh ghi": {
                        resetRegisterWindow rrw = new resetRegisterWindow();
                        rrw.ShowDialog();
                        break;
                    }
                case "About": {
                        About ab = new About();
                        ab.ShowDialog();
                        break;
                    }
                case "Mode RD": {
                        RDWindow rd = new RDWindow();
                        rd.ShowDialog();
                        break;
                    }

                default: break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            switch (b.Content) {
                case "START": {

                        switch (GlobalData.initSetting.STATION) {
                            case "Trước đóng vỏ": {
                                    if (!RDWindow.GetRDMode()) {
                                        //check đo suy hao trạm
                                        //if (GlobalData.initSetting.CALIBCOUNTED > 0) {
                                        //    GlobalData.initSetting.CALIBCOUNTED--;
                                        //    Properties.Settings.Default.Save();
                                        //}
                                        //else { MessageBox.Show("Vui lòng đo lại suy hao trạm test.", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                                    }

                                    //calib or test anten
                                    GlobalData.testingData.Initialize();
                                    GlobalData.datagridlogTX.Clear();
                                    GlobalData.datagridlogRX.Clear();
                                    Thread t = new Thread(new ThreadStart(() => {
                                        GlobalData.testingData.BUTTONCONTENT = "STOP";
                                        RUNALL();
                                        GlobalData.testingData.BUTTONCONTENT = "START";
                                    }));
                                    t.IsBackground = true;
                                    t.Start();
                                    break;
                                }
                            case "Sau đóng vỏ": {
                                    Thread zzz = new Thread(new ThreadStart(() => {
                                        GlobalData.testingData.Initialize();
                                        GlobalData.testingData.BUTTONCONTENT = "STOP";
                                        int c = 0;
                                    RE:
                                        bool r = Network.pingToHost(GlobalData.initSetting.ONTIP);
                                        Thread.Sleep(1000);
                                        if (r) {
                                            c = 0;
                                            if (GlobalData.testingData.TOTALRESULT == InitParameters.Statuses.None) {
                                                GlobalData.testingData.Initialize();
                                                RUNALL();
                                                GlobalData.testingData.LOGSYSTEM += "-----------------------------------------------------------------------\n";
                                                GlobalData.testingData.LOGSYSTEM += "Vui lòng rút dây LAN ra khỏi sản phẩm.";
                                            }
                                        }
                                        else {
                                            if (c % 10 == 0) GlobalData.testingData.LOGSYSTEM = "...\n";
                                            c++;
                                            GlobalData.testingData.LOGSYSTEM += string.Format("Ping to dut {0} ===> request time out [{1}]\n", GlobalData.initSetting.ONTIP, c);
                                            if (GlobalData.testingData.TOTALRESULT != InitParameters.Statuses.None) GlobalData.testingData.Initialize();
                                        }
                                        
                                        goto RE;
                                    }));
                                    zzz.IsBackground = true;
                                    zzz.Start();
                                    break;
                                }
                        }
                        break;
                    }
                default: break;
            }
        }

        #region MAIN_FUNCTION 

        private bool Read_All_Config_File() {
            try {
                if (!WaveForm.readFromFile()) return false;
                if (!Attenuator.readFromFile()) return false;
                if (!LimitTx.readFromFile()) return false;
                if (!LimitRx.readFromFile()) return false;
                if (!BIN.readFromFile()) return false;
                if (!CalibTx.readFromFile()) return false;
                if (!LimitWifiFinal.readFromFile()) return false;
                return true;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }
        private void RUNALL() {

            _isScroll = true;
            Stopwatch _st = new Stopwatch();
            _st.Start();
            bool _flag = true;
            GlobalData.logManager = null; //ko luu log neu logManager = null
            GlobalData.logRegister = null;
            GlobalData.testingData.TOTALRESULT = InitParameters.Statuses.Wait;
            GlobalData.testingData.LOGSYSTEM = $"{GlobalData.version} \n\n";

            //Đọc file cấu hình config
            if (!Read_All_Config_File()) {
                GlobalData.testingData.ERRORCODE = "{ ErrorCode: 0x001 }";
                GlobalData.testingData.LOGERROR += string.Format($"[FAIL] Lỗi khi đọc file cấu hình \n");

                _flag = false;
                goto Finished;
            }
            //Check Config antena sau đóng vỏ
            //--------------------------Check Config test antena sau đóng vỏ-----------------------------------------------------//
            if (!GlobalData.initSetting.STATION.Equals("Trước đóng vỏ")) {
                foreach (var tmp1 in GlobalData.listTestAnten1)//Check Antena 1
                {
                    bool isOk = false; float a;
                    foreach (var tmp2 in GlobalData.lstWifiTestingInfor.LstWifiTesting) {
                        if (tmp1.channelfreq.Trim() == tmp2.Frequency.Trim()) {
                            if (float.TryParse(tmp2.Frequency.Trim(), out a) && float.TryParse(tmp2.Antena.Trim(), out a) && float.TryParse(tmp2.Attenuator.Trim(), out a) && float.TryParse(tmp2.UpperLimit.Trim(), out a) && float.TryParse(tmp2.LowerLimit.Trim(), out a)) {
                                isOk = true;
                            }
                        }
                    }
                    if (!isOk) {
                        _flag = false;
                        // MessageBox.Show("Xin hãy kiểm tra lại danh mục 'Thiết Lập Tiêu Chuẩn'");
                        GlobalData.testingData.LOGERROR += string.Format($"[FAIL] Lỗi khi Thiết lập cấu hình Test antena 1 sau đóng vỏ \n");
                        return;//goto Finished;
                    }
                }
                foreach (var tmp1 in GlobalData.listTestAnten2) //Check Antena 2
                {
                    bool isOk = false; float a;
                    foreach (var tmp2 in GlobalData.lstWifiTestingInfor.LstWifiTesting) {
                        if (tmp1.channelfreq.Trim() == tmp2.Frequency.Trim()) {
                            if (float.TryParse(tmp2.Frequency.Trim(), out a) && float.TryParse(tmp2.Antena.Trim(), out a) && float.TryParse(tmp2.Attenuator.Trim(), out a) && float.TryParse(tmp2.UpperLimit.Trim(), out a) && float.TryParse(tmp2.LowerLimit.Trim(), out a)) {
                                isOk = true;
                            }
                        }
                    }
                    if (!isOk) {
                        _flag = false;
                        //MessageBox.Show("Xin hãy kiểm tra lại danh mục 'Thiết Lập Tiêu Chuẩn'");
                        GlobalData.testingData.LOGERROR += string.Format($"[FAIL] Lỗi khi Thiết lập cấu hình Test antena 2 sau đóng vỏ \n");
                        return; //goto Finished;
                    }
                }
            }
            GlobalData.logManager = new logdata(); //luu log test

            //Kết nối telnet tới ONT và máy đo
            if (!baseFunction.Connect_Function()) {
                GlobalData.testingData.ERRORCODE = "{ ErrorCode: 0x002 }";
                _flag = false;
                goto Finished;
            }

            if (GlobalData.initSetting.STATION == "Trước đóng vỏ") {
                Calibration calib = new Calibration(GlobalData.MODEM, GlobalData.INSTRUMENT);
                if (!calib.Excute()) {
                    _flag = false;
                    goto Finished;
                }
            }
            else {
                TestAnten testat = new TestAnten(GlobalData.MODEM, GlobalData.INSTRUMENT);
                if (!testat.Excute()) {
                    _flag = false;
                    goto Finished;
                }
            }

        Finished:
            try {
                GlobalData.MODEM.Close();
            }
            catch { }
            GlobalData.testingData.LOGSYSTEM += string.Format("{0} ĐÃ HOÀN THÀNH.\r\n", _flag == true ? "[OK]" : "[NG]");
            _st.Stop();
            GlobalData.testingData.LOGSYSTEM += string.Format("Tổng thời gian test: {0} sec.\r\n", _st.ElapsedMilliseconds / 1000);
            GlobalData.testingData.TOTALRESULT = _flag == true ? "PASS" : "FAIL";
            GlobalData.logManager.errorCode = GlobalData.testingData.ERRORCODE;
            GlobalData.logManager.totalResult = GlobalData.testingData.TOTALRESULT;

            if (GlobalData.logManager != null) {
                LogFile log = new LogFile(GlobalData.testingData.MACADDRESS, GlobalData.testingData.TOTALRESULT);
                log.saveTestLog(GlobalData.logManager);
                log.saveDetailLog(GlobalData.testingData.LOGSYSTEM);
                log.saveInstrumentlog(GlobalData.testingData.LOGINSTRUMENT);
                if (GlobalData.testingData.STATION == "Sau đóng vỏ") {
                    log.saveTelnetLog(GlobalData.testingData.LOGTELNET);
                }
                else {
                    log.saveUartLog(GlobalData.testingData.LOGUART);
                    log.savereViewLog(GlobalData.testingData.TOTALRESULT, (GlobalData.testingData.TESTRX2GRESULT == "FAIL" || GlobalData.testingData.TESTRX5GRESULT == "FAIL") ? "FAIL" : "PASS");
                }
                if (GlobalData.testingData.LOGERROR != "") {
                    log.saveErrorLog(GlobalData.testingData.LOGERROR);
                }
            }
            if (GlobalData.logRegister != null) {
                GlobalData.logRegister.macaddress = GlobalData.logManager.mac;
                GlobalData.logRegister.totalresult = GlobalData.testingData.TOTALRESULT;
                LogRegister.Save(GlobalData.logRegister);
            }
            _isScroll = false;
        }
        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            App.Current.Shutdown();
            //try
            //{
            //    GlobalData.MODEM.Close();
            //}
            //catch
            //{

            //}
            //try
            //{
            //    GlobalData.INSTRUMENT.Close();
            //}
            //catch
            //{
            //}
        }
    }
    class Station {
        public Station() {
            StationName = "Trước đóng vỏ";
        }
        string _station_name;
        public string StationName {
            get { return _station_name; }
            set {
                _station_name = value;
            }
        }
    }
}
