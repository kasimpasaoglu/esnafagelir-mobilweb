public interface IRegisterService
{
    Task<UserDTO> SignInWithPhoneNumber(UserDTO model);
    Task<int> RegisterUserWithBusiness(UserDTO user, BusinessDTO business);
}