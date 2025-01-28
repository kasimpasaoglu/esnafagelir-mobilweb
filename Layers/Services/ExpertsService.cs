
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
}