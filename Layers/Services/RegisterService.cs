
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
    {
        // tek sorguda hem deviceId ve ya PhoneNmber eslesen kullaniciyi getir 
        var matchingUsers = await _userRepo.FindAsync(u =>
            u.PhoneNumber == model.PhoneNumber || u.DeviceId == model.DeviceId);

        // hic bisey bulmazsa yeni kullanici olusturup kaydet
        if (!matchingUsers.Any())
        {
            var newUser = _mapper.Map<User>(model);

            newUser.LastLogin = DateTime.Now;
            newUser.RegisterDate = DateTime.Now;

            await _userRepo.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDTO>(newUser);
        }

        // kullanici bulunduysa guncelleme islemi yapilacak. hangi field eslesiyorsa o kullaniciyi yakala
        var userToUpdate = matchingUsers.FirstOrDefault(u => u.PhoneNumber == model.PhoneNumber)
            ?? matchingUsers.FirstOrDefault(u => u.DeviceId == model.DeviceId); // ya device id eslesecek ya da phoneNumber

        if (userToUpdate != null)
        {
            // phone number eslesiyorsa device id degistir
            if (userToUpdate.PhoneNumber == model.PhoneNumber)
            {
                userToUpdate.DeviceId = model.DeviceId;
            }
            // deviceid eslesiyorsa phoneNumber'i degistir
            else if (userToUpdate.DeviceId == model.DeviceId)
            {
                userToUpdate.PhoneNumber = model.PhoneNumber;
            }

            userToUpdate.LastLogin = DateTime.Now;

            _userRepo.Update(userToUpdate);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDTO>(userToUpdate);
        }

        return null; // beklenmeyen durumda null don
    }


    public async Task<int> RegisterUserWithBusiness(UserDTO user, BusinessDTO business)
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
        if (result > 0)
        {
            return bussinesDMO.BusinessId;
        }
        return 0;
        // NOT: unitofwork transaction yonetimi lazim gibi. yoksa db'de tutarsizliklar olusabilir...

    }
}