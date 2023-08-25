using System.Runtime.Serialization;

namespace WindowsSDK.Objects.Info
{
    [DataContract(Name = "BrowserVersion", Namespace = "BrowserExtension")]
    public class BrowserVersion
    {
        [DataMember(Name = "NameOfBrowser")]
        public string NameOfBrowser { get; set; }

        [DataMember(Name = "Version")]
        public string Version { get; set; }

        [DataMember(Name = "PathOfFile")]
        public string PathOfFile { get; set; }
    }
}
