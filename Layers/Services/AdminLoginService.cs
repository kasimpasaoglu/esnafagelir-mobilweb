
using AutoMapper;
using esnafagelir_mobilweb.DataAccessLayer;
using esnafagelir_mobilweb.DMO;

public class AdminLoginService : IAdminLoginService
{
    private IGenericRepository<Admin> _adminRepo;
    private IMapper _mapper;
    private DataBaseContext _context;

    public AdminLoginService(IGenericRepository<Admin> adminRepo, DataBaseContext context, IMapper mapper)
    {
        _adminRepo = adminRepo;
        _context = context;
        _mapper = mapper;
    }
    public async Task<bool> Login(AdminVM admin)
    {
        try
        {
            var dbModel = await _adminRepo.FindSingleAsync(x => x.UserName == admin.UserName);
            if (dbModel == null)
            {
                throw new Exception("Kullanıcı adı hatalı");
            }

            var isPwCorrect = ShaHelper.VerifyPassword(admin.UserPassword, dbModel.Salt, dbModel.UserPassword);
            if (!isPwCorrect)
            {
                throw new Exception("Şifre hatalı");
            }

            var isDeviceIdMatch = dbModel.DeviceId == admin.DeviceId;
            if (!isDeviceIdMatch)
            {
                throw new Exception("Admin panele farklı bir cihazdan giriş yapamazsınız. Lütfen sistem yöneticinize başvurunuz.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return true;
    }

    public async Task<bool> UpdateAdminAsync(AdminDTO admin)
    {
        try
        {
            _adminRepo.Update(_mapper.Map<Admin>(admin));
            var result = await _context.SaveChangesAsync();
            if (result <= 0)
            {
                throw new Exception("Admin güncellenirken bir hata oluştu.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return true;
    }

    public async Task<bool> CreateAdminAsync(AdminDTO admin)
    {
        await _adminRepo.AddAsync(_mapper.Map<Admin>(admin));
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<List<AdminDTO>> GetAdminsAsync()
    {
        var admins = await _adminRepo.GetAllAsync();
        return _mapper.Map<List<AdminDTO>>(admins);
    }
}
