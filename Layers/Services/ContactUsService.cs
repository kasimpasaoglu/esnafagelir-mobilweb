
using AutoMapper;
using esnafagelir_mobilweb.DataAccessLayer;
using esnafagelir_mobilweb.DMO;

public class ContactUsService : IContactUsService
{
    private IGenericRepository<ContactRequest> _contactRequestRepo;
    private DataBaseContext _context;
    private IMapper _mapper;
    public ContactUsService(IGenericRepository<ContactRequest> contactRequestRepo, DataBaseContext context, IMapper mapper)
    {
        _contactRequestRepo = contactRequestRepo;
        _context = context;
        _mapper = mapper;
    }
    public async Task<int> Put(int userId, string message)
    {
        var dtoModel = new ContactRequestDTO
        {
            UserId = userId,
            Message = message,
            RecordDate = DateTime.Now,
            RecordStatus = 1,
        };
        var request = _mapper.Map<ContactRequest>(dtoModel);
        await _contactRequestRepo.AddAsync(request);
        var result = await _context.SaveChangesAsync();
        return result;
    }
}