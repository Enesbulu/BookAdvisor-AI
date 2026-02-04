namespace BookAdvisor.Application.Features.Reviews.Queries.GetReviewsByBookId;

public record ReviewDto(Guid BookId, string UserId, int Rating, string? Comment, DateTime CreatedDate);


