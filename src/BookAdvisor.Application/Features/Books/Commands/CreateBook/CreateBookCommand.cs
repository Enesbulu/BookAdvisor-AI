using MediatR;

namespace BookAdvisor.Application.Features.Books.Commands.CreateBook
{
    public record CreateBookCommand(
        string Title,
        string Author,
        string ISBN,
        string Description
    ) : IRequest<Guid>;
}
