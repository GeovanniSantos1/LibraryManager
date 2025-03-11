using LibraryManager.Application.Models;
using LibraryManager.Core.Interfaces;
using MediatR;

namespace LibraryManager.Application.Queries.Books.GetBookById
{
    public class GetBookByIdHandler : IRequestHandler<GetBookByIdQuery, ResultViewModel<BookItemViewModel>>
    {
        private readonly IBookRepository _repository;

        public GetBookByIdHandler(IBookRepository repository)
        {
            _repository = repository;
        }
        public async Task<ResultViewModel<BookItemViewModel>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _repository.GetById(request.Id);

            if (book is null)
                return ResultViewModel<BookItemViewModel>.Error("Livro não existe");

            var bookViewModel = BookItemViewModel.FromEntity(book);

            return ResultViewModel<BookItemViewModel>.Sucess(bookViewModel);
        }
    }
}
