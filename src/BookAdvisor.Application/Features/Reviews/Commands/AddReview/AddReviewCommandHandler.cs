using BookAdvisor.Application.Constants;
using BookAdvisor.Application.Interfaces;
using BookAdvisor.Domain.Entities;
using BookAdvisor.Domain.Interfaces;
using MediatR;

namespace BookAdvisor.Application.Features.Reviews.Commands.AddReview;

public class AddReviewCommandHandler : IRequestHandler<AddReviewCommand, Guid>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ICurrentUserService _currentUserService;

    public AddReviewCommandHandler(IReviewRepository reviewRepository, ICurrentUserService currentUserService)
    {
        _reviewRepository = reviewRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(AddReviewCommand request, CancellationToken cancellationToken)
    {
        //kullanıcı kontrolü
        var userId = _currentUserService.UserId;
        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedAccessException(ApplicationMessages.LoginRequired);

        //Tekrarlanan yorum kontrolü
        var hasReview = await _reviewRepository.HasUserReviewedBookAsync(userId, request.BookId);
        if (hasReview)
            throw new InvalidOperationException(ApplicationMessages.AlreadyReviewed + ApplicationMessages.UseToUpdateRating);

        //Entity oluşturma
        var review = new Review(bookId: request.BookId, userId: userId, rating: request.Rating, comment: request.Comment);

        //Kayıt işlemi.
        await _reviewRepository.AddAsync(review);
        return review.Id;

    }
}
