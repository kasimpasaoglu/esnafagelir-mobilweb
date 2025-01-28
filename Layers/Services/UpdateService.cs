
using AutoMapper;
using esnafagelir_mobilweb.DataAccessLayer;
using esnafagelir_mobilweb.DMO;

public class UpdateService : IUpdateService
{
    public IGenericRepository<User> _userRepo;
    public IGenericRepository<Business> _businessRepo;
    private readonly DataBaseContext _context;
    private readonly IMapper _mapper;
    public UpdateService(IGenericRepository<User> userRepo, IGenericRepository<Business> businessRepo, DataBaseContext context, IMapper mapper)
    {
        _userRepo = userRepo;
        _businessRepo = businessRepo;
        _context = context;
        _mapper = mapper;
    }
    public async Task<int> UpdateUserAndBussines(UserDTO user, BusinessDTO business)
    {
        var userDMO = _mapper.Map<User>(user);
        var businessDMO = _mapper.Map<Business>(business);

        if (businessDMO.BusinessId != 0) // businessId varsa, business entity guncellenecek
        {
            _businessRepo.Update(businessDMO);
            await _context.SaveChangesAsync();
        }
        else // yoksa yeni bir business entity eklenecek ve gelen businessId user'a konacak
        {
            await _businessRepo.AddAsync(businessDMO);
            await _context.SaveChangesAsync();
            userDMO.BusinessId = businessDMO.BusinessId;
        }
        _userRepo.Update(userDMO);
        await _context.SaveChangesAsync();
        return businessDMO.BusinessId;
    }

    public async Task<int> UpdateCoopDecision(int businessId, bool coopDecision)
    {
        var business = _businessRepo.FindAsync(x => x.BusinessId == businessId).Result.FirstOrDefault();
        business.AllowsCooperation = coopDecision;
        _businessRepo.Update(business);
        return await _context.SaveChangesAsync();
    }

}