using System.Runtime.Serialization;

namespace WindowsSDK.Objects.Browsers
{
    [DataContract(Name = "CC", Namespace = "BrowserExtension")]
    public class CC
    {
        [DataMember(Name = "HolderName")]
        public string HolderName { get; set; }

        [DataMember(Name = "Month")]
        public int Month { get; set; }

        [DataMember(Name = "Year")]
        public int Year { get; set; }

        [DataMember(Name = "Number")]
        public string Number { get; set; }
    }
}
