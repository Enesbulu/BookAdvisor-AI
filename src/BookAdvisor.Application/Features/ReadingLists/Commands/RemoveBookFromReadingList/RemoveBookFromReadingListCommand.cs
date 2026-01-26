using MediatR;

namespace BookAdvisor.Application.Features.ReadingLists.Commands.RemoveBookFromReadingList
{
    public record RemoveBookFromReadingListCommand(Guid ReadingListId, Guid BookId) : IRequest;
}
