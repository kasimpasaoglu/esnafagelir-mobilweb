
using AutoMapper;
using esnafagelir_mobilweb.DataAccessLayer;
using esnafagelir_mobilweb.DMO;

public class OpportunitiesService : IOpportunitiesService
{
    private readonly IGenericRepository<Opportunity> _opportunityRepo;
    private readonly DataBaseContext _context;
    private readonly IMapper _mapper;

    public OpportunitiesService(IGenericRepository<Opportunity> repository, DataBaseContext context, IMapper mapper)
    {
        _opportunityRepo = repository;
        _context = context;
        _mapper = mapper;
    }



    public async Task<OpportunityDTO> GetOpportunityAsync(int opportunityId)
    {
        var dmoModel = await _opportunityRepo.GetByIdAsync(opportunityId);
        return _mapper.Map<OpportunityDTO>(dmoModel);
    }

    public async Task<List<OpportunityDTO>> GetAllOpportunitiesAsync()
    {
        return _mapper.Map<List<OpportunityDTO>>(await _opportunityRepo.GetAllAsync());
    }
    public async Task<List<OpportunityDTO>> GetPrimaryOpportunitiesAsync()
    {
        var dmoModel = await _opportunityRepo.FindAsync(x => x.IsPrimary && x.EndDate > DateTime.Now.AddHours(2));
        return _mapper.Map<List<OpportunityDTO>>(dmoModel);
    }

    public async Task<List<OpportunityDTO>> GetSecondaryOpportunitiesAsync()
    {
        var dmoModel = await _opportunityRepo.FindAsync(x => !x.IsPrimary && x.EndDate > DateTime.Now.AddHours(2));
        return _mapper.Map<List<OpportunityDTO>>(dmoModel);
    }

    public async Task<int> AddOpportunityAsync(OpportunityDTO model)
    {
        var dmoModel = _mapper.Map<Opportunity>(model);
        await _opportunityRepo.AddAsync(dmoModel);
        return await _context.SaveChangesAsync();
    }

    public async Task RemoveOpportunityByIdsAsync(List<int> opportunityIds)
    {
        _opportunityRepo.RemoveBy(x => opportunityIds.Contains(x.OpportunityId));
        await _context.SaveChangesAsync();
    }

    public async Task<List<OpportunityDTO>> GetOpportunitiesByIdsAsync(List<int> idList)
    {
        var opportunities = await _opportunityRepo.FindAsync(x => idList.Contains(x.OpportunityId));
        return _mapper.Map<List<OpportunityDTO>>(opportunities);
    }
}