using AppointmentBooking.src.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;


namespace AppointmentBooking.src.Domain.Entities;

[Table("Appointments")]
public class Appointment
{
    public Guid Id { get; set; }
    public Guid ProviderId { get; set; }

    public string CustomerName { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public string CustomerPhone { get; set; } = default!;

    public DateTime AppointmentDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public AppointmentStatus Status { get; set; }
    public DateTime AppointmentStartUtc { get; set; }
    public string TimeZoneId { get; set; } = "Georgian Standard Time";


    public string? CancellationReason { get; set; }
    public bool IsRecurring { get; set; }
    public string? RecurrenceRule { get; set; }
    public Guid? ParentAppointmentId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ServiceProvider Provider { get; set; } = default!;
    public bool IsReminderSent { get; set; } = false;


}
