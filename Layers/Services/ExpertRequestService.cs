
using AutoMapper;
using esnafagelir_mobilweb.DataAccessLayer;
using esnafagelir_mobilweb.DMO;

public class ExpertRequestService : IExpertRequestService
{
    private readonly IGenericRepository<ExpertRequest> _expertRequestsRepo;
    private readonly DataBaseContext _context;
    private readonly IMapper _mapper;

    public ExpertRequestService(IGenericRepository<ExpertRequest> expertRequestRepo, DataBaseContext context, IMapper mapper)
    {
        _expertRequestsRepo = expertRequestRepo;
        _context = context;
        _mapper = mapper;
    }
    public async Task<int> AddExpertRequestAsync(ExpertRequestDTO model)
    {
        model.RecordDate = DateTime.Now;
        await _expertRequestsRepo.AddAsync(_mapper.Map<ExpertRequest>(model));
        return await _context.SaveChangesAsync();
    }
}
