using AppointmentBooking.src.Application.Common.Interfaces;
using AppointmentBooking.src.Application.Providers.Commands.DeleteProvider;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.src.Application.Appointments.Commands.DeleteAppointment;

public class DeleteAppointment : IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class DeleteAppointementHandler : IRequestHandler<DeleteAppointment, Unit>
{
    private readonly IAppDbContext _context;

    public DeleteAppointementHandler(IAppDbContext context)
    {
        _context = context;
    } 

    public async Task<Unit> Handle(DeleteAppointment request, CancellationToken cancellationToken)
    {
        var appointement = await _context.Appointments
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (appointement == null) 
            throw new KeyNotFoundException($"Appointement with ID {request.Id} not found.");

        _context.Appointments.Remove(appointement);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }




}
