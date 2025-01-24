using FluentValidation;

public class LoginValidator : AbstractValidator<Login>
{
    public LoginValidator()
    {
        RuleFor(login => login.PhoneNumber)
            .NotEmpty().WithMessage("Bu alan doldurulmalidir")
            .Must(phone => phone.StartsWith("05")).WithMessage("Telefon numaraniz '05' ile baslamalidir")
            .Matches(@"^\d{11}$").WithMessage("Telefon numarasi hatali, lutfen tekrar deneyiniz");
    }
}