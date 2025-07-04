namespace AppointmentBooking.src.Domain.Entities
{
    public class BlockedTime
    {
        public Guid Id { get; set; }
        public Guid ProviderId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public required string Reason { get; set; }
        public ServiceProvider Provider { get; set; } = default!;

    }

}
