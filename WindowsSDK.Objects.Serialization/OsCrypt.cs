using System.Runtime.Serialization;

namespace WindowsSDK.Objects.Serialization
{
    [DataContract(Name = "OsCrypt")]
    public class OsCrypt
    {
        [DataMember(Name = "encrypted_key")]
        public string encrypted_key { get; set; }
    }
}
