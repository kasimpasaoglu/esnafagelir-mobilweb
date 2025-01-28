using FluentValidation;

public class ContactFormValidator : AbstractValidator<ContactFormVM>
{
    public ContactFormValidator()
    {
        RuleFor(x => x.Message)
            .MinimumLength(4).WithMessage("Lütfen en az 4 karakterden oluşan bir not girin.")
            .NotNull().WithMessage("Lütfen en az 4 karakterden oluşan bir not girin.");
    }
}