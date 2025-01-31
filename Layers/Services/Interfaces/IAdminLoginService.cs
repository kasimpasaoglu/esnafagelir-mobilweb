public interface IAdminLoginService
{
    Task<bool> Login(AdminVM admin);
    Task<bool> UpdateAdminAsync(AdminDTO admin);
    Task<bool> CreateAdminAsync(AdminDTO admin);
    Task<List<AdminDTO>> GetAdminsAsync();
}