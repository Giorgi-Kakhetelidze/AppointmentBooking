using AppointmentBooking.src.Application.Common.Interfaces;
using AppointmentBooking.src.Infrastructure.Email;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentBooking.src.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MailTestController : ControllerBase
{
    private readonly IEmailService _emailService;

    public MailTestController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] SmtpEmailService.SendEmailRequest request)
    {
        await _emailService.SendEmailAsync(request.To, request.Subject, request.Body);
        return Ok("Email sent.");
    }

}
