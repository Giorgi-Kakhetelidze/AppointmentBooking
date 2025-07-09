using AppointmentBooking.src.Application.Common.Interfaces;
using AppointmentBooking.src.Application.Providers.Commands.DeleteProvider;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.src.Application.WorkingHoursF.Commands;

public class DeleteWorkingHours : IRequest<Unit>
{
    public Guid WorkingHourId { get; set; }

}

public class DeleteWorkingHoursHandler : IRequestHandler<DeleteWorkingHours, Unit>
{
    private readonly IAppDbContext _context;
    public DeleteWorkingHoursHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteWorkingHours request, CancellationToken cancellationToken)
    {
        var workingHours = await _context.WorkingHours
            .FirstOrDefaultAsync(p => p.Id == request.WorkingHourId, cancellationToken);

        if (workingHours == null)
            throw new KeyNotFoundException($"Working hour with ID {request.WorkingHourId} not found");

        _context.WorkingHours.Remove(workingHours);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;

        
    }

}
