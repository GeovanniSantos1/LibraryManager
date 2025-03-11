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

        public InsertBookHandler(IBookRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }
        public async Task<ResultViewModel<int>> Handle(InsertBookCommand request, CancellationToken cancellationToken)
        {
            var books = await _repository.GetAll();
            if (books.Any(b => b.ISBN == request.ISBN))
                return (ResultViewModel<int>)ResultViewModel.Error("Já existe um livro cadastrado com este ISBN.");

            var book = request.ToEntity();
            var id = await _repository.Add(book);

            var bookCreated = new BookCreatedNotification(book);
            await _mediator.Publish(bookCreated);

            return ResultViewModel<int>.Sucess(id);
        }
    }
}
