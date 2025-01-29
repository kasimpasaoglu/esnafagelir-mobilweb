using AutoMapper;
using esnafagelir_mobilweb.DMO;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IExpertService _expertService;
        private readonly IOpportunitiesService _opportunitiesService;
        private readonly IValidator<ExpertCategoryAdminModel> _expertValidator;
        private readonly IValidator<OpportunityAdminModel> _opportunityValidator;
        private readonly IMapper _mapper;
        public AdminPanelController(
            IFileService fileService,
            IExpertService expertService,
            IOpportunitiesService opportunitiesService,
            IValidator<ExpertCategoryAdminModel> expertValidator,
            IMapper mapper,
            IValidator<OpportunityAdminModel> opportunityValidator
            )
        {
            _fileService = fileService;
            _expertService = expertService;
            _expertValidator = expertValidator;
            _opportunitiesService = opportunitiesService;
            _mapper = mapper;
            _opportunityValidator = opportunityValidator;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddExpert()
        {
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

        public async Task<IActionResult> RemoveExpert()
        {
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


        public IActionResult AddOpportunity()
        {
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


        public async Task<IActionResult> RemoveOpportunity()
        {
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
    }
}
