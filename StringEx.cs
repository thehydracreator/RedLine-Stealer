using System.Globalization;

public static class StringEx
{
    public static string ToTitleCase(this string str)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
    }
}
