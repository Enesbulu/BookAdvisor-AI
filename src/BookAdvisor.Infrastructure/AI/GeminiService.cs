using BookAdvisor.Application.Interfaces;
using Microsoft.SemanticKernel;

namespace BookAdvisor.Infrastructure.AI
{
    public class GeminiService : IAiService
    {
        public async Task<string> GenerateBookSummaryAsync(string title, string author, string apiKey)
        {
            var modelId = "gemini-3-flash-preview"; // Example model ID
            var builder = Kernel.CreateBuilder();

            builder.AddGoogleAIGeminiChatCompletion(modelId: modelId, apiKey: apiKey);
            var _kernel = builder.Build();

            var prompt = $@"
            Sen uzman bir edebiyat eleştirmenisin.
            Bu kitap için Türkçe, ilgi çekici ve kısa bir özet (arka kapak yazısı tadında) oluştur.
            
            Kitap Adı: {title}
            Yazar: {author}
            
            Özet:";
            var result = await _kernel.InvokePromptAsync(prompt);
            return result.ToString().Trim();
        }
    }
}
