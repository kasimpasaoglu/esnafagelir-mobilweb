using Infrastructure.DTOs;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace Infrastructure.Services
{
    public class LoginService : ILoginService
    {
        private readonly IGenericRepository<LoginDTO> _loginRepository;
        private readonly DbContext _context;

        public LoginService(IGenericRepository<LoginDTO> loginRepository, DbContext context)
        {
            _loginRepository = loginRepository;
            _context = context;
        }

        public async Task Login(LoginDTO login)
        {
            // convert dto to dmo

            await _loginRepository.AddAsync(login);
            await _context.SaveChangesAsync();
        }
    }
} 