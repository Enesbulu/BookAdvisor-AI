using BookAdvisor.Domain.Entities;

namespace BookAdvisor.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task<Book?> GetByIdAsync(Guid id);

        /// <summary>
        /// Kitapları sayfalar ve arama kelimesine göre filtreler.
        /// </summary>
        /// <param name="pageNumber">Kaçıncı sayfa (1'den başlar)</param>
        /// <param name="pageSize">Sayfada kaç kitap olacak</param>
        /// <param name="searchKeyword">Kitap adı veya yazar araması (Opsiyonel)</param>
        /// <returns>Kitap listesi</returns>
        Task<List<Book>> GetAllAsync(int pageNumber, int pageSize, string searchKeyword);

        /// <summary>
        /// Arama kriterlerine uygun toplam kitap sayısını döner
        /// </summary>
        /// <param name="searchKeyword">Filtreleme için girilen kelime</param>
        /// <returns></returns>
        Task<int> GetCountAsync(string searchKeyword);
    }
}
