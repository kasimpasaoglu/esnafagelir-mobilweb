public class LoginVM
{
    public Guid DeviceId { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsPrivacyPolicyAccepted { get; set; }
    public DateTime LastLoginDate { get; set; }
}