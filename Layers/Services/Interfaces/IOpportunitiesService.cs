using esnafagelir_mobilweb.DMO;

public interface IOpportunitiesService
{
    Task<List<OpportunityDTO>> GetPrimaryOpportunitiesAsync();
    Task<List<OpportunityDTO>> GetSecondaryOpportunitiesAsync();
    Task<OpportunityDTO> GetOpportunityAsync(int opportunityId);
    Task<int> AddOpportunityAsync(OpportunityDTO model);
    Task RemoveOpportunityByIdsAsync(List<int> opportunityIds);
    Task<List<OpportunityDTO>> GetOpportunitiesByIdsAsync(List<int> idList);

}