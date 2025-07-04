using AppointmentBooking.src.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.src.Application.Providers.Commands.DeleteProvider;


public class DeleteProviderCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class DeleteProviderCommandHandler : IRequestHandler<DeleteProviderCommand, Unit>
{
    private readonly IAppDbContext _context;

    public DeleteProviderCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteProviderCommand request, CancellationToken cancellationToken)
    {
        var provider = await _context.ServiceProviders
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (provider == null)
            throw new KeyNotFoundException($"Provider with ID {request.Id} not found.");

        provider.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
