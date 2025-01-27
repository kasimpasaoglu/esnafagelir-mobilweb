using Microsoft.EntityFrameworkCore;
using AutoMapper;
using esnafagelir_mobilweb.DMO;
using esnafagelir_mobilweb.DataAccessLayer;


public class LoginService : ILoginService
{
    private readonly IGenericRepository<User> _userRepo;
    private readonly IGenericRepository<Business> _bussinesRepo;
    private readonly DataBaseContext _context;
    private readonly IMapper _mapper;

    public LoginService(
        IGenericRepository<User> userRepo,
        DataBaseContext context,
        IMapper mapper,
        IGenericRepository<Business> bussinesRepo
        )
    {
        _userRepo = userRepo;
        _bussinesRepo = bussinesRepo;
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDTO> FindByDeviceId(string deviceId)
    {
        var request = await _userRepo.FindAsync(x => x.DeviceId == deviceId);
        var user = request.FirstOrDefault(); // gecici cozum!!!
        return _mapper.Map<UserDTO>(user);
    }

    public async Task<UserDTO> FindByPhoneNumber(string phoneNumber)
    {
        var request = await _userRepo.FindAsync(x => x.PhoneNumber == phoneNumber);
        var user = request.FirstOrDefault(); // gecici cozum!!!

        // TODO: expression ile calisan FindSingle Gerenirc metodu lazim
        return _mapper.Map<UserDTO>(user);
    }

    public async Task<BusinessDTO> FindBusinessById(int id)
    {
        var request = await _bussinesRepo.FindAsync(x => x.BusinessId == id);
        var business = request.FirstOrDefault(); // gecici cozum
        // todo find single generic metodu lazim

        return _mapper.Map<BusinessDTO>(business);
    }

    public async Task<bool> UpdateLastLoginDate(UserDTO user)
    {
        var existingUser = await _userRepo.GetByIdAsync(user.UserId);
        if (existingUser == null)
        {
            throw new Exception("User not found");
            // DB'de bir sorun yoksa bu asamaya geldiyse kullancinin bulunmasi gereklidir
        }

        existingUser.LastLogin = DateTime.Now;
        return await _context.SaveChangesAsync() > 0;
    }
}
