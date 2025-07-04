using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AppointmentBooking.src.Domain.Entities;

using ServiceProvider = AppointmentBooking.src.Domain.Entities.ServiceProvider;


namespace AppointmentBooking.src.Infrastructure.Persistence.Configurations
{
    public class ServiceProviderConfiguration : IEntityTypeConfiguration<ServiceProvider>
    {
        public void Configure(EntityTypeBuilder<ServiceProvider> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Specialty).IsRequired();
        }
    }
}
