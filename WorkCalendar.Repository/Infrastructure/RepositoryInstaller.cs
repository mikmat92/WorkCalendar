using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WorkCalendar.Repository.Infrastructure;

public class RepositoryInstaller
{
    public static void Install(IServiceCollection services)
    {
        services.AddScoped<DbContext, WorkCalendarContext>();
        services.AddScoped<IWorkCalendarRepository, WorkCalendarRepository>();
    }
}