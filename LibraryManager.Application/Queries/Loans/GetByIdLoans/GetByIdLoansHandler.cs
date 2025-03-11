using LibraryManager.Application.Models;
using LibraryManager.Core.Interfaces;
using MediatR;

namespace LibraryManager.Application.Queries.Loans.GetByIdLoans
{
    public class GetByIdLoansHandler : IRequestHandler<GetByIdLoansQuery, ResultViewModel<LoanItemViewModel>>
    {
        private readonly IloanRepository _repository;

        public GetByIdLoansHandler(IloanRepository repository)
        {
            _repository = repository;
        }
        public async Task<ResultViewModel<LoanItemViewModel>> Handle(GetByIdLoansQuery request, CancellationToken cancellationToken)
        {
            var loan = await _repository.GetById(request.Id);

            if (loan == null)
                return ResultViewModel<LoanItemViewModel>.Error("Empréstimo não encontrado.");

            var loanViewModel = LoanItemViewModel.FromEntity(loan);
            return ResultViewModel<LoanItemViewModel>.Sucess(loanViewModel);
        }
    }
}
