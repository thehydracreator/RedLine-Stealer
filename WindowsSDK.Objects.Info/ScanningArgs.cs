using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WindowsSDK.Objects.Info
{
    [DataContract(Name = "ScanningArgs", Namespace = "BrowserExtension")]
    public class ScanningArgs
    {
        [DataMember(Name = "ScanBrowsers")]
        public bool ScanBrowsers { get; set; }

        [DataMember(Name = "ScanFiles")]
        public bool ScanFiles { get; set; }

        [DataMember(Name = "ScanFTP")]
        public bool ScanFTP { get; set; }

        [DataMember(Name = "ScanWallets")]
        public bool ScanWallets { get; set; }

        [DataMember(Name = "ScanScreen")]
        public bool ScanScreen { get; set; }

        [DataMember(Name = "ScanTelegram")]
        public bool ScanTelegram { get; set; }

        [DataMember(Name = "ScanVPN")]
        public bool ScanVPN { get; set; }

        [DataMember(Name = "ScanSteam")]
        public bool ScanSteam { get; set; }

        [DataMember(Name = "ScanDiscord")]
        public bool ScanDiscord { get; set; }

        [DataMember(Name = "ScanFilesPaths")]
        public List<string> ScanFilesPaths { get; set; }

        [DataMember(Name = "BlockedCountry")]
        public List<string> BlockedCountry { get; set; }

        [DataMember(Name = "BlockedIP")]
        public List<string> BlockedIP { get; set; }

        [DataMember(Name = "ScanChromeBrowsersPaths")]
        public List<string> ScanChromeBrowsersPaths { get; set; }

        [DataMember(Name = "ScanGeckoBrowsersPaths")]
        public List<string> ScanGeckoBrowsersPaths { get; set; }
    }
}
