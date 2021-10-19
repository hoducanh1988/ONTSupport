using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HEC {

    public class Parser {
        public static bool IsIPAddress(string ip) {
            string _pattern = "^[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}$";
            return Regex.IsMatch(ip, _pattern, RegexOptions.IgnoreCase);
        }

    }
}
