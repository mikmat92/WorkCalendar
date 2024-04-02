namespace WorkCalendar.Api.ViewModels;

public class CalculationRequest
{
    public double NumberOfDays { get; set; }
    public DateTimeOffset StartingPoint { get; set; }
    public int BusinessHourStart { get; set; }
    public int BusinessHourEnd { get; set; }
}