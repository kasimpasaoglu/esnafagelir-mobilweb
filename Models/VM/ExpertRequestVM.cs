public partial class ExpertRequestVM
{
    public int ExpertRequestId { get; set; }

    public int UserId { get; set; }

    public int BusinessId { get; set; }

    public int ExpertCategoryId { get; set; }

    public string Description { get; set; } = null!;

    public DateTime RecordDate { get; set; }

    public int RecordStatus { get; set; }
}