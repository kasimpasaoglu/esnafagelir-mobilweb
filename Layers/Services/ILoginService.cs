
public interface ILoginService
{
    Task<UserDTO> Register(UserDTO model);
    Task<UserDTO> CheckPhoneNumber(string phoneNumber);
    Task<bool> UpdateLoginDetails(UserDTO user);
}
