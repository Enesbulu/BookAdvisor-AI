using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Features.ReadingLists.Commands.CreateReadingList;
using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Entities;
using BookAdvisor.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace BookAdvisor.UnitTests.Application.Features.ReadingLists.Commands.CreateReadingList
{
    public class CreateReadingListCommandHandlerTests
    {
        private readonly Mock<IReadingListRepository> _mockReadingListRepository;
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;
        private readonly CreateReadingListCommandHandler _handler;

        public CreateReadingListCommandHandlerTests()
        {
            //Bağımlılıkların sahtesini oluşturma
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockReadingListRepository = new Mock<IReadingListRepository>();

            //Tesk edilecek sınıf ayağa kaldırılıyor.
            _handler = new CreateReadingListCommandHandler(
                _mockReadingListRepository.Object,
                _mockCurrentUserService.Object);
        }

        [Fact]
        public async Task Handler_Should_CreateReadinglist_When_User_Is_Authorized()
        {
            ///Arrange --Hazırlık
            var userId = "user-123";
            var command = new CreateReadingListCommand("Favarilerim");

            //kullanıcı girişi simülasyonu
            _mockCurrentUserService.Setup(x => x.UserId).Returns(userId);

            ///Act--Eylem
            var result = await _handler.Handle(command, CancellationToken.None);

            ///Assert --doğrulama
            //Geriye geçerli birid dönmeli
            result.Should().NotBeEmpty();

            //Repository AddAsync doğru parametreler ile çağrılmalı
            _mockReadingListRepository.Verify(
                x => x.AddAsync(It.Is<ReadingList>(
                    r => r.Name == command.Name && r.UserId == userId)), Times.Once);

        }


        [Fact]
        public async Task Handle_Should_Throw_UnauthorizedException_When_UserId_Is_Null()
        {
            ///Arrange --Hazırlık
            //Kullanıcı giriş yapmamış olsun
            _mockCurrentUserService.Setup(x => x.UserId).Returns((string)null);

            var command = new CreateReadingListCommand("Hata verecek Liste");

            ///Act--Aylem & Assert-Doğrulama
            //Metod hata fırlatmalı
            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage(ApplicationMessages.LoginRequired);

            //hatalı olduğu için db kayıt işlemi hiç çağrılmamalı
            _mockReadingListRepository.Verify(x => x.AddAsync(It.IsAny<ReadingList>()), Times.Never);
        }

    }
}
