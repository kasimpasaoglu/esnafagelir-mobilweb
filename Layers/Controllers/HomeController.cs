using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using esnafagelir_mobilweb.Models;
using Newtonsoft.Json;

namespace esnafagelir_mobilweb.Controllers;

public class HomeController : Controller
{

    public HomeController()
    {

    }

    public IActionResult Index()
    {
        var userString = HttpContext.Session.GetString("userVm");
        if (string.IsNullOrEmpty(userString))
        {
            return RedirectToAction("Index", "Login");
        }
        var user = JsonConvert.DeserializeObject<UserVM>(userString);
        List<CardMainModel> cards =
        [
            new CardMainModel { Title = "Brand Name" , Description = "Tüm alışverişlerde geçerli özel fırsatlar bu kampanyada", ImgUrl = "/images/home/maincardDemo.png"},
            new CardMainModel { Title = "Brand Name" , Description = "Tüm alışverişlerde geçerli esnaflara özel fırsatlar bu kampanyada", ImgUrl = "/images/home/maincardDemo.png"},
            new CardMainModel { Title = "Brand Name" , Description = "Tüm alışverişlerde geçerli özel fırsatlar bu kampanyada", ImgUrl = "/images/home/maincardDemo.png"},
        ];

        var indexVm = new HomeIndexVM()
        {
            User = user,
            MainCards = cards,
        };

        return View(indexVm);
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
