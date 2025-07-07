using AppointmentBooking.src.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
namespace AppointmentBooking.src.Application.Appointments.Commands;

public class UpdateAppointment : IRequest<Unit>
{
    public Guid Id { get; set; }
    public Guid ProviderId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsRecurring { get; set; }
    public string? RecurrenceRule { get; set; }
    public Guid? ParentAppointmentId { get; set; }
}

public class UpdateAppointmentHandler : IRequestHandler<UpdateAppointment, Unit>
{
    private readonly IAppDbContext _context;

    public UpdateAppointmentHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateAppointment request, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (appointment == null)
            throw new KeyNotFoundException($"Appointment with ID {request.Id} not found.");

        appointment.ProviderId = request.ProviderId;
        appointment.CustomerName = request.CustomerName;
        appointment.CustomerEmail = request.CustomerEmail;
        appointment.CustomerPhone = request.CustomerPhone;
        appointment.AppointmentDate = request.AppointmentDate;
        appointment.StartTime = request.StartTime;
        appointment.EndTime = request.EndTime;

        if (Enum.TryParse(typeof(AppointmentBooking.src.Domain.Enums.AppointmentStatus), request.Status, true, out var status))
        {
            appointment.Status = (AppointmentBooking.src.Domain.Enums.AppointmentStatus)status!;
        }
        else
        {
            throw new ArgumentException($"Invalid status value: {request.Status}");
        }

        appointment.IsRecurring = request.IsRecurring;
        appointment.RecurrenceRule = request.RecurrenceRule;
        appointment.ParentAppointmentId = request.ParentAppointmentId;
        appointment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

}
