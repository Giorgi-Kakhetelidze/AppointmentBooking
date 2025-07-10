using AppointmentBooking.src.Application.Appointments.Commands;
using AppointmentBooking.src.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointmentBooking.src.Application.Appointments.Commands.UpdateAppointment;

public class UpdateAppointment : AppointmentDtoBase, IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public Guid? ParentAppointmentId { get; set; }
}

public class UpdateAppointmentHandler : IRequestHandler<UpdateAppointment, Unit>
{
    private readonly IAppDbContext _context;
    private readonly IAppointmentValidatorService _validator;

    public UpdateAppointmentHandler(IAppDbContext context, IAppointmentValidatorService validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<Unit> Handle(UpdateAppointment request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAsync(request, request.Id, cancellationToken);

        var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        if (appointment == null)
            throw new KeyNotFoundException($"Appointment with ID {request.Id} not found.");

        appointment.ProviderId = request.ProviderId;
        appointment.CustomerName = request.CustomerName;
        appointment.CustomerEmail = request.CustomerEmail;
        appointment.CustomerPhone = request.CustomerPhone;
        appointment.AppointmentDate = request.AppointmentDate;
        appointment.StartTime = request.StartTime;
        appointment.EndTime = request.EndTime;
        appointment.IsRecurring = request.IsRecurring;
        appointment.RecurrenceRule = request.RecurrenceRule;
        appointment.ParentAppointmentId = request.ParentAppointmentId;
        appointment.UpdatedAt = DateTime.UtcNow;

        if (Enum.TryParse(typeof(Domain.Enums.AppointmentStatus), request.Status, true, out var status))
            appointment.Status = (Domain.Enums.AppointmentStatus)status!;
        else
            throw new ArgumentException($"Invalid status value: {request.Status}");

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}