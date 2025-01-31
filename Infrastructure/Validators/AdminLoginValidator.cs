using System.Data;
using FluentValidation;

public class AdminLoginValidator : AbstractValidator<AdminVM>
{
    public AdminLoginValidator()
    {
        RuleFor(x => x.UserName)
            .NotNull().WithMessage("Kullanıcı adı boş olamaz.")
            .MinimumLength(5).WithMessage("Kullanıcı adı en az 5 karakter olmalıdır.");

        RuleFor(x => x.UserPassword)
            .NotNull().WithMessage("Şifre boş olamaz.")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
            .Matches(@"[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
            .Matches(@"[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
            .Matches(@"\d").WithMessage("Şifre en az bir rakam içermelidir.")
            .Matches(@"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]").WithMessage("Şifre en az bir özel karakter içermelidir (!@#$ vb.).")
            .WithMessage("Şifre, kullanıcı adıyla aynı olmamalıdır.");

        RuleFor(x => x.ReUserPassword)
            .NotNull().WithMessage("Şifre tekrarı boş olamaz.")
            .Equal(x => x.UserPassword).WithMessage("Şifreler uyuşmuyor.");
    }
}