using esnafagelir_mobilweb.DMO;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class LoginController : Controller
{
    private readonly ILoginService _loginService;
    private readonly IValidator<LoginVM> _loginValidator;

    public LoginController(ILoginService loginService, IValidator<LoginVM> loginValidator)
    {
        _loginValidator = loginValidator;
        _loginService = loginService;
    }
    public IActionResult Index()
    {
        // query string device id gelecek, veritabaninda sorgulanacak,
        // ona gore dogru sayfaya yonelndirilecek.

        return View(new LoginVM());
    }

    [HttpPost]
    public IActionResult Index(LoginVM model)
    {
        // todo cihaz id yi sorgula, kayit varsa tarih sorgula, 
        // son giristen beri 14 gun gectiyse RequestDetail sayfasina yonlendir, 
        // 14 gunden azsa anasayfaya

        var validationResult = _loginValidator.Validate(model);
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

    public IActionResult RegisterFirst()
    {
        var roles = new List<Role>
    {
        new Role { RoleId = 1, RoleName = "Isveren" },
        new Role { RoleId = 2, RoleName = "Mudur" },
        new Role { RoleId = 3, RoleName = "Muhasebe" },
        new Role { RoleId = 4, RoleName = "Satin Alma" }
    };

        var model = new RegisterVM
        {
            User = new UserVM(),
            Roles = roles,
            SelectedRoleId = 0
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult RegisterFirst(RegisterVM model)
    {
        var firstStepValidator = new RegisterFirstStep();
        var validationResult = firstStepValidator.Validate(model);
        if (!validationResult.IsValid)
        {
            var roles = new List<Role>(); //. todo database'den rol listesini cek
            model.Roles = roles;
            return View(model);
        }
        TempData["RegisterVM"] = JsonConvert.SerializeObject(model);
        return RedirectToAction("RegisterSecond");
    }

    public IActionResult RegisterSecond()
    {
        var model = JsonConvert.DeserializeObject<RegisterVM>((string)TempData["RegisterVM"]);
        model.BusinessTypes = new List<BusinessType>(); // todo veritabanindan business tiplerini cek
        model.Cities = new List<City>(); // todo veritabanindan sehir listesini cek
        model.Districts = new List<District>(); // todo veritaninindan districtleri cek
        return View(model);
    }

    [HttpPost]
    public IActionResult RegisterSecond(RegisterVM model)
    {
        var secondStepValidator = new RegisterSecondStep();
        var validationResult = secondStepValidator.Validate(model);
        if (!validationResult.IsValid)
        {
            // todo
            return View();
        }

        return RedirectToAction("Index", "Home");
    }
}