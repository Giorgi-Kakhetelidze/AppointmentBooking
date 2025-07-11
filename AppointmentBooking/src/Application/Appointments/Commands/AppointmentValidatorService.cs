using AppointmentBooking.src.Application.Common.Interfaces;
using AppointmentBooking.src.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.src.Application.Appointments.Commands;

public interface IAppointmentValidatorService
{
    Task ValidateAsync(AppointmentDtoBase request, Guid? appointmentIdToExclude = null, CancellationToken cancellationToken = default);
}

public class AppointmentValidatorService : IAppointmentValidatorService
{
    private readonly IAppDbContext _context;

    public AppointmentValidatorService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task ValidateAsync(AppointmentDtoBase request, Guid? appointmentIdToExclude = null, CancellationToken cancellationToken = default)
    {
        if (request.StartTime >= request.EndTime)
            throw new ArgumentException("Start time must be before end time.");

        if (request.IsRecurring && string.IsNullOrWhiteSpace(request.RecurrenceRule))
            throw new ArgumentException("Recurrence rule is required for recurring appointments.");

        var provider = await _context.ServiceProviders
            .Include(p => p.WorkingHours)
            .FirstOrDefaultAsync(p => p.Id == request.ProviderId && p.IsActive, cancellationToken);

        if (provider == null)
            throw new Exception("Provider not found or inactive.");

        var workingHours = provider.WorkingHours
            .FirstOrDefault(w => w.DayOfWeek == request.AppointmentDate.DayOfWeek && w.IsActive);

        if (workingHours == null)
            throw new Exception("Provider does not work on the selected day.");

        if (request.StartTime < workingHours.StartTime || request.EndTime > workingHours.EndTime)
            throw new Exception($"Appointment must be within working hours: {workingHours.StartTime} - {workingHours.EndTime}");

        if (workingHours.BreakStart.HasValue && workingHours.BreakEnd.HasValue)
        {
            bool overlapsBreak = request.StartTime < workingHours.BreakEnd && request.EndTime > workingHours.BreakStart;
            if (overlapsBreak)
                throw new Exception($"Appointment overlaps with provider's break time: {workingHours.BreakStart} - {workingHours.BreakEnd}");
        }

        bool overlaps = await _context.Appointments.AnyAsync(a =>
            a.Id != appointmentIdToExclude &&
            a.ProviderId == request.ProviderId &&
            a.AppointmentDate.Date == request.AppointmentDate.Date &&
            a.StartTime < request.EndTime &&
            a.EndTime > request.StartTime,
            cancellationToken);

        if (overlaps)
            throw new Exception("This time slot is already booked.");

        var now = DateTime.UtcNow;
        var appointmentDateTime = DateTime.SpecifyKind(request.AppointmentDate.Date, DateTimeKind.Utc) + request.StartTime.ToTimeSpan();

        if (appointmentDateTime < now.AddHours(24))
            throw new ArgumentException("Appointments must be booked at least 24 hours in advance.");

        if (appointmentDateTime > now.AddMonths(3))
            throw new ArgumentException("Appointments cannot be booked more than 3 months in advance.");

        var duration = request.EndTime - request.StartTime;
        var durationMinutes = (int)duration.TotalMinutes;

        if (!Enum.IsDefined(typeof(AppointmentDuration), durationMinutes))
            throw new ArgumentException("Invalid appointment duration. Allowed: 15, 30, 45, or 60 minutes.");
    }
}