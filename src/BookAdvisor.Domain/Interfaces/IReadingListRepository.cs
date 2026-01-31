using BookAdvisor.Domain.Entities;

namespace BookAdvisor.Domain.Interfaces
{
    public interface IReadingListRepository
    {
        /// <summary>
        /// Yeni bir liste oluşturma
        /// </summary>
        /// <param name="readingList"></param>
        /// <returns></returns>
        Task AddAsync(ReadingList readingList);

        /// <summary>
        /// Bir kullanıcının tüm listelerini getirme
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<ReadingList>> GetUserListsAsync(string userId);

        /// <summary>
        /// Listeyi Id ile bulma
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ReadingList?> GetByIdAsync(Guid id);

        /// <summary>
        /// Değişiklikleri kaydetme
        /// </summary>
        /// <param name="readingList"></param>
        /// <returns></returns>
        Task UpdateAsync(ReadingList readingList);

        /// <summary>
        /// Id ile listenin kitap bilgisini kitapla birlikte alma
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        Task<ReadingList?> GetByIdWithBooksAsync(Guid bookId);

        /// <summary>
        /// Listeye Eleman ekleme(?)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task AddListItemAsync(ReadingListItem item);

        /// <summary>
        /// Listeden Kitap Silme işlemi yapar.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        Task RemoveBookFromListItemAsync(ReadingListItem items);

        /// <summary>
        /// Okuam Listesini siler
        /// </summary>
        /// <param name="readingList"></param>
        /// <returns></returns>
        Task DeleteAsync(ReadingList readingList);

        /// <summary>
        /// Listeye Toplu Kitap Ekleme
        /// </summary>
        /// <param name="items">Eklenecek kitapların listesini içerir</param>
        /// <returns></returns>
        Task AddListItemsRepository(List<ReadingListItem> items);

        /// <summary>
        /// Okuma listesinden toplu kitap silme
        /// </summary>
        /// <param name="itemsToDelete">Silinecek veritabanı nesnelerinin listesi</param>
        /// <returns></returns>
        Task RemoveBooksFromListItemsAsync(List<ReadingListItem> itemsToDelete);


    }
}
