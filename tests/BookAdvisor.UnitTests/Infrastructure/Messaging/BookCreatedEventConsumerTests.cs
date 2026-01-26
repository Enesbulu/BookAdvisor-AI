using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Entities;
using BookAdvisor.Domain.Events;
using BookAdvisor.Domain.Interfaces;
using BookAdvisor.Infrastructure.Messaging;
using MassTransit;
using Moq;

namespace BookAdvisor.UnitTests.Infrastructure.Messaging
{
    public class BookCreatedEventConsumerTests
    {
        private readonly Mock<IAiService> _mockAiService;
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly Mock<IAiKeyProvider> _mockKeyProvider;
        private readonly BookCreatedEventConsumer _consumer;

        public BookCreatedEventConsumerTests()
        {
            _mockAiService = new Mock<IAiService>();
            _mockBookRepository = new Mock<IBookRepository>();
            _mockKeyProvider = new Mock<IAiKeyProvider>();

            _consumer = new BookCreatedEventConsumer(aiService: _mockAiService.Object, bookRepository: _mockBookRepository.Object, aiKeyProvider: _mockKeyProvider.Object);
        }



        [Fact]
        public async Task Consume_Should_Call_AI_And_Update_Book()
        {
            var bookId = Guid.NewGuid();
            var userId = "user-1";
            var existingBook = new Book(title: "Dune", author: "Frank Herbert", description: null, isbn: "123");
            var expectedSummary = "Bu kitap çok iyi bir bilimkurgu eseri";
            var userApiKey = "User-Secret-Key";

            var context = Mock.Of<ConsumeContext<BookCreatedEvent>>(
                c => c.Message == new BookCreatedEvent(BookId: bookId, Title: existingBook.Title, Author: existingBook.Author, UserId: userId));

            _mockBookRepository.Setup(r => r.GetByIdAsync(bookId)).ReturnsAsync(existingBook);
            _mockKeyProvider.Setup(k => k.GetApiKeyAsync(userId)).ReturnsAsync(userApiKey);
            _mockAiService.Setup(ai => ai.GenerateBookSummaryAsync(title: existingBook.Title, author: existingBook.Author, apiKey: userApiKey)).ReturnsAsync(expectedSummary);

            await _consumer.Consume(context);

            _mockBookRepository.Verify(
                r => r.UpdateAsync(It.Is<Book>(b => b.Description == expectedSummary)), Times.Once());


        }



    }
}
