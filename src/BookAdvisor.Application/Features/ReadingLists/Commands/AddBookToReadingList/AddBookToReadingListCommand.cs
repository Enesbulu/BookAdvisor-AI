using MediatR;

namespace BookAdvisor.Application.Features.ReadingLists.Commands.AddBookToReadingList
{
    /// <summary>
    /// Var olan bir okuma listesine kitap ekleme komutu.
    /// </summary>
    /// <param name="ReadingListId">Eklenecek listenin id</param>
    /// <param name="BookId">Eklenecek kitap id</param>
    public record AddBookToReadingListCommand(Guid ReadingListId, Guid BookId) : IRequest;
}
