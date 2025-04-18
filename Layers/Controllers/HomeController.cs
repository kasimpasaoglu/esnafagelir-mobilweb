using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using esnafagelir_mobilweb.Models;
using Newtonsoft.Json;
using AutoMapper;
using FluentValidation;
using System.Threading.Tasks;

namespace esnafagelir_mobilweb.Controllers;

public class HomeController : Controller
{
    private readonly ISelectorsService _selectorsService;
    private readonly IUpdateService _updateService;
    private readonly IOpportunitiesService _opportunitiesService;

    private readonly IMapper _mapper;
    private readonly IValidator<MyProfileVM> _updateValidator;

    private readonly List<RoleVM> _rolesList;
    private readonly List<BusinessTypeVM> _businessTypesList;
    private readonly List<CityVM> _cityList;
    private readonly List<DistrictVM> _districtList;

    public HomeController(
        ISelectorsService selectorsService,
        IMapper mapper,
        IValidator<MyProfileVM> updateValidator,
        IUpdateService updateService,
        IOpportunitiesService opportunitiesService
        )
    {
        _selectorsService = selectorsService;
        _updateService = updateService;
        _opportunitiesService = opportunitiesService;
        _mapper = mapper;
        _updateValidator = updateValidator;

        _rolesList = _mapper.Map<List<RoleVM>>(_selectorsService.GetRolesList().Result.ToList());
        _businessTypesList = _mapper.Map<List<BusinessTypeVM>>(_selectorsService.GetBusinessTypesList().Result.ToList());
        _cityList = _mapper.Map<List<CityVM>>(_selectorsService.GetCitiesList().Result.ToList());
    }

    public IActionResult Index()
    {
        var userString = HttpContext.Session.GetString("UserVM");
        if (string.IsNullOrEmpty(userString))
        {
            return RedirectToAction("Index", "Login");
        }
        var user = JsonConvert.DeserializeObject<UserVM>(userString);


        var cardsDMO = _opportunitiesService.GetPrimaryOpportunitiesAsync().Result.OrderByDescending(x => x.CreatedDate).Take(5).ToList();
        var cards = _mapper.Map<List<OpportunityVM>>(cardsDMO);


        var indexVm = new HomeIndexVM()
        {
            User = user,
            MainCards = cards,
        };

        return View(indexVm);
    }


    public async Task<IActionResult> MyProfile()
    {
        var userString = HttpContext.Session.GetString("UserVM");
        if (string.IsNullOrEmpty(userString))
        {
            return RedirectToAction("Index", "Login");
        }

        var user = JsonConvert.DeserializeObject<UserVM>(userString); // user

        var businessString = HttpContext.Session.GetString("BusinessVM");
        if (string.IsNullOrEmpty(businessString))
        {
            return RedirectToAction("Index", "Login");
        }
        var business = JsonConvert.DeserializeObject<BusinessVM>(businessString); // business

        var selectedDisrictId = business.DistrictId;
        var selectedCityId = await _selectorsService.GetCityIdByDisrictId(selectedDisrictId);

        var disrictList = await _selectorsService.GetDistrictsByCityId(selectedCityId);

        var model = new MyProfileVM
        {
            User = user,
            Business = business,
            Roles = _rolesList,
            BusinessTypes = _businessTypesList,
            Cities = _cityList,
            Districts = _mapper.Map<List<DistrictVM>>(disrictList),
            SelectedCityId = selectedCityId,
            SelectedDisrictId = selectedDisrictId,
            SelectedRoleId = user.RoleId,
            SelectedBusinessTypeId = business.BusinessTypeId,
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> MyProfile(MyProfileVM model)
    {
        var userString = HttpContext.Session.GetString("UserVM");
        if (string.IsNullOrEmpty(userString))
        {
            return RedirectToAction("Index", "Login");
        }
        var user = JsonConvert.DeserializeObject<UserVM>(userString); // user

        var businessString = HttpContext.Session.GetString("BusinessVM");
        if (string.IsNullOrEmpty(businessString))
        {
            return RedirectToAction("Index", "Login");
        }
        var business = JsonConvert.DeserializeObject<BusinessVM>(businessString); // business


        var validationResult = _updateValidator.Validate(model);

        if (!validationResult.IsValid)
        {
            model.Roles = _rolesList;

            model.BusinessTypes = _businessTypesList;

            model.Cities = _cityList;

            model.Districts = _mapper.Map<List<DistrictVM>>(await _selectorsService.GetDistrictsByCityId(model.SelectedCityId));

            return View(model);
        }

        #region  Validasyon basariliysa formdan gelen model icindeki degerleri, sessiondan alinan user ve business modellerinin icine yerlestir
        user.Name = model.User.Name;
        user.Surname = model.User.Surname;
        user.PhoneNumber = model.User.PhoneNumber;
        user.RoleId = model.SelectedRoleId;

        business.BusinessName = model.Business.BusinessName;
        business.BusinessTypeId = model.SelectedBusinessTypeId;
        business.Address = model.Business.Address;
        business.DistrictId = model.SelectedDisrictId;
        #endregion

        var userDTO = _mapper.Map<UserDTO>(user);
        var businessDto = _mapper.Map<BusinessDTO>(business);
        var businessId = await _updateService.UpdateUserAndBussines(userDTO, businessDto); // guncelleme basariliysa businessId 0 dan buyuk donmeli?

        if (businessId > 0)
        {
            user.BusinessId = businessId; // gelen business id user ve business'a eklenir
            business.BusinessId = businessId;
            model.IsUpdatedSuccesfully = true; //ekranda guncelleme basarili metni gelmesi icin
        }


        #region son halini modele koy
        model.User = user;
        model.Business = business;

        model.Roles = _rolesList;
        model.SelectedRoleId = user.RoleId;

        model.BusinessTypes = _businessTypesList;
        model.SelectedBusinessTypeId = business.BusinessTypeId;

        model.Cities = _cityList;
        model.SelectedCityId = await _selectorsService.GetCityIdByDisrictId(business.DistrictId);

        model.Districts = _mapper.Map<List<DistrictVM>>(await _selectorsService.GetDistrictsByCityId(model.SelectedCityId));
        model.SelectedDisrictId = business.DistrictId;
        #endregion

        // Guncellenmis halini sessiona geri koy
        HttpContext.Session.SetString("UserVM", JsonConvert.SerializeObject(user));
        HttpContext.Session.SetString("BusinessVM", JsonConvert.SerializeObject(business));

        return View(model);
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAllowsCoop([FromQuery] int businessId, [FromQuery] bool allowsCoop)
    {
        var result = await _updateService.UpdateCoopDecision(businessId, allowsCoop);
        if (result > 0)
        {
            var bussinesVm = JsonConvert.DeserializeObject<BusinessVM>(HttpContext.Session.GetString("BusinessVM"));
            bussinesVm.AllowsCooperation = allowsCoop;
            await _updateService.UpdateCoopDecision(bussinesVm.BusinessId, bussinesVm.AllowsCooperation);
            HttpContext.Session.SetString("BusinessVM", JsonConvert.SerializeObject(bussinesVm));
            return Ok();
        }
        else return BadRequest("Güncelleme sırasında bir hata oluştu.");
    }
}
