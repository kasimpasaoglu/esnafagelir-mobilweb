
using AutoMapper;
using esnafagelir_mobilweb.DataAccessLayer;
using esnafagelir_mobilweb.DMO;

public class RegisterService : IRegisterService
{
    private readonly IGenericRepository<User> _userRepo;
    private readonly IGenericRepository<Business> _bussinesRepo;
    private readonly DataBaseContext _context;
    private readonly IMapper _mapper;

    public RegisterService
    (
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
    public async Task<UserDTO> SignInWithPhoneNumber(UserDTO model)
    {   // duruma gore kayit ve ya guncelleme islemi yapar, basariliysa islenen modeli doner, basarisizsa null

        // TODO: 
        /// Gelen modeldeki phoneNumber ve deviceId degerlerini veritabainda sorgula
        /// Eger veritabaninda eslesme yoksa yeni kullanici kaydi olustur,
        /// Eslesme varsa var olan kullanicinin bilgileirni guncelle
        model.DeviceId = Guid.NewGuid();
        model.LastLogin = DateTime.Now;
        model.RegisterDate = DateTime.Now;

        var user = _mapper.Map<User>(model);

        await _userRepo.AddAsync(user);
        var result = await _context.SaveChangesAsync();
        return result > 0 ? _mapper.Map<UserDTO>(user) : null; //basariliysa modeli don degilse null
    }
}