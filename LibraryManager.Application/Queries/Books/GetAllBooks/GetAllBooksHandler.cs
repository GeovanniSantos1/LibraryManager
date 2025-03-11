using LibraryManager.Application.Models;
using LibraryManager.Core.Interfaces;
using MediatR;

namespace LibraryManager.Application.Queries.Books.GetAllBooks
{
    public class GetAllBooksHandler : IRequestHandler<GetAllBooksQuery, ResultViewModel<List<BookItemViewModel>>>
    {
        private readonly IBookRepository _repository;

        public GetAllBooksHandler(IBookRepository repository)
        {
            _repository = repository;
        }
        public async Task<ResultViewModel<List<BookItemViewModel>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _repository.GetAll();

            var bookViewModels = books.Select(book => BookItemViewModel.FromEntity(book)).ToList();

            return ResultViewModel<List<BookItemViewModel>>.Sucess(bookViewModels);
        }
    }
}
