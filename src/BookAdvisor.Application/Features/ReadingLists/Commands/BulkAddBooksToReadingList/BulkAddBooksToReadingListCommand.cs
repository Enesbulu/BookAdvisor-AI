using MediatR;

namespace BookAdvisor.Application.Features.ReadingLists.Commands.BulkAddBooksToReadingList
{
    public record BulkAddBooksToReadingListCommand(Guid ReadingListId, List<Guid> BookIds) : IRequest;
}
