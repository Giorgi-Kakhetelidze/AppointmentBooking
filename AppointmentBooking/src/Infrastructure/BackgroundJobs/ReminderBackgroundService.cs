namespace AppointmentBooking.src.Infrastructure.BackgroundJobs;

public class ReminderBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public ReminderBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var reminderService = scope.ServiceProvider.GetRequiredService<AppointmentReminderService>();
            await reminderService.ExecuteAsync(stoppingToken);

            //await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

        }
    }
}

