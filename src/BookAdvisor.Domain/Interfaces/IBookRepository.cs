using BookAdvisor.Domain.Entities;

namespace BookAdvisor.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(Guid id);
        Task AddAsync(Book book);
    }
}
