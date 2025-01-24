using FluentValidation;

public class LoginValidator : AbstractValidator<Login>
{
    public LoginValidator()
    {
        RuleFor(login => login.PhoneNumber)
            .NotEmpty().WithMessage("Bu alan doldurulmalidir")
            .Must(phone => phone.StartsWith("05")).Matches(@"^\d{11}$").WithMessage("Telefon numarasi hatali, lutfen tekrar deneyiniz");
    }
}