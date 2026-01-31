using MediatR;

namespace BookAdvisor.Application.Features.ReadingLists.Commands.BulkRemoveBooksFromReadingList
{
    public record BulkRemoveBooksFromReadingListCommand(Guid TargetReadingListId, List<Guid> BookIdsToRemove) : IRequest;
}
