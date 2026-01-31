using MediatR;

namespace BookAdvisor.Application.Features.ReadingLists.Queries.GetUserReadingLists
{
    public record GetUserReadingListQuery : IRequest<List<ReadingListDto>>;
}
