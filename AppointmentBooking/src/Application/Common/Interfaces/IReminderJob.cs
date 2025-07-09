namespace AppointmentBooking.src.Application.Common.Interfaces;

public interface IReminderJob
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}

