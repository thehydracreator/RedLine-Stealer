using System;

public static class TypeConverter
{
    public static T Get<T>(object value, bool DefaultIfNull = false)
    {
        Type type = typeof(T);
        Type underlyingType = Nullable.GetUnderlyingType(type);
        if ((DefaultIfNull || underlyingType != null) && IsNull(value))
        {
            return default(T);
        }
        if (underlyingType != null)
        {
            type = underlyingType;
        }
        return (T)Convert.ChangeType(value, type);
    }

    public static bool IsNull(object value)
    {
        if (value != null)
        {
            return value == DBNull.Value;
        }
        return true;
    }
}
