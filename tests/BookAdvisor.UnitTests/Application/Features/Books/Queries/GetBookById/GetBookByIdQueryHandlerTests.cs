using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Features.Books.Queries.GetBookById;
using BookAdvisor.Domain.Common;
using BookAdvisor.Domain.Entities;
using BookAdvisor.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace BookAdvisor.UnitTests.Application.Features.Books.Queries.GetBookById
{
    public class GetBookByIdQueryHandlerTests
    {
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly GetBookByIdQueryHandler _handler;

        public GetBookByIdQueryHandlerTests()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _handler = new GetBookByIdQueryHandler(_mockBookRepository.Object);
        }

        [Fact]
        public async Task Handler_Should_return_BookDetail_When_Book_Exists()
        {
            ///Arrange
            var bookId = Guid.NewGuid();
            var book = new Book(title: "Dune", author: "Frank Herbert", description: "Bilim kurgu klasiği", isbn: "123-123-123");

            //elle id set etme
            typeof(BaseEntity).GetProperty("Id").SetValue(book, bookId);
            _mockBookRepository.Setup(x => x.GetByIdAsync(bookId)).ReturnsAsync(book);
            var query = new GetBookByIdQuery(BookId: bookId);

            ///ACT
            var result = await _handler.Handle(query, CancellationToken.None);

            ///Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(bookId);
            result.Title.Should().Be("Dune");
            result.Author.Should().Be("Frank Herbert");
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Book_DoesNotExist()
        {
            ///Arrange
            var bookId = Guid.NewGuid();

            //repo null olarak dönmeli
            _mockBookRepository.Setup(x => x.GetByIdAsync(bookId)).ReturnsAsync((Book)null);
            var query = new GetBookByIdQuery(BookId: bookId);

            ///ACT && Assert
            await FluentActions.Invoking(() => _handler.Handle(query, CancellationToken.None)).Should().ThrowAsync<Exception>().WithMessage(ApplicationMessages.BookNotFound);

        }
    }
}
