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
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            string b_content = (string)b.Content;

            switch (b_content) {
                case "Start": {
                        b.Content = "Stop";
                        Thread t = new Thread(new ThreadStart(() => {
                            myGlobal.myController.Open(myGlobal.mySetting.controllerPort, "9600", "8", System.IO.Ports.Parity.None, System.IO.Ports.StopBits.One);
                            myGlobal.myDut.Open(myGlobal.mySetting.dutPort, "115200", "8", System.IO.Ports.Parity.None, System.IO.Ports.StopBits.One);
                            int count = 0;
                            int total = 0;


                        BEGIN:
                            int op_time = int.Parse(myGlobal.mySetting.opTime);
                            double cl_time = double.Parse(myGlobal.mySetting.clTime);
                            int loop_time = int.Parse(myGlobal.mySetting.loopTime);
                            int delay_time = int.Parse(myGlobal.mySetting.delayTime);

                            myGlobal.myTesting.loopRemaining = loop_time - total;
                            total++;
                            myGlobal.myTesting.initParams();
                            myGlobal.myController.Start();
                            count = 0;
                            int d = 0;
                            int od = 0;
                            int c = 0;
                        RE_START:
                            myGlobal.myTesting.timeRemaining = op_time - count;
                            count++;
                            if (count < op_time) {
                                d = myGlobal.myTesting.logDut.Length;
                                if (od == d) {
                                    c++;
                                    if (c == delay_time) {
                                        //MessageBox.Show("NG");
                                    }
                                }
                                else { od = d; c = 0; }
                                Thread.Sleep(1000);
                                goto RE_START;
                            }

                            myGlobal.myController.Stop();
                            count = 0;
                        RE_STOP:
                            //myGlobal.myTesting.timeRemaining = (int)cl_time - count;
                            count++;
                            if (count < cl_time * 10) {
                                Thread.Sleep(100);
                                goto RE_STOP;
                            }

                            if (total < loop_time) goto BEGIN;
                            Dispatcher.Invoke(new Action(() => { b.Content = "Start"; }));
                            myGlobal.myController.Stop();
                            myGlobal.myController.Close();
                            myGlobal.myDut.Close();

                        }));
                        t.IsBackground = true;
                        t.Start();
                        break;
                    }
                case "Stop": {
                        b.Content = "Start";
                        myGlobal.myController.Stop();
                        myGlobal.myController.Close();
                        myGlobal.myDut.Close();
                        break;
                    }
                default: break;
            }

        }
    }
}
