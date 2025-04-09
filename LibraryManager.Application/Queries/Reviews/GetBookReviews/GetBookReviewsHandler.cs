using LibraryManager.Application.Models;
using LibraryManager.Core.Interfaces;
using LibraryManager.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManager.Application.Queries.Reviews.GetBookReviews
{
    public class GetBookReviewsHandler : IRequestHandler<GetBookReviewsQuery, ResultViewModel<IEnumerable<BookReviewViewModel>>>
    {
        private readonly IBookReviewRepository _repository;
        private readonly ILogger<GetBookReviewsHandler> _logger;

        public GetBookReviewsHandler(IBookReviewRepository repository, ILogger<GetBookReviewsHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ResultViewModel<IEnumerable<BookReviewViewModel>>> Handle(
            GetBookReviewsQuery request, 
            CancellationToken cancellationToken)
        {
            try
            {
                var reviews = await _repository.GetByBookIdAsync(request.BookId);
                var viewModels = reviews.Select(review => new BookReviewViewModel
                {
                    Id = review.PK,
                    BookId = review.BookId,
                    UserId = review.UserId,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    CreatedAt = review.CreatedAt
                });

                return ResultViewModel<IEnumerable<BookReviewViewModel>>.Sucess(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar reviews");
                return ResultViewModel<IEnumerable<BookReviewViewModel>>.Error("Erro ao buscar reviews");
            }
        }
    }
} 