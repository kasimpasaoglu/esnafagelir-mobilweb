using FluentValidation;
using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
{
    private readonly ILoginService _loginService;
    private readonly IValidator<Login> _validator;
    public LoginController(ILoginService loginService, IValidator<Login> validator)
    {
        _validator = validator;
        _loginService = loginService;
    }
    public IActionResult Index()
    {

        return View(new Login());
    }

    [HttpPost]
    public IActionResult Index(Login model)
    {
        // todo cihaz id yi sorgula, kayit varsa tarih sorgula, 
        // son giristen beri 14 gun gectiyse RequestDetail sayfasina yonlendir, 
        // 14 gunden azsa anasayfaya

        var validationResult = _validator.Validate(model);
        if (!validationResult.IsValid)
        {
            return View(model);
        }

        // validasyon basarili
        // todo : servise git kayit olustur, userId degerini al, kayit olusmazsa hata mesaji goster, olusursa RequestDetail sayfasina yonlendir.
        return RedirectToAction("RequestDetail");
    }
    public IActionResult LegalNotice()
    {
        return View();
    }

    public IActionResult RequestDetail()
    {
        return View();
    }
}