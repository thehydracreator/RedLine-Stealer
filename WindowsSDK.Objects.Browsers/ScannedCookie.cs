using System.Runtime.Serialization;

namespace WindowsSDK.Objects.Browsers
{
    [DataContract(Name = "ScannedCookie", Namespace = "BrowserExtension")]
    public class ScannedCookie
    {
        [DataMember(Name = "Host")]
        public string Host { get; set; }

        [DataMember(Name = "Http")]
        public bool Http { get; set; }

        [DataMember(Name = "Path")]
        public string Path { get; set; }

        [DataMember(Name = "Secure")]
        public bool Secure { get; set; }

        [DataMember(Name = "Expires")]
        public long Expires { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Value")]
        public string Value { get; set; }
    }
}
