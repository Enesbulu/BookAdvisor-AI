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

        public async Task<List<ReadingList>> GetISerListsAsync(string userId)
        {
            return await _context.ReadingLists.Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreateDate)
                .ToListAsync();
        }

        public async Task UpdateAsync(ReadingList readingList)
        {
            /*foreach (var item in readingList.Items)
            {
                var entry = _context.Entry(item);

                //if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                //{
                //    entry.State = EntityState.Added;
                //}
                //var isTracked = _context.ChangeTracker.Entries<ReadingListItem>().Any(e => e.Entity.Id == item.Id);
                //if (!isTracked)
                //{
                //    _context.Entry(item).State = EntityState.Added;
                //}
                if (entry.State == EntityState.Detached)
                {
                    entry.State = EntityState.Added;
                }
            }*/

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

        // Yeni oluşturulan ListItem alır ve veritabanına ekler.
        public async Task AddListItemAsync(ReadingListItem item)
        {
            await _context.ReadingListItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }
    }
}
