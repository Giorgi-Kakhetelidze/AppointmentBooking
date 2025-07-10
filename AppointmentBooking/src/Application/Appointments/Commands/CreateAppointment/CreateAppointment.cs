using AppointmentBooking.src.Application.Common.Interfaces;
using AppointmentBooking.src.Domain.Entities;
using AppointmentBooking.src.Domain.Enums;
using MediatR;

namespace AppointmentBooking.src.Application.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommand : AppointmentDtoBase, IRequest<Guid>
{
    public string TimeZoneId { get; set; } = "Georgian Standard Time";
    public Guid? ParentAppointmentId { get; set; }
}

public class CreateAppointmentHandler : IRequestHandler<CreateAppointmentCommand, Guid>
{
    private readonly IAppDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IAppointmentValidatorService _validator;

    public CreateAppointmentHandler(IAppDbContext context, IEmailService emailService, IAppointmentValidatorService validator)
    {
        _context = context;
        _emailService = emailService;
        _validator = validator;
    }

    public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAsync(request, null, cancellationToken);

        var localDate = DateTime.SpecifyKind(request.AppointmentDate.Date, DateTimeKind.Unspecified);
        var localDateTime = localDate + request.StartTime.ToTimeSpan();

        var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(request.TimeZoneId);
        var appointmentStartUtc = TimeZoneInfo.ConvertTimeToUtc(localDateTime, userTimeZone);

        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            ProviderId = request.ProviderId,
            CustomerName = request.CustomerName,
            CustomerEmail = request.CustomerEmail,
            CustomerPhone = request.CustomerPhone,
            AppointmentDate = request.AppointmentDate,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            AppointmentStartUtc = appointmentStartUtc,
            TimeZoneId = request.TimeZoneId,
            Status = AppointmentStatus.Scheduled,
            IsRecurring = request.IsRecurring,
            RecurrenceRule = request.RecurrenceRule,
            ParentAppointmentId = request.ParentAppointmentId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync(cancellationToken);

        var localTime = TimeZoneInfo.ConvertTimeFromUtc(appointmentStartUtc, userTimeZone);

        await _emailService.SendEmailAsync(
            request.CustomerEmail,
            "✅ Appointment Confirmed",
            $"""
            Hello {request.CustomerName},

            Your appointment has been successfully booked.

            📅 Date: {localTime:yyyy-MM-dd}
            🕒 Time: {localTime:HH:mm}

            Thank you for choosing us!
            """
        );

        return appointment.Id;
    }
}