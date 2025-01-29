using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class ExpertController : Controller
{
    private IExpertService _expertService;
    IMapper _mapper;
    public ExpertController(IMapper mapper, IExpertService expertService)
    {
        _expertService = expertService;
        _mapper = mapper;
    }
    public async Task<IActionResult> Index()
    {
        var dtoModel = await _expertService.GetExpertCategoriesAsync();

        return View(_mapper.Map<List<ExpertCategoryVM>>(dtoModel));
    }

    public IActionResult Form(int CategoryId)
    {
        var user = JsonConvert.DeserializeObject<UserVM>(HttpContext.Session.GetString("UserVM"));
        var business = JsonConvert.DeserializeObject<BusinessVM>(HttpContext.Session.GetString("BusinessVM"));
        return View();
    }

    [HttpPost]
    public IActionResult Form()
    {
        return View();
    }
}