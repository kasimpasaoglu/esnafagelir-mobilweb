public partial class UserDTO
{
    public int UserId { get; set; }
    public Guid DeviceId { get; set; }
    public DateTime LastLogin { get; set; } = DateTime.Now;
    public bool IsPrivacyPolicyAccepted { get; set; } = false;
    public string PhoneNumber { get; set; } = null!;
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public int RoleId { get; set; } = 0;
    public DateTime RegisterDate { get; set; } = DateTime.Now;
    public int BusinessId { get; set; }
}
