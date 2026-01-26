namespace BookAdvisor.Application.Features.ReadingLists.Queries.GetReadingListById
{
    //Listenin Kendisi
    public record GetReadingListByIdDto(Guid Id, string Name, List<ReadingListBookDto> Items);

    //Listenin içindeki kitaplar
    public record ReadingListBookDto(Guid BookId, string Title, string Author);
}
