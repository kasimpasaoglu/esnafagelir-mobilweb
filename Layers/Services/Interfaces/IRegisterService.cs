public interface IRegisterService
{
    Task<UserDTO> SignInWithPhoneNumber(UserDTO model);
    Task<bool> RegisterUserWithBusiness(UserDTO user, BusinessDTO business);
}