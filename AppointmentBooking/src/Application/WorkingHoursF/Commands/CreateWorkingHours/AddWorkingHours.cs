using AppointmentBooking.src.Application.Common.Interfaces;
using AppointmentBooking.src.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.src.Application.WorkingHoursF.Commands.CreateWorkingHours;

public class AddWorkingHours : IRequest<Unit>
{
    public Guid ProviderId { get; set; }
    public List<WorkingHoursItem> Hours { get; set; } = new();

    public class WorkingHoursItem
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public TimeOnly? BreakStart { get; set; }
        public TimeOnly? BreakEnd { get; set; }
        public bool IsActive { get; set; }
    }
}

public class AddWorkingHoursHandler : IRequestHandler<AddWorkingHours, Unit>
{
    private readonly IAppDbContext _context;

    public AddWorkingHoursHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddWorkingHours request, CancellationToken cancellationToken)
    {
        var providerExists = await _context.ServiceProviders
            .AnyAsync(p => p.Id == request.ProviderId, cancellationToken);

        if (!providerExists)
            throw new KeyNotFoundException($"Provider with ID {request.ProviderId} not found.");

        var workingHours = new List<WorkingHours>();

        foreach (var h in request.Hours)
        {
            if (h.BreakStart.HasValue && h.BreakEnd.HasValue)
            {
                if (h.BreakStart >= h.BreakEnd)
                    throw new ArgumentException($"Break start must be before break end on {h.DayOfWeek}.");

                if (h.BreakStart < h.StartTime || h.BreakEnd > h.EndTime)
                    throw new ArgumentException($"Break time on {h.DayOfWeek} must be within working hours.");
            }

            workingHours.Add(new WorkingHours
            {
                ProviderId = request.ProviderId,
                DayOfWeek = h.DayOfWeek,
                StartTime = h.StartTime,
                EndTime = h.EndTime,
                BreakStart = h.BreakStart,
                BreakEnd = h.BreakEnd,
                IsActive = h.IsActive
            });
        }

        await _context.WorkingHours.AddRangeAsync(workingHours, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
