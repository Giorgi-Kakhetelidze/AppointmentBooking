using AppointmentBooking.src.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.src.Application.Appointments.Queries;

public class GetAppointmentsQuery : IRequest<GetAppointmentsResponse>
{
    public Guid? ProviderId { get; set; }
    public DateTime? Date { get; set; }
    public string? CustomerEmail { get; set; }
}

public class GetAppointmentsResponse
{
    public List<AppointmentDto> Appointments { get; set; } = new();
}

public class AppointmentDto
{
    public Guid Id { get; set; }
    public Guid ProviderId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class GetAppointmentsQueryHandler : IRequestHandler<GetAppointmentsQuery, GetAppointmentsResponse>
{
    private readonly IAppDbContext _context;

    public GetAppointmentsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetAppointmentsResponse> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Appointments.AsQueryable();

        if (request.ProviderId.HasValue)
            query = query.Where(a => a.ProviderId == request.ProviderId.Value);

        if (!string.IsNullOrWhiteSpace(request.CustomerEmail))
            query = query.Where(a => a.CustomerEmail.ToLower() == request.CustomerEmail.ToLower());

        if (request.Date.HasValue)
            query = query.Where(a => a.AppointmentDate.Date == request.Date.Value.Date);

        var appointments = await query
            .OrderBy(a => a.AppointmentDate)
            .ThenBy(a => a.StartTime)
            .Select(a => new AppointmentDto
            {
                Id = a.Id,
                ProviderId = a.ProviderId,
                CustomerName = a.CustomerName,
                CustomerEmail = a.CustomerEmail,
                CustomerPhone = a.CustomerPhone,
                AppointmentDate = a.AppointmentDate,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                Status = a.Status.ToString()
            })
            .ToListAsync(cancellationToken);

        return new GetAppointmentsResponse { Appointments = appointments };
    }
}
