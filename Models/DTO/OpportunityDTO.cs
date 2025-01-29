public class OpportunityDTO
{
    public int OpportunityId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ImagePath { get; set; } = null!;

    public string? Url { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime EndDate { get; set; }
    public bool IsPrimary { get; set; }
}