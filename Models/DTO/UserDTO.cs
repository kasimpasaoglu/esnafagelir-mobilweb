public partial class UserDTO
{
    public int UserId { get; set; }
    public string DeviceId { get; set; }
    public DateTime LastLogin { get; set; }
    public bool IsPrivacyPolicyAccepted { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public int RoleId { get; set; } = 0;
    public DateTime RegisterDate { get; set; }
    public int BusinessId { get; set; } = 0;
}
