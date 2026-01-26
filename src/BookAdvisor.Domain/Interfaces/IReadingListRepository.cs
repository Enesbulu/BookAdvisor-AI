using BookAdvisor.Domain.Entities;

namespace BookAdvisor.Domain.Interfaces
{
    public interface IReadingListRepository
    {
        //Yeni bir liste oluşturma
        Task AddAsync(ReadingList readingList);

        //Bir kullalnıcının tüm listelerini getirme
        Task<List<ReadingList>> GetISerListsAsync(string userId);

        //Listeyi Id ile bulma
        Task<ReadingList?> GetByIdAsync(Guid id);

        //Değişiklikleri kaydetme
        Task UpdateAsync(ReadingList readingList);

        //Id ile listenin kitap bilgisini kitapla birlikte alma
        Task<ReadingList?> GetByIdWithBooksAsync(Guid bookId);

        Task AddListItemAsync(ReadingListItem item);

    }
}
