using FluentValidation;

public class LoginValidator : AbstractValidator<LoginVM>
{
    public LoginValidator()
    {
        RuleFor(login => login.PhoneNumber)
            .NotEmpty().WithMessage("Bu alan doldurulmalidir")
            .Must(phone => phone != null && phone.StartsWith("05")).WithMessage("Telefon numaraniz '05' ile baslamalidir")
            .Matches(@"^\d{11}$").WithMessage("Telefon numarasi hatali, lutfen tekrar deneyiniz");

        RuleFor(login => login.IsPrivacyPolicyAccepted)
            .Equal(true).WithMessage("Aydinlatma metnini onaylamaniz gerekmektedir.");
    }
}