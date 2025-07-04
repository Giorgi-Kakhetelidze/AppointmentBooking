using Microsoft.EntityFrameworkCore;
using AppointmentBooking.src.Domain.Entities;
using ServiceProvider = AppointmentBooking.src.Domain.Entities.ServiceProvider;

namespace AppointmentBooking.src.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<ServiceProvider> ServiceProviders { get; }
    DbSet<WorkingHours> WorkingHours { get; }
    DbSet<BlockedTime> BlockedTimes { get; }

    DbSet<Appointment> Appointments { get; set; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
