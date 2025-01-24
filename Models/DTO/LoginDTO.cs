
public class LoginDTO
{
    public int UserId { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsPrivacyPolicyAccepted { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastLoginDate { get; set; }
}
