using System.Threading.Tasks;
using Infrastructure.DTOs;

namespace Infrastructure.Services
{
    public interface ILoginService
    {
        public Task Register(LoginDTO login);
    }
}