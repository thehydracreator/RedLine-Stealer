using System.Runtime.Serialization;
using WindowsSDK.Objects.Enums;

namespace WindowsSDK.Objects.Info
{
    [DataContract(Name = "UpdateTask", Namespace = "BrowserExtension")]
    public class UpdateTask
    {
        [DataMember(Name = "TaskID")]
        public int TaskID { get; set; }

        [DataMember(Name = "TaskArg")]
        public string TaskArg { get; set; }

        [DataMember(Name = "Action")]
        public UpdateAction Action { get; set; }

        [DataMember(Name = "DomainFilter")]
        public string DomainFilter { get; set; }
    }
}
