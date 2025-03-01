using System.Net.Mail;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class ContactUsController : Controller
{
    private readonly IValidator<ContactFormVM> _validator;
    private readonly IContactUsService _contactUsService;
    private readonly ILoginService _loginService;

    public ContactUsController(IValidator<ContactFormVM> validator, IContactUsService contactUsService, ILoginService loginService)
    {
        _validator = validator;
        _contactUsService = contactUsService;
        _loginService = loginService;
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
            try
            {
                await SendEmail(model, user);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Success");
            }

            return RedirectToAction("Success");
        }
        return View(model);
    }

    public IActionResult Success()
    {
        return View();
    }

    private async Task SendEmail(ContactFormVM model, UserVM user)
    {

        var business = await _loginService.FindBusinessById(user.BusinessId); // burda bir hata yok. 

        // template ayri bir dosayada almak yerine direk burda olusturulabilir
        string template = $@" 
            <html>
            <body>
                <h2>Yeni Mesaj</h2>
                <p><strong>Ad:</strong> {user.Name}</p>
                <p><strong>Soyad:</strong> {user.Surname}</p>
                <p><strong>Şirket:</strong> {business.BusinessName}</p>
                <p><strong>Telefon:</strong> {user.PhoneNumber}</p>
                <p><strong>Mesaj:</strong> {model.Message}</p>
            </body>
            </html>";

        // Send Email 
        using (SmtpClient cli = new SmtpClient("smtp.gmail.com", 587)) //gmail protu 587 olmalu
        {

            cli.EnableSsl = true;
            cli.UseDefaultCredentials = false;
            cli.DeliveryMethod = SmtpDeliveryMethod.Network;
            cli.Credentials = new System.Net.NetworkCredential("developer@esnafagelir.com", "wbwskowbzfctaggo"); // gonderici hesaba ait giris bilgileri env olarak atanmali
            MailMessage message = new()
            {
                Subject = $"Esnafa Gelir {user.Name} kullanıcısı mesaj gönderdi",
                Body = template,
                From = new MailAddress("developer@esnafagelir.com"),
                IsBodyHtml = true,
            };

            // alicilar
            message.To.Add("emrahelis@gmail.com");
            message.To.Add("bilgi@esnafagelir.com");
            message.To.Add("orhunozbalkan@gmail.com");
            message.To.Add("ismerd73@hotmail.com");

            await cli.SendMailAsync(message); // send yerine asenkron olani tercih edilmeli
        }
    }
}