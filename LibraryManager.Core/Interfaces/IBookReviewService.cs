using LibraryManager.Core.Entities.NoSQL;

namespace LibraryManager.Core.Interfaces
{
    public interface IBookReviewService
    {
        Task AddReviewAsync(string bookId, string userId, int rating, string comment);
        Task<BookReview> GetReviewByIdAsync(string id, string bookId);
    }
} 