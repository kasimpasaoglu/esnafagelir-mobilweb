
public interface ILoginService
{
    Task<UserDTO> FindByDeviceId(Guid deviceId);
    Task<UserDTO> FindByPhoneNumber(string phoneNumber);
    Task<bool> UpdateLastLoginDate(UserDTO user);
}
