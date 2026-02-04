using BookAdvisor.Domain.Interfaces;
using MediatR;

namespace BookAdvisor.Application.Features.Reviews.Queries.GetReviewsByBookId;

public class GetReviewsByBookIdQueryHandler : IRequestHandler<GetReviewsByBookIdQuery, BookReviewsVm>
{
    private readonly IReviewRepository _reviewRepository;

    public GetReviewsByBookIdQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<BookReviewsVm> Handle(GetReviewsByBookIdQuery request, CancellationToken cancellationToken)
    {
        //Yorumları getir.
        var reviews = await _reviewRepository.GetReviewsByBookIdAsync(request.BookId);

        //İstatistik hesaplama. Hiç değerlendirme yoksa ortalama 0 olmalı
        double averageRating = 0;
        if (reviews.Any())
            averageRating = reviews.Average(r => r.Rating);

        //Mapping Entity-> Dto
        var reviewDtos = reviews.Select(r => new ReviewDto(
            BookId: r.BookId,
            UserId: r.UserId,
            Rating: r.Rating,
            Comment: r.Comment,
            CreatedDate: r.CreateDate
            )).ToList();

        //sonuçları dönme
        return new BookReviewsVm
        {
            Reviews = reviewDtos,
            TotalCount = reviews.Count,
            AvarageRating = Math.Round(averageRating, 1) //virgülden sonra tek haneli olacak şekilde yuvarlama
        };
    }
}
