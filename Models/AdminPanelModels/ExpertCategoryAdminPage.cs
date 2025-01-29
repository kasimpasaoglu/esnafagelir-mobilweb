public class ExpertCategoryAdminPage
{
    public ExpertCategoryAdminModel NewCategory { get; set; }
    public List<ExpertCategoryVM> ExistingCategoriesList { get; set; }
    public List<int> SelectedCategoryIds { get; set; }
}
public partial class ExpertCategoryAdminModel
{
    public string CategoryName { get; set; } = null!;
    public string CategoryDescription { get; set; } = null!;
    public IFormFile ImageFile { get; set; }
}
