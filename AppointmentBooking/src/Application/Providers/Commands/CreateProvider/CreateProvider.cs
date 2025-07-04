using AppointmentBooking.src.Application.Common.Interfaces;
using MediatR;
using DomainServiceProvider = AppointmentBooking.src.Domain.Entities.ServiceProvider;

namespace AppointmentBooking.src.Application.Providers.Commands.CreateProvider;

// 🟢 Command (Request)
public class CreateProviderCommand : IRequest<CreateProviderResponse>
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Specialty { get; set; }
}

// 🟢 Handler (Business Logic)
public class CreateProviderHandler : IRequestHandler<CreateProviderCommand, CreateProviderResponse>
{
    private readonly IAppDbContext _context;

    public CreateProviderHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<CreateProviderResponse> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
    {
        var provider = new DomainServiceProvider
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Specialty = request.Specialty,
            CreatedAt = DateTime.UtcNow
        };

        _context.ServiceProviders.Add(provider);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateProviderResponse
        {
            Id = provider.Id,
            Name = provider.Name,
            Email = provider.Email,
            Specialty = provider.Specialty,
            CreatedAt = provider.CreatedAt
        };
    }
}

// 🟢 Response DTO (Result)
public class CreateProviderResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Specialty { get; set; }
    public DateTime CreatedAt { get; set; }
}
