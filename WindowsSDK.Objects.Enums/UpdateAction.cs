using System.Runtime.Serialization;

namespace WindowsSDK.Objects.Enums
{
    [DataContract(Name = "RemoteTaskAction")]
    public enum UpdateAction
    {
        [EnumMember]
        Download,
        [EnumMember]
        RunPE,
        [EnumMember]
        DownloadAndEx,
        [EnumMember]
        OpenLink,
        [EnumMember]
        Cmd
    }
}
