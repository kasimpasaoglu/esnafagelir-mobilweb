public interface IRegisterService
{
    Task<UserDTO> SignInWithPhoneNumber(UserDTO model);
}