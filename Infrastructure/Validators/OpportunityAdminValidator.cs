using FluentValidation;

public class OpportunityAdminValidator : AbstractValidator<OpportunityAdminModel>
{
    public OpportunityAdminValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Bu Alan Zorunlu");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Bu Alan Zorunlu");
        RuleFor(x => x.ImageFile).NotNull().WithMessage("Bu Alan Zorunlu");
        RuleFor(x => x.EndDate)
            .NotNull().WithMessage("Bu Alan Zorunlu")
            .GreaterThan(DateTime.Now).WithMessage("Firsat Bitis Tarihi Gelecekte Olmalidir");
    }
}