using MediatR;

namespace BookAdvisor.Application.Features.Reviews.Queries.GetReviewsByBookId;

public record GetReviewsByBookIdQuery(Guid BookId) : IRequest<BookReviewsVm>;
