using AppointmentBooking.src.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.src.Infrastructure.BackgroundJobs;

public class AppointmentReminderService : IReminderJob
{
    private readonly IAppDbContext _context;
    private readonly IEmailService _emailService;

    public AppointmentReminderService(IAppDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    // 👇 Implementing the interface method properly
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var currentTime = TimeOnly.FromDateTime(now);

        // 1-hour reminders
        var oneHourReminders = await _context.Appointments
            .Where(a => !a.ReminderOneHourSent &&
                        a.AppointmentDate.Date == now.Date &&
                        a.StartTime <= TimeOnly.FromDateTime(now.AddHours(1)) &&
                        a.StartTime > TimeOnly.FromDateTime(now))
            .ToListAsync(cancellationToken);

        foreach (var appt in oneHourReminders)
        {
            await _emailService.SendEmailAsync(
                appt.CustomerEmail,
                "Reminder: Your appointment is in 1 hour!",
                $"Dear user, this is a reminder that your appointment is today at {appt.StartTime}."
            );
            appt.ReminderOneHourSent = true;
        }

        // 24-hour reminders
        if (currentTime >= new TimeOnly(9, 0) && currentTime <= new TimeOnly(20, 0))
        {
            var dayBeforeReminders = await _context.Appointments
                .Where(a => !a.ReminderDayBeforeSent &&
                            a.AppointmentDate.Date == now.Date.AddDays(1))
                .ToListAsync(cancellationToken);

            foreach (var appt in dayBeforeReminders)
            {
                await _emailService.SendEmailAsync(
                    appt.CustomerEmail,
                    "Reminder: Your appointment is tomorrow",
                    $"Hello, just a heads-up — you have an appointment tomorrow at {appt.StartTime}."
                );
                appt.ReminderDayBeforeSent = true;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
