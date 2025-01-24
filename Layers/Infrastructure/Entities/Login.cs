namespace Infrastructure.Entities
{
    public class Login
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPrivacyPolicyAccepted { get; set; }
        public DateTime CreatedDate { get; set; }
    }
} 