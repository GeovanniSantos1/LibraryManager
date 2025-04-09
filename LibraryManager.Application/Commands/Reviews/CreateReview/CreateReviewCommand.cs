using LibraryManager.Application.Models;
using LibraryManager.Core.Entities.NoSQL;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager.Application.Commands.Reviews.CreateReview
{
    public class CreateReviewCommand : IRequest<ResultViewModel<string>>
    {
        [Required]
        public string BookId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; }

        public BookReview ToEntity()
        {
            return new BookReview
            {
                PK = $"REVIEW_{DateTime.UtcNow.Ticks}_{Guid.NewGuid():N}",
                BookId = BookId,
                UserId = UserId,
                Rating = Rating,
                Comment = Comment ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
} 