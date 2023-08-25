using System.Runtime.Serialization;

namespace WindowsSDK.Objects.Serialization
{
    [DataContract(Name = "LocalState")]
    public class LocalState
    {
        [DataMember(Name = "os_crypt")]
        public OsCrypt os_crypt { get; set; }
    }
}
