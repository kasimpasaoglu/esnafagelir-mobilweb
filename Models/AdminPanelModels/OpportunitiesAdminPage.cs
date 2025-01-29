public class OpportunitiesAdminPage
{
    public OpportunityAdminModel NewOpportunity { get; set; }
    public List<OpportunityVM> ExistingOpportunities { get; set; }
    public List<int> SelectedOpportunityIds { get; set; }
}

public class OpportunityAdminModel
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IFormFile ImageFile { get; set; }
    public string? Url { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; }
    public bool IsPrimary { get; set; }
}