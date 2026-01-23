using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Events;
using BookAdvisor.Domain.Interfaces;
using MassTransit;

namespace BookAdvisor.Infrastructure.Messaging
{
    public class BookCreatedEventConsumer : IConsumer<BookCreatedEvent>
    {
        private readonly IAiService _aiService;
        private readonly IBookRepository _bookRepository;
        private readonly IAiKeyProvider _aiKeyProvider;

        public BookCreatedEventConsumer(IAiService aiService, IBookRepository bookRepository, IAiKeyProvider aiKeyProvider)
        {
            _aiService = aiService;
            _bookRepository = bookRepository;
            _aiKeyProvider = aiKeyProvider;
        }

        public async Task Consume(ConsumeContext<BookCreatedEvent> context)
        {
            try
            {
                //Event'ten verileri al
                var message = context.Message;
                var book = await _bookRepository.GetByIdAsync(message.BookId);
                if (book == null) return; // İşlemi kes

                //Kullanıcının (veya sistemin) API Key'ini bul
                var apiKey = await _aiKeyProvider.GetApiKeyAsync(message.UserId);

                // AI Servisine git (Burası uzun sürse de API'yi kilitlemez!)
                var aiSummary = await _aiService.GenerateBookSummaryAsync(title: message.Title, author: message.Author, apiKey: apiKey);

                // Kitabı güncelle
                if (book != null)
                {
                    book.UpdateDetails(book.Title, aiSummary);
                    await _bookRepository.UpdateAsync(book);
                }
            }
            catch (Exception ex)
            {
                // Hatanın ne olduğunu burada göreceğiz
                Console.WriteLine($"[Consumer FATAL ERROR]: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

    }
}
