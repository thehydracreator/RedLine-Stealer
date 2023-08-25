using System.Runtime.Serialization;

namespace WindowsSDK.Objects.Browsers
{
    [DataContract(Name = "Autofill", Namespace = "BrowserExtension")]
    public class Autofill
    {
        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Value")]
        public string Value { get; set; }
    }
}
