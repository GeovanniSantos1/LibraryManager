using LibraryManager.Application.Models;
using MediatR;

namespace LibraryManager.Application.Commands.Loans.DeleteLoan
{
    public class DeleteLoanCommand : IRequest<ResultViewModel<int>>
    {
        public int LoanId { get; set; }
        public DeleteLoanCommand(int loanId)
        {
            LoanId = loanId;
        }
    }
}
