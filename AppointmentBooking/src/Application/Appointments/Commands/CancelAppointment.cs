using AppointmentBooking.src.Application.Common.Interfaces;
using AppointmentBooking.src.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace AppointmentBooking.src.Application.Appointments.Commands;

public class CancelAppointmentCommand : IRequest<Unit>
{
    public Guid AppointmentId { get; set; }
    public string Reason { get; set; } = string.Empty;


}

public class CancelAppointmentHandler : IRequestHandler<CancelAppointmentCommand, Unit>
{
    private readonly IAppDbContext _context;
    private readonly IEmailService _emailService;

    public CancelAppointmentHandler(IAppDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<Unit> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, cancellationToken);

        if (appointment == null)
            throw new KeyNotFoundException("Appointment not found.");

        if (appointment.Status == AppointmentStatus.Cancelled)
            throw new InvalidOperationException("Appointment is already cancelled.");

        appointment.Status = AppointmentStatus.Cancelled;
        appointment.CancellationReason = request.Reason;
        appointment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        await _emailService.SendEmailAsync(
            appointment.CustomerEmail,
            "❌ Appointment Cancelled",
            $"""
            Hello {appointment.CustomerName},

            Your appointment on {appointment.AppointmentDate:yyyy-MM-dd} at {appointment.StartTime} has been cancelled.

            Reason: {request.Reason}

            We hope to see you again soon.
            """
        );

        return Unit.Value;
    }

}
