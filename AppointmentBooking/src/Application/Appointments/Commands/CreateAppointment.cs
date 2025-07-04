using AppointmentBooking.src.Application.Common.Interfaces;
using AppointmentBooking.src.Domain.Entities;
using AppointmentBooking.src.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.src.Application.Appointments.Commands;

public class CreateAppointmentCommand : IRequest<Guid>
{
    public Guid ProviderId { get; set; }

    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;

    public DateTime AppointmentDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public bool IsRecurring { get; set; }
    public string? RecurrenceRule { get; set; }
    public Guid? ParentAppointmentId { get; set; }
}

public class CreateAppointmentHandler : IRequestHandler<CreateAppointmentCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CreateAppointmentHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var provider = await _context.ServiceProviders
            .Include(p => p.WorkingHours)
            .FirstOrDefaultAsync(p => p.Id == request.ProviderId && p.IsActive, cancellationToken);

        if (provider == null)
            throw new Exception("Provider not found or inactive.");

        var now = DateTime.UtcNow;
        if (request.AppointmentDate < now.AddHours(24))
            throw new Exception("Appointments must be booked at least 24 hours in advance.");

        if (request.AppointmentDate > now.AddMonths(3))
            throw new Exception("Appointments can’t be booked more than 3 months ahead.");

        bool overlaps = await _context.Appointments
            .AnyAsync(a =>
                a.ProviderId == request.ProviderId &&
                a.AppointmentDate.Date == request.AppointmentDate.Date &&
                a.StartTime < request.EndTime &&
                a.EndTime > request.StartTime,
                cancellationToken);

        if (overlaps)
            throw new Exception("This time slot is already booked.");

        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            ProviderId = request.ProviderId,
            CustomerName = request.CustomerName,
            CustomerEmail = request.CustomerEmail,
            CustomerPhone = request.CustomerPhone,
            AppointmentDate = request.AppointmentDate,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Status = AppointmentStatus.Scheduled,
            IsRecurring = request.IsRecurring,
            RecurrenceRule = request.RecurrenceRule,
            ParentAppointmentId = request.ParentAppointmentId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync(cancellationToken);

        return appointment.Id;
    }
}
