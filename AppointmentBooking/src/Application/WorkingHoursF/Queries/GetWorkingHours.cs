using AppointmentBooking.src.Application.Common.Interfaces;
using AppointmentBooking.src.Application.Providers.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.src.Application.WorkingHoursF.Queries;

public class GetWorkingHours : IRequest<List<WorkingHoursDto>>
{
    public Guid ProviderId { get; set; }
}

public class WorkingHoursDto
{
    public Guid Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsActive { get; set; }
}


public class GetWorkingHoursHandler : IRequestHandler<GetWorkingHours, List<WorkingHoursDto>>
{
    private readonly IAppDbContext _context;

    public GetWorkingHoursHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task <List<WorkingHoursDto>> Handle(GetWorkingHours request, CancellationToken cancellationToken)
    {
        var providerExists = await _context.ServiceProviders
            .AnyAsync(p => p.Id == request.ProviderId, cancellationToken);

        if (!providerExists)
            throw new KeyNotFoundException($"Provider with ID {request.ProviderId} not found.");

        var hours = await _context.WorkingHours
            .Where(w => w.ProviderId == request.ProviderId)
            .OrderBy(w => w.DayOfWeek)
            .ThenBy(w => w.StartTime)
            .Select(w => new WorkingHoursDto
            {
                Id = w.Id,
                DayOfWeek = w.DayOfWeek,
                StartTime = w.StartTime,
                EndTime = w.EndTime,
                IsActive = w.IsActive
            })
            .ToListAsync(cancellationToken);

        return hours;
    }
}