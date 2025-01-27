using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Razor.Templating.Core;

public class OpportunitiesController : Controller
{
    public IActionResult Index()
    {
        var list = CMS.FirsatlarKartlari().ToList();
        var model = new OpportunitiesIndexVM
        {
            PrimaryList = list.Where(x => x.IsPrimary && x.EndDate > DateTime.Now).OrderByDescending(x => x.ReleaseDate).Take(5).ToList(),
            SecondaryList = list.Where(x => !x.IsPrimary && x.EndDate > DateTime.Now).OrderByDescending(x => x.ReleaseDate).Take(5).ToList(),
        };
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetMoreOpportunities([FromQuery] int page)
    {
        int pageSize = 5;
        var items = CMS.FirsatlarKartlari()
                       .Where(x => !x.IsPrimary && x.EndDate > DateTime.Now)
                       .OrderByDescending(x => x.ReleaseDate)
                       .Skip((page - 1) * pageSize)
                       .Take(pageSize)
                       .ToList();

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