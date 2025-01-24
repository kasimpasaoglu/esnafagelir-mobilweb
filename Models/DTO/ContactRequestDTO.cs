public partial class ContactRequestDTO
{
    public int ContactRequestId { get; set; }

    public int UserId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime RecordDate { get; set; }

    public int RecordStatus { get; set; }

}