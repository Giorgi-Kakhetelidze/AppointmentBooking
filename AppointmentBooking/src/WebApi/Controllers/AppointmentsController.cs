using AppointmentBooking.src.Application.Appointments.Commands.CreateAppointment;
using AppointmentBooking.src.Application.Appointments.Commands.DeleteAppointment;
using AppointmentBooking.src.Application.Appointments.Commands.UpdateAppointment;
using AppointmentBooking.src.Application.Appointments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentBooking.src.WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentCommand command)
    {
        var appointmentId = await _mediator.Send(command);
        return Ok(new
        {
            message = "Appointment booked successfully.",
            appointmentId
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAppointments(
        [FromQuery] Guid? providerId,
        [FromQuery] DateTime? date,
        [FromQuery] string? customerEmail)
    {
        var query = new GetAppointmentsQuery
        {
            ProviderId = providerId,
            Date = date,
            CustomerEmail = customerEmail
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointementById(Guid id)
    {
        var result = await _mediator.Send(new GetAppointmentById { Id = id });
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointment(Guid id, [FromBody] UpdateAppointment command)
    {
        if (id != command.Id)
            return BadRequest("Mismatched appointment ID.");

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(Guid id) { 
        var command = new DeleteAppointment { Id =  id };
        await _mediator.Send(command);
        return NoContent();
    }


}
