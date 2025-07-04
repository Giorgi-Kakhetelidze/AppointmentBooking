using AppointmentBooking.src.Application.Common.Interfaces;
using AppointmentBooking.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;


using DomainServiceProvider = AppointmentBooking.src.Domain.Entities.ServiceProvider;

namespace AppointmentBooking.src.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<DomainServiceProvider> ServiceProviders => Set<DomainServiceProvider>();
    public DbSet<WorkingHours> WorkingHours => Set<WorkingHours>();
    public DbSet<BlockedTime> BlockedTimes => Set<BlockedTime>();
    public DbSet<Appointment> Appointments { get; set; } 


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
