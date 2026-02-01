using BookAdvisor.Domain.Entities;
using BookAdvisor.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookAdvisor.Infrastructure.Persistence.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookAdvisorDbContext _context;
        public BookRepository(BookAdvisorDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task<Book?> GetByIdAsync(Guid id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCountAsync(string searchKeyword)
        {
            var query = _context.Books.AsQueryable();
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                query = query.Where(b => b.Title.Contains(searchKeyword) || b.Author.Contains(searchKeyword));
            }

            return await query.CountAsync();
        }

        public async Task<List<Book>> GetAllAsync(int pageNumber, int pageSize, string searchKeyword)
        {
            //Sorguyu başlatma, db işlemi başlamadan
            var query = _context.Books.AsQueryable();

            //arama filtresi var mı kontrolü
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                //title veya yazar hakkında arama yapma
                query = query.Where(b => b.Title.Contains(searchKeyword) || b.Author.Contains(searchKeyword));
            }

            //sayfalama mantığı
            return await query.OrderBy(b => b.Title).Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync();
        }
    }
}
