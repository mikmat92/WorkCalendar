namespace WorkCalendar.Domain;

public class Holiday : Day
{
    public Holiday(DateTimeOffset dateOfDay, bool isRecurringHoliday) : base(dateOfDay)
    {
        IsRecurringHoliday = isRecurringHoliday;
    }

    public bool IsRecurringHoliday { get; }
}