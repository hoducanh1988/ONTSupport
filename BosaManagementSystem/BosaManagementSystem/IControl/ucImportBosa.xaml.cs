using BosaManagementSystem.IFunction.Global;
using BosaManagementSystem.IFunction.Custom;
using BosaManagementSystem.IFunction.IO;
using BosaManagementSystem.IFunction.DataBase;
using BosaManagementSystem.IWindow;
using System;
using System.Collections.Generic;
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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Security.Cryptography;
using System.Data;
using System.Windows.Threading;

namespace BosaManagementSystem.IControl {
    /// <summary>
    /// Interaction logic for ucImportBosa.xaml
    /// </summary>
    public partial class ucImportBosa : UserControl {
        
        public ucImportBosa() {
            InitializeComponent();
            this.DataContext = myGlobal.myImportInfo;
            this.dgBosaFile.ItemsSource = myGlobal.datagridBosaFile;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            string bTg = (string)b.Tag;

            switch (bTg.ToLower()) {
                case "browser": {
                        CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                        dialog.IsFolderPicker = true;
                        if (dialog.ShowDialog() == CommonFileDialogResult.Ok) {
                            myGlobal.myImportInfo.bosaFolderPath = dialog.FileName;

                            myGlobal.datagridBosaFile.Clear();
                            int idx = 0;
                            string supportedExtensions = "*.xls,*.xlsx";
                            foreach (string excel_file in Directory.GetFiles(myGlobal.myImportInfo.bosaFolderPath, "*.*", SearchOption.AllDirectories).Where(s => supportedExtensions.Contains(System.IO.Path.GetExtension(s).ToLower()))) {
                                idx++;
                                string[] buffer = excel_file.Split('\\');
                                bosaFileInfo bsi = new bosaFileInfo() { Index = $"{idx}", FileName = buffer[buffer.Length - 1], BosaCounted = "", FileMD5 = new SmbClient().GetHash<MD5>(excel_file), Status = "" };
                                myGlobal.datagridBosaFile.Add(bsi);
                            }
                        }
                        break;
                    }
            }

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            MenuItem mi = sender as MenuItem;
            string mTag = (string)mi.Tag;

            switch (mTag.ToLower()) {
                case "import": {
                        if (myGlobal.datagridBosaFile == null || myGlobal.datagridBosaFile.Count == 0) return;
                        if (MessageBox.Show("Bạn muốn nhập dữ liệu từ excel file vào database phải không?\nChọn 'Yes' là đồng ý, chọn 'No' để thoát.", "Import Excel File", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                            this.Opacity = 0.3;
                            ImportFromExcelFileWindow iw = new ImportFromExcelFileWindow();
                            iw.ShowDialog();
                            bool r = myGlobal.myImportInfo.totalResult.ToLower().Equals("passed");
                            MessageBox.Show( r ? "Success!" : "Failured!", "Import to database", MessageBoxButton.OK,r ? MessageBoxImage.Information : MessageBoxImage.Error);
                            this.Opacity = 1;
                        }
                        break;
                    }
                case "refresh": {
                        this.dgBosaFile.UnselectAllCells();
                        break;
                    }
            }
        }


    }

    public class BosaPathToBooleanConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (value == null) return false;
            if ((value as string).Trim().Length == 0) return false;
            else return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new Exception();
        }
    }
}
