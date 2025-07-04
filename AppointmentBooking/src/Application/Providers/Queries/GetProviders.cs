using AppointmentBooking.src.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.src.Application.Providers.Queries;

public class GetProvidersQuery : IRequest<GetProvidersResponse>
{
    public bool? IsActive { get; set; }
    public string? Specialty { get; set; }
}

public class GetProvidersResponse
{
    public List<ProviderDto> Providers { get; set; } = new();
}

public class ProviderDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<WorkingHoursDto> WorkingHours { get; set; } = new();
}

public class WorkingHoursDto
{
    public Guid Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsActive { get; set; }
}

public class GetProvidersQueryHandler : IRequestHandler<GetProvidersQuery, GetProvidersResponse>
{
    private readonly IAppDbContext _context;

    public GetProvidersQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetProvidersResponse> Handle(GetProvidersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ServiceProviders.AsQueryable();

        if (request.IsActive.HasValue)
        {
            query = query.Where(p => p.IsActive == request.IsActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Specialty))
        {
            var search = request.Specialty.Trim();
            query = query.Where(p => EF.Functions.ILike(p.Specialty, $"%{search}%"));
        }



        var providers = await query
            .Include(p => p.WorkingHours.Where(wh => wh.IsActive))
            .OrderBy(p => p.Name)
            .Select(p => new ProviderDto
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
            .ToListAsync(cancellationToken);

        return new GetProvidersResponse
        {
            Providers = providers
        };
    }
}