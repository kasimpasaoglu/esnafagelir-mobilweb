using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Razor.Templating.Core;

public class OpportunitiesController : Controller
{
    private readonly IOpportunitiesService _opportunitiesService;
    private readonly IMapper _mapper;

    public OpportunitiesController(IOpportunitiesService opportunitiesService, IMapper mapper)
    {
        _opportunitiesService = opportunitiesService;
        _mapper = mapper;
    }
    public async Task<IActionResult> Index()
    {
        var primaryListDMO = await _opportunitiesService.GetPrimaryOpportunitiesAsync();
        var primaryList = _mapper.Map<List<OpportunityVM>>(primaryListDMO);

        var secondaryListDMO = await _opportunitiesService.GetSecondaryOpportunitiesAsync();
        var secondaryList = _mapper.Map<List<OpportunityVM>>(secondaryListDMO);

        var model = new OpportunitiesIndexVM
        {
            PrimaryList = primaryList.OrderByDescending(x => x.CreatedDate).ToList(),
            SecondaryList = secondaryList.OrderByDescending(x => x.CreatedDate).Take(5).ToList(),
        };
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetMoreOpportunities([FromQuery] int page)
    {
        int pageSize = 5;
        var items = _opportunitiesService.GetSecondaryOpportunitiesAsync().Result
                       .OrderByDescending(x => x.CreatedDate)
                       .Skip((page - 1) * pageSize)
                       .Take(pageSize)
                       .ToList(); // bu filtreleme sorgu gonderilmeden once yapilmali, sorgu filtreye gore gonderilmeli, Su an butun liste geliyor her defasinda

        if (!items.Any())
        {
            return NoContent();
        }

        var renderedHtml = string.Join("", await Task.WhenAll(
            items.Select(async item => await RazorTemplateEngine.RenderAsync("_CardSecondary", item))
        ));

        return Content(renderedHtml, "text/html");
    }




}