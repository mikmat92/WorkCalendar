using Microsoft.Extensions.DependencyInjection;

namespace WorkCalendar.Service.Installers;

public class WorkCalendarServiceInstallers
{
    public static void Install(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IWorkCalendarService, WorkCalendarService>();
    }
}