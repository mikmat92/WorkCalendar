using Microsoft.EntityFrameworkCore;
using WorkCalendar.Repository.Models;

namespace WorkCalendar.Repository.Infrastructure;

public class WorkCalendarContext : DbContext
{
    protected override void OnConfiguring
        (DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "WorkCalendarDb");
    }
    public DbSet<HolidayEntity> HolidayEntities { get; set; }
}