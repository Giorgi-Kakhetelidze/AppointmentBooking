using AppointmentBooking.src.Application.Providers.Commands.CreateProvider;
using FluentValidation;

public class ProviderValidator : AbstractValidator<CreateProviderCommand>
{
    public ProviderValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.")
            .Matches(@"^[a-zA-Z\s'-]+$").WithMessage("Name can only contain letters, spaces, apostrophes, and hyphens.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.");

        RuleFor(x => x.Specialty)
            .NotEmpty().WithMessage("Specialty is required.")
            .MinimumLength(3).WithMessage("Specialty must be at least 3 characters long.")
            .MaximumLength(100).WithMessage("Specialty must not exceed 100 characters.")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("Specialty should only contain letters and spaces.");
    }
}
