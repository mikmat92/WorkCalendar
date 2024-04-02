namespace WorkCalendar.Domain.Extensions;

public static class TimeSpanExtensions
{
    public static bool IsWithinBusinessHours(this TimeSpan source, BusinessHours hours)
    {
        return !IsLessThanBusinessStart(source, hours) && !IsGreaterThanBusinessEnd(source, hours);
    }

    public static bool IsGreaterThanBusinessEnd(this TimeSpan source, BusinessHours hours)
    {
        return source > hours.DayEnd;
    }

    public static bool IsLessThanBusinessStart(this TimeSpan source, BusinessHours hours)
    {
        return source < hours.DayStart;
    }
}