using BookAdvisor.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

namespace BookAdvisor.Infrastructure.AI
{
    public class GeminiService : IAiService
    {
        private readonly Kernel _kernel;

        public GeminiService(IConfiguration configuration)
        {
            var apiKey = configuration["AiSettings:ApiKey"];
            var modelId = "gemini-3-flash-preview"; // Example model ID
            var builder = Kernel.CreateBuilder();

            builder.AddGoogleAIGeminiChatCompletion(modelId: modelId, apiKey: apiKey);
            _kernel = builder.Build();
        }

        public async Task<string> GenerateBookSummaryAsync(string title, string author)
        {
            var prompt = $@"
            Sen uzman bir edebiyat eleştirmenisin.
            Aşağıdaki kitap için ilgi çekici, yaklaşık 2-3 cümlelik kısa bir özet ve tanıtım yazısı yaz.
            
            Kitap Adı: {title}
            Yazar: {author}
            
            Özet:";
            var result =
                await _kernel.InvokePromptAsync(prompt);
            return result.ToString().Trim();

        }
    }
}
