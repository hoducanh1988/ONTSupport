using BosaManagementSystem.IFunction.Custom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BosaManagementSystem.IFunction.DataBase
{
    public interface IDatabase {

        bool isConnected();
        bool createDatabase();
        bool createTableBosaFile();
        bool createTableBosaSerialNumber();
        bool createTableClient();
        bool isExistInTableBosaFile(bosaFileInfo bsi);
        bool insertRowIntoTableBosaSerialNumber(bosaInfo bsi);
        bool insertRowIntoTableBosaSerialNumber(List<bosaInfo> bsis, out string log_dupplicate);
        bool insertRowIntoTableBosaFile(bosaFileInfo bsf);
        bool deleteAllTableBosaFile();
        bool deleteAllTableBosaSerialNumber();
        DataTable selectAllBosaFileFromTableBosaSerialNumber();
        DataTable selectFromTableBosaSerialNumber(string bosa_sn, string ith, string vbr, string bosa_file, int idx, int split_value);
        bool selectFromTableBosaSerialNumber(string bosa_sn, string ith, string vbr, string bosa_file, out string count);
    }
}
