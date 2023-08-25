using System;
using System.Web.Script.Serialization;

namespace WindowsSDK.Extensions
{
    public static class Json
    {
        private static JavaScriptSerializer json;

        public static JavaScriptSerializer JSON
        {
            get
            {
                object obj = json;
                if (obj == null)
                {
                    obj = new JavaScriptSerializer
                    {
                        MaxJsonLength = int.MaxValue
                    };
                    json = (JavaScriptSerializer)obj;
                }
                return (JavaScriptSerializer)obj;
            }
        }

        public static T FromJSON<T>(this string @this)
        {
            try
            {
                return JSON.Deserialize<T>(@this.Trim());
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static string ToJSON(this object @this)
        {
            return JSON.Serialize(@this);
        }
    }
}
