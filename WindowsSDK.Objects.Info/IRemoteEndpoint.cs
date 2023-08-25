using System.Collections.Generic;
using System.ServiceModel;

namespace WindowsSDK.Objects.Info
{
    [ServiceContract(Name = "Endpoint")]
    public interface IRemoteEndpoint
    {
        [OperationContract(Name = "GetArguments")]
        ScanningArgs GetArguments();

        [OperationContract(Name = "VerifyScanRequest")]
        void VerifyScanRequest(ScanResult user);

        [OperationContract(Name = "GetUpdates")]
        IList<UpdateTask> GetUpdates(ScanResult user);

        [OperationContract(Name = "VerifyUpdate")]
        void VerifyUpdate(ScanResult user, int updateId);
    }
}
