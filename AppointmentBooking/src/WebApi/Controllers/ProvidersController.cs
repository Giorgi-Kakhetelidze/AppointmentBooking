using AppointmentBooking.src.Application.Providers.Commands.CreateProvider;
using AppointmentBooking.src.Application.Providers.Commands.DeleteProvider;
using AppointmentBooking.src.Application.Providers.Commands.UpdateProvider;
using AppointmentBooking.src.Application.Providers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentBooking.src.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProvidersController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProvidersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<CreateProviderResponse>> CreateProvider(CreateProviderCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProvider), new { id = response.Id }, response);
    }

    [HttpGet]
    public async Task<ActionResult<GetProvidersResponse>> GetProviders(
        [FromQuery] bool? isActive = null,
        [FromQuery] string? specialty = null)
    {
        var query = new GetProvidersQuery
        {
            IsActive = isActive,
            Specialty = specialty
        };

        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetProviderResponse>> GetProvider(Guid id)
    {
        var query = new GetProviderQuery { Id = id };
        var provider = await _mediator.Send(query);

        if (provider == null)
        {
            return NotFound(new { Message = $"Provider with ID {id} not found." });
        }

        return Ok(provider);
    }

    

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProvider(Guid id, [FromBody] UpdateProviderCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID in URL and body must match.");

        await _mediator.Send(command);
        return Ok(new
        {
            message = "Provider successfully updated."
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProvider(Guid id)
    {
        await _mediator.Send(new DeleteProviderCommand { Id = id });

        return Ok(new
        {
            message = "Provider successfully deleted (soft deleted)."
        });
    }



}