using LibraryManager.Application.Models;
using MediatR;

namespace LibraryManager.Application.Queries.Loans.GetByIdLoans
{
    public class GetByIdLoansQuery : IRequest<ResultViewModel<LoanItemViewModel>>
    {
        public int Id { get; set; }

        public GetByIdLoansQuery(int id)
        {
            Id = id;
        }
    }
}
