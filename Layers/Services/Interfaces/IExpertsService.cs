public interface IExpertService
{
    Task<List<ExpertCategoryDTO>> GetExpertCategoriesAsync();
    Task<int> AddExpertCategoryAsync(ExpertCategoryDTO model);
    Task RemoveCategory(List<int> categoryIds);
    Task<List<ExpertCategoryDTO>> GetCategoriesByIds(List<int> idList);
}