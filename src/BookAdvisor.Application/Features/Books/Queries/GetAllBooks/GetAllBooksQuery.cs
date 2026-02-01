using BookAdvisor.Application.Comman.Models;
using MediatR;

namespace BookAdvisor.Application.Features.Books.Queries.GetAllBooks
{
    public class GetAllBooksQuery : IRequest<PagedResult<BookDto>>
    {
        public int PageNumber { get; set; } = 1;    //varsayılan : 1. sayfa
        public int PageSize { get; set; } = 10;     //varsayılan:  10 adet
        public string? SearchKeyword { get; set; }  //varsayılan arama kelimesi boş
    }
}
