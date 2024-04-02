using Microsoft.AspNetCore.Mvc;
using WorkCalendar.Api.ViewModels;
using WorkCalendar.Domain;
using WorkCalendar.Service;

namespace WorkCalendar.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkCalendarController : ControllerBase
{
    private readonly IWorkCalendarService _service;

    public WorkCalendarController(IWorkCalendarService service)
    {
        _service = service;
    }

    [HttpPost(Name = "CalculateWorkingDays")]
    public ActionResult<string> CalculateWorkingDays([FromBody] CalculationRequest request)
    {
        var businessHours = new BusinessHours(request.BusinessHourStart, request.BusinessHourEnd);
        var startDay = new Day(request.StartingPoint);
        var result = _service.CalculateWorkingDays(request.NumberOfDays, startDay, businessHours);

        return Ok(result.ToString());
    }

    [HttpPut(Name = "PersistHoliday")]
    public ActionResult PersistHoliday([FromBody] HolidayViewModel viewModel)
    {
        var holiday = new Holiday(viewModel.DateOfHoliday, viewModel.IsRecurring);
        _service.PersistHoliday(holiday);
        return Ok();
    }
}