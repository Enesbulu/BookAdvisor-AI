using BookAdvisor.Domain.Entities;
using MediatR;

namespace BookAdvisor.Application.Features.Books.Queries.GetBookById
{
    public record GetBookByIdQuery(Guid BookId) : IRequest<Book>;
}
