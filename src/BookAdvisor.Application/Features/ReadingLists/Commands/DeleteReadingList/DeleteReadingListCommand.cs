using MediatR;

namespace BookAdvisor.Application.Features.ReadingLists.Commands.DeleteReadingList
{
    public record DeleteReadingListCommand(Guid Id) : IRequest;
}
