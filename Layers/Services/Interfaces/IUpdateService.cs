public interface IUpdateService
{
    Task<int> UpdateUserAndBussines(UserDTO user, BusinessDTO business);
    Task<int> UpdateCoopDecision(int businessId, bool coopDecision);
}