using System.Threading.Tasks;
using AutoMapper;
using esnafagelir_mobilweb.DMO;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class RegisterController : Controller
{
    private readonly IRegisterService _registerService;
    private readonly ISelectorsService _selectorsService;
    private readonly IValidator<RegisterFirstVM> _firstStepValidator;
    private readonly IValidator<RegisterSecondVM> _secondStepValidator;
    private readonly IMapper _mapper;

    private readonly List<RoleVM> _rolesList;
    private readonly List<BusinessTypeVM> _businessTypesList;
    private readonly List<CityVM> _cityList;
    public RegisterController
    (
        IRegisterService registerService,
        ISelectorsService selectorsService,
        IValidator<RegisterFirstVM> firstStepValidator,
        IValidator<RegisterSecondVM> secondStepValidator,
        IMapper mapper
    )
    {
        _registerService = registerService;
        _selectorsService = selectorsService;
        _firstStepValidator = firstStepValidator;
        _secondStepValidator = secondStepValidator;
        _mapper = mapper;


        // selector listeleri
        _rolesList = _mapper.Map<List<RoleVM>>(_selectorsService.GetRolesList().Result.ToList());
        _businessTypesList = _mapper.Map<List<BusinessTypeVM>>(_selectorsService.GetBusinessTypesList().Result.ToList());
        _cityList = _mapper.Map<List<CityVM>>(_selectorsService.GetCitiesList().Result.ToList());

    }
    public IActionResult First()
    {
        var sessionModel = HttpContext.Session.GetString("UserVM");
        if (string.IsNullOrEmpty(sessionModel)) return RedirectToAction("Index", "Login"); // sessionda kullanici yoksa login sayfasina geri gonder
        var user = JsonConvert.DeserializeObject<UserVM>(sessionModel);

        var model = new RegisterFirstVM
        {
            User = user,
            Roles = _rolesList,
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult First(RegisterFirstVM model)
    {
        var validation = _firstStepValidator.Validate(model);

        if (!validation.IsValid)
        {
            model.Roles = _rolesList;
            return View(model);
        }

        var sessionModel = HttpContext.Session.GetString("UserVM"); // sessiondaki modeli cek
        if (string.IsNullOrEmpty(sessionModel)) return RedirectToAction("Index", "Login"); // bu noktada sessionda model yoksa biseyler cok yanlis gidiyor demektir 
        var sessionUser = JsonConvert.DeserializeObject<UserVM>(sessionModel);
        // yeni eklenen bilgileri modele ekle
        sessionUser.Name = model.User.Name;
        sessionUser.Surname = model.User.Surname;
        sessionUser.RoleId = model.SelectedRoleId;
        // modeli tekrar sessiona gonder
        HttpContext.Session.SetString("UserVM", JsonConvert.SerializeObject(sessionUser));
        return RedirectToAction("Second"); // bir sonraki adima gec
    }

    public async Task<IActionResult> Second()
    {
        var stringModel = HttpContext.Session.GetString("UserVM");
        var user = JsonConvert.DeserializeObject<UserVM>(stringModel);

        var model = new RegisterSecondVM()
        {
            User = user,
            BusinessTypes = _businessTypesList,
            Cities = _cityList,
            Districts = new List<DistrictVM>(),//bos liste gidecek, listeyi on tarafda JS ile olusturuor
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Second(RegisterSecondVM model)
    {
        var stringModel = HttpContext.Session.GetString("UserVM");
        var user = JsonConvert.DeserializeObject<UserVM>(stringModel);
        model.User = user;

        var validationResult = _secondStepValidator.Validate(model);
        if (!validationResult.IsValid)
        {
            model.BusinessTypes = _businessTypesList;
            model.Cities = _cityList;
            model.Districts = new List<DistrictVM>();
            return View(model);
        }

        model.Business.BusinessTypeId = model.SelectedBusinessTypeId;
        model.Business.DistrictId = model.SelectedDisrictId;

        var businessToRegister = _mapper.Map<BusinessDTO>(model.Business);
        var userToRegister = _mapper.Map<UserDTO>(model.User);
        var registeredBusinessId = await _registerService.RegisterUserWithBusiness(userToRegister, businessToRegister); // user ve business registration islemi yapildiktan sonra businessId'yi almamiz lazim. O yuzden bu metod businessId'yi donmeli
        if (registeredBusinessId == 0)
        {
            ModelState.AddModelError(string.Empty, "Kayıt sırasında bir hata oluştu. Lütfen tekrar deneyiniz.");
            return View(model);
        }

        // gelen business id her iki entity'e eklenip sessiona atiliyor
        userToRegister.BusinessId = registeredBusinessId;
        businessToRegister.BusinessId = registeredBusinessId;
        HttpContext.Session.SetString("UserVM", JsonConvert.SerializeObject(userToRegister));
        HttpContext.Session.SetString("BusinessVM", JsonConvert.SerializeObject(businessToRegister));
        return RedirectToAction("Index", "Home");
    }

    // district listesini on tarafta js ile olusturmak icin bu endpointe gelinecek
    [HttpGet]
    public async Task<IActionResult> GetDistrictsByCityId(int cityId)
    {
        var districtsDTO = await _selectorsService.GetDistrictsByCityId(cityId);
        return Json(_mapper.Map<List<DistrictVM>>(districtsDTO)); // JSON formatında döndür
    }

}