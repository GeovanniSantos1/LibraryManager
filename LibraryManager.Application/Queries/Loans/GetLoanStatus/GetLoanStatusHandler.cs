using LibraryManager.Application.Models;
using LibraryManager.Core.Interfaces;
using MediatR;

namespace LibraryManager.Application.Queries.Loans.GetLoanStatus
{
    public class GetLoanStatusHandler : IRequestHandler<GetLoanStatusQuery, ResultViewModel<LoanStatusViewModel>>
    {
        private readonly IloanRepository _repository;

        public GetLoanStatusHandler(IloanRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResultViewModel<LoanStatusViewModel>> Handle(GetLoanStatusQuery request, CancellationToken cancellationToken)
        {
            var loan = await _repository.GetById(request.Id);

            if (loan == null)
                return ResultViewModel<LoanStatusViewModel>.Error("Empréstimo não encontrado.");

            if (!loan.ReturnDate.HasValue)
                return ResultViewModel<LoanStatusViewModel>.Error("Data de devolução não definida para este empréstimo.");

            var statusViewModel = new LoanStatusViewModel
            {
                IsReturned = loan.ActualReturnDate.HasValue
            };

            var delayedDays = loan.GetDelayedDays();

            if (loan.ActualReturnDate.HasValue)
            {
                statusViewModel.DelayedDays = delayedDays;
                statusViewModel.Status = delayedDays > 0
                    ? $"Livro foi devolvido com {delayedDays} dias de atraso."
                    : "Livro foi devolvido no prazo.";
            }
            else
            {
                if (delayedDays > 0)
                {
                    statusViewModel.DelayedDays = delayedDays;
                    statusViewModel.Status = $"Empréstimo está atrasado em {delayedDays} dias.";
                }
                else
                {
                    var remainingDays = (loan.ReturnDate.Value - DateTime.Now).Days;
                    statusViewModel.RemainingDays = remainingDays;
                    statusViewModel.Status = $"Empréstimo em dia. Faltam {remainingDays} dias para a devolução.";
                }
            }

            return ResultViewModel<LoanStatusViewModel>.Sucess(statusViewModel);
        }
    }
}
