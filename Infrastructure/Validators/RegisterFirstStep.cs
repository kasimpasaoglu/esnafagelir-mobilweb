using FluentValidation;

public class RegisterFirstStep : AbstractValidator<RegisterVM>
{
    public RegisterFirstStep()
    {
        #region  1. adim
        RuleFor(x => x.User.Name)
            .NotEmpty().WithMessage("Ad alanı zorunludur.")
            .Length(2, 30).WithMessage("Ad en az 2, en fazla 30 karakter olmalıdır.")
            .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ]+$").WithMessage("Ad sadece harflerden oluşmalıdır.");

        RuleFor(x => x.User.Surname)
            .NotEmpty().WithMessage("Soyad alanı zorunludur.")
            .Length(2, 30).WithMessage("Soyad en az 2, en fazla 30 karakter olmalıdır.")
            .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ]+$").WithMessage("Soyad sadece harflerden oluşmalıdır.");

        RuleFor(x => x.SelectedRoleId)
            .Must(id => id > 0).WithMessage("Lütfen işletmedeki rolünüzü seçiniz.");
        #endregion

    }

}

