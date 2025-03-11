using LibraryManager.Application.Models;
using MediatR;

namespace LibraryManager.Application.Queries.Books.GetAllBooks
{
    public class GetAllBooksQuery : IRequest<ResultViewModel<List<BookItemViewModel>>>
    {
    }
}
