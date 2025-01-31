using AutoMapper;
using esnafagelir_mobilweb.DMO;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IAdminLoginService _adminLoginService;
        private readonly IExpertService _expertService;
        private readonly IOpportunitiesService _opportunitiesService;
        private readonly IValidator<ExpertCategoryAdminModel> _expertValidator;
        private readonly IValidator<AdminVM> _adminLoginValidator;
        private readonly IValidator<OpportunityAdminModel> _opportunityValidator;
        private readonly IMapper _mapper;
        public AdminPanelController(
            IFileService fileService,
            IAdminLoginService adminLoginService,
            IExpertService expertService,
            IOpportunitiesService opportunitiesService,
            IValidator<ExpertCategoryAdminModel> expertValidator,
            IValidator<AdminVM> adminLoginValidator,
            IMapper mapper,
            IValidator<OpportunityAdminModel> opportunityValidator
            )
        {
            _fileService = fileService;
            _adminLoginService = adminLoginService;
            _expertService = expertService;
            _expertValidator = expertValidator;
            _adminLoginValidator = adminLoginValidator;
            _opportunitiesService = opportunitiesService;
            _mapper = mapper;
            _opportunityValidator = opportunityValidator;
        }


        #region Login Ekrani
        public async Task<IActionResult> Login([FromQuery] string DeviceId)
        {
            HttpContext.Session.SetString("DeviceId", DeviceId);
            return View(new AdminVM());
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminVM model)
        {
            var deviceId = HttpContext.Session.GetString("DeviceId");
            model.DeviceId = deviceId;
            try
            {
                await _adminLoginService.Login(model);
                model.UserPassword = ""; // guvenlik icin sifre alanlarini temizle
                model.ReUserPassword = "";
                HttpContext.Session.SetString("Admin", JsonConvert.SerializeObject(model)); // modeli sessiona at
                return RedirectToAction("Index"); // admin paneline yonlendir
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }
            return View(model);
        }
        #endregion

        #region Admin Logini guncelleme ekrani
        public IActionResult UpdateAdmin()
        {
            var admin = JsonConvert.DeserializeObject<AdminVM>(HttpContext.Session.GetString("Admin"));
            if (admin == null)
            {
                return RedirectToAction("Login");
            }
            return View(admin);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAdmin(AdminVM model)
        {
            var validationResult = _adminLoginValidator.Validate(model);
            if (!validationResult.IsValid)
            {
                return View(model);
            }

            string hashedPassword = ShaHelper.HashPassword(model.UserPassword, out string salt);

            model.UserPassword = hashedPassword;
            model.Salt = salt;
            model.DeviceId = HttpContext.Session.GetString("DeviceId");

            try
            {
                await _adminLoginService.UpdateAdminAsync(_mapper.Map<AdminDTO>(model));
                HttpContext.Session.SetString("Admin", JsonConvert.SerializeObject(model));
                HttpContext.Session.Remove("DeviceId");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }
            return View(model);
        }
        #endregion

        #region Admin Kayit Ekrani (Sadece bir kere kullanilabilir, Sistemde kayitli bir admin varsa bu ekran acilmaz, deviceId olmadan acilmaz)
        public async Task<IActionResult> AddAdmin([FromQuery] string deviceId)
        {
            if (deviceId == null) return NotFound(); // device id yok, giris yapilamaz

            var adminList = await _adminLoginService.GetAdminsAsync();
            if (adminList.Count > 0) return NotFound(); // sistemde kayitli admin var, bu ekrani acilamaz

            HttpContext.Session.SetString("DeviceId", deviceId);
            return View(new AdminVM());
        }
        [HttpPost]
        public async Task<IActionResult> AddAdmin(AdminVM model)
        {
            var validationResult = _adminLoginValidator.Validate(model);
            if (!validationResult.IsValid)
            {
                return View(model);
            }

            string hashedPassword = ShaHelper.HashPassword(model.UserPassword, out string salt);

            model.UserPassword = hashedPassword;
            model.Salt = salt;
            model.DeviceId = HttpContext.Session.GetString("DeviceId");

            try
            {
                await _adminLoginService.CreateAdminAsync(_mapper.Map<AdminDTO>(model));
                HttpContext.Session.Remove("DeviceId");
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }
            return View(model);
        }
        #endregion

        #region ana menu
        public IActionResult Index()
        {
            var admin = HttpContext.Session.GetString("Admin");
            if (admin == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        #endregion

        #region Uzmanlik Kategorileri Ekleme Ekrani
        public async Task<IActionResult> AddExpert()
        {
            var admin = HttpContext.Session.GetString("Admin");
            if (admin == null)
            {
                return RedirectToAction("Login");
            }
            var dtoList = await _expertService.GetExpertCategoriesAsync();
            var vmList = _mapper.Map<List<ExpertCategoryVM>>(dtoList);
            var model = new ExpertCategoryAdminPage { ExistingCategoriesList = vmList };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddExpert(ExpertCategoryAdminPage model)
        {
            var validationResult = _expertValidator.Validate(model.NewCategory);
            if (!validationResult.IsValid)
            {
                // nested bir model oldugu icin validasyon mesajlarini elle doldurmak gerekiyor
                ModelState.Clear();
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError($"NewCategory.{error.PropertyName}", error.ErrorMessage);
                }
                model.ExistingCategoriesList = _mapper.Map<List<ExpertCategoryVM>>(await _expertService.GetExpertCategoriesAsync());
                return View(model);
            }
            try
            {
                string imagePath = await _fileService.UploadFileAsync(model.NewCategory.ImageFile, "expertCategories");
                // maplemeyi bilerek elle yapiyorum hata ihtimalini sifirlamak icin
                var newExpertCategory = new ExpertCategoryVM
                {
                    CategoryName = model.NewCategory.CategoryName,
                    CategoryDescription = model.NewCategory.CategoryDescription,
                    ImagePath = imagePath,
                };
                var result = await _expertService.AddExpertCategoryAsync(_mapper.Map<ExpertCategoryDTO>(newExpertCategory));
                if (result <= 0)
                {
                    throw new InvalidOperationException("Kayit Basarisiz");
                }

            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Hata: {ex.Message}";
                model.ExistingCategoriesList = _mapper.Map<List<ExpertCategoryVM>>(await _expertService.GetExpertCategoriesAsync());
                return View(model);
            }
            model.NewCategory.CategoryName = null;
            model.NewCategory.CategoryDescription = null;
            model.NewCategory.ImageFile = null;
            model.ExistingCategoriesList = _mapper.Map<List<ExpertCategoryVM>>(await _expertService.GetExpertCategoriesAsync());
            TempData["Message"] = "Dosya başarıyla yüklendi";
            return View(model);
        }
        #endregion

        #region Uzmanlik kategorileri silme ekrani
        public async Task<IActionResult> RemoveExpert()
        {
            var admin = HttpContext.Session.GetString("Admin");
            if (admin == null)
            {
                return RedirectToAction("Login");
            }
            var dtoList = await _expertService.GetExpertCategoriesAsync();
            var vmList = _mapper.Map<List<ExpertCategoryVM>>(dtoList);
            var model = new ExpertCategoryAdminPage { ExistingCategoriesList = vmList };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveExpert(ExpertCategoryAdminPage model)
        {
            if (model.SelectedCategoryIds == null)
            {
                var dtoList = await _expertService.GetExpertCategoriesAsync();
                var vmList = _mapper.Map<List<ExpertCategoryVM>>(dtoList);
                model.ExistingCategoriesList = vmList;
                TempData["Message"] = "Silmek için en az bir kategori seçmelisiniz.";
                return View(model);
            }

            var selectedCategories = await _expertService.GetCategoriesByIds(model.SelectedCategoryIds);
            var imagePaths = selectedCategories.Select(c => c.ImagePath).Where(x => !string.IsNullOrEmpty(x)).ToList();

            // veritabanindan sil
            await _expertService.RemoveCategory(model.SelectedCategoryIds);

            // dosyalari sil
            foreach (var path in imagePaths)
            {
                await _fileService.DeleteFileAsync(path);
            }

            // guncellenmis kategori listesini getir
            model.ExistingCategoriesList = _mapper.Map<List<ExpertCategoryVM>>(await _expertService.GetExpertCategoriesAsync());
            return View(model);
        }
        #endregion

        #region Firsat Ekleme Ekrani
        public IActionResult AddOpportunity()
        {
            var admin = HttpContext.Session.GetString("Admin");
            if (admin == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddOpportunity(OpportunitiesAdminPage model)
        {
            var validationResult = _opportunityValidator.Validate(model.NewOpportunity);

            if (!validationResult.IsValid)
            {
                // nested bir model oldugu icin validasyon mesajlarini elle doldurmak gerekiyor
                ModelState.Clear();
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError($"NewOpportunity.{error.PropertyName}", error.ErrorMessage);
                }
                return View(model);
            }
            try
            {
                string imagePath = await _fileService.UploadFileAsync(model.NewOpportunity.ImageFile, "opportunities");
                // maplemeyi bilerek elle yapiyorum hata ihtimalini sifirlamak icin
                var newOpportunity = new OpportunityVM
                {
                    Title = model.NewOpportunity.Title,
                    Description = model.NewOpportunity.Description,
                    ImagePath = imagePath,
                    Url = model.NewOpportunity.Url,
                    CreatedDate = DateTime.Now,
                    EndDate = model.NewOpportunity.EndDate,
                    IsPrimary = model.NewOpportunity.IsPrimary,
                };
                var result = await _opportunitiesService.AddOpportunityAsync(_mapper.Map<OpportunityDTO>(newOpportunity));
                if (result <= 0)
                {
                    throw new InvalidOperationException("Kayit Basarisiz");
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Hata: {ex.Message}";
                return View(model);
            }
            TempData["Message"] = "Dosya başarıyla yüklendi";
            return View(model);
        }
        #endregion

        #region Firsat Silme Ekrani
        public async Task<IActionResult> RemoveOpportunity()
        {
            var admin = HttpContext.Session.GetString("Admin");
            if (admin == null)
            {
                return RedirectToAction("Login");
            }
            List<OpportunityVM> allOpportunities = _mapper.Map<List<OpportunityVM>>(await _opportunitiesService.GetAllOpportunitiesAsync());

            return View(new OpportunitiesAdminPage
            {
                ExistingOpportunities = allOpportunities,
            });
        }
        [HttpPost]
        public async Task<IActionResult> RemoveOpportunity(OpportunitiesAdminPage model)
        {
            if (model.SelectedOpportunityIds == null)
            {
                List<OpportunityVM> allOpportunities = _mapper.Map<List<OpportunityVM>>(await _opportunitiesService.GetAllOpportunitiesAsync());
                model.ExistingOpportunities = allOpportunities;
                TempData["Message"] = "Silmek için en az bir kategori seçmelisiniz.";
                return View(model);
            }

            var selectedOpportunities = await _opportunitiesService.GetOpportunitiesByIdsAsync(model.SelectedOpportunityIds);
            var imagePaths = selectedOpportunities.Select(c => c.ImagePath).Where(x => !string.IsNullOrEmpty(x)).ToList();

            // veritabanindan sil
            await _opportunitiesService.RemoveOpportunityByIdsAsync(model.SelectedOpportunityIds);

            // dosyalari sil
            foreach (var path in imagePaths)
            {
                await _fileService.DeleteFileAsync(path);
            }

            // guncellenmis listeyi getir
            model.ExistingOpportunities = _mapper.Map<List<OpportunityVM>>(await _opportunitiesService.GetAllOpportunitiesAsync());
            TempData["Message"] = "Secili Firsatlar Basariyla Silindi";
            return View(model);
        }
        #endregion
    }
}
