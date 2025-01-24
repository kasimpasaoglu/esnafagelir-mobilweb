public partial class UserDTO
{
    public int UserId { get; set; }
    public Guid DeviceId { get; set; } = Guid.NewGuid();
    public DateTime LastLogin { get; set; } = DateTime.Now;
    public bool IsPrivacyPolicyAccepted { get; set; } = false;
    public string PhoneNumber { get; set; } = null!;
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public int RoleId { get; set; } = 99;
    public DateTime RegisterDate { get; set; } = DateTime.Now;
    public int BusinessId { get; set; } = 99;

}
