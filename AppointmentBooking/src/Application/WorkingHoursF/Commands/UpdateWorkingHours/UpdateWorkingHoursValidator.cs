using FluentValidation;

namespace AppointmentBooking.src.Application.WorkingHoursF.Commands.UpdateWorkingHours;

public class UpdateWorkingHoursValidator : AbstractValidator<UpdateWorkingHours>
{
    public UpdateWorkingHoursValidator()
    {
        RuleFor(x => x.StartTime)
            .LessThan(x => x.EndTime)
            .WithMessage("Start time must be before end time.");
    }
}
