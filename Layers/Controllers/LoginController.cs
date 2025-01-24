using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
{
    private readonly ILoginService _loginService;
    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(Login model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        return RedirectToAction("RequestDetail");
    }
    public IActionResult LegalNotice()
    {
        return View();
    }

    public IActionResult RequestDetail()
    {
        return View();
    }
}