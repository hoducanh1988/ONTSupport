using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BosaManagementSystem.IFunction.IO {
    public class SmbClient {

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);
        [DllImport("kernel32", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);
        private IntPtr userHandle = IntPtr.Zero;
        private WindowsImpersonationContext impersonationContext;

        public bool IsDirectoryExist(string folder_path) {
            return Directory.Exists(folder_path);
        }

        public bool IsFileExist(string file_full_name) {
            return File.Exists(file_full_name);
        }

        public string GetHash<T>(string filename) where T : HashAlgorithm {
            using (FileStream fStream = File.OpenRead(filename)) {
                return GetHash<T>(fStream);
            }
        }

        private string GetHash<T>(Stream stream) where T : HashAlgorithm {
            StringBuilder sb = new StringBuilder();

            MethodInfo create = typeof(T).GetMethod("Create", new Type[] { });
            using (T crypt = (T)create.Invoke(null, null)) {
                byte[] hashBytes = crypt.ComputeHash(stream);
                foreach (byte bt in hashBytes) {
                    sb.Append(bt.ToString("x2"));
                }
            }
            return sb.ToString();
        }

        public void Dispose() {
            try {
                if (userHandle != IntPtr.Zero)
                    CloseHandle(userHandle);
                if (impersonationContext != null)
                    impersonationContext.Undo();
            }
            catch { }
        }

    }
}
