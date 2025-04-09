using LibraryManager.Application.Models;

namespace LibraryManager.Application.Interfaces
{
    public interface IBookReviewService
    {
        Task AddReviewAsync(string bookId, string userId, int rating, string comment);
        Task<BookReviewViewModel> GetReviewByIdAsync(string id, string bookId);
    }
} 