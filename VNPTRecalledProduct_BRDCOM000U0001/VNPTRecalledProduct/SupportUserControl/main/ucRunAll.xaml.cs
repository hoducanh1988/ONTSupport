using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using VNPTRecalledProduct.Function.Custom;
using VNPTRecalledProduct.Function.Global;
using VNPTRecalledProduct.Function.IO;
using VNPTRecalledProduct.SupportWindow;
using System.Text.RegularExpressions;
using System.Windows.Threading;

namespace VNPTRecalledProduct.SupportUserControl {
    /// <summary>
    /// Interaction logic for ucRunAll.xaml
    /// </summary>
    public partial class ucRunAll : UserControl {

        //Khai bao bien o day .....................//
        List<string> list_func = new List<string>();
        CSharpCodeProvider provider;
        CompilerParameters parameters;
        Assembly assembly;
        OutputTestLed ledwindow;
        OutputTestButtonFeedBack buttonWindow;
        OutputTestVoice voiceWindow;
        string[] setting;
        string code = "", name_space = "", name_class = "";
        int timer_count = 0;
        bool isRunning = false;
        int ping_count = 0;
        //...

        public ucRunAll() {
            InitializeComponent();
            myGlobal.myTesting.Clear();
            this.DataContext = myGlobal.myTesting;
            myGlobal.datagridResult.Clear();
            this.dg_result.ItemsSource = myGlobal.datagridResult;
            this.loadCodeFromTextFile();
            this.provider = new CSharpCodeProvider();
            this.parameters = new CompilerParameters();
            this.cbb_ischeck.ItemsSource = new List<string>() { "Yes", "No" };


            //timer đo thời gian test sản phẩm
            DispatcherTimer timer = new DispatcherTimer();
            int timer_interval = 500;
            timer.Interval = TimeSpan.FromMilliseconds(timer_interval);
            timer.Tick += (s, e) => {
                if (myGlobal.myTesting.totalResult.ToLower().Contains("waiting")) {
                    timer_count++;
                    myGlobal.myTesting.totalTime = HEC.Converter.intToTimeSpan(timer_count * timer_interval);
                }
                else timer_count = 0;
            };
            timer.Start();


            //timer test sản phẩm
            string address, user, password;
            loadSettingFile(out address, out user, out password);
            ping_count = 0;
            isRunning = false;
            DispatcherTimer timer_start = new DispatcherTimer();
            int interval = 500;
            Thread t = null;
            timer_start.Interval = TimeSpan.FromMilliseconds(interval);
            timer_start.Tick += (s, e) => {
                if (myGlobal.myTesting.autoStart) {
                    bool ___ = HEC.Parser.IsIPAddress(address);
                    if (___) {
                        bool r = Network.pingNetwork(address);
                        if (r) {
                            ping_count++;
                            if (ping_count >= 3) {
                                if (!isRunning) {
                                    t = new Thread(new ThreadStart(() => {
                                        startCheck();
                                    }));
                                    t.IsBackground = true;
                                    t.Start();
                                    isRunning = true;
                                }
                            }
                        }
                        else {
                            if (t.IsAlive == false) {
                                ping_count = 0;
                                isRunning = false;
                            }
                        }
                    }

                }
            };
            timer_start.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            string b_cnt = (string)b.Content;

            switch (b_cnt) {
                case "Start": {
                        Thread t = new Thread(new ThreadStart(() => {
                            Dispatcher.Invoke(new Action(() => { b.Content = "Stop"; }));
                            startCheck();
                            Dispatcher.Invoke(new Action(() => { b.Content = "Start"; }));
                        }));
                        t.IsBackground = true;
                        t.Start();
                        break;
                    }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            MenuItem mi = sender as MenuItem;
            string m_header = (string)mi.Header;
            switch (m_header.ToLower()) {
                case "refresh": {
                        this.dg_result.UnselectAllCells();
                        break;
                    }
            }
        }

        #region sub function

        private void startCheck() {
            Stopwatch st = new Stopwatch();
            st.Start();

            Dispatcher.Invoke(new Action(() => {
                foreach (var item in myGlobal.datagridResult) item.itemInput = item.itemOutput = item.itemResult = "";
            }));
            myGlobal.myTesting.waitParam();

            bool r = false, total_result = true;
            string address, user, password;

            //load setting file
            r = loadSettingFile(out address, out user, out password); if (!r) goto END;
            //add library dll file
            r = addLibrary(); if (!r) goto END;
            //compiler source code
            r = compilerSourceCode(); if (!r) goto END;
            //run test
            runAll(address, user, password, ref total_result);

        END:
            st.Stop();
            myGlobal.myTesting.logSystem += string.Format("-----------------------------------------------------------\n");
            myGlobal.myTesting.logSystem += $"> total result: { string.Format("{0}", total_result ? "Passed" : "Failed") }\n";
            myGlobal.myTesting.logSystem += $"> total time: {st.ElapsedMilliseconds} ms\n";

            //show total result
            bool ___ = total_result ? myGlobal.myTesting.passParam() : myGlobal.myTesting.failParam();
            myGlobal.myTesting.productTested++;
            myGlobal.myTesting.productPassed = total_result ? myGlobal.myTesting.productPassed + 1 : myGlobal.myTesting.productPassed;
            myGlobal.myTesting.productFailed = total_result ? myGlobal.myTesting.productFailed : myGlobal.myTesting.productFailed + 1;
            myGlobal.myTesting.productErrorRate = (myGlobal.myTesting.productFailed * 100) / myGlobal.myTesting.productTested;

            //show input barcode
            foreach (var item in myGlobal.listBarcode) {
                myGlobal.myTesting.inputBarcode += $"{item.Name.ToUpper().Replace("_", "").Replace("BARCODE", "").Trim()} : {item.Content}\n";
            }

            //save log detail
            saveLogDetail(total_result);
            saveLogTotal(total_result);
        }


        private void countTotalItemTest() {
            myGlobal.myTesting.totalTestItem = 0;
            foreach (var item in myGlobal.datagridResult) {
                if (item.isCheck.ToLower().Contains("yes")) myGlobal.myTesting.totalTestItem++;
            }
        }

        private void loadCodeFromTextFile() {
            code = File.ReadAllText(myGlobal.myTesting.fileProduct);
            if (code.Contains("using") == false) {
                HEC.Encryption encryption = new HEC.Encryption(myGlobal.myTesting.fileProduct);
                code = encryption.readSource();
            }

            string[] buffer = code.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            //read station
            myGlobal.myTesting.productName = buffer.Where(x => x.ToLower().Contains("product=")).FirstOrDefault().Split('=')[1].Trim();
            myGlobal.myTesting.Station = buffer.Where(x => x.ToLower().Contains("station=")).FirstOrDefault().Split('=')[1].Trim();

            //read app title
            string version = buffer.Where(x => x.ToLower().Contains("version=")).FirstOrDefault().Split('=')[1].Trim();
            string buildtime = buffer.Where(x => x.ToLower().Contains("buildtime=")).FirstOrDefault().Split('=')[1].Trim();
            string copyright = buffer.Where(x => x.ToLower().Contains("copyright=")).FirstOrDefault().Split('=')[1].Trim();
            myGlobal.myTesting.appTitle = $"Version: {version} - Build time: {buildtime} - {copyright}";

            //read namespace and class
            name_space = buffer.Where(x => x.ToLower().Contains("namespace")).FirstOrDefault().Replace("namespace", "").Replace("{", "").Replace("\r", "").Trim();
            name_class = buffer.Where(x => x.ToLower().Contains("class")).FirstOrDefault().Replace("class", "").Replace("public", "").Replace("{", "").Replace("\r", "").Trim();

            //read public function
            int i = 0, idx = 1;
            foreach (var x in buffer) {
                if (x.Contains("public bool") && x.Contains("(")) {
                    string s = x.Replace("{", "").Replace("public bool", "").Trim();
                    string ck = buffer[i - 1].Replace("//", "").Replace("[", "").Replace("]", "").Replace("\r", "").Trim();
                    list_func.Add(s);
                    Dispatcher.Invoke(new Action(() => { myGlobal.datagridResult.Add(new ResultInfo() { Index = idx.ToString(), itemName = s.Split('(')[0].Trim(), isCheck = ck, itemResult = "", itemInput = "", itemOutput = "" }); }));
                    idx++;
                }
                i++;
            }
        }

        private bool loadSettingFile(out string address, out string user, out string password) {
            address = ""; user = ""; password = "";
            try {
                //show info to log
                myGlobal.myTesting.logSystem += string.Format("> App info: {0}\n", myGlobal.myTesting.appTitle);
                myGlobal.myTesting.logSystem += string.Format("> product name: {0}\n", myGlobal.myTesting.productName);
                myGlobal.myTesting.logSystem += string.Format("> product station: {0}\n", myGlobal.myTesting.Station);
                myGlobal.myTesting.logSystem += string.Format("-----------------------------------------------------------\n");

                //load setting
                //setting = File.ReadAllLines(myGlobal.myTesting.fileSetting);
                HEC.Encryption encryption = new HEC.Encryption(myGlobal.myTesting.fileSetting);
                string data = encryption.readSource();
                setting = data.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                address = setting.ToList().Where(x => x.ToLower().Contains("ont_ip=")).FirstOrDefault().Split('=')[1].Replace("\r", "").Trim();
                user = setting.ToList().Where(x => x.ToLower().Contains("ont_telnet_user=")).FirstOrDefault().Split('=')[1].Replace("\r", "").Trim();
                password = setting.ToList().Where(x => x.ToLower().Contains("ont_telnet_password=")).FirstOrDefault().Split('=')[1].Replace("\r", "").Trim();

                //show setting to log
                foreach (var line in setting) myGlobal.myTesting.logSystem += string.Format("> {0}\n", line);
                myGlobal.myTesting.logSystem += string.Format("-----------------------------------------------------------\n");

                return true;
            }
            catch { return false; }
        }

        private bool addLibrary() {
            try {
                // Reference to System.Drawing library
                parameters.ReferencedAssemblies.Add("System.Drawing.dll");
                parameters.ReferencedAssemblies.Add("System.Core.dll");
                parameters.ReferencedAssemblies.Add("System.dll");
                parameters.ReferencedAssemblies.Add("HEC.dll");
                return true;
            }
            catch { return false; }
        }

        private bool compilerSourceCode() {
            try {
                // True - memory generation, false - external file generation
                parameters.GenerateInMemory = true;
                parameters.GenerateExecutable = true;
                CompilerResults results = provider.CompileAssemblyFromSource(parameters, this.code);

                if (results.Errors.HasErrors) {
                    StringBuilder sb = new StringBuilder();
                    foreach (CompilerError error in results.Errors) {
                        sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                    }
                    throw new Exception(sb.ToString());
                }

                assembly = results.CompiledAssembly;
                return true;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); return false; }
        }

        private void getListBarcode(out List<ItemInputBarcode> items) {
            items = new List<ItemInputBarcode>();
            int i = 0;
            foreach (var f in list_func) {
                if (myGlobal.datagridResult[i].isCheck.ToLower().Equals("yes")) {
                    string f_para_name = f.Split('(')[1].Split(')')[0].Trim();
                    if (string.IsNullOrEmpty(f_para_name) == false) {
                        string[] bff = f_para_name.Split(',');
                        foreach (var bf in bff) {
                            string[] ss = bf.Split(' ');
                            string s = ss[ss.Length - 1];

                            if (s.Contains("barcode")) {
                                ItemInputBarcode item = new ItemInputBarcode() { Name = s, Content = "" };
                                items.Add(item);
                            }
                        }
                    }
                }
                i++;
            }

        }

        private void showWindowInputBarcode() {
            if (myGlobal.listBarcode.Count == 0) return;
            Dispatcher.Invoke(new Action(() => {
                InputFromBarcode input = new InputFromBarcode();
                foreach (var item in myGlobal.listBarcode) {
                    ucItemInputBarcode uci = new ucItemInputBarcode(item);
                    input.mainViewer.Children.Add(uci);
                }
                input.ShowDialog();
            }));
        }

        private bool testLED(object classInstance, MethodInfo method, object[] function_parameter_value) {
            //check led function
            Dictionary<string, string> dictLed = function_parameter_value[function_parameter_value.Length - 1] as Dictionary<string, string>;
            List<string> listColor = new List<string>();
            foreach (var item in dictLed) {
                string value = item.Value;
                if (listColor.Count == 0) {
                    listColor.Add(value);
                }
                else {
                    var d = listColor.Where(x => x.Equals(value)).ToList();
                    if (d == null || d.Count == 0) listColor.Add(value);
                }
            }

            Dispatcher.Invoke(new Action(() => {
                ledwindow = new OutputTestLed();
                foreach (var c in listColor) {

                    //add label
                    Label lb = new Label();
                    lb.Content = $"LED Sáng {c.Replace("_", " ")} :";
                    lb.FontSize = 15;
                    lb.FontWeight = FontWeights.SemiBold;
                    lb.Margin = new Thickness(0, 5, 0, -5);
                    ledwindow.mainViewer.Children.Add(lb);

                    //add led
                    WrapPanel wp = new WrapPanel();
                    foreach (var item in dictLed) {
                        if (item.Value.Equals(c)) {
                            ItemLed led = new ItemLed() { Name = item.Key.Replace("_", " "), Value = true };
                            ucSingleLed ucled = new ucSingleLed(led);
                            wp.Children.Add(ucled);
                        }
                    }
                    ledwindow.mainViewer.Children.Add(wp);
                }
                ledwindow.Show();
            }));
        RE:
            method.Invoke(classInstance, function_parameter_value);
            if (ledwindow.isShowing) {
                goto RE;
            }

            bool r = ledwindow.Result;
            return r;

        }

        private bool testVoice(object classInstance, MethodInfo method, object[] function_parameter_value) {
            int time_out_sec = 30;
            bool r = false;
            int count = 0;

            Dispatcher.Invoke(new Action(() => {
                voiceWindow = new OutputTestVoice(time_out_sec);
                voiceWindow.warningText = "Vui lòng rút 2 điện trở ra khỏi 2 cổng voice...";
                voiceWindow.Show();
            }));
            function_parameter_value[0] = "1" as object;

        RE:
            count++;
            r = (bool)method.Invoke(classInstance, function_parameter_value);
            if (!r) {
                if (count < time_out_sec) {
                    goto RE;
                }
            }

            Dispatcher.Invoke(new Action(() => { voiceWindow.Close(); }));

            return r;
        }


        private bool testButton(object classInstance, MethodInfo method, object[] function_parameter_value) {
            Dictionary<string, string> dictButton = function_parameter_value[function_parameter_value.Length - 1] as Dictionary<string, string>;
            string name = "", legend = "";
            foreach (var i in dictButton) {
                name = i.Key;
                legend = i.Value;
                break;
            }
            ItemButton item = new ItemButton() { Name = name, Legend = legend, Value = null };

            Dispatcher.Invoke(new Action(() => {
                buttonWindow = new OutputTestButtonFeedBack(30);
                buttonWindow.warningText = name;
                buttonWindow.legendText = legend;
                buttonWindow.Show();
            }));

            bool r = false;
            int count = 0;
        RE:
            count++;
            //get text and command ...
            r = (bool)method.Invoke(classInstance, function_parameter_value);
            if (!r) {
                if (count < 30) goto RE;
            }
            Dispatcher.Invoke(new Action(() => { buttonWindow.Close(); }));

            return r;
        }

        //private bool testButton(object classInstance, MethodInfo method, object[] function_parameter_value) {
        //    Dictionary<string, string> dictButton = function_parameter_value[function_parameter_value.Length - 1] as Dictionary<string, string>;
        //    string name = "", legend = "";
        //    foreach (var i in dictButton) {
        //        name = i.Key;
        //        legend = i.Value;
        //        break;
        //    }
        //    ItemButton item = new ItemButton() { Name = name, Legend = legend, Value = null };

        //    Dispatcher.Invoke(new Action(() => {
        //        buttonWindow = new OutputTestButtonNoFeedBack(item);
        //        buttonWindow.Show();
        //    }));

        //    bool r = false;
        //RE:
        //    if ((function_parameter_value[0] as string).Trim().Equals("")) {
        //        if (buttonWindow.item.Value == null) {
        //            Thread.Sleep(100);
        //            goto RE;
        //        }
        //    }
        //    else {
        //        //get text and command ...
        //    }

        //    r = buttonWindow.item.Value == true;
        //    return r;
        //}

        private bool runAll(string address, string user, string password, ref bool total_result) {
            try {
                //
                Type program = assembly.GetType($"{name_space}.{name_class}");
                object classInstance = Activator.CreateInstance(program, new object[] { address, user, password });
                PropertyInfo property = program.GetProperty("logData");
                MethodInfo method = null;

                //Nhập thông tin từ tem
                getListBarcode(out myGlobal.listBarcode);
                showWindowInputBarcode();
                countTotalItemTest();

                //test
                int i = 0;
                foreach (var func in list_func) {
                    if (myGlobal.datagridResult[i].isCheck.ToLower().Equals("yes")) {
                        Dispatcher.Invoke(new Action(() => { myGlobal.datagridResult[i].itemResult = "Waiting..."; }));

                        string function_name = func.Split('(')[0].Trim();
                        string function_parameter_name = func.Split('(')[1].Split(')')[0].Trim();
                        object[] function_parameter_value = null;

                        if (string.IsNullOrEmpty(function_parameter_name) == false) {
                            var buffer = function_parameter_name.Split(' ').Where(x => x.Contains("_")).ToArray();
                            function_parameter_value = new object[buffer.Length];

                            for (int k = 0; k < buffer.Length; k++) {
                                string s = buffer[k].Replace(",", "");
                                if (s.Contains("barcode")) {
                                    var item = myGlobal.listBarcode.Where(x => x.Name.Equals(s)).FirstOrDefault();
                                    function_parameter_value[k] = item.Content;
                                }
                                else if (s.Contains("actual")) {
                                    function_parameter_value[k] = new object() as string;
                                }
                                else if (s.Contains("dictionary_led")) {
                                    function_parameter_value[k] = new object() as Dictionary<string, string>;
                                }
                                else if (s.Contains("dictionary_button")) {
                                    function_parameter_value[k] = new object() as Dictionary<string, string>;
                                }
                                else if (s.Contains("voice_index_control")) {
                                    function_parameter_value[k] = "0" as object;
                                }
                                else {
                                    function_parameter_value[k] = setting.Where(x => x.ToLower().Contains(s.ToLower())).FirstOrDefault().Split('=')[1].Trim();
                                }
                            }

                            string std = "";
                            foreach (var f in function_parameter_value) {
                                std += string.Format("{0},", f);
                            }
                            Dispatcher.Invoke(new Action(() => { myGlobal.datagridResult[i].itemInput = std.Length >= 2 ? std.Substring(0, std.Length - 2) : std.Substring(0, std.Length - 1); }));
                        }

                        //set window opacity
                        method = program.GetMethod(function_name);
                        if (function_name.ToLower().Contains("rssi") ||
                            function_name.ToLower().Contains("led") ||
                            function_name.ToLower().Contains("button") ||
                            function_name.ToLower().Contains("voice")) {
                            myGlobal.myTesting.Opacity = 0.5;
                        }

                        //check other function
                        bool r = (bool)method.Invoke(classInstance, function_parameter_value);

                        //check led
                        if (function_parameter_value[function_parameter_value.Length - 1] is Dictionary<string, string> && function_name.ToLower().Contains("led")) {
                            r = testLED(classInstance, method, function_parameter_value);
                        }
                        //check button
                        if (function_parameter_value[function_parameter_value.Length - 1] is Dictionary<string, string> && function_name.ToLower().Contains("button")) {
                            r = testButton(classInstance, method, function_parameter_value);
                        }
                        //check voice
                        if (function_name.ToLower().Contains("voice")) {
                            r = testVoice(classInstance, method, function_parameter_value);
                        }

                        myGlobal.myTesting.Opacity = 1;
                        Dispatcher.Invoke(new Action(() => {
                            if (function_parameter_value[function_parameter_value.Length - 1] is string) {
                                string s = (string)function_parameter_value[function_parameter_value.Length - 1];
                                string pattern = "^[0-9,A-F]{12}$";
                                if (Regex.IsMatch(s, pattern, RegexOptions.IgnoreCase) == true) {
                                    myGlobal.myTesting.inputBarcode = $"MAC: {s.ToUpper()}";
                                }
                                myGlobal.datagridResult[i].itemOutput = s;
                            }

                            myGlobal.datagridResult[i].itemResult = r ? "Passed" : "Failed";
                        }));

                        myGlobal.myTesting.testedItemCount++;

                        if (!r) {
                            total_result = false;
                            myGlobal.myTesting.errorItemCount++;
                            myGlobal.myTesting.errorRate = (myGlobal.myTesting.errorItemCount * 100) / myGlobal.myTesting.totalTestItem;
                            if (myGlobal.myTesting.continueCheck == false) break;
                        }
                    }
                    i++;
                }

                myGlobal.myTesting.logSystem += (string)property.GetValue(classInstance, null);
                return true;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); return false; }
        }

        private bool saveLogDetail(bool total_result) {
            string dir = $"{AppDomain.CurrentDomain.BaseDirectory}LogDetail\\{myGlobal.myTesting.productName.Replace(" ", "")}\\{myGlobal.myTesting.Station.Replace(" ", "").Trim() }\\{DateTime.Now.ToString("yyyy-MM-dd")}";
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

            string f = "";
            foreach (var item in myGlobal.listBarcode) {
                f += item.Content.Replace("-", "").Replace(" ", "").Trim() + "_";
            }
            if (f == "") f = myGlobal.myTesting.inputBarcode.Replace(":", "").Replace(" ", "").Replace("MAC", "").Trim() + "_";

            f = $"{f}{DateTime.Now.ToString("HHmmss")}_{string.Format("{0}", total_result ? "Passed" : "Failed")}.txt";

            using (StreamWriter sw = new StreamWriter($"{dir}\\{f}", true, Encoding.Unicode)) {
                sw.WriteLine(myGlobal.myTesting.logSystem);
            }

            //string data = File.ReadAllText($"{dir}\\{f}");
            //HEC.Encryption encryption = new HEC.Encryption($"{dir}\\{f}");
            //encryption.saveSource(data);
            return true;
        }

        private bool saveLogTotal(bool total_result) {
            string dir = $"{AppDomain.CurrentDomain.BaseDirectory}LogTotal\\{myGlobal.myTesting.productName.Replace(" ", "")}\\{myGlobal.myTesting.Station.Replace(" ", "").Trim() }";
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

            string f = $"{dir}\\{DateTime.Now.ToString("yyyy-MM-dd")}.csv";

            string title = "DateTime", content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (File.Exists(f) == false) {
                foreach (var item in myGlobal.datagridResult) {
                    title += $",{item.itemName}";
                    string c = "";
                    if (item.itemName.ToLower().Contains("get")) {
                        c = item.itemOutput;
                    }
                    else if (item.isCheck.ToLower().Contains("no")) {
                        c = item.isCheck;
                    }
                    else {
                        c = item.itemResult;
                    }
                    content += $",{c}";
                }
            }
            else {
                foreach (var item in myGlobal.datagridResult) {
                    string c = "";
                    if (item.itemName.ToLower().Contains("get")) {
                        c = item.itemOutput;
                    }
                    else if (item.isCheck.ToLower().Contains("no")) {
                        c = item.isCheck;
                    }
                    else {
                        c = item.itemResult;
                    }
                    content += $",{c}";
                }
            }
            title += ",Total Result";
            content += total_result ? ",Passed" : ",Failed";

            using (StreamWriter sw = new StreamWriter(f, true, Encoding.Unicode)) {
                if (title != "DateTime,Total Result") sw.WriteLine(title);
                sw.WriteLine(content);
            }

            return true;
        }


        #endregion

    }
}
