using FluentValidation;

namespace AppointmentBooking.src.Application.Appointments.Commands
{
    public class UpdateAppointmentValidator : AbstractValidator<UpdateAppointment>
    {
        public UpdateAppointmentValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.ProviderId).NotEmpty();
            RuleFor(x => x.CustomerName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.CustomerEmail).NotEmpty().EmailAddress();
            RuleFor(x => x.CustomerPhone).NotEmpty().MaximumLength(20);

            RuleFor(x => x.StartTime)
                .LessThan(x => x.EndTime)
                .WithMessage("Start time must be before end time.");

            RuleFor(x => x.Status)
                .IsEnumName(typeof(AppointmentBooking.src.Domain.Enums.AppointmentStatus), caseSensitive: false)
                .WithMessage("Invalid appointment status.");
        }
    }
}







