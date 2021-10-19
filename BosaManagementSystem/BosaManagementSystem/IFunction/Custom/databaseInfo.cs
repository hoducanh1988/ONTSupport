using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BosaManagementSystem.IFunction.Custom {
    public class databaseInfo : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public databaseInfo() {
            dbServerType = "PostgreSQL";
            dbServerIP = "127.0.0.1";
            dbServerUser = "postgres";
            dbServerPass = "P@ssword";
            dbName = "opticalbosa";
            autoCreateDB = true;
        }

        string _db_server_type;
        public string dbServerType {
            get { return _db_server_type; }
            set { _db_server_type = value; OnPropertyChanged(nameof(dbServerType)); }
        }
        string _db_server_ip;
        public string dbServerIP {
            get { return _db_server_ip; }
            set { _db_server_ip = value; OnPropertyChanged(nameof(dbServerIP)); }
        }
        string _db_server_user;
        public string dbServerUser {
            get { return _db_server_user; }
            set { _db_server_user = value; OnPropertyChanged(nameof(dbServerUser)); }
        }
        string _db_server_pass;
        public string dbServerPass {
            get { return _db_server_pass; }
            set { _db_server_pass = value; OnPropertyChanged(nameof(dbServerPass)); }
        }
        string _db_name;
        public string dbName {
            get { return _db_name; }
            set { _db_name = value; OnPropertyChanged(nameof(dbName)); }
        }
        bool _auto_create_db;
        public bool autoCreateDB {
            get { return _auto_create_db; }
            set { _auto_create_db = value; OnPropertyChanged(nameof(autoCreateDB)); }
        }


    }
}
