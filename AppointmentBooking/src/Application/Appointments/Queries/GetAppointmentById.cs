using AppointmentBooking.src.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.src.Application.Appointments.Queries;

public class GetAppointmentById : IRequest<AppointmentDetailsDto>
{
    public Guid Id { get; set; }
}

public class AppointmentDetailsDto
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

public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentById, AppointmentDetailsDto>
{
    private readonly IAppDbContext _context;

    public GetAppointmentByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<AppointmentDetailsDto> Handle(GetAppointmentById request, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (appointment == null)
            throw new KeyNotFoundException($"Appointment with ID {request.Id} not found.");

        return new AppointmentDetailsDto
        {
            Id = appointment.Id,
            ProviderId = appointment.ProviderId,
            CustomerName = appointment.CustomerName,
            CustomerEmail = appointment.CustomerEmail,
            CustomerPhone = appointment.CustomerPhone,
            AppointmentDate = appointment.AppointmentDate,
            StartTime = appointment.StartTime,
            EndTime = appointment.EndTime,
            Status = appointment.Status.ToString(),
            IsRecurring = appointment.IsRecurring,
            RecurrenceRule = appointment.RecurrenceRule,
            ParentAppointmentId = appointment.ParentAppointmentId
        };
    }
}





