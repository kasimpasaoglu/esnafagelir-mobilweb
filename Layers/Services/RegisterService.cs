
using AutoMapper;
using esnafagelir_mobilweb.DataAccessLayer;
using esnafagelir_mobilweb.DMO;

public class RegisterService : IRegisterService
{
    private readonly IGenericRepository<User> _userRepo;
    private readonly IGenericRepository<Business> _businessRepo;
    private readonly DataBaseContext _context;
    private readonly IMapper _mapper;

    public RegisterService
    (
        IGenericRepository<User> userRepo,
        DataBaseContext context,
        IMapper mapper,
        IGenericRepository<Business> businessRepo
    )
    {
        _userRepo = userRepo;
        _businessRepo = businessRepo;
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


    public async Task<bool> RegisterUserWithBusiness(UserDTO user, BusinessDTO business)
    {   // user ve bussines bilgilerini alip iki ayri tabloda islem yapar
        // once business tablosuna kaydi gonderir, aldigi businessId'yi user modelinin icine koyup, user modelinin kaydini gunceller(cunku ilk kayit zaten telefon numarasi ile yapilmisti)

        // 1. Adim -> Business kaydini ekle ve BusinessId'yi al
        var bussinesDMO = _mapper.Map<Business>(business);
        await _businessRepo.AddAsync(bussinesDMO);
        var result = await _context.SaveChangesAsync();
        var bussinesId = bussinesDMO.BusinessId;

        // 2. Adim -> BusinessId'yi user'a ekle, user kaydini guncelle
        user.BusinessId = bussinesId;
        var userDMO = _mapper.Map<User>(user);
        _userRepo.Update(userDMO);
        result += await _context.SaveChangesAsync();
        return result > 0 ? true : false;
        // NOT: unitofwork transaction yonetimi lazim gibi. yoksa db'de tutarsizliklar olusabilir...

    }
}