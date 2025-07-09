using AppointmentBooking.src.Application.Common.Interfaces;
using AppointmentBooking.src.Application.Providers.Commands.UpdateProvider;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.src.Application.WorkingHoursF.Commands.UpdateWorkingHours;


public class UpdateWorkingHours : IRequest<Unit>
{
    public Guid WorkingHourId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}

public class UpdateWorkingHoursHandler : IRequestHandler<UpdateWorkingHours, Unit>
{
    private readonly IAppDbContext _context;

    public UpdateWorkingHoursHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateWorkingHours request, CancellationToken cancellationToken)
    {
        var hours = await _context.WorkingHours
            .FirstOrDefaultAsync(w => w.Id == request.WorkingHourId, cancellationToken);

        if (hours is null)
            throw new KeyNotFoundException($"Working hour with ID {request.WorkingHourId} not found.");

        hours.DayOfWeek = request.DayOfWeek;
        hours.StartTime = request.StartTime;
        hours.EndTime = request.EndTime;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
