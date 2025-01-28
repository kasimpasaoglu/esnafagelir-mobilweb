using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class ContactUsController : Controller
{
    private readonly IValidator<ContactFormVM> _validator;
    private readonly IContactUsService _contactUsService;

    public ContactUsController(IValidator<ContactFormVM> validator, IContactUsService contactUsService)
    {
        _validator = validator;
        _contactUsService = contactUsService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(ContactFormVM model)
    {
        var validationResult = _validator.Validate(model);
        if (!validationResult.IsValid)
        {
            return View(model);
        }

        var userString = HttpContext.Session.GetString("UserVM");
        var user = JsonConvert.DeserializeObject<UserVM>(userString);

        var result = await _contactUsService.Put(user.UserId, model.Message);
        if (result > 0)
        {
            return RedirectToAction("Success");
        }
        return View(model);
    }

    public IActionResult Success()
    {
        return View();
    }
}