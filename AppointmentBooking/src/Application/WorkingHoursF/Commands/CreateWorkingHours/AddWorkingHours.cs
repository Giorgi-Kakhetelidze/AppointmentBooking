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
        public bool IsActive { get; set; }

    }

}

public class AddWorkingHoursHandler : IRequestHandler<AddWorkingHours, Unit>
{
    private readonly IAppDbContext _context;

    public AddWorkingHoursHandler(IAppDbContext context){
        _context = context;
    }

    public async Task<Unit> Handle(AddWorkingHours request,CancellationToken cancellationToken)
    {
        var providerExists = await _context.ServiceProviders
            .AnyAsync(p => p.Id == request.ProviderId, cancellationToken);

        if (!providerExists)
            throw new KeyNotFoundException($"Provider with ID {request.ProviderId} not found.");

        var workingHours = request.Hours.Select(h => new WorkingHours
        {
            ProviderId = request.ProviderId,
            DayOfWeek = h.DayOfWeek,
            StartTime = h.StartTime,
            EndTime = h.EndTime,
            IsActive = h.IsActive
        });

        await _context.WorkingHours.AddRangeAsync(workingHours, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

}
