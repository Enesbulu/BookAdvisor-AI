using BookAdvisor.Application.Constants;
using BookAdvisor.Domain.Interfaces;
using MediatR;

namespace BookAdvisor.Application.Features.Books.Queries.GetBookById
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDetailDto>
    {
        private readonly IBookRepository _bookRepository;

        public GetBookByIdQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<BookDetailDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.BookId);
            if (book == null)
                throw new Exception(ApplicationMessages.BookNotFound);
            return new BookDetailDto(
                Id: book.Id,
                Title: book.Title,
                Author: book.Author,
                Description: book.Description,
                CreatedDate: book.CreateDate);
        }
    }
}
