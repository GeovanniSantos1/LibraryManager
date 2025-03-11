using LibraryManager.Application.Models;
using MediatR;

namespace LibraryManager.Application.Commands.Loans.LoanReturn
{
    public class LoanReturnCommand : IRequest<ResultViewModel<string>>
    {
        public LoanReturnCommand(int loanId)
        {
            LoanId = loanId;
        }

        public int LoanId { get; set; }
    }
}
