using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using LibraryManager.Core.Entities.NoSQL;
using LibraryManager.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace LibraryManager.Infrastructure.Persistence.Repositories
{
    public class BookReviewRepository : IBookReviewRepository
    {
        private readonly IDynamoDBContext _context;
        private readonly ILogger<BookReviewRepository> _logger;

        public BookReviewRepository(IDynamoDBContext context, ILogger<BookReviewRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(BookReview review)
        {
            try
            {
                _logger.LogInformation($"Iniciando salvamento da review. PK={review.PK}");
                
                var config = new DynamoDBOperationConfig
                {
                    OverrideTableName = "BookReviews",
                    Conversion = DynamoDBEntryConversion.V2
                };

                await _context.SaveAsync(review, config);
                _logger.LogInformation($"Review salva com sucesso. PK={review.PK}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao salvar review. PK={review.PK}. Erro: {ex.Message}");
                throw;
            }
        }

        public async Task<BookReview> GetByIdAsync(string id, string bookId)
        {
            var config = new DynamoDBOperationConfig
            {
                OverrideTableName = "BookReviews"
            };

            return await _context.LoadAsync<BookReview>(id, config);
        }

        public async Task<IEnumerable<BookReview>> GetByBookIdAsync(string bookId)
        {
            var config = new DynamoDBOperationConfig
            {
                OverrideTableName = "BookReviews"
            };

            var conditions = new List<ScanCondition>
            {
                new ScanCondition("BookId", ScanOperator.Equal, bookId)
            };

            return await _context.ScanAsync<BookReview>(conditions, config).GetRemainingAsync();
        }

        public async Task UpdateAsync(BookReview review)
        {
            await _context.SaveAsync(review);
        }

        public async Task DeleteAsync(string id, string bookId)
        {
            await _context.DeleteAsync<BookReview>(id);
        }
    }
}
