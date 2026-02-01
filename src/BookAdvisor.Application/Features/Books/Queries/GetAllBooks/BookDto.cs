namespace BookAdvisor.Application.Features.Books.Queries.GetAllBooks
{
    public record BookDto(Guid Id, string Title, string Author, string Description);
}
