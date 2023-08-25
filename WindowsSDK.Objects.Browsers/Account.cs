using System.Runtime.Serialization;

namespace WindowsSDK.Objects.Browsers
{
    [DataContract(Name = "Account", Namespace = "BrowserExtension")]
    public class Account
    {
        [DataMember(Name = "URL")]
        public string URL { get; set; }

        [DataMember(Name = "Username")]
        public string Username { get; set; }

        [DataMember(Name = "Password")]
        public string Password { get; set; }
    }
}
