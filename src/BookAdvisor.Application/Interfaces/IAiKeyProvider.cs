namespace BookAdvisor.Application.Interfaces
{
    public interface IAiKeyProvider
    {
        Task<string> GetApiKeyAsync(string userId);
    }
}
