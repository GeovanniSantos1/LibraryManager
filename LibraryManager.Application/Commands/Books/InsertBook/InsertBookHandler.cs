using LibraryManager.Application.Models;
using LibraryManager.Application.Notification.BookCreated;
using LibraryManager.Core.Interfaces;
using MediatR;

namespace LibraryManager.Application.Commands.Books.InsertBook
{
    public class InsertBookHandler : IRequestHandler<InsertBookCommand, ResultViewModel<int>>
    {
        private readonly IBookRepository _repository;
        private readonly IMediator _mediator;
        private readonly IStorageService _storageService;

        public InsertBookHandler(
            IBookRepository repository, 
            IMediator mediator,
            IStorageService storageService)
        {
            _repository = repository;
            _mediator = mediator;
            _storageService = storageService;
        }

        public async Task<ResultViewModel<int>> Handle(InsertBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var books = await _repository.GetAll();
                if (books.Any(b => b.ISBN == request.ISBN))
                    return ResultViewModel<int>.Error("Já existe um livro cadastrado com este ISBN.");

                var book = request.ToEntity();

                if (request.ImageFile != null && request.ImageFile.Length > 0)
                {
                    try
                    {
                        var extension = Path.GetExtension(request.ImageFile.FileName);
                        var fileName = $"{Guid.NewGuid()}{extension}";
                        var (originalUrl, thumbnailUrl) = await _storageService.UploadImageAsync(request.ImageFile, fileName);
                        
                        book.ImageUrl = originalUrl;
                        book.ThumbnailUrl = thumbnailUrl;
                    }
                    catch (Exception ex)
                    {
                        return ResultViewModel<int>.Error($"Erro ao fazer upload da imagem: {ex.Message}");
                    }
                }

                var id = await _repository.Add(book);
                var bookCreated = new BookCreatedNotification(book);
                await _mediator.Publish(bookCreated);

                return ResultViewModel<int>.Sucess(id);
            }
            catch (Exception ex)
            {
                return ResultViewModel<int>.Error($"Erro ao criar livro: {ex.Message}");
            }
        }
    }
}
