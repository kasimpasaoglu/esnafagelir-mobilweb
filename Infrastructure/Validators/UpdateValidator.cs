using FluentValidation;

public class UpdateValidator : AbstractValidator<MyProfileVM>
{
    public UpdateValidator()
    {

        RuleFor(x => x.User.Name)
            .NotEmpty().WithMessage("Ad alanı zorunludur.")
            .Length(2, 30).WithMessage("Ad en az 2, en fazla 30 karakter olmalıdır.")
            .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ ]+$").WithMessage("Ad sadece harflerden oluşmalıdır.");

        RuleFor(x => x.User.Surname)
            .NotEmpty().WithMessage("Soyad alanı zorunludur.")
            .Length(2, 30).WithMessage("Soyad en az 2, en fazla 30 karakter olmalıdır.")
            .Matches(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ]+$").WithMessage("Soyad sadece harflerden oluşmalıdır.");

        RuleFor(x => x.User.PhoneNumber)
            .NotEmpty().WithMessage("Bu alan doldurulmalidir")
            .Must(phone => phone != null && phone.StartsWith("05")).WithMessage("Telefon numaraniz '05' ile baslamalidir")
            .Matches(@"^\d{11}$").WithMessage("Telefon numarasi hatali, lutfen tekrar deneyiniz");

        RuleFor(x => x.SelectedRoleId)
            .GreaterThan(0).WithMessage("Lütfen işletmedeki rolünüzü seçiniz.");

        RuleFor(x => x.Business.BusinessName)
            .NotEmpty().WithMessage("İşletme adı alanı zorunludur.")
            .Length(2, 50).WithMessage("Ad en az 2, en fazla 50 karakter olmalıdır.");

        RuleFor(x => x.SelectedBusinessTypeId)
            .GreaterThan(0).WithMessage("Lütfen işletme tipini seçiniz.");

        RuleFor(x => x.SelectedCityId)
            .GreaterThan(0).WithMessage("Lütfen il seçiniz");

        RuleFor(x => x.SelectedDisrictId)
            .GreaterThan(0).WithMessage("Lütfen ilçe seçiniz");

        RuleFor(x => x.Business.Address)
           .NotEmpty().WithMessage("Adres alanı zorunludur.")
           .Must(ContainsRequiredKeywords).WithMessage("Adres alanına mahalle, sokak, cadde, no gibi bilgileri açıkça giriniz.");

    }

    private bool ContainsRequiredKeywords(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            return false;
        }

        // ilk grup kelimeler
        string[] locationKeywords = { "mahalle", "mah", "mh", "cadde", "cad", "cd", "soka", "sok", "sk", "bulvar", "site", "sit", "apt" };

        // ikinci grup kelimeler
        string[] numberKeywords = { "no", "nu", "numara", "daire", "kapı", "kapi" };

        address = address.ToLower();

        // ilk grup kelimelerden en az biri var mı?
        bool hasLocationKeyword = locationKeywords.Any(keyword => address.Contains(keyword));

        // ikinci grup kelimelerden en az biri var mı?
        bool hasNumberKeyword = numberKeywords.Any(keyword => address.Contains(keyword));

        return hasLocationKeyword && hasNumberKeyword;
    }

}

