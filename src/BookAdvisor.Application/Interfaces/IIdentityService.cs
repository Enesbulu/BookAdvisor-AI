using BookAdvisor.Application.DTOs.Auth;

namespace BookAdvisor.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<(string Token, bool Success, string ErrorMessage, string UserId)> LoginAsync(LoginRequest request);
        Task<(bool Success, string UserId, string ErrorMessage)> RegisterAsync(RegisterRequest request);

    }
}
