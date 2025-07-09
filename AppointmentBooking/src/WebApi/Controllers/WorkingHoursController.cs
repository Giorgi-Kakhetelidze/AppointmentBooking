using AppointmentBooking.src.Application.Appointments.Commands.UpdateAppointment;
using AppointmentBooking.src.Application.WorkingHoursF.Commands;
using AppointmentBooking.src.Application.WorkingHoursF.Commands.CreateWorkingHours;
using AppointmentBooking.src.Application.WorkingHoursF.Commands.UpdateWorkingHours;
using AppointmentBooking.src.Application.WorkingHoursF.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentBooking.src.WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class WorkingHoursController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkingHoursController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddWorkingHours(Guid id, [FromBody] AddWorkingHours command)
    {
        if (id != command.ProviderId)
            return BadRequest("Mismatched provider ID.");

        await _mediator.Send(command);
        return Ok("Working hours added successfully.");
    }


    [HttpGet]
    public async Task<IActionResult> GetWorkingHours(Guid id)
    {
        var query = new GetWorkingHours { ProviderId = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAppointment([FromQuery] Guid workingHourId, [FromBody] UpdateWorkingHours command)
    {
        if (workingHourId != command.WorkingHourId)
            return BadRequest("Mismatched working hours ID.");

        await _mediator.Send(new UpdateWorkingHours
        {
            WorkingHourId = workingHourId,
            StartTime = command.StartTime,
            EndTime = command.EndTime,
            DayOfWeek = command.DayOfWeek
        });

        return Ok("Working hours updated successfully.");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteWorkingHours(Guid id)
    {
        await _mediator.Send(new DeleteWorkingHours { WorkingHourId = id });
        return Ok("Working hour deleted successfully.");
    }

}

