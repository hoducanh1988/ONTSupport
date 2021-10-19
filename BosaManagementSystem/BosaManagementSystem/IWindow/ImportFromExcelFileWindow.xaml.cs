using BosaManagementSystem.IFunction.Custom;
using BosaManagementSystem.IFunction.DataBase;
using BosaManagementSystem.IFunction.Global;
using BosaManagementSystem.IFunction.IO;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BosaManagementSystem.IWindow {
    /// <summary>
    /// Interaction logic for ImportFromExcelFileWindow.xaml
    /// </summary>
    public partial class ImportFromExcelFileWindow : Window {

        int time_count = 0;

        public ImportFromExcelFileWindow() {
            InitializeComponent();
            this.DataContext = myGlobal.myImportInfo;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += (s, e) => {
                if (myGlobal.myImportInfo.totalResult == null) return;
                if (myGlobal.myImportInfo.totalResult.ToLower().Contains("waiting")) {
                    time_count++;
                    myGlobal.myImportInfo.totalTime = UtilityPack.Converter.myConverter.intToTimeSpan(time_count * 500);
                }
            };
            timer.Start();

            Thread t = new Thread(new ThreadStart(() => {
                time_count = 0;
                myGlobal.myImportInfo.Init();
                myGlobal.myImportInfo.totalResult = "Waiting...";
                bool r = startImport();
                myGlobal.myImportInfo.totalResult = r ? "Passed" : "Failed";
                Dispatcher.Invoke(new Action(() => { this.Close(); }));
            }));
            t.IsBackground = true;
            t.Start();
        }

        #region import bosa serial from excel file to database ##########################

        private bool startImport() {
            bool ret = true;
            try {
                bool r = false;
                if (myGlobal.datagridBosaFile.Count == 0) goto OK;
                myGlobal.myImportInfo.bosaFileMaxValue = myGlobal.datagridBosaFile.Count;
                myGlobal.myImportInfo.bosaFileProgressValue = 0;
                myGlobal.myImportInfo.logImport += $"> Bosa folder path: {myGlobal.myImportInfo.bosaFolderPath}\n";
                foreach (var bsf in myGlobal.datagridBosaFile) {
                    myGlobal.myImportInfo.logImport += $"$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$- {myGlobal.myImportInfo.bosaFileProgressValue + 1}/{myGlobal.myImportInfo.bosaFileMaxValue}\n";
                    myGlobal.myImportInfo.logImport += $"> Bosa file: {bsf.FileName}\n";
                    myGlobal.myImportInfo.logImport += $"> Bosa md5: {bsf.FileMD5}\n";
                    myGlobal.myImportInfo.logImport += $"> Bosa md5 input: {bsf.InputMD5}\n";
                    myGlobal.myImportInfo.importFileName = bsf.FileName;

                    myGlobal.myImportInfo.bosaFileProgressValue++;
                    bsf.Result = "Waiting...";

                    //check md5
                    myGlobal.myImportInfo.logImport += $"# Kiểm tra md5 inputed và md5 thực tế...\n";
                    r = bsf.InputMD5.ToLower().Trim().Equals(bsf.FileMD5.ToLower().Trim());
                    myGlobal.myImportInfo.logImport += $"... kết quả: {bsf.Status}\n";
                    if (!r) { goto file_fail; }

                    //check already imported or not yet
                    IDatabase db = null;
                    switch (myGlobal.mydbInfo.dbServerType) {
                        case "PostgreSQL": {
                                db = new PostgreSQL();
                                break;
                            }
                    }

                    myGlobal.myImportInfo.logImport += $"# Kiểm tra file đã import vào database hay chưa...\n";
                    r = db.isExistInTableBosaFile(bsf);
                    bsf.Status = r ? "imported" : "new";
                    myGlobal.myImportInfo.logImport += $"... kết quả: {bsf.Status}\n";
                    if (r) goto file_none;

                    //check file format
                    r = checkFormatFileBosa(System.IO.Path.Combine(myGlobal.myImportInfo.bosaFolderPath, bsf.FileName));
                    if (!r) { goto file_fail; }

                    //read bosa file to list
                    List<bosaInfo> listBosaSN;
                    readBosaFile(System.IO.Path.Combine(myGlobal.myImportInfo.bosaFolderPath, bsf.FileName), bsf.FileMD5, out listBosaSN);
                    r = listBosaSN == null || listBosaSN.Count == 0;
                    if (r) { goto file_fail; }
                    bsf.BosaCounted = $"{listBosaSN.Count}";

                    //import list bosa to database
                    myGlobal.myImportInfo.logImport += $"# Import bosa serial lên server database...\n";
                    string log_dupplicate = "";
                    r = db.insertRowIntoTableBosaSerialNumber(listBosaSN, out log_dupplicate);
                    if (log_dupplicate.Length > 0) MessageBox.Show(log_dupplicate, "Bosa bị trùng lặp", MessageBoxButton.OK, MessageBoxImage.Error);
                    myGlobal.myImportInfo.logImport += $"... kết quả: {r}\n";
                    if (!r) { goto file_fail; }

                    //save bosa file
                    myGlobal.myImportInfo.logImport += $"# Lưu log file bosa, file md5 lên server database...\n";
                    r = db.insertRowIntoTableBosaFile(bsf);
                    myGlobal.myImportInfo.logImport += $"... kết quả: {r}\n";
                    if (!r) { goto file_fail; }

                    goto file_pass;

                file_pass:
                    bsf.Result = "Passed";
                    continue;

                file_fail:
                    ret = false;
                    bsf.Result = "Failed";
                    continue;

                file_none:
                    bsf.Result = "None";
                    continue;
                }

            }
            catch { goto NG; }

        OK:
            return ret;
        NG:
            return ret;

        }


        private bool checkFormatFileBosa(string bosa_file) {
            myGlobal.myImportInfo.logImport += $"# Kiểm tra định dạng file bosa...\n";
            try {
                bool r = false;
                DataTable dt = new DataTable();
                dt = BosaReport.readSampleData(bosa_file);
                for (int i = 0; i < 8; i++) {
                    myGlobal.myImportInfo.logImport += $"... Column {i + 1}, {dt.Rows[0].ItemArray[i]}".PadLeft(20, ' ').Trim() + "\n";
                }
                myGlobal.myImportInfo.logImport += $"\n\n";
                string reg_vbr = dt.Rows[0].ItemArray[5].ToString().Trim();
                myGlobal.myImportInfo.logImport += $"... recognize vbr={reg_vbr}\n";
                myGlobal.myImportInfo.logImport += $"... pattern vbr=vbr\n";
                string reg_ith = dt.Rows[0].ItemArray[1].ToString().Trim();
                myGlobal.myImportInfo.logImport += $"... recognize ith={reg_ith}\n";
                myGlobal.myImportInfo.logImport += $"... pattern ith=ith\n";
                //r = (reg_vbr.ToLower().Contains("vbr")== true || double.Parse(reg_vbr) > 35) && (reg_ith.ToLower().Contains("ith") || double.Parse(reg_ith) < 15);
                r = (reg_vbr.ToLower().Contains("vbr") == true) && (reg_ith.ToLower().Contains("ith") == true);
                myGlobal.myImportInfo.logImport += $"... Kết quả: {r}\n";
                return r;
            }
            catch (Exception ex) {
                myGlobal.myImportInfo.logImport += $"...Kết quả: {false}\n";
                myGlobal.myImportInfo.logImport += $"...{ex.ToString()}\n";
                return false;
            }
        }

        private List<bosaInfo> readBosaFile(string bosa_file, string bosa_md5, out List<bosaInfo> lst) {
            lst = new List<bosaInfo>();
            string[] buffer = bosa_file.Split('\\');
            myGlobal.myImportInfo.logImport += $"# Đọc dữ liệu từ file bosa report...\n";
            try {
                var dt = new DataTable();
                dt = BosaReport.readData(bosa_file);

                for (int i = 0; i < dt.Rows.Count; i++) {
                    string _bosaSN = "", _Ith = "", _Vbr = "";
                    _bosaSN = dt.Rows[i].ItemArray[0].ToString().Trim();
                    if (_bosaSN.Length > 0) {
                        _Ith = dt.Rows[i].ItemArray[1].ToString().Trim();
                        _Vbr = dt.Rows[i].ItemArray[5].ToString().Trim();

                        try {
                            if (double.Parse(_Ith) > 0 && double.Parse(_Ith) < 15 && double.Parse(_Vbr) > 35) {
                                bosaInfo bsi = new bosaInfo() {
                                    bosasn = _bosaSN,
                                    ith = _Ith,
                                    pf = dt.Rows[i].ItemArray[2].ToString().Trim(),
                                    vf = dt.Rows[i].ItemArray[3].ToString().Trim(),
                                    im = dt.Rows[i].ItemArray[4].ToString().Trim(),
                                    vbr = _Vbr,
                                    sen = dt.Rows[i].ItemArray[6].ToString().Trim(),
                                    psat = dt.Rows[i].ItemArray[7].ToString().Trim(),
                                    bosafile = buffer[buffer.Length - 1],
                                    bosamd5 = bosa_md5
                                };
                                lst.Add(bsi);
                            }
                            else {
                                myGlobal.myImportInfo.logImport += $"...Kết quả: {false}\n";
                                myGlobal.myImportInfo.logImport += $"...Bosa={_bosaSN}, Ith={_Ith}, Vbr={_Vbr} sai tiêu chuẩn\n";
                                goto END;
                            }
                        }
                        catch (Exception ex) {
                            myGlobal.myImportInfo.logImport += $"...Kết quả: {false}\n";
                            myGlobal.myImportInfo.logImport += $"...Bosa={_bosaSN}, Ith={_Ith}, Vbr={_Vbr}\n";
                            myGlobal.myImportInfo.logImport += $"...{ex.ToString()}\n";
                            goto END;
                        }
                    }
                }

                myGlobal.myImportInfo.logImport += $"...Tổng số bosa serial đọc được: {lst.Count}\n";
                return lst;
            }
            catch (Exception ex) {
                myGlobal.myImportInfo.logImport += $"...Kết quả: {false}\n";
                myGlobal.myImportInfo.logImport += $"...{ex.ToString()}\n";
                goto END;
            }

        END:
            return null;
        }

        #endregion
    }
}
