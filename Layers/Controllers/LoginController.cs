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
    // private CookieOptions CookieOptions { get; set; } = new CookieOptions
    // {
    //     Expires = DateTime.Now.AddDays(90),
    //     HttpOnly = true,
    //     Secure = false,
    //     SameSite = SameSiteMode.Strict
    // };

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
        if (deviceId == null)
        {
            return NotFound(); // device id yoksa girisi engelle
        }

        var user = await _loginService.FindByDeviceId(deviceId); // user'i bulmaya calis

        if (user == null || user.DeviceId != deviceId) // kullanici bulunamazsa ve ya device id degismisse
        {
            HttpContext.Session.SetString("DeviceID", deviceId);
            return View(); // giris yapma ekraninda kal tekrar login yap
        }

        var daysSinceRegister = (DateTime.Now - user.RegisterDate).TotalDays;
        if (daysSinceRegister % 90 == 0) // kullanicinin kayit tarihinden itibaren 90 gun gecmisse (90-180... vs)
        {
            HttpContext.Session.SetString("DeviceID", deviceId);
            return View(); // tekrar login iste
        }


        // device id'den useri bulduysak...
        var userVM = _mapper.Map<UserVM>(user);
        await _loginService.UpdateLastLoginDate(user);
        var businessVm = _mapper.Map<BusinessVM>(await _loginService.FindBusinessById(userVM.BusinessId)); // business bilginisi sorgula

        HttpContext.Session.SetString("UserVM", JsonConvert.SerializeObject(userVM)); //  user bilgisini sessiona at
        HttpContext.Session.SetString("BusinessVM", JsonConvert.SerializeObject(businessVm)); // business bilgisini sessiona bas

        return RedirectToAction("RequestDetail");
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginVM model)
    {   // kullanici bu formu gonderiyorsa iki ihtimal var
        // ya ilk kez giris yapiyor
        // ya da device id degismis

        var deviceId = HttpContext.Session.GetString("DeviceID");

        var validationResult = _loginValidator.Validate(model);
        if (!validationResult.IsValid)
        {
            return View(model); // validasyon basarisiz
        }

        var user = new UserVM
        {
            DeviceId = deviceId,
            PhoneNumber = model.PhoneNumber,
            IsPrivacyPolicyAccepted = model.IsPrivacyPolicyAccepted,
        };

        // kayit islemini iki durum icinde service katmaninda cozecek
        var registeredUserDTO = await _registerService.SignInWithPhoneNumber(_mapper.Map<UserDTO>(user)); // kayit isleminden sonra gelen modelde userId bilgisi oldugu icin sessiona bu model basilacak
        if (registeredUserDTO == null)
        {
            ModelState.AddModelError(string.Empty, "Kayıt sırasında bir hata oluştu. Lütfen tekrar deneyiniz."); // TODO bunu ekranda goster
            return View(model);
        }

        var registeredUserVM = _mapper.Map<UserVM>(registeredUserDTO);
        var businessVm = _mapper.Map<BusinessVM>(await _loginService.FindBusinessById(registeredUserVM.BusinessId)); // business bilginisi sorgula

        HttpContext.Session.SetString("UserVM", JsonConvert.SerializeObject(registeredUserVM)); //  user bilgisini sessiona at
        HttpContext.Session.SetString("BusinessVM", JsonConvert.SerializeObject(businessVm)); // business bilgisini sessiona bas
        HttpContext.Session.Remove("DeviceID"); // device id'yi sil
        return RedirectToAction("RequestDetail");

    }
    public IActionResult LegalNotice()
    {
        return Redirect("https://www.esnafagelir.com/kvkk");
    }

    public IActionResult RequestDetail()
    {
        var userVmJson = HttpContext.Session.GetString("UserVM"); // sessiondan kullanci bilgilerini al
        if (string.IsNullOrEmpty(userVmJson))
        {
            return RedirectToAction("Index");
        }

        var userVM = JsonConvert.DeserializeObject<UserVM>(userVmJson);
        var daysSinceRegister = (DateTime.Now - userVM.RegisterDate).TotalDays;
        var minsSinceRegister = (DateTime.Now - userVM.RegisterDate).TotalMinutes;

        /// bu kontrolde sorun var, 
        /// gun farkini TotalDays olarak almazsam 0-14-28... gunlderde sgun boyu her loginde ekrani goruyor
        /// totaldays olarak alirsam, kullanicinin tam olarak kayit oldugu saat ve dakikada 14 gunde bir, bir kere ekrani goruyor.
        /// daha dogru bir cozum lazim
        if (minsSinceRegister > 1 && daysSinceRegister % 14 != 0)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }
}