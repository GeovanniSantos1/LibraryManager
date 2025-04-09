using LibraryManager.Core.Entities.NoSQL;
using LibraryManager.Core.Interfaces;
using LibraryManager.Application.Models;
using LibraryManager.Application.Interfaces;
using IBookReviewService = LibraryManager.Application.Interfaces.IBookReviewService;

namespace LibraryManager.Application.Services
{
    public class BookReviewService : IBookReviewService
    {
        private readonly IBookReviewRepository _repository;

        public BookReviewService(IBookReviewRepository repository)
        {
            _repository = repository;
        }

        public async Task AddReviewAsync(string bookId, string userId, int rating, string comment)
        {
            var review = new BookReview
            {
                PK = $"REVIEW#{Guid.NewGuid()}",
                BookId = bookId,
                UserId = userId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(review);
        }

        public async Task<BookReviewViewModel> GetReviewByIdAsync(string id, string bookId)
        {
            var review = await _repository.GetByIdAsync(id, bookId);
            if (review == null) return null;

            return new BookReviewViewModel
            {
                Id = review.PK,
                BookId = review.BookId,
                UserId = review.UserId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }
    }
}
