using Microsoft.AspNetCore.Identity;


namespace BookAdvisor.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string? EncryptedGeminiApiKey { get; set; }
        public bool UseSystemKeys { get; set; }
    }
}
