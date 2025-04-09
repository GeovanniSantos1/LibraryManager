using LibraryManager.Application.Models;
using LibraryManager.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LibraryManager.Application.Commands.Reviews.CreateReview
{
    public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, ResultViewModel<string>>
    {
        private readonly IBookReviewRepository _repository;
        private readonly ILogger<CreateReviewHandler> _logger;

        public CreateReviewHandler(IBookReviewRepository repository, ILogger<CreateReviewHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ResultViewModel<string>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var review = request.ToEntity();
                await _repository.AddAsync(review);
                return ResultViewModel<string>.Sucess(review.PK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar review");
                return ResultViewModel<string>.Error($"Erro ao criar review: {ex.Message}");
            }
        }
    }
} 