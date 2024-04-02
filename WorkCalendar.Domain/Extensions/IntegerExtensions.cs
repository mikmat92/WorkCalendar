namespace WorkCalendar.Domain.Extensions;

public static class IntegerExtensions
{
    public static bool IsPositive(this int value)
    {
        return value > 0;
    }
}