using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Features.ReadingLists.Commands.DeleteReadingList;
using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Entities;
using BookAdvisor.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace BookAdvisor.UnitTests.Application.Features.ReadingLists.Commands.DeleteReadingList
{
    public class DeleteReadingListCommandHandlerTests
    {
        private readonly Mock<IReadingListRepository> _mockReadingListRepository;
        private readonly Mock<ICurrentUserService> _mockUserService;
        private readonly DeleteReadingListCommandHandler _handler;

        public DeleteReadingListCommandHandlerTests()
        {
            _mockReadingListRepository = new Mock<IReadingListRepository>();
            _mockUserService = new Mock<ICurrentUserService>();
            _handler = new DeleteReadingListCommandHandler(readingListRepository: _mockReadingListRepository.Object, currentUserService: _mockUserService.Object);
        }

        [Fact]
        public async Task Handle_Should_DeleteList_When_User_Is_Owner()
        {
            //arr
            var userId = "user-1";
            var listId = Guid.NewGuid();
            var readingList = new ReadingList("Silinecek Liste", userId);

            typeof(Domain.Common.BaseEntity).GetProperty("Id").SetValue(readingList, listId);

            _mockUserService.Setup(x => x.UserId).Returns(userId);
            _mockReadingListRepository.Setup(x => x.GetByIdAsync(listId)).ReturnsAsync(readingList);
            var command = new DeleteReadingListCommand(listId);

            //Act
            await _handler.Handle(command, CancellationToken.None);

            //assert
            _mockReadingListRepository.Verify(x => x.DeleteAsync(readingList), Times.Once);
        }


        [Fact]
        public async Task Handle_Should_throw_Unauthorized_When_User_Is_Not_Owner()
        {
            ///arr
            var ownerId = "user-1";
            var atteckerId = "attac-01";
            var listId = Guid.NewGuid();
            var readingList = new ReadingList("Başkasına ait liste", ownerId);

            _mockUserService.Setup(x => x.UserId).Returns(atteckerId);
            _mockReadingListRepository.Setup(x => x.GetByIdAsync(listId)).ReturnsAsync(readingList);

            var command = new DeleteReadingListCommand(listId);

            //ACT-Assert
            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None)).Should().ThrowAsync<UnauthorizedAccessException>().WithMessage(ApplicationMessages.UnauthorizedAccess);

            _mockReadingListRepository.Verify(x => x.DeleteAsync(It.IsAny<ReadingList>()), Times.Never);


        }


    }
}
