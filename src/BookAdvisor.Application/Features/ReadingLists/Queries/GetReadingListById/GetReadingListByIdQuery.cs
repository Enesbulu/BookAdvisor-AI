using BookAdvisor.Application.Constants;
using BookAdvisor.Domain.Interfaces;
using MediatR;

namespace BookAdvisor.Application.Features.ReadingLists.Queries.GetReadingListById
{
    //Query-İstek
    public record GetReadingListByIdQuery(Guid Id) : IRequest<GetReadingListByIdDto>;

    //Handler-İşleyici
    public class GetReadingListByIdQueryHandler : IRequestHandler<GetReadingListByIdQuery, GetReadingListByIdDto>
    {

        private readonly IReadingListRepository _readingListRepository;

        public GetReadingListByIdQueryHandler(IReadingListRepository readingListRepository)
        {
            _readingListRepository = readingListRepository;
        }

        public async Task<GetReadingListByIdDto> Handle(GetReadingListByIdQuery request, CancellationToken cancellationToken)
        {
            var list = await _readingListRepository.GetByIdWithBooksAsync(request.Id);

            if (list == null)
                throw new Exception(ApplicationMessages.ReadingListNotFound);

            return new GetReadingListByIdDto(

                Id: list.Id,
                Name: list.Name,
                Items: list.Items.Select(i => new ReadingListBookDto(
                    BookId: i.BookId,
                    Title: i.Book != null ? i.Book.Title : "Silinmiş Kitap",
                    Author: i.Book != null ? i.Book.Author : "Bilinmiyor")).ToList());
        }
    }
}
