// Application/Providers/Queries/GetProvider.cs
using AppointmentBooking.src.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.src.Application.Providers.Queries;

public class GetProviderQuery : IRequest<GetProviderResponse?>
{
    public Guid Id { get; set; }
}

public class GetProviderResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<WorkingHoursDto> WorkingHours { get; set; } = new();
}

public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, GetProviderResponse?>
{
    private readonly IAppDbContext _context;

    public GetProviderQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetProviderResponse?> Handle(GetProviderQuery request, CancellationToken cancellationToken)
    {
        var provider = await _context.ServiceProviders
            .AsNoTracking()
            .Include(p => p.WorkingHours.Where(wh => wh.IsActive))
            .Where(p => p.Id == request.Id)
            .Select(p => new GetProviderResponse
            {
                Id = p.Id,
                Name = p.Name,
                Email = p.Email,
                Specialty = p.Specialty,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                WorkingHours = p.WorkingHours
                    .Where(wh => wh.IsActive)
                    .Select(wh => new WorkingHoursDto
                    {
                        Id = wh.Id,
                        DayOfWeek = wh.DayOfWeek,
                        StartTime = wh.StartTime,
                        EndTime = wh.EndTime,
                        IsActive = wh.IsActive
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        return provider;
    }
}