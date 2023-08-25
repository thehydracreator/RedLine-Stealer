using System;

public static class TimeStampEx
{
    public static TimeSpan Mult(this TimeSpan ts, int multiplier)
    {
        return TimeSpan.FromTicks(ts.Ticks * multiplier);
    }

    public static TimeSpan Div(this TimeSpan ts, int denominator)
    {
        return TimeSpan.FromTicks(ts.Ticks / denominator);
    }

    public static double Div(this TimeSpan ts, TimeSpan denominator)
    {
        return ts.Ticks / (double)denominator.Ticks;
    }
}
