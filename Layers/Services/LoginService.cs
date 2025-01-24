using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using esnafagelir_mobilweb.DMO;
using esnafagelir_mobilweb.DataAccessLayer;

namespace Infrastructure.Services
{
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

        public async Task<UserDTO> Register(UserDTO model)
        {   // kayit yapar, basariliysa kaydedilen modeli doner, basarisizsa null

            // ilk loginde roleid ve bussines id 0 olarak girilecek
            // database'de bu degerlere karsilik 'unknown' yazar
            // kullanici sonraki adimda bilgilerini girerse ve ya daha sonra girmek isterse kullanici sorgulanip, update islemi yapilacak
            // kayit isleminden once sorgulama yapilmasi lazim ona gore ilerlenecek.


            var user = _mapper.Map<User>(model);
            user.DeviceId = Guid.NewGuid();
            user.LastLogin = DateTime.Now;
            user.RegisterDate = DateTime.Now;


            await _userRepo.AddAsync(user);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? _mapper.Map<UserDTO>(user) : null; //basariliysa modeli don degilse null

        }

        public async Task<UserDTO> CheckPhoneNumber(string phoneNumber)
        {
            var user = _userRepo.FindAsync(x => x.PhoneNumber == phoneNumber).Result.FirstOrDefault();
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<bool> UpdateLoginDetails(UserDTO user)
        {
            var existingUser = await _userRepo.GetByIdAsync(user.UserId);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            existingUser.LastLogin = DateTime.Now;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}