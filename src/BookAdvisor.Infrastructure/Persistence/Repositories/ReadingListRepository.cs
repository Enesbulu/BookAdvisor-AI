using BookAdvisor.Domain.Entities;
using BookAdvisor.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookAdvisor.Infrastructure.Persistence.Repositories
{
    public class ReadingListRepository : IReadingListRepository
    {
        private readonly BookAdvisorDbContext _context;

        public ReadingListRepository(BookAdvisorDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ReadingList readingList)
        {
            await _context.ReadingLists.AddAsync(readingList);
            await _context.SaveChangesAsync();
        }

        public async Task<ReadingList?> GetByIdAsync(Guid id)
        {// Listeyi getirirken içindeki kitapları (Items) da beraber getir (Include)
            return await _context.ReadingLists.Include(r => r.Items).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<ReadingList>> GetUserListsAsync(string userId)
        {
            return await _context.ReadingLists.Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreateDate)
                .ToListAsync();
        }

        public async Task UpdateAsync(ReadingList readingList)
        {
            _context.ReadingLists.Update(readingList);
            await _context.SaveChangesAsync();
        }

        public async Task<ReadingList?> GetByIdWithBooksAsync(Guid bookId)
        {
            return await _context.ReadingLists
                .Include(r => r.Items)
                .ThenInclude(i => i.Book)   //ListItems tablosunu getir
                .FirstOrDefaultAsync(r => r.Id == bookId);  //Her item'ın içindeki Book tablosunu getir (JOIN)
        }

        /// <summary>
        /// Yeni oluşturulan ListItem alır ve veritabanına ekler.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddListItemAsync(ReadingListItem item)
        {
            await _context.ReadingListItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Okuma listesinden kitap silmek
        /// </summary>
        /// <param name="item">Listeden silinecek kitap</param>
        /// <returns></returns>
        public async Task RemoveBookFromListItemAsync(ReadingListItem item)
        {
            _context.ReadingListItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ReadingList readingList)
        {
            _context.ReadingLists.Remove(readingList);
            await _context.SaveChangesAsync();
        }


        public async Task AddListItemsRepository(List<ReadingListItem> items)
        {
            await _context.ReadingListItems.AddRangeAsync(items);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveBooksFromListItemsAsync(List<ReadingListItem> itemsToDelete)
        {
            //Performansılı olarak silme yöntemi(RemoveRange). Toplu silme sorgusu gönderme. 
            _context.ReadingListItems.RemoveRange(itemsToDelete);
            await _context.SaveChangesAsync();  //Değişiklikleri kaydet
        }
    }
}
