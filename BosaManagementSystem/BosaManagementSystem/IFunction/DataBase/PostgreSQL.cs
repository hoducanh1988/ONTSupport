using BosaManagementSystem.IFunction.Custom;
using BosaManagementSystem.IFunction.Global;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BosaManagementSystem.IFunction.DataBase {

    public class PostgreSQL : IDatabase {

        public bool isConnected() {
            try {
                bool r = false;
                string connStr = string.Format("Server={0};Port=5432;User Id={1};Password={2};", myGlobal.mydbInfo.dbServerIP, myGlobal.mydbInfo.dbServerUser, myGlobal.mydbInfo.dbServerPass);
                var m_conn = new NpgsqlConnection(connStr);
                m_conn.Open();
                r = m_conn.State == System.Data.ConnectionState.Open;
                m_conn.Close();
                return r;
            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show(ex.ToString(), "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }
        }

        public bool createDatabase() {
            try {
                string connStr = string.Format("Server={0};Port=5432;User Id={1};Password={2};", myGlobal.mydbInfo.dbServerIP, myGlobal.mydbInfo.dbServerUser, myGlobal.mydbInfo.dbServerPass);
                var m_conn = new NpgsqlConnection(connStr);
                string cmd_createdb = string.Format(@"CREATE DATABASE {0} WITH OWNER = postgres ENCODING = 'UTF8' CONNECTION LIMIT = -1;", myGlobal.mydbInfo.dbName);
                var m_createdb_cmd = new NpgsqlCommand(cmd_createdb, m_conn);
                m_conn.Open();
                m_createdb_cmd.ExecuteNonQuery();
                m_conn.Close();
                return true;
            }
            catch (Exception ex) {
                myGlobal.myImportInfo.logImport += $"{ex.ToString()}\n";
                if (ex.ToString().ToLower().Contains("already exists")) return true;
                else return false;
            }
        }

        public bool createTableBosaFile() {
            try {
                string connStr = string.Format("Server={0};Port=5432;User Id={1};Password={2};Database={3};", myGlobal.mydbInfo.dbServerIP, myGlobal.mydbInfo.dbServerUser, myGlobal.mydbInfo.dbServerPass, myGlobal.mydbInfo.dbName);
                var m_conn = new NpgsqlConnection(connStr);
                string cmd_createtb = string.Format(@"CREATE TABLE tbBosaFile(tbID SERIAL,BosaFile text,BosaMD5 text PRIMARY KEY,BosaSerialCounted text,DateTimeCreated timestamp);");
                var m_createtb_cmd = new NpgsqlCommand(cmd_createtb, m_conn);
                m_conn.Open();
                m_createtb_cmd.ExecuteNonQuery();
                m_conn.Close();

                return true;
            }
            catch (Exception ex) {
                myGlobal.myImportInfo.logImport += $"{ex.ToString()}\n";
                if (ex.ToString().ToLower().Contains("already exists")) return true;
                else return false;
            }
        }

        public bool createTableBosaSerialNumber() {
            try {
                string connStr = string.Format("Server={0};Port=5432;User Id={1};Password={2};Database={3};", myGlobal.mydbInfo.dbServerIP, myGlobal.mydbInfo.dbServerUser, myGlobal.mydbInfo.dbServerPass, myGlobal.mydbInfo.dbName);
                var m_conn = new NpgsqlConnection(connStr);
                string cmd_createtb = string.Format(@"CREATE TABLE tbBosaSerialNumber (tbID SERIAL,BosaSN text PRIMARY KEY,Ith text,Pf text,Vf text,Im text,Vbr text,Sen text,Psat text,BosaFile text,BosaMD5 text,DateTimeCreated timestamp,SynDriver text);");
                var m_createtb_cmd = new NpgsqlCommand(cmd_createtb, m_conn);
                m_conn.Open();
                m_createtb_cmd.ExecuteNonQuery();
                m_conn.Close();

                return true;
            }
            catch (Exception ex) {
                myGlobal.myImportInfo.logImport += $"{ex.ToString()}\n";
                if (ex.ToString().ToLower().Contains("already exists")) return true;
                else return false;
            }
        }

        public bool createTableClient() {
            try {
                string connStr = string.Format("Server={0};Port=5432;User Id={1};Password={2};Database={3};", myGlobal.mydbInfo.dbServerIP, myGlobal.mydbInfo.dbServerUser, myGlobal.mydbInfo.dbServerPass, myGlobal.mydbInfo.dbName);
                var m_conn = new NpgsqlConnection(connStr);
                string cmd_createtb = string.Format(@"CREATE TABLE tbClient(tbID SERIAL,PC text PRIMARY KEY,IP text,dbType text,dbUser text,dbPassword text,DateTimeCreated timestamp);");
                var m_createtb_cmd = new NpgsqlCommand(cmd_createtb, m_conn);
                m_conn.Open();
                m_createtb_cmd.ExecuteNonQuery();
                m_conn.Close();

                return true;
            }
            catch (Exception ex) {
                myGlobal.myImportInfo.logImport += $"{ex.ToString()}\n";
                if (ex.ToString().ToLower().Contains("already exists")) return true;
                else return false;
            }
        }

        public bool isExistInTableBosaFile(bosaFileInfo bsf) {
            try {
                bool r = false;
                string connStr = string.Format("Server={0};Port=5432;User Id={1};Password={2};Database={3};", myGlobal.mydbInfo.dbServerIP, myGlobal.mydbInfo.dbServerUser, myGlobal.mydbInfo.dbServerPass, myGlobal.mydbInfo.dbName);
                var m_conn = new NpgsqlConnection(connStr);
                m_conn.Open();

                NpgsqlCommand command = new NpgsqlCommand($"select * from tbbosafile where bosamd5='{bsf.FileMD5}';", m_conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                m_conn.Close();

                r = dt.Rows.Count > 0;
                return r;
            }
            catch (Exception ex) {
                myGlobal.myImportInfo.logImport += $"{ex.ToString()}\n";
                return true;
            }
        }

        public bool insertRowIntoTableBosaSerialNumber(bosaInfo bsi) {
            try {
                bool r = false;
                string connStr = string.Format("Server={0};Port=5432;User Id={1};Password={2};Database={3};", myGlobal.mydbInfo.dbServerIP, myGlobal.mydbInfo.dbServerUser, myGlobal.mydbInfo.dbServerPass, myGlobal.mydbInfo.dbName);
                var m_conn = new NpgsqlConnection(connStr);
                m_conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO tbbosaserialnumber(bosasn,ith,pf,vf,im,vbr,sen,psat,bosafile,bosamd5,datetimecreated,syndriver) VALUES(@bosasn,@ith,@pf,@vf,@im,@vbr,@sen,@psat,@bosafile,@bosamd5,@datetimecreated,@syndriver)", m_conn);

                try {
                    cmd.Parameters.AddWithValue("bosasn", bsi.bosasn);
                    cmd.Parameters.AddWithValue("ith", bsi.ith);
                    cmd.Parameters.AddWithValue("pf", bsi.pf);
                    cmd.Parameters.AddWithValue("vf", bsi.vf);
                    cmd.Parameters.AddWithValue("im", bsi.im);
                    cmd.Parameters.AddWithValue("vbr", bsi.vbr);
                    cmd.Parameters.AddWithValue("sen", bsi.sen);
                    cmd.Parameters.AddWithValue("psat", bsi.psat);
                    cmd.Parameters.AddWithValue("bosafile", bsi.bosafile);
                    cmd.Parameters.AddWithValue("bosamd5", bsi.bosamd5);
                    cmd.Parameters.AddWithValue("datetimecreated", DateTime.Now);
                    cmd.Parameters.AddWithValue("syndriver", "");
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                    r = true;
                }
                catch (Exception ex) {
                    myGlobal.myImportInfo.logImport += $"{ex.ToString()}\n";
                    r = false;
                }

                m_conn.Close();
                return r;
            }
            catch (Exception ex) {
                myGlobal.myImportInfo.logImport += $"{ex.ToString()}\n";
                return false;
            }
        }

        public bool insertRowIntoTableBosaSerialNumber(List<bosaInfo> bsis, out string log_dupplicate) {
            log_dupplicate = "";
            try {
                bool r = false;
                string connStr = string.Format("Server={0};Port=5432;User Id={1};Password={2};Database={3};", myGlobal.mydbInfo.dbServerIP, myGlobal.mydbInfo.dbServerUser, myGlobal.mydbInfo.dbServerPass, myGlobal.mydbInfo.dbName);
                var m_conn = new NpgsqlConnection(connStr);
                m_conn.Open();

                myGlobal.myImportInfo.bosaSerialMaxValue = bsis.Count;
                myGlobal.myImportInfo.bosaSerialProgressValue = 0;
                int count = 0;

                foreach (var bsi in bsis) {
                    myGlobal.myImportInfo.bosaSerialProgressValue++;
                    NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO tbbosaserialnumber(bosasn,ith,pf,vf,im,vbr,sen,psat,bosafile,bosamd5,datetimecreated,syndriver) VALUES(@bosasn,@ith,@pf,@vf,@im,@vbr,@sen,@psat,@bosafile,@bosamd5,@datetimecreated,@syndriver)", m_conn);
                    try {
                        cmd.Parameters.AddWithValue("bosasn", bsi.bosasn);
                        cmd.Parameters.AddWithValue("ith", bsi.ith);
                        cmd.Parameters.AddWithValue("pf", bsi.pf);
                        cmd.Parameters.AddWithValue("vf", bsi.vf);
                        cmd.Parameters.AddWithValue("im", bsi.im);
                        cmd.Parameters.AddWithValue("vbr", bsi.vbr);
                        cmd.Parameters.AddWithValue("sen", bsi.sen);
                        cmd.Parameters.AddWithValue("psat", bsi.psat);
                        cmd.Parameters.AddWithValue("bosafile", bsi.bosafile);
                        cmd.Parameters.AddWithValue("bosamd5", bsi.bosamd5);
                        cmd.Parameters.AddWithValue("datetimecreated", DateTime.Now);
                        cmd.Parameters.AddWithValue("syndriver", "");
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        r = true;
                        count++;
                    }
                    catch (Exception ex) {
                        if (ex.HResult == -2147467259) {
                            log_dupplicate += $"{bsi.bosasn}, Ith={bsi.ith}, Vbr={bsi.vbr}.\n";
                            myGlobal.myImportInfo.logImport += $"bosa serial={bsi.bosasn}, Ith={bsi.ith}, Vbr={bsi.vbr} bị trùng lặp.\n";
                            r = true;
                        }
                        else r = false;
                    }
                }

                m_conn.Close();
                myGlobal.myImportInfo.logImport += $"... imported: {count}\n";
                return r;
            }
            catch (Exception ex) {
                myGlobal.myImportInfo.logImport += $"{ex.ToString()}\n";
                return false;
            }
        }

        public bool insertRowIntoTableBosaFile(bosaFileInfo bsf) {
            try {
                bool r = false;
                string connStr = string.Format("Server={0};Port=5432;User Id={1};Password={2};Database={3};", myGlobal.mydbInfo.dbServerIP, myGlobal.mydbInfo.dbServerUser, myGlobal.mydbInfo.dbServerPass, myGlobal.mydbInfo.dbName);
                var m_conn = new NpgsqlConnection(connStr);
                m_conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO tbbosafile(bosafile,bosamd5,bosaserialcounted,datetimecreated) VALUES(@bosafile,@bosamd5,@bosaserialcounted,@datetimecreated)", m_conn);

                try {
                    cmd.Parameters.AddWithValue("bosafile", bsf.FileName);
                    cmd.Parameters.AddWithValue("bosamd5", bsf.FileMD5);
                    cmd.Parameters.AddWithValue("bosaserialcounted", bsf.BosaCounted);
                    cmd.Parameters.AddWithValue("datetimecreated", DateTime.Now);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                    r = true;
                }
                catch (Exception ex) {
                    myGlobal.myImportInfo.logImport += $"{ex.ToString()}\n";
                    r = false;
                }

                m_conn.Close();
                return r;
            }
            catch (Exception ex) {
                myGlobal.myImportInfo.logImport += $"{ex.ToString()}\n";
                return false;
            }
        }

        public bool deleteAllTableBosaFile() {
            try {
                bool r = false;
                string connStr = string.Format("Server={0};Port=5432;User Id={1};Password={2};Database={3};", myGlobal.mydbInfo.dbServerIP, myGlobal.mydbInfo.dbServerUser, myGlobal.mydbInfo.dbServerPass, myGlobal.mydbInfo.dbName);
                var m_conn = new NpgsqlConnection(connStr);
                m_conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand("delete from tbbosafile", m_conn);

                try {
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                    r = true;
                }
                catch (Exception ex) {
                    myGlobal.myImportInfo.logImport += $"{ex.ToString()}\n";
                    r = false;
                }

                m_conn.Close();
                return r;
            }
            catch (Exception ex) {
                myGlobal.myImportInfo.logImport += $"{ex.ToString()}\n";
                return false;
            }
        }

        public bool deleteAllTableBosaSerialNumber() {
            try {
                bool r = false;
                string connStr = string.Format("Server={0};Port=5432;User Id={1};Password={2};Database={3};", myGlobal.mydbInfo.dbServerIP, myGlobal.mydbInfo.dbServerUser, myGlobal.mydbInfo.dbServerPass, myGlobal.mydbInfo.dbName);
                var m_conn = new NpgsqlConnection(connStr);
                m_conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand("delete from tbbosaserialnumber", m_conn);

                try {
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                    r = true;
                }
                catch (Exception ex) {
                    myGlobal.myImportInfo.logImport += $"{ex.ToString()}\n";
                    r = false;
                }

                m_conn.Close();
                return r;
            }
            catch (Exception ex) {
                myGlobal.myImportInfo.logImport += $"{ex.ToString()}\n";
                return false;
            }
        }

        public DataTable selectAllBosaFileFromTableBosaSerialNumber() {
            try {
                string connStr = string.Format("Server={0};Port=5432;User Id={1};Password={2};Database={3};", myGlobal.mydbInfo.dbServerIP, myGlobal.mydbInfo.dbServerUser, myGlobal.mydbInfo.dbServerPass, myGlobal.mydbInfo.dbName);
                var m_conn = new NpgsqlConnection(connStr);
                m_conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT DISTINCT bosafile FROM tbbosaserialnumber;", m_conn);
                cmd.Prepare();
                NpgsqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);

                m_conn.Close();
                return dt;
            }
            catch {
                return null;
            }
        }

        public DataTable selectFromTableBosaSerialNumber(string bosa_sn, string ith, string vbr, string bosa_file, int idx, int split_value) {
            try {
                string connStr = string.Format("Server={0};Port=5432;User Id={1};Password={2};Database={3};", myGlobal.mydbInfo.dbServerIP, myGlobal.mydbInfo.dbServerUser, myGlobal.mydbInfo.dbServerPass, myGlobal.mydbInfo.dbName);
                var m_conn = new NpgsqlConnection(connStr);
                m_conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(string.Format("SELECT bosasn,ith,pf,vf,im,vbr,sen,psat,bosafile,bosamd5,datetimecreated FROM tbbosaserialnumber WHERE bosasn LIKE @bosasn AND ith LIKE @ith AND vbr LIKE @vbr AND bosafile LIKE @bosafile LIMIT {0} OFFSET {1};", split_value, idx * split_value), m_conn);
                cmd.Parameters.AddWithValue("bosasn", $"%{bosa_sn}%");
                cmd.Parameters.AddWithValue("ith", $"%{ith}%");
                cmd.Parameters.AddWithValue("vbr", $"%{vbr}%");
                cmd.Parameters.AddWithValue("bosafile", $"%{bosa_file}%");
                cmd.Prepare();
                NpgsqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                m_conn.Close();
                return dt;
            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show(ex.ToString(), "", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return null;
            }
        }

        public DataTable selectFromTableBosaSerialNumber(string bosa_sn, string ith, string vbr, string bosa_file) {
            try {
                string connStr = string.Format("Server={0};Port=5432;User Id={1};Password={2};Database={3};", myGlobal.mydbInfo.dbServerIP, myGlobal.mydbInfo.dbServerUser, myGlobal.mydbInfo.dbServerPass, myGlobal.mydbInfo.dbName);
                var m_conn = new NpgsqlConnection(connStr);
                m_conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(string.Format("SELECT bosasn,ith,pf,vf,im,vbr,sen,psat,bosafile,bosamd5,datetimecreated FROM tbbosaserialnumber WHERE bosasn LIKE @bosasn AND ith LIKE @ith AND vbr LIKE @vbr AND bosafile LIKE @bosafile LIMIT 1000;"), m_conn);
                cmd.Parameters.AddWithValue("bosasn", $"%{bosa_sn}%");
                cmd.Parameters.AddWithValue("ith", $"%{ith}%");
                cmd.Parameters.AddWithValue("vbr", $"%{vbr}%");
                cmd.Parameters.AddWithValue("bosafile", $"%{bosa_file}%");
                cmd.Prepare();
                NpgsqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                m_conn.Close();
                return dt;
            }
            catch (Exception ex) {
                System.Windows.MessageBox.Show(ex.ToString(), "", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return null;
            }
        }

        public bool selectFromTableBosaSerialNumber(string bosa_sn, string ith, string vbr, string bosa_file, out string count) {
            count = "0";
            try {
                string connStr = string.Format("Server={0};Port=5432;User Id={1};Password={2};Database={3};", myGlobal.mydbInfo.dbServerIP, myGlobal.mydbInfo.dbServerUser, myGlobal.mydbInfo.dbServerPass, myGlobal.mydbInfo.dbName);
                var m_conn = new NpgsqlConnection(connStr);
                m_conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT count(*) FROM tbbosaserialnumber WHERE bosasn LIKE @bosasn AND ith LIKE @ith AND vbr LIKE @vbr AND bosafile LIKE @bosafile;", m_conn);
                cmd.Parameters.AddWithValue("bosasn", $"%{bosa_sn}%");
                cmd.Parameters.AddWithValue("ith", $"%{ith}%");
                cmd.Parameters.AddWithValue("vbr", $"%{vbr}%");
                cmd.Parameters.AddWithValue("bosafile", $"%{bosa_file}%");
                cmd.Prepare();
                NpgsqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);

                m_conn.Close();
                count = dt.Rows[0].ItemArray[0].ToString();
                return true;
            }
            catch {
                return false;
            }
        }

    }
}
