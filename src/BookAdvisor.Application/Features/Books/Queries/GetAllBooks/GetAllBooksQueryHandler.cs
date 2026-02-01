using BookAdvisor.Application.Comman.Models;
using BookAdvisor.Domain.Interfaces;
using MediatR;

namespace BookAdvisor.Application.Features.Books.Queries.GetAllBooks
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, PagedResult<BookDto>>
    {
        private readonly IBookRepository _bookRepository;

        public GetAllBooksQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<PagedResult<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _bookRepository.GetAllAsync(request.PageNumber, request.PageSize, request.SearchKeyword ?? "");
            var totalCount = await _bookRepository.GetCountAsync(request.SearchKeyword ?? "");

            var bookDtos = books.Select(b => new BookDto(
                Id: b.Id,
                Title: b.Title,
                Author: b.Author,
                Description: b.Description)).ToList();

            return new PagedResult<BookDto>(request.PageNumber, request.PageSize, totalCount: totalCount, items: bookDtos);
        }
    }
}
