using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Features.ReadingLists.Queries.GetUserReadingLists;
using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Entities;
using BookAdvisor.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace BookAdvisor.UnitTests.Application.Features.ReadingLists.Queries.GetUserReadingLists
{
    public class GetUserReadingListsQueryHandlerTests
    {
        private readonly Mock<IReadingListRepository> _mockReadingListRepository;
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        private readonly GetUserReadingListsQueryHandler _handler;

        public GetUserReadingListsQueryHandlerTests()
        {
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockReadingListRepository = new Mock<IReadingListRepository>();
            _handler = new GetUserReadingListsQueryHandler(_mockReadingListRepository.Object, _mockCurrentUserService.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_Mapped_Dtos_When_User_Is_LoggedIn()
        {
            ///Arrange

            var userId = "user";
            //Örnek listeler
            var list1 = new ReadingList("Favoriler", userId);   //test için boş kalacak
            var list2 = new ReadingList("Okunacaklar", userId);    //test için eleman ekleyeceğiz

            list2.AddBook(Guid.NewGuid());
            var databaseLists = new List<ReadingList> { list1, list2 };
            _mockCurrentUserService.Setup(x => x.UserId).Returns(userId);
            _mockReadingListRepository.Setup(x => x.GetUserListsAsync(userId)).ReturnsAsync(databaseLists);
            var query = new GetUserReadingListQuery();

            ///ACT

            var result = await _handler.Handle(query, CancellationToken.None);

            ///Assert

            //liste boş dönmemeli
            result.Should().NotBeNull();
            //iki adet liste dönmeli
            result.Should().HaveCount(2);

            //İlk liste eleman sayısı 0 olmalı
            result[0].Name.Should().Be("Favoriler");
            result[0].BookCount.Should().Be(0);

            //ikinci liste eleman sayısı 1 olmalı.
            result[1].Name.Should().Be("Okunacaklar");
            result[1].BookCount.Should().Be(1);
        }

        [Fact]
        public async Task Handle_Should_Throw_Uanauthorized_When_User_Is_Not_LoggedIn()
        {
            ///Arrange
            _mockCurrentUserService.Setup(x => x.UserId).Returns((string)null);
            var query = new GetUserReadingListQuery();

            ///ACT - Assert
            await FluentActions.Invoking(() => _handler.Handle(query, CancellationToken.None)).Should().ThrowAsync<UnauthorizedAccessException>().WithMessage(ApplicationMessages.LoginRequired);

            //GetUserListAsync metodu hiç çağrılmamalı
            _mockReadingListRepository.Verify(x => x.GetUserListsAsync(It.IsAny<string>()), Times.Never);
        }
    }
}
