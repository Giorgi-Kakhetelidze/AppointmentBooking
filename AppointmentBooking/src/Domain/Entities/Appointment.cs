using AppointmentBooking.src.Domain.Enums;

namespace AppointmentBooking.src.Domain.Entities
{
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

        public string? CancellationReason { get; set; }
        public bool IsRecurring { get; set; }
        public string? RecurrenceRule { get; set; }
        public Guid? ParentAppointmentId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ServiceProvider Provider { get; set; } = default!;
        public bool ReminderOneHourSent { get; set; }
        public bool ReminderDayBeforeSent { get; set; }


    }
}
