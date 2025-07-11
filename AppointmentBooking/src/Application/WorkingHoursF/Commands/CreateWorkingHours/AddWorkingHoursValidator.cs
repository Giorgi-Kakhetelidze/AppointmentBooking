using FluentValidation;
using static AppointmentBooking.src.Application.WorkingHoursF.Commands.CreateWorkingHours.AddWorkingHours;

namespace AppointmentBooking.src.Application.WorkingHoursF.Commands.CreateWorkingHours;

public class AddWorkingHoursValidator : AbstractValidator<AddWorkingHours>
{
    public AddWorkingHoursValidator()
    {
        RuleFor(x => x.ProviderId)
            .NotEmpty().WithMessage("ProviderId is required.");

        RuleForEach(x => x.Hours).SetValidator(new WorkingHoursItemValidator());
    }
}

public class WorkingHoursItemValidator : AbstractValidator<WorkingHoursItem>
{
    public WorkingHoursItemValidator()
    {
        RuleFor(x => x.StartTime)
            .LessThan(x => x.EndTime)
            .WithMessage("StartTime must be earlier than EndTime.");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("EndTime must be greater than StartTime");

        RuleFor(x => x.DayOfWeek)
            .IsInEnum()
            .WithMessage("Invalid DayOfWeek");

        When(x => x.BreakStart.HasValue && x.BreakEnd.HasValue, () =>
        {
            RuleFor(x => x.BreakStart)
                .LessThan(x => x.BreakEnd)
                .WithMessage("BreakStart must be earlier than BreakEnd.");

            RuleFor(x => x.BreakStart)
                .GreaterThanOrEqualTo(x => x.StartTime)
                .WithMessage("BreakStart must be within working hours.");

            RuleFor(x => x.BreakEnd)
                .LessThanOrEqualTo(x => x.EndTime)
                .WithMessage("BreakEnd must be within working hours.");
        });

        RuleFor(x => x)
            .Must(x => !x.BreakStart.HasValue || !x.BreakEnd.HasValue || (x.BreakStart.HasValue && x.BreakEnd.HasValue))
            .WithMessage("Both BreakStart and BreakEnd must be provided together.");
    }
}
