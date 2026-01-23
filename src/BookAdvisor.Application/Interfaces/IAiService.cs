namespace BookAdvisor.Application.Interfaces
{
    public interface IAiService
    {
        // Kitap başlığı ve yazarını verince bize özet dönece
        Task<string> GenerateBookSummaryAsync(string title, string author,string apiKey);
    }
}
