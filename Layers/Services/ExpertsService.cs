
using AutoMapper;
using esnafagelir_mobilweb.DataAccessLayer;
using esnafagelir_mobilweb.DMO;

public class ExpertsService : IExpertService
{
    private IGenericRepository<ExpertCategory> _categoryRepo;
    private DataBaseContext _context;
    private IMapper _mapper;

    public ExpertsService(IGenericRepository<ExpertCategory> categoryRepo, DataBaseContext context, IMapper mapper)
    {
        _categoryRepo = categoryRepo;
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<ExpertCategoryDTO>> GetExpertCategoriesAsync()
    {
        var dmoList = await _categoryRepo.GetAllAsync();
        return _mapper.Map<List<ExpertCategoryDTO>>(dmoList);
    }
    public async Task<int> AddExpertCategoryAsync(ExpertCategoryDTO model)
    {
        var dmoModel = _mapper.Map<ExpertCategory>(model);
        await _categoryRepo.AddAsync(dmoModel);
        return await _context.SaveChangesAsync();
    }

    public async Task RemoveCategory(List<int> categoryIds)
    {
        _categoryRepo.RemoveBy(x => categoryIds.Contains(x.ExpertCategoryId));
        await _context.SaveChangesAsync();
    }

    public async Task<List<ExpertCategoryDTO>> GetCategoriesByIds(List<int> idList)
    {
        var categories = await _categoryRepo.FindAsync(x => idList.Contains(x.ExpertCategoryId));
        return _mapper.Map<List<ExpertCategoryDTO>>(categories);
    }
}