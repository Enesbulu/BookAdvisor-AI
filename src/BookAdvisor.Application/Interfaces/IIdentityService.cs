namespace BookAdvisor.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<(string Token, bool Success, string ErrorMessage)> LoginAsync(string email, string password);
        Task<(bool Success, string UserId, string ErrorMessage)> RegisterAsync(string email, string password, string firstName, string lastName);

    }
}
