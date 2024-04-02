using WorkCalendar.Domain;
using WorkCalendar.Repository;

namespace WorkCalendar.Service;

public interface IWorkCalendarService
{
    public Day CalculateWorkingDays(double numberOfDays, Day startingDay, BusinessHours businessHours);

    public void PersistHoliday(Holiday holiday);
}

public class WorkCalendarService : IWorkCalendarService
{
    private readonly IWorkCalendarRepository _workCalendarRepository;

    public WorkCalendarService(IWorkCalendarRepository workCalendarRepository)
    {
        _workCalendarRepository = workCalendarRepository;
    }

    public Day CalculateWorkingDays(double numberOfDays, Day startingDay, BusinessHours businessHours)
    {
        var holidays = _workCalendarRepository.GetHolidays(startingDay);
        var calculator = new WorkdayCalculator(businessHours, holidays);

        return calculator.CalculateWorkingDays(startingDay, numberOfDays);
    }

    public void PersistHoliday(Holiday holiday)
    {
        _workCalendarRepository.PersistHoliday(holiday);
    }

}