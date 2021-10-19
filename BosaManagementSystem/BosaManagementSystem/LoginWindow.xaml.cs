using BosaManagementSystem.IFunction.Custom;
using BosaManagementSystem.IFunction.DataBase;
using BosaManagementSystem.IFunction.Global;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using UtilityPack.IO;

namespace BosaManagementSystem {
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {
        public LoginWindow() {
            InitializeComponent();
            this.cbb_servertype.ItemsSource = myParameter.listServerType;
            if (File.Exists(myGlobal.dbInfoFile)) myGlobal.mydbInfo = XmlHelper<databaseInfo>.FromXmlFile(myGlobal.dbInfoFile);
            this.DataContext = myGlobal.mydbInfo;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            string b_content = (string)b.Content;

            switch (b_content) {
                case "OK": {
                        b.IsEnabled = false;
                        b.Content = "Please waiting...";
                        IDatabase db = null;
                        string db_type = "";
                        if (cbb_servertype.SelectedValue != null)
                            db_type = cbb_servertype.SelectedValue.ToString();

                        switch (db_type) {
                            case "PostgreSQL": { db = new PostgreSQL(); break; }
                            default: break;
                        }

                        if (db != null) {
                            Thread t = new Thread(new ThreadStart(() => {
                                if (db.isConnected()) {
                                    if (myGlobal.mydbInfo.autoCreateDB) {
                                        db.createDatabase();
                                        db.createTableBosaFile();
                                        db.createTableBosaSerialNumber();
                                        db.createTableClient();
                                    }

                                    App.Current.Dispatcher.Invoke(new Action(() => {
                                        MainWindow w = new MainWindow();
                                        w.Show();
                                        this.Close();
                                    }));
                                }
                                else {
                                    MessageBox.Show("Không thể kết nối tới database server.\nVui lòng check lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    Dispatcher.Invoke(new Action(() => {
                                        b.IsEnabled = true;
                                        b.Content = "OK";
                                    }));
                                }
                            }));
                            t.IsBackground = true;
                            t.Start();
                        }
                        else {
                            MessageBox.Show("Không thể kết nối tới database server.\nVui lòng check lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            b.IsEnabled = true;
                            b.Content = "OK";
                        }

                        //save setting
                        XmlHelper<databaseInfo>.ToXmlFile(myGlobal.mydbInfo, myGlobal.dbInfoFile);
                        break;
                    }
                case "Exit": {
                        this.Close();
                        break;
                    }
            }
        }
    }
}
