namespace WorkCalendar.Domain.Extensions;

public static class DateTimeOffsetExtensions
{
    public static bool IsWorkingDay(this DateTimeOffset date, HashSet<Holiday> holidays)
    {
        if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            return false;

        if (holidays.Any(i => i.Date.Month == date.Month && i.Date.Day == date.Day && i.IsRecurringHoliday))
            return false;

        if (holidays.Any(i =>
                i.Date.Date.Year == date.Year && i.Date.Date.Month == date.Month && i.Date.Date.Day == date.Day))
            return false;

        return true;
    }
}