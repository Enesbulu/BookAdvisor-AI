using System;
using System.Collections.Generic;
using System.Text;

namespace BookAdvisor.Application.Features.Books.Queries.GetBookById
{
    public record BookDetailDto(Guid Id, string Title, string Author, string Description, DateTime CreatedDate);
}
