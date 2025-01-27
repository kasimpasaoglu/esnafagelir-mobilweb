using System.Threading.Tasks;
using AutoMapper;
using esnafagelir_mobilweb.DMO;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


public class LoginController : Controller
{
    private readonly ILoginService _loginService;
    private readonly IRegisterService _registerService;
    private readonly IValidator<LoginVM> _loginValidator;
    private readonly IMapper _mapper;
    private CookieOptions CookieOptions { get; set; } = new CookieOptions
    {
        Expires = DateTime.Now.AddDays(90),
        HttpOnly = true,
        Secure = false,
        SameSite = SameSiteMode.Strict
    };

    public LoginController
    (
        ILoginService loginService,
        IValidator<LoginVM> loginValidator,
        IMapper mapper,
        IRegisterService registerService
    )
    {
        _loginValidator = loginValidator;
        _loginService = loginService;
        _registerService = registerService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index([FromQuery] string deviceId)
    {
        UserDTO user = new UserDTO();

        var deviceIdCookie = Request.Cookies["DeviceId"];

        if (deviceIdCookie == null)
        {
            var loginVM = new LoginVM() { DeviceId = deviceId };

            // deviceId cookie yoksa yeni kullanici ve ya yeni cihaz. oturum acmali
            return View(loginVM);
        }

        user = await _loginService.FindByDeviceId(deviceId);

        var phoneNumberCookie = Request.Cookies["PhoneNumber"];

        if (phoneNumberCookie == null || user?.PhoneNumber != phoneNumberCookie)
        {
            return View(new LoginVM()); // telefon numarasi cookie bulunamadi ve ya numara ile eslesmedi giris yap!
        }
        await _loginService.UpdateLastLoginDate(user); // LastLogin tarihi guncellensin TODO Hata olursa ekrana goster???
        var userVM = _mapper.Map<UserVM>(user);
        HttpContext.Session.SetString("userVm", JsonConvert.SerializeObject(userVM)); // user bilgisini sessiona bas

        var businessVm = _mapper.Map<BusinessVM>(await _loginService.FindBusinessById(userVM.BusinessId)); // business bilginisi sorgula
        HttpContext.Session.SetString("businessVm", JsonConvert.SerializeObject(businessVm)); // business bilgisini sessiona bas
        return RedirectToAction("RequestDetail");
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginVM model)
    {   // SORUN! :
        // kullanici farkli bir cihazdan oturum actiginda yine bu ekrana geliyor, ancak telefon numarasi girdikten sonra yeni bir hesap actiriyor,
        // COZOM : 
        // Alinan telefon numarasinin kaydini kontrol edip, registered user'sa direk ana sayfaya yonelndirilmeli.
        // SONRA BAKILACAK

        var validationResult = _loginValidator.Validate(model);
        if (!validationResult.IsValid)
        {
            return View(model); // validasyon basarisiz
        }

        // kullanici kimligi olusturma 
        var user = _mapper.Map<UserVM>(model);
        var registeredUserDTO = await _registerService.SignInWithPhoneNumber(_mapper.Map<UserDTO>(user)); // kayit denemesi...
        if (registeredUserDTO != null)
        {
            var registeredUserVM = _mapper.Map<UserVM>(registeredUserDTO);
            // kayit basarili cookielere detaylari koy
            Response.Cookies.Append("PhoneNumber", registeredUserVM.PhoneNumber, CookieOptions);
            Response.Cookies.Append("DeviceId", registeredUserVM.DeviceId.ToString(), CookieOptions);
            HttpContext.Session.SetString("userVm", JsonConvert.SerializeObject(registeredUserVM));
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
            return RedirectToAction("Index");
        }

        var userVM = JsonConvert.DeserializeObject<UserVM>(userVmJson);
        var daysSinceRegister = (DateTime.Now - userVM.RegisterDate).Days;
        var minsSinceRegister = (DateTime.Now - userVM.RegisterDate).TotalMinutes;

        if (minsSinceRegister > 1 && daysSinceRegister % 14 == 0)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }
}