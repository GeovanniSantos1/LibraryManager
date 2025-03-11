using LibraryManager.Application.Models;
using LibraryManager.Core.Interfaces;
using MediatR;

namespace LibraryManager.Application.Commands.Loans.DeleteLoan
{
    public class DeleteLoanHandler : IRequestHandler<DeleteLoanCommand, ResultViewModel<int>>
    {
        private readonly IloanRepository _repository;
        public DeleteLoanHandler(IloanRepository repository)
        {
            _repository = repository;
        }
        public async Task<ResultViewModel<int>> Handle(DeleteLoanCommand request, CancellationToken cancellationToken)
        {
            var loan = await _repository.GetById(request.LoanId);

            if (loan == null)
            {
                return ResultViewModel<int>.Error("Empréstimo não encontrado.");
            }

            loan.SetAsDeletedAsync();

            var updated = await _repository.Update(loan);

            if (updated == null)
                return ResultViewModel<int>.Error("Não foi possível deletar o empréstimo.");

            return ResultViewModel<int>.Sucess(loan.Id);
        }
    }
}
