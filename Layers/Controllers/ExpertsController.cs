using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class ExpertController : Controller
{
    private readonly IExpertService _expertService;
    private readonly IExpertRequestService _expertRequestService;
    private readonly ISelectorsService _selectorsService;
    private readonly IUpdateService _updateService;
    private readonly IMapper _mapper;
    private readonly IValidator<ExpertRequestFormMain> _expertFormValidator;

    private readonly List<RoleVM> _rolesList;
    private readonly List<BusinessTypeVM> _businessTypesList;
    private readonly List<CityVM> _cityList;

    public ExpertController
    (
        IMapper mapper,
        IExpertService expertService,
        IExpertRequestService expertRequestService,
        ISelectorsService selectorsService,
        IValidator<ExpertRequestFormMain> expertFormValidator,
        IUpdateService updateService
    )
    {
        _expertService = expertService;
        _expertRequestService = expertRequestService;
        _selectorsService = selectorsService;
        _updateService = updateService;

        _mapper = mapper;
        _expertFormValidator = expertFormValidator;

        _rolesList = _mapper.Map<List<RoleVM>>(_selectorsService.GetRolesList().Result.ToList());
        _businessTypesList = _mapper.Map<List<BusinessTypeVM>>(_selectorsService.GetBusinessTypesList().Result.ToList());
        _cityList = _mapper.Map<List<CityVM>>(_selectorsService.GetCitiesList().Result.ToList());
    }


    public async Task<IActionResult> Index()
    {
        var dtoModel = await _expertService.GetExpertCategoriesAsync();

        return View(_mapper.Map<List<ExpertCategoryVM>>(dtoModel));
    }

    public async Task<IActionResult> Form(int expertCategoryId)
    {
        var user = JsonConvert.DeserializeObject<UserVM>(HttpContext.Session.GetString("UserVM"));
        var business = JsonConvert.DeserializeObject<BusinessVM>(HttpContext.Session.GetString("BusinessVM"));
        var model = new ExpertRequestFormMain();

        model.ExpertCategoryId = expertCategoryId;

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

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Form(ExpertRequestFormMain model)
    {
        var user = JsonConvert.DeserializeObject<UserVM>(HttpContext.Session.GetString("UserVM"));
        var business = JsonConvert.DeserializeObject<BusinessVM>(HttpContext.Session.GetString("BusinessVM"));

        var validationResult = _expertFormValidator.Validate(model);

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

        var expertRequestResult = await _expertRequestService.AddExpertRequestAsync(new ExpertRequestDTO()
        {
            UserId = user.UserId,
            BusinessId = business.BusinessId,
            ExpertCategoryId = model.ExpertCategoryId,
            Description = model.Message,
        });

        if (businessId > 0)
        {
            user.BusinessId = businessId; // gelen business id, user ve business'a eklenir
            business.BusinessId = businessId;
        }

        // Guncellenmis halini sessiona geri koy
        HttpContext.Session.SetString("UserVM", JsonConvert.SerializeObject(user));
        HttpContext.Session.SetString("BusinessVM", JsonConvert.SerializeObject(business));


        return RedirectToAction("Success"); // basariliysa sayfaya yonlendir
    }

    public IActionResult Success()
    {
        return View();
    }
}