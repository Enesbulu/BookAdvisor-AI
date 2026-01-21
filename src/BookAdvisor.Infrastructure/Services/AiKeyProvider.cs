using BookAdvisor.Application.Interfaces;
using BookAdvisor.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BookAdvisor.Infrastructure.Services
{
    public class AiKeyProvider : IAiKeyProvider
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AiKeyProvider(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> GetApiKeyAsync(string userId)
        {
            //Kullanıcıyı bul
            var user = await _userManager.FindByIdAsync(userId);

            //Kullanıcı varsa ve kendi API anahtarı varsa, onu döndür
            if (user != null && !user.UseSystemKeys && !string.IsNullOrEmpty(user.EncryptedGeminiApiKey))
            {
                return user.EncryptedGeminiApiKey;
            }

            //Sistem key'ini döndür
            var systemKey = _configuration["AiSettings:ApiKey"];
            if (string.IsNullOrEmpty(systemKey))
            {
                throw new InvalidOperationException("CRITICAL: AI API Key not found in User settings or System configuration.");
            }
            return systemKey;
        }
    }
}
