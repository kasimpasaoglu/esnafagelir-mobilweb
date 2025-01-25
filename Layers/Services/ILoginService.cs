
public interface ILoginService
{
    Task<UserDTO> Register(UserDTO model);
    Task<UserDTO> FindByPhoneNumber(string phoneNumber);
    Task<bool> UpdateLastLoginDate(UserDTO user);
}
