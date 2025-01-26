using System.Threading.Tasks;
using AutoMapper;
using esnafagelir_mobilweb.DMO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class RegisterController : Controller
{
    private readonly IRegisterService _registerService;
    private readonly ISelectorsService _selectorsService;
    private readonly IMapper _mapper;

    private readonly List<RoleVM> _rolesList;
    private readonly List<BusinessTypeVM> _businessTypesList;
    private readonly List<CityVM> _cityList;
    public RegisterController
    (
        IRegisterService registerService,
        ISelectorsService selectorsService,
        IMapper mapper
    )
    {
        _registerService = registerService;
        _selectorsService = selectorsService;
        _mapper = mapper;

        _rolesList = _mapper.Map<List<RoleVM>>(_selectorsService.GetRolesList().Result.ToList());
        _businessTypesList = _mapper.Map<List<BusinessTypeVM>>(_selectorsService.GetBusinessTypesList().Result.ToList());
        _cityList = _mapper.Map<List<CityVM>>(_selectorsService.GetCitiesList().Result.ToList());

    }
    public IActionResult First()
    {
        var sessionModel = HttpContext.Session.GetString("userVm");
        if (string.IsNullOrEmpty(sessionModel)) return RedirectToAction("Index", "Login");
        var user = JsonConvert.DeserializeObject<UserVM>(sessionModel);

        var model = new RegisterVM
        {
            User = user,
            Roles = _rolesList,
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult First(RegisterVM model)
    {
        // ASP.NET Core'un eklediği hatayı temizle
        ModelState.Remove("SelectedRoleId");

        var firstStepValidator = new RegisterFirstStep();
        var validation = firstStepValidator.Validate(model);

        if (!validation.IsValid)
        {
            foreach (var error in validation.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            model.Roles = _rolesList;
            return View(model);
        }

        // validasyondan gecerse
        var sessionModel = HttpContext.Session.GetString("userVm"); // sessiondaki modeli cek
        if (string.IsNullOrEmpty(sessionModel)) return RedirectToAction("Index", "Login"); // bu noktada sessionda model yoksa biseyler cok yanlis gidiyor demektir 
        var sessionUser = JsonConvert.DeserializeObject<UserVM>(sessionModel); // deserialize islemi
        // yeni eklenen bilgileri modele ekle
        sessionUser.Name = model.User.Name;
        sessionUser.Surname = model.User.Surname;
        sessionUser.RoleId = model.SelectedRoleId;
        // modeli tekrar sessiona gonder
        HttpContext.Session.SetString("userVm", JsonConvert.SerializeObject(sessionUser));
        return RedirectToAction("Second"); // bir sonraki adima gec
    }

    public async Task<IActionResult> Second()
    {
        var registerVmData = TempData["RegisterVM"] as string;

        if (string.IsNullOrEmpty(registerVmData))
        {
            return RedirectToAction("First"); // TempData'da RegisterVM yoksa First aksiyonuna yönlendir
        }

        var model = JsonConvert.DeserializeObject<RegisterVM>(registerVmData);
        // var model = new RegisterVM();
        model.BusinessTypes = _businessTypesList;
        model.Cities = _cityList;
        model.Districts = new List<DistrictVM>();//bos liste gidecek, listeyi on tarafda JS ile olustur
        return View(model);
    }

    [HttpPost]
    public IActionResult Second(RegisterVM model)
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

    // district listesini on tarafta js ile olusturmak icin bu endpointe gelinecek
    [HttpGet]
    public async Task<IActionResult> GetDistrictsByCityId(int cityId)
    {
        var districtsDTO = await _selectorsService.GetDistrictsByCityId(cityId);
        return Json(_mapper.Map<List<DistrictVM>>(districtsDTO)); // JSON formatında döndür
    }

}