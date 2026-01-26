using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Features.ReadingLists.Commands.AddBookToReadingList;
using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Entities;
using BookAdvisor.Domain.Interfaces;
using FluentAssertions;
using Moq;


namespace BookAdvisor.UnitTests.Application.Features.ReadingLists.Commands.AddBookToReadingList
{
    public class AddBookToReadingListCommandHandlerTests
    {
        private readonly Mock<IReadingListRepository> _mockReadingListRepository;
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        private readonly AddBookToReadingListCommandHandler _handler;

        public AddBookToReadingListCommandHandlerTests()
        {
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockReadingListRepository = new Mock<IReadingListRepository>();
            _handler = new AddBookToReadingListCommandHandler(_mockReadingListRepository.Object, _mockCurrentUserService.Object);
        }

        [Fact]
        public async Task Handler_Should_AddBook_When_User_Is_Owner()
        {
            ///Arrage-hazırlık
            var userId = "user-1";
            var listId = Guid.NewGuid();
            var bookId = Guid.NewGuid();

            //Kullanıcınnın kendisinin bir listesi olsun
            var readingList = new ReadingList("Favoriler", userId);

            _mockCurrentUserService.Setup(x => x.UserId).Returns(userId);
            _mockReadingListRepository.Setup(x => x.GetByIdAsync(listId)).ReturnsAsync(readingList);

            var command = new AddBookToReadingListCommand(listId, bookId);

            ///Act-Eylem
            await _handler.Handle(command, CancellationToken.None);

            ///Assert-Doğrulama
            //UpdateAsync  çağrısı ve işlemi başarılı mı
            _mockReadingListRepository.Verify(x => x.UpdateAsync(readingList), Times.Once);

            //Listeye eleman eklendi mi kontrolü
            readingList.Items.Should().Contain(x => x.BookId == bookId);
        }

        [Fact]
        public async Task Handle_Should_Throw_Unauthorized_When_User_Is_NOT_Owner()
        {
            ///Arrange - Hazırlık
            var ownerId = "user-1";
            var unauthUserId = "UnauthorizUser";    //yetkisi olmayan kullanıcı-saldırgan
            var listId = Guid.NewGuid();

            var readingList = new ReadingList("Başkasına ait liste", ownerId);

            //saldırgan girmiş olsun
            _mockCurrentUserService.Setup(z => z.UserId).Returns(unauthUserId);

            //liste bulunması
            _mockReadingListRepository.Setup(x => x.GetByIdAsync(listId)).ReturnsAsync(readingList);

            var command = new AddBookToReadingListCommand(listId, Guid.NewGuid());

            ///Act-Eylem && Assert-Doğrulama
            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage(ApplicationMessages.UnauthorizedAccess);
            //Db güncelleme çağrılmamış olmalı
            _mockReadingListRepository.Verify(x => x.UpdateAsync(It.IsAny<ReadingList>()), Times.Never);

        }

    }
}
