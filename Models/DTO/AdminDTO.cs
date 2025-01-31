public class AdminDTO
{
    public int AdminId { get; set; }

    public string DeviceId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string Salt { get; set; } = null!;
}