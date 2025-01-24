using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using esnafagelir_mobilweb.Models;

namespace esnafagelir_mobilweb.Controllers;

public class HomeController : Controller
{

    public HomeController()
    {

    }

    public IActionResult Index()
    {
        return View();
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
