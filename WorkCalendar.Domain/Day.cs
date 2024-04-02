namespace WorkCalendar.Domain;

public class Day
{
    public Day(DateTimeOffset dateOfDay)
    {
        Date = dateOfDay;
    }

    public DateTimeOffset Date { get; }

    public override string ToString()
    {
        return Date.ToString("g");
    }
}