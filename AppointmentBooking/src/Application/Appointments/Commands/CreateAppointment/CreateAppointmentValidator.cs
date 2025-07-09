using FluentValidation;

namespace AppointmentBooking.src.Application.Appointments.Commands.CreateAppointment;

public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentValidator()
    {
        RuleFor(x => x.CustomerName).NotEmpty();
        RuleFor(x => x.CustomerEmail).NotEmpty().EmailAddress();
        RuleFor(x => x.CustomerPhone).NotEmpty();

        RuleFor(x => x.StartTime).LessThan(x => x.EndTime)
            .WithMessage("Start time must be before end time.");

        RuleFor(x => x.AppointmentDate).NotEmpty();

        RuleFor(x => x.RecurrenceRule)
            .NotEmpty()
            .When(x => x.IsRecurring)
            .WithMessage("Recurrence rule is required for recurring appointments.");
    }
}
