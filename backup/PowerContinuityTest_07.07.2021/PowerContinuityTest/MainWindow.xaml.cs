using System;
using System.Collections.Generic;
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
using System.Threading;

using PowerContinuityTest.Function;
using System.Windows.Threading;

namespace PowerContinuityTest {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        Thread t = null;

        public MainWindow() {
            InitializeComponent();
            this.DataContext = myGlobal.myTesting;
            this.sp_setting.DataContext = myGlobal.mySetting;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += (s, e) => {
                if (myGlobal.myController.IsConnected()) {
                    this.Scr_LogController.ScrollToEnd();
                    this.Scr_LogDut.ScrollToEnd();
                }
            };
            timer.Start();

            myGlobal.myTesting.initParams();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            string b_content = (string)b.Content;

            switch (b_content) {
                case "Start": {
                        b.Content = "Stop";
                        myGlobal.myTesting.initParams();

                        t = new Thread(new ThreadStart(() => {
                            myGlobal.myController.Open(myGlobal.mySetting.controllerPort, "9600", "8", System.IO.Ports.Parity.None, System.IO.Ports.StopBits.One);
                            myGlobal.myDut.Open(myGlobal.mySetting.dutPort, "115200", "8", System.IO.Ports.Parity.None, System.IO.Ports.StopBits.One);

                            double op_time = double.Parse(myGlobal.mySetting.opTime);
                            double cl_time = double.Parse(myGlobal.mySetting.clTime);
                            int loop_time = int.Parse(myGlobal.mySetting.loopTime);
                            int allow_time = (int)(double.Parse(myGlobal.mySetting.allowTime) * 10);

                            myGlobal.myController.Stop();
                            myGlobal.myController.Start(loop_time + 1, op_time, cl_time);

                            int count = 0;
                            int d = 0;

                            int olen = 0;
                            int olog = 0;
                            int c = 0;
                            myGlobal.myTesting.waitParams();
                        LOOP:
                            int time_count = 0;
                            myGlobal.myTesting.loopRemaining = loop_time - count;
                            myGlobal.myTesting.logBinding = "";

                            while (d == count) {
                                if (string.IsNullOrEmpty(myGlobal.myTesting.logDut) == false) {
                                    //loop time
                                    //--------------------------//
                                    string[] buffer = myGlobal.myTesting.logDut.Split(new string[] { myGlobal.mySetting.logString }, StringSplitOptions.None);
                                    d = buffer.Length - 1;

                                    //count delay time
                                    //--------------------------//
                                    olen = myGlobal.myTesting.logDut.Length;
                                    if (olen == olog) {
                                        c++;
                                        myGlobal.myTesting.timeRemaining = c / 10.0;
                                        if (c >= allow_time) {
                                            goto FAIL;
                                        }
                                    }
                                    else { olog = olen; c = 0; }
                                }

                                //online time
                                time_count++;
                                myGlobal.myTesting.onlineRemaining = (int)(time_count / 10);
                                Thread.Sleep(100);
                            }

                            count = d;
                            if (myGlobal.myTesting.loopRemaining >= 1) goto LOOP;
                            goto PASS;

                        PASS:
                            myGlobal.myController.Stop();
                            myGlobal.myTesting.passParams();
                            goto END;

                        FAIL:
                            myGlobal.myController.stopHighLevel();
                            myGlobal.myTesting.failParams();
                            goto END;

                        END:
                            Dispatcher.Invoke(new Action(() => { b.Content = "Start"; }));
                            myGlobal.myController.Close();
                            myGlobal.myDut.Close();

                        }));
                        t.IsBackground = true;
                        t.Start();
                        break;
                    }
                case "Stop": {
                        t.Abort();
                        b.Content = "Start";
                        myGlobal.myController.Stop();
                        myGlobal.myController.Close();
                        myGlobal.myDut.Close();
                        myGlobal.myTesting.abortParams();

                        break;
                    }
                default: break;
            }

        }
    }
}
