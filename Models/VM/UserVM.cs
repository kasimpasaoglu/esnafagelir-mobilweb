public class UserVM
{
    public int UserId { get; set; }
    public Guid DeviceId { get; set; }
    public DateTime LastLogin { get; set; }
    public bool IsPrivacyPolicyAccepted { get; set; }
    public string PhoneNumber { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int RoleId { get; set; }
    public DateTime RegisterDate { get; set; }
    public int BusinessId { get; set; }
}