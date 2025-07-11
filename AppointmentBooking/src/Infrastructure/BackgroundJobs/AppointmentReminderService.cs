using Microsoft.EntityFrameworkCore;
using AppointmentBooking.src.Application.Common.Interfaces;

namespace AppointmentBooking.src.Infrastructure.BackgroundJobs;

public class AppointmentReminderService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider; 
    private readonly ILogger<AppointmentReminderService> _logger;

    public AppointmentReminderService(IServiceProvider serviceProvider, ILogger<AppointmentReminderService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                var now = DateTime.UtcNow;
                var targetStart = now.AddHours(23).AddMinutes(55);
                var targetEnd = now.AddHours(24).AddMinutes(5);



                var upcomingAppointments = await dbContext.Appointments
                    .Where(a => a.AppointmentDate.Date == targetStart.Date
                                && a.StartTime >= TimeOnly.FromDateTime(targetStart)
                                && a.StartTime <= TimeOnly.FromDateTime(targetEnd))
                    .ToListAsync(stoppingToken);

                foreach (var appointment in upcomingAppointments)
                {
                    var email = appointment.CustomerEmail;
                    var subject = "Appointment Reminder";
                    var body = $"Dear {appointment.CustomerName},\n\n" +
                               $"This is a reminder for your appointment scheduled at {appointment.StartTime} on {appointment.AppointmentDate:yyyy-MM-dd}.\n\n" +
                               "Thank you.";

                    await emailService.SendEmailAsync(email, subject, body);
                    _logger.LogInformation($"Sent reminder email to {email} for appointment {appointment.Id}.");
                }

                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in AppointmentReminderService.");
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);

        }
    }
}