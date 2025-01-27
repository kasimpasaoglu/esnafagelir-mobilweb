
public interface ILoginService
{
    Task<UserDTO> FindByDeviceId(Guid deviceId);
    Task<UserDTO> FindByPhoneNumber(string phoneNumber);
    Task<BusinessDTO> FindBusinessById(int id);
    Task<bool> UpdateLastLoginDate(UserDTO user);
}
