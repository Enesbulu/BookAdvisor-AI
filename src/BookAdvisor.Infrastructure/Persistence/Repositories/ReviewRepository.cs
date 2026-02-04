using BookAdvisor.Domain.Entities;
using BookAdvisor.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookAdvisor.Infrastructure.Persistence.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly BookAdvisorDbContext _dbContext;

        public ReviewRepository(BookAdvisorDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Review review)
        {
            await _dbContext.Reviews.AddAsync(review);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Review>> GetReviewsByBookIdAsync(Guid bookId)
        {
            //en yeni yorum en başta görünecek şekilde listele.
            return await _dbContext.Reviews.Where(r => r.BookId == bookId).OrderByDescending(r => r.CreateDate).ToListAsync();
        }

        public async Task<bool> HasUserReviewedBookAsync(string userId, Guid bookId)
        {
            //Eğer userId ve bookID ile eşleşen bir kayıt varsa true döner.
            return await _dbContext.Reviews.AnyAsync(r => r.UserId == userId && r.BookId == bookId);
        }
    }
}
