using BookAdvisor.Domain.Entities;

namespace BookAdvisor.Domain.Interfaces
{
    public interface IReviewRepository
    {
        Task AddAsync(Review review);

        //Kullanıcının belirli bir kitaba yorum yapıp yapmadığını kontrol eder.
        Task<bool> HasUserReviewedBookAsync(string userId, Guid bookId);

        //Veritabanından id'li kitabın yorumlarını çekme metodu.
        Task<List<Review>> GetReviewsByBookIdAsync(Guid bookId);
    }
}
