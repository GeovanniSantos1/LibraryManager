using LibraryManager.Application.Models;
using LibraryManager.Core.Interfaces;
using MediatR;

namespace LibraryManager.Application.Queries.Loans.GetAllLoans
{
    public class GetAllLoansHandler : IRequestHandler<GetAllLoansQuery, ResultViewModel<List<LoanItemViewModel>>>
    {
        private readonly IloanRepository _repository;

        public GetAllLoansHandler(IloanRepository repository)
        {
            _repository = repository;
        }
        public async Task<ResultViewModel<List<LoanItemViewModel>>> Handle(GetAllLoansQuery request, CancellationToken cancellationToken)
        {
            var loans = await _repository.GetAll();
            var loanViewModels = loans.Select(LoanItemViewModel.FromEntity).ToList();

            return ResultViewModel<List<LoanItemViewModel>>.Sucess(loanViewModels);
        }
    }
}
