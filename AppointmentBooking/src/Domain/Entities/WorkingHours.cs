namespace AppointmentBooking.src.Domain.Entities;

public class WorkingHours
{
    public Guid Id { get; set; }
    public Guid ProviderId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }  
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsActive { get; set; }
    public ServiceProvider Provider { get; set; } = default!;

}
