public interface IContactUsService
{
    Task<int> Put(int userId, string message);
}