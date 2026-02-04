using MediatR;

namespace BookAdvisor.Application.Features.Reviews.Commands.AddReview;

public record AddReviewCommand(Guid BookId, int Rating, string? Comment) : IRequest<Guid>;
