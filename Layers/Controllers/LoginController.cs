using System.Threading.Tasks;
using AutoMapper;
using esnafagelir_mobilweb.DMO;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


public class LoginController : Controller
{
    private readonly ILoginService _loginService;
    private readonly IValidator<LoginVM> _loginValidator;
    private readonly IMapper _mapper;
    private CookieOptions CookieOptions { get; set; } = new CookieOptions
    {
        Expires = DateTime.Now.AddDays(90),
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict
    };

    public LoginController(ILoginService loginService, IValidator<LoginVM> loginValidator, IMapper mapper)
    {
        _loginValidator = loginValidator;
        _loginService = loginService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {

        var phoneNumber = Request.Cookies["PhoneNumber"];
        if (phoneNumber == null)
        {
            return View(new LoginVM()); // telefon numarasi cookie bulunamadi giris yap!
        }

        // Kullanıcı bilgilerini getir
        var userDTO = await _loginService.FindByPhoneNumber(phoneNumber);
        if (userDTO == null)
        {
            return View(new LoginVM()); // telefon numarasi db'de yok giris yap!
        }

        var userVM = _mapper.Map<UserVM>(userDTO); //vm'e donusum


        var deviceId = Request.Cookies["DeviceId"];
        if (deviceId == null || deviceId != userVM.DeviceId.ToString())
        {
            return View(new LoginVM()); // device degismis ve ya yok, giris yap!
        }

        await _loginService.UpdateLastLoginDate(userDTO); // LastLogin tarihi guncellensin TODO Hata olursa ekrana goster???
        HttpContext.Session.SetString("userVm", JsonConvert.SerializeObject(userVM));
        return RedirectToAction("RequestDetail");
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginVM model)
    {
        var validationResult = _loginValidator.Validate(model);
        if (!validationResult.IsValid)
        {
            return View(model); // validasyon basarisiz
        }

        // kullanici kimligi olusturma 
        var user = new UserVM
        {
            IsPrivacyPolicyAccepted = model.IsPrivacyPolicyAccepted,
            PhoneNumber = model.PhoneNumber,
        };
        var registeredUserDTO = await _loginService.Register(_mapper.Map<UserDTO>(user)); // kayit denemesi...
        if (registeredUserDTO != null)
        {
            var newUser = _mapper.Map<UserVM>(registeredUserDTO);
            // kayit basarili cookielere detaylari koy
            Response.Cookies.Append("PhoneNumber", newUser.PhoneNumber, CookieOptions);
            Response.Cookies.Append("DeviceId", newUser.DeviceId.ToString(), CookieOptions);
            HttpContext.Session.SetString("userVm", JsonConvert.SerializeObject(newUser));
            return RedirectToAction("RequestDetail");
        }
        ModelState.AddModelError(string.Empty, "Kayıt sırasında bir hata oluştu. Lütfen tekrar deneyiniz."); // TODO bunu ekranda goster
        return View(model); // giris basarisiz,
    }
    public IActionResult LegalNotice()
    {
        return View();
    }

    public IActionResult RequestDetail()
    {
        var userVmJson = HttpContext.Session.GetString("userVm"); // sessiondan kullanci bilgilerini al
        if (string.IsNullOrEmpty(userVmJson))
        {
            // oturum yoksa giris ekranina geri don
            return RedirectToAction("Index");
        }

        var userVM = JsonConvert.DeserializeObject<UserVM>(userVmJson);
        var daysSinceRegister = (DateTime.Now - userVM.RegisterDate).Days;
        var minsSinceRegister = (DateTime.Now - userVM.RegisterDate).Minutes;

        if (minsSinceRegister > 1 && daysSinceRegister % 14 != 0)
        {   // eger kullanici kayit yapali 1.dk dan fazla olduysa
            // ve eger kayit tarihinden itibaren 14 ve 14. kati gun gecmediyse
            // ana sayfaya gidecek
            return RedirectToAction("Index", "Home");
        }

        // 1 dk dan az ve 14. ve kati gunlerde bu sayfada kalacak
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