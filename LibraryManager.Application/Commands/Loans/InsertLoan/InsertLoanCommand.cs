using LibraryManager.Application.Models;
using MediatR;

namespace LibraryManager.Application.Commands.Loans.InsertLoan
{
    public class InsertLoanCommand : IRequest<ResultViewModel<int>>
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}
