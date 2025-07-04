using AppointmentBooking.src.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.src.Application.Providers.Commands.UpdateProvider;

public class UpdateProviderCommand : IRequest<Unit> 
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class UpdateProviderCommandHandler : IRequestHandler<UpdateProviderCommand, Unit>
{
    private readonly IAppDbContext _context;

    public UpdateProviderCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProviderCommand request, CancellationToken cancellationToken)
    {
        var provider = await _context.ServiceProviders
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (provider == null)
            throw new KeyNotFoundException($"Provider with ID {request.Id} not found.");

        provider.Name = request.Name;
        provider.Email = request.Email;
        provider.Specialty = request.Specialty;
        provider.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

