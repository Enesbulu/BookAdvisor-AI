using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Features.ReadingLists.Commands.BulkRemoveBooksFromReadingList;
using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Entities;
using BookAdvisor.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace BookAdvisor.UnitTests.Application.Features.ReadingLists.Commands.BulkRemoveBooksFromReadingList
{
    public class RemoveMultipleBooksFromReadingListCommandHandlerTests
    {
        private readonly Mock<IReadingListRepository> _mockReadingListRepository;
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        private readonly RemoveMultipleBooksFromReadingListCommandHandler _commandHandler;
        public RemoveMultipleBooksFromReadingListCommandHandlerTests()
        {
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockReadingListRepository = new Mock<IReadingListRepository>();
            _commandHandler = new RemoveMultipleBooksFromReadingListCommandHandler(_mockReadingListRepository.Object, _mockCurrentUserService.Object);
        }

        [Fact]
        public async Task Handle_Should_RemoveMultipleBooks_When_User_Is_Owner_And_Books_Exits()
        {
            ///arr
            var userId = "user-1";
            var targetListId = Guid.NewGuid();

            //silinecek kitapların listesi
            var bookId1 = Guid.NewGuid();
            var bookId2 = Guid.NewGuid();
            var bookId3 = Guid.NewGuid();
            //silinecek kitapların listesi
            var booksToRemoveList = new List<Guid> { bookId1, bookId2, bookId3 };

            //listeyi oluşturma
            var readingList = new ReadingList("Test Listesi", userId);

            //Okuma listesine kitapların eklenmesi
            readingList.AddBookAtReadingList(bookId1);
            readingList.AddBookAtReadingList(bookId2);
            readingList.AddBookAtReadingList(bookId3);

            //kullanıcıyı tanıtma
            _mockCurrentUserService.Setup(serv => serv.UserId).Returns(userId);

            //istek yapıldığında benim liste veriliyor
            _mockReadingListRepository.Setup(repo => repo.GetByIdAsync(targetListId))
                .ReturnsAsync(readingList);
            var command = new BulkRemoveBooksFromReadingListCommand(targetListId, booksToRemoveList);

            ///ACT
            await _commandHandler.Handle(command, CancellationToken.None);

            //assert
            //Toplu silme metodu doğru şekilde çalışıyor mu?
            _mockReadingListRepository.Verify(repo => repo.RemoveBooksFromListItemsAsync(It.Is<List<ReadingListItem>>(items => items.Count == 3)), Times.Once);
            //bellekten gerçekten sildi mi?
            readingList.Items.Should().BeEmpty();

        }

        [Fact]
        public async Task Handle_ShouldDo_Nothing_If_Books_Do_Not_Exits_In_List()
        {
            ///Arr

            var userId = "user-1";
            var targetListId = Guid.NewGuid();
            //olmayan kitap id leri gönderme
            var randomBookId = Guid.NewGuid();
            var booksToRemoveList = new List<Guid> { randomBookId };

            //boş liste oluşturma
            var readingList = new ReadingList("Boş liste", userId);

            _mockCurrentUserService.Setup(x => x.UserId).Returns(userId);
            _mockReadingListRepository.Setup(x => x.GetByIdAsync(targetListId)).ReturnsAsync(readingList);

            var command = new BulkRemoveBooksFromReadingListCommand(targetListId, booksToRemoveList);

            ///ACT
            await _commandHandler.Handle(command, CancellationToken.None);

            ///assert
            //silinecek kitap olmadığından repository silme metodu çağırılmamalı
            _mockReadingListRepository.Verify(repo => repo.RemoveBooksFromListItemsAsync(It.IsAny<List<ReadingListItem>>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Throw_Unauthorized_User_Is_Not_Owner()
        {
            ///ARR
            var ownerId = "user-1";
            var otherUser = "otherUser";
            var targetListId = Guid.NewGuid();

            var readingList = new ReadingList("başkasına ait liste", ownerId);

            //başka kullanıcı girişi
            _mockCurrentUserService.Setup(x => x.UserId).Returns(otherUser);
            _mockReadingListRepository.Setup(x => x.GetByIdAsync(targetListId)).ReturnsAsync(readingList);
            var command = new BulkRemoveBooksFromReadingListCommand(targetListId, new List<Guid>());

            ///ACT & Assert
            //yetki hatası
            await FluentActions.Invoking(() => _commandHandler.Handle(command, CancellationToken.None)).Should().ThrowAsync<UnauthorizedAccessException>().WithMessage(ApplicationMessages.UnauthorizedAccess);
        }
    }
}
