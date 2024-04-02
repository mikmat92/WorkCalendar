namespace WorkCalendar.Domain;

public class BusinessHours
{
    public BusinessHours(int dayStart, int dayEnd)
    {
        if (dayStart > dayEnd) throw new Exception("Day start has to be less than day end");
        DayStart = new TimeSpan(dayStart, 0, 0);
        DayEnd = new TimeSpan(dayEnd, 0, 0);
        LengthOfDay = DayEnd - DayStart;
    }

    public BusinessHours(TimeSpan dayStart, TimeSpan dayEnd)
    {
        if (dayStart > dayEnd) throw new Exception("Day start has to be less than day end");
        DayStart = dayStart;
        DayEnd = dayEnd;
        LengthOfDay = DayEnd - DayStart;
    }

    public TimeSpan LengthOfDay { get; }
    public TimeSpan DayStart { get; }
    public TimeSpan DayEnd { get; }
}