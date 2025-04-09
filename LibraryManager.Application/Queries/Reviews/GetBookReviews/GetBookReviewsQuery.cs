using LibraryManager.Application.Models;
using MediatR;

namespace LibraryManager.Application.Queries.Reviews.GetBookReviews
{
    public class GetBookReviewsQuery : IRequest<ResultViewModel<IEnumerable<BookReviewViewModel>>>
    {
        public string BookId { get; }

        public GetBookReviewsQuery(string bookId)
        {
            BookId = bookId;
        }
    }
} 