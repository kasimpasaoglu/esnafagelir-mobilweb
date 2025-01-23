using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
{

    public LoginController()
    {

    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult LegalNotice()
    {
        return View();
    }
}