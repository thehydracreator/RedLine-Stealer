using System;
using System.Text.RegularExpressions;

public static class RegexEx
{
    public static bool IsValid(string pattern)
    {
        try
        {
            Regex.IsMatch(string.Empty, pattern);
        }
        catch (ArgumentException)
        {
            return false;
        }
        return true;
    }
}
