using Infrastructure.DTOs;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using AutoMapper;
using esnafagelir_mobilweb.DMO;

namespace Infrastructure.Services
{
    public class LoginService : ILoginService
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly DbContext _context;
        private readonly IMapper _mapper;

        public LoginService(IGenericRepository<User> userRepo, DbContext context, IMapper mapper)
        {
            _userRepo = userRepo;
            _context = context;
            _mapper = mapper;
        }

        public async Task Register(LoginDTO model)
        {
            // login bilgilerinden DMO.User model olusturulup, database'e kayit yapilacak

            await _userRepo.AddAsync(new User());
            await _context.SaveChangesAsync();
        }
    }
}