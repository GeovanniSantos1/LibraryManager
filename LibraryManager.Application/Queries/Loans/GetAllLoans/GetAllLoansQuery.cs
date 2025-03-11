using LibraryManager.Application.Models;
using MediatR;

namespace LibraryManager.Application.Queries.Loans.GetAllLoans
{
    public class GetAllLoansQuery : IRequest<ResultViewModel<List<LoanItemViewModel>>>
    {
    }
}
