using LibraryManager.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManager.Application.Queries.Books.GetBookById
{
    public class GetBookByIdQuery : IRequest<ResultViewModel<BookItemViewModel>>
    {
        public GetBookByIdQuery(int id) => Id = id;

        public int Id { get; set; }
    }
}
