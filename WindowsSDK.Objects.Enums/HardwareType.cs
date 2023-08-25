using System.Runtime.Serialization;

namespace WindowsSDK.Objects.Enums
{
    [DataContract(Name = "HardwareType")]
    public enum HardwareType
    {
        [EnumMember]
        Processor,
        [EnumMember]
        Graphic
    }
}
