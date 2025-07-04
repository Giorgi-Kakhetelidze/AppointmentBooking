namespace AppointmentBooking.src.Domain.Entities
{
    public class ServiceProvider
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Specialty { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<WorkingHours> WorkingHours { get; set; } = new();
        public List<BlockedTime> BlockedTimes { get; set; } = new();
        public List<Appointment> Appointments { get; set; } = new();
    }

}
