public interface IExpertRequestService
{
    Task<int> AddExpertRequestAsync(ExpertRequestDTO model);
}