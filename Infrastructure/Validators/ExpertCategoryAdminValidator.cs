using FluentValidation;

public class ExpertCategoryAdminValidator : AbstractValidator<ExpertCategoryAdminModel>
{
    public ExpertCategoryAdminValidator()
    {
        RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Bu Alan Zorunlu");
        RuleFor(x => x.CategoryDescription).NotEmpty().WithMessage("Bu Alan Zorunlu");
        RuleFor(x => x.ImageFile).NotNull().WithMessage("Bu Alan Zorunlu");
    }
}
