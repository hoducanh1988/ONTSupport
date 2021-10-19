using BosaManagementSystem.IFunction.DataBase;
using BosaManagementSystem.IFunction.Global;
using BosaManagementSystem.IWindow;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace BosaManagementSystem.IControl {
    /// <summary>
    /// Interaction logic for ucExportBosa.xaml
    /// </summary>
    public partial class ucExportBosa : UserControl {

        string total_row = "0";

        public ucExportBosa() {
            InitializeComponent();
            this.DataContext = myGlobal.myExportInfo;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += (s, e) => {
                if (myGlobal.isUpdateListBosaFile == true) {
                    this.cbb_bosafile.ItemsSource = myParameter.listBosaFile;
                    myGlobal.isUpdateListBosaFile = false;
                }
            };
            timer.Start();

        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            string bTag = (string)b.Tag;

            switch (bTag) {
                case "search": {
                        Thread t = new Thread(new ThreadStart(() => {
                            Dispatcher.Invoke(new Action(() => { b.IsEnabled = false; b.Content = "Searching..."; this.dgBosaInfo.ItemsSource = null; }));
                            IDatabase db = null;
                            switch (myGlobal.mydbInfo.dbServerType) {
                                case "PostgreSQL": {
                                        db = new PostgreSQL();
                                        break;
                                    }
                            }
                            (db as PostgreSQL).selectFromTableBosaSerialNumber(myGlobal.myExportInfo.bosaSerial,
                                                                               myGlobal.myExportInfo.Ith,
                                                                               myGlobal.myExportInfo.Vbr,
                                                                               myGlobal.myExportInfo.bosaFile, out total_row);

                            DataTable dt = (db as PostgreSQL).selectFromTableBosaSerialNumber(myGlobal.myExportInfo.bosaSerial,
                                                                               myGlobal.myExportInfo.Ith,
                                                                               myGlobal.myExportInfo.Vbr,
                                                                               myGlobal.myExportInfo.bosaFile);
                            Dispatcher.Invoke(new Action(() => {
                                if (dt != null) this.dgBosaInfo.ItemsSource = dt.DefaultView;
                                MessageBox.Show(string.Format("Đã tìm thấy {0} kết quả.", total_row), "Tìm kiếm", MessageBoxButton.OK, MessageBoxImage.Information);
                                b.IsEnabled = true;
                                b.Content = "Search";
                            }));
                        }));
                        t.IsBackground = true;
                        t.Start();
                        break;
                    }
            }
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            MenuItem mi = sender as MenuItem;
            string mTag = (string)mi.Tag;

            switch (mTag) {
                case "refresh": {
                        this.dgBosaInfo.UnselectAllCells();
                        break;
                    }
                case "export": {
                        if (total_row == "" || total_row == "0") return;
                        if (MessageBox.Show("Bạn muốn xuất dữ liệu từ database ra excel file phải không?\nChọn 'Yes' là đồng ý, chọn 'No' để thoát.", "Export Excel File", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                            dialog.IsFolderPicker = true;
                            if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                                this.Opacity = 0.3;
                                myGlobal.myExportInfo.exportFolderPath = dialog.FileName;
                                myGlobal.myExportInfo.totalResult = "Waiting...";
                                ExportToExcelFileWindow ew = new ExportToExcelFileWindow(this.total_row);
                                ew.ShowDialog();
                                MessageBox.Show("Sucess!", "Export Excel", MessageBoxButton.OK, MessageBoxImage.Information);
                                this.Opacity = 1;
                            }
                        }
                        break;
                    }
            }
        }

    }
}
