using LibraryManager.Core.Entities.NoSQL;

namespace LibraryManager.Core.Interfaces
{
    public interface IBookReviewRepository
    {
        Task AddAsync(BookReview review);
        Task<BookReview> GetByIdAsync(string id, string bookId);
        Task<IEnumerable<BookReview>> GetByBookIdAsync(string bookId);
        Task UpdateAsync(BookReview review);
        Task DeleteAsync(string id, string bookId);
    }
}
