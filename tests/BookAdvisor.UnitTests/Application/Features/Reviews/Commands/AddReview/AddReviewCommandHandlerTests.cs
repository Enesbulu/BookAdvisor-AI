using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Features.Reviews.Commands.AddReview;
using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Entities;
using BookAdvisor.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace BookAdvisor.UnitTests.Application.Features.Reviews.Commands.AddReview;

public class AddReviewCommandHandlerTests
{
    private readonly Mock<IReviewRepository> _mockReviewsRepo;
    private readonly Mock<ICurrentUserService> _mockCurrentUser;
    private readonly AddReviewCommandHandler _handler;

    public AddReviewCommandHandlerTests()
    {
        _mockReviewsRepo = new Mock<IReviewRepository>();
        _mockCurrentUser = new Mock<ICurrentUserService>();
        _handler = new AddReviewCommandHandler(reviewRepository: _mockReviewsRepo.Object, currentUserService: _mockCurrentUser.Object);
    }

    [Fact]
    public async Task Handler_Should_AddReview_When_User_Has_Not_Reviewed_Before()
    {
        ///Arrange
        var userId = "user-1";
        var bookId = Guid.NewGuid();
        var command = new AddReviewCommand(BookId: bookId, Rating: 5, Comment: "Kitap çok iyi");
        _mockCurrentUser.Setup(x => x.UserId).Returns(userId);

        //Daha önce yorum yapmamış olsun - false dönmeli 
        _mockReviewsRepo.Setup(x => x.HasUserReviewedBookAsync(userId, bookId)).ReturnsAsync(false);

        ///ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        ///ASSERT
        //Repository'nin addAsync metodu çalıştı mı kontrolü
        _mockReviewsRepo.Verify(x => x.AddAsync(It.IsAny<Review>()), Times.Once);

        //Boş id dönmemeli
        result.Should().NotBeEmpty();


    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_User_Already_Reviewed()
    {
        ///Arr
        _mockCurrentUser.Setup(x => x.UserId).Returns((string)null); //kullanıcı yok

        var command = new AddReviewCommand(Guid.NewGuid(), Rating: 5, Comment: "Loggin olunmadı");

        ///Act && Assert
        await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None)).Should().ThrowAsync<UnauthorizedAccessException>().WithMessage(ApplicationMessages.LoginRequired);
    }

    [Fact]
    public async Task Handle_Should_Throw_InvalidOperation_When_Has_Already_Reviewed_The_Same_Book()
    {
        //Arr
        var userId = "user-1";
        var bookId = Guid.NewGuid();

        //kullanıcı daha önce yorumda bulunmuş durumu simüle edildi.
        _mockReviewsRepo.Setup(x => x.HasUserReviewedBookAsync(userId: userId, bookId: bookId)).ReturnsAsync(true);

        _mockCurrentUser.Setup(x => x.UserId).Returns(userId);

        //Aynı kitap için 2. yorum yapılmak isteniyor.
        var command = new AddReviewCommand(BookId: bookId, Rating: 3, Comment: "İkinci yorum denemesi");

        ///ACT & ASS
        //InvalidOperationException Fırlatmalı
        var exception = await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None)).Should().ThrowAsync<InvalidOperationException>();

        //Hata mesajı doğru verilmeli
        exception.WithMessage(ApplicationMessages.AlreadyReviewed + ApplicationMessages.UseToUpdateRating);

        //Db'ye kayıt yapmamalı
        _mockReviewsRepo.Verify(x => x.AddAsync(It.IsAny<Review>()), Times.Never);


    }
}
