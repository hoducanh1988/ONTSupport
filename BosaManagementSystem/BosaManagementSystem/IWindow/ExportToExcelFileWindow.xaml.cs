using BosaManagementSystem.IFunction.DataBase;
using BosaManagementSystem.IFunction.Global;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Threading;

namespace BosaManagementSystem.IWindow {
    /// <summary>
    /// Interaction logic for ExportToExcelFile.xaml
    /// </summary>
    public partial class ExportToExcelFileWindow : Window {

        int timer_count = 0;
        
        public ExportToExcelFileWindow(string total_row) {
            InitializeComponent();
            this.DataContext = myGlobal.myExportInfo;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += (s, e) => {
                if (myGlobal.myExportInfo.totalResult == "Waiting...") {
                    timer_count++;
                    myGlobal.myExportInfo.totalTime = UtilityPack.Converter.myConverter.intToTimeSpan(timer_count * 500);
                }
            };
            timer.Start();

            Thread t = new Thread(new ThreadStart(() => {
                int max_value = int.Parse(total_row);
                int num_of_file = max_value / myGlobal.myExportInfo.splitValue;
                num_of_file = max_value % myGlobal.myExportInfo.splitValue == 0 ? num_of_file : num_of_file + 1;
                myGlobal.myExportInfo.progressValue = 0;
                myGlobal.myExportInfo.progressMax = num_of_file;
                string f = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                IDatabase db = null;
                switch (myGlobal.mydbInfo.dbServerType) {
                    case "PostgreSQL": {
                            db = new PostgreSQL();
                            break;
                        }
                }

                for (int i = 1; i <= num_of_file; i++) {
                    myGlobal.myExportInfo.progressValue++;
                    DataTable dt = (db as PostgreSQL).selectFromTableBosaSerialNumber(myGlobal.myExportInfo.bosaSerial,
                                                                               myGlobal.myExportInfo.Ith,
                                                                               myGlobal.myExportInfo.Vbr,
                                                                               myGlobal.myExportInfo.bosaFile, 
                                                                               i - 1,
                                                                               myGlobal.myExportInfo.splitValue);

                    string file_name = $"{myGlobal.myExportInfo.exportFolderPath}\\{f}_{i}.xlsx";
                    myGlobal.myExportInfo.exportFileName = file_name;
                    exportToExcelFile(dt, file_name);
                }
                myGlobal.myExportInfo.totalResult = "Finished";
                Dispatcher.Invoke(new Action(() => { this.Close(); }));
            }));
            t.IsBackground = true;
            t.Start();
        }


        bool exportToExcelFile(DataTable dt, string file_name) {
            try {

                if (File.Exists(file_name)) File.Delete(file_name);
                Thread.Sleep(100);

                FileInfo fi = new FileInfo(file_name);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excel = new ExcelPackage(fi);
                
                ExcelWorksheet ws = excel.Workbook.Worksheets.Add(string.Format("Sheet{0}", 1));
                ws.Cells["A8"].LoadFromDataTable(dt, true);

                excel.Save();
                return true;

            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
