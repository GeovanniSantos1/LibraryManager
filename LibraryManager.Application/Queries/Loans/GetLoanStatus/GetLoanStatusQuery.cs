using LibraryManager.Application.Models;
using MediatR;

namespace LibraryManager.Application.Queries.Loans.GetLoanStatus
{
    public class GetLoanStatusQuery : IRequest<ResultViewModel<LoanStatusViewModel>>
    {
        public GetLoanStatusQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
