public interface IExpertService
{
    Task<List<ExpertCategoryDTO>> GetExpertCategoriesAsync();
}