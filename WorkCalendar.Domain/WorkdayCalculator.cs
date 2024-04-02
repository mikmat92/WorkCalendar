using WorkCalendar.Domain.Extensions;

namespace WorkCalendar.Domain;

public class WorkdayCalculator
{
    private readonly HashSet<Holiday> Holidays;
    private readonly BusinessHours Hours;
    private bool IsWorkingDay(DateTimeOffset date) => date.IsWorkingDay(Holidays);

    public WorkdayCalculator(BusinessHours hours, HashSet<Holiday> holidays)
    {
        Hours = hours;
        Holidays = holidays;
    }

    public Day CalculateWorkingDays(Day startDay, double numDays)
    {
        var direction = numDays > 0 ? 1 : -1;
        var fullDays = (int)Math.Truncate(numDays);
        var remainder = numDays - fullDays;
        var currentDay = startDay.Date;

        while (fullDays != 0)
        {
            currentDay = currentDay.AddDays(direction);
            if (IsWorkingDay(currentDay))
                fullDays -= direction;
        }

        var dayToReturn = AdjustTime(currentDay, remainder, direction);
        return dayToReturn;
    }

    private Day AdjustTime(DateTimeOffset date, double remainder, int direction)
    {
        if (remainder != 0 || !date.TimeOfDay.IsWithinBusinessHours(Hours))
        {
            if (direction.IsPositive()) return HandlePositiveDirection(date, remainder, direction);

            return HandleNegativeDirection(date, remainder, direction);
        }

        return new Day(date);
    }

    private Day HandlePositiveDirection(DateTimeOffset date, double remainder, int direction)
    {
        var currentTime = date.TimeOfDay;
        var hoursToAdd = TimeSpan.FromHours(Hours.LengthOfDay.Hours * remainder);
        var newTime = currentTime.Add(hoursToAdd);

        //If the time is before day start then start at 0800
        if (currentTime.IsLessThanBusinessStart(Hours))
        {
            newTime = Hours.DayStart.Add(hoursToAdd);
            return new Day(date.Date + newTime);
        }

        //If the time is greater than 16, start at 0800 the following day
        if (currentTime.IsGreaterThanBusinessEnd(Hours))
        {
            newTime = Hours.DayStart.Add(hoursToAdd);
            return GetDayToReturn(date, newTime, direction);
        }

        //If the current time is within business hours, but after we add the remainder it gets past 16, we want to subtract the rest of the day and add that to the next day
        if ((currentTime + hoursToAdd).IsGreaterThanBusinessEnd(Hours))
        {
            newTime = Hours.DayStart.Add(date.TimeOfDay + hoursToAdd - Hours.DayEnd);
            return GetDayToReturn(date, newTime, direction);
        }

        return new Day(date.Date + newTime);
    }

    private Day HandleNegativeDirection(DateTimeOffset date, double remainder, int direction)
    {
        var currentTime = date.TimeOfDay;
        var hoursToAdd = TimeSpan.FromHours(Hours.LengthOfDay.Hours * remainder);
        var newTime = currentTime.Add(hoursToAdd);

        //If the time is greater than 16, start at 1600
        if (currentTime.IsGreaterThanBusinessEnd(Hours))
        {
            newTime =Hours.DayEnd.Add(hoursToAdd);
            return new Day(date.Date + newTime);
        }

        //If the time is less than 0800, start 1600 the previous day
        if (currentTime.IsLessThanBusinessStart(Hours))
        {
            newTime = Hours.DayEnd.Add(hoursToAdd);
            return GetDayToReturn(date, newTime, direction);
        }

        //If the current time is within business hours, but after we add the remainder it gets below 8, we want to subtract the rest of the day and add that to the previous day
        if ((currentTime + hoursToAdd).IsLessThanBusinessStart(Hours))
        {
            var timeToSubtract = hoursToAdd - Hours.LengthOfDay.Subtract(date.TimeOfDay);
            newTime = Hours.DayEnd.Add(timeToSubtract);

            return GetDayToReturn(date, newTime, direction);
        }

        return new Day(date.Date + newTime);
    }

    //This takes the time adjustment into account and ensures that we don't land on a holiday or weekend. 
    private Day GetDayToReturn(DateTimeOffset date, TimeSpan newTime, int direction)
    {
        var dateToReturn = date.Date.AddDays(direction) + newTime;
        
        while (!IsWorkingDay(dateToReturn))
        {
            dateToReturn = dateToReturn.AddDays(direction);
        }

        return new Day(dateToReturn);
    }
}