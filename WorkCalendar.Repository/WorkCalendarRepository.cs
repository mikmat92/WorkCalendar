using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WorkCalendar.Domain;
using WorkCalendar.Repository.Infrastructure;
using WorkCalendar.Repository.Models;

namespace WorkCalendar.Repository
{
    public interface IWorkCalendarRepository
    {
        public void PersistHoliday(Holiday holiday);
        public HashSet<Holiday> GetHolidays(Day startingDay);
    }

    public class WorkCalendarRepository : IWorkCalendarRepository
    {
        private readonly WorkCalendarContext _context;

        public WorkCalendarRepository(WorkCalendarContext context)
        {
            _context = context;
            PersistDefaultHolidays();
        }

        private void PersistDefaultHolidays()
        {
            var defaultHoliday1 = new HolidayEntity
            {
                HolidayDateTimeOffset = new DateTime(2000, 5, 17),
                IsRecurring = true
            };
            var defaultHoliday2 = new HolidayEntity
            {
                HolidayDateTimeOffset = new DateTime(2004, 5, 27),
                IsRecurring = false
            };
            _context.HolidayEntities.AddIfNotExists(defaultHoliday1);
            _context.HolidayEntities.AddIfNotExists(defaultHoliday2);
            _context.SaveChanges();
        }

        public void PersistHoliday(Holiday holiday)
        {
            var entity = new HolidayEntity
            {
                HolidayDateTimeOffset = holiday.Date,
                IsRecurring = holiday.IsRecurringHoliday
            };
            _context.HolidayEntities.AddIfNotExists(entity);
            _context.SaveChanges();
        }

        public HashSet<Holiday> GetHolidays(Day startingDay)
        {
            var entities = _context.HolidayEntities
                .Where(i =>  i.IsRecurring || (!i.IsRecurring && startingDay.Date.Year == i.HolidayDateTimeOffset.Year));

            return entities.Select(i => new Holiday(i.HolidayDateTimeOffset, i.IsRecurring)).ToHashSet();
        }
    }
}