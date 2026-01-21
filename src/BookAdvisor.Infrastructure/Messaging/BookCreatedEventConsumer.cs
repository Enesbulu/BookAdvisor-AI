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

        public BookCreatedEventConsumer(IAiService aiService, IBookRepository bookRepository)
        {
            _aiService = aiService;
            _bookRepository = bookRepository;
        }

        public async Task Consume(ConsumeContext<BookCreatedEvent> context)
        {
            try
            {
                //Event'ten verileri al
                var message = context.Message;

                Console.WriteLine($"[Consumer] Mesaj alındı! Kitap ID: {message.BookId}, Başlık: {message.Title}");
                var book = await _bookRepository.GetByIdAsync(message.BookId);
                if (book == null)
                {
                    Console.WriteLine($"[Consumer HATA] Kitap veritabanında bulunamadı! ID: {message.BookId}");
                    return; // İşlemi kes
                }
                Console.WriteLine("[Consumer] Kitap bulundu, AI servisine gidiliyor...");


                // AI Servisine git (Burası uzun sürse de API'yi kilitlemez!)
                var aiSummary = await _aiService.GenerateBookSummaryAsync(message.Title, message.Author);

                Console.WriteLine($"[Consumer] AI Cevap döndü: {aiSummary.Substring(0, 20)}..."); // İlk 20 karakteri göster



                // Kitabı bul ve güncelle
                //var book = await _bookRepository.GetByIdAsync(message.BookId);
                if (book != null)
                {
                    book.UpdateDetails(book.Title, aiSummary);

                    await _bookRepository.UpdateAsync(book);
                    Console.WriteLine("[Consumer] Veritabanı başarıyla güncellendi!");
                }
            }
            catch (Exception ex)
            {
                // Hatanın ne olduğunu burada göreceğiz
                Console.WriteLine($"[Consumer FATAL ERROR]: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw; // Hatayı yutma, RabbitMQ tekrar denesin (Retry)
            }
        }

    }
}
