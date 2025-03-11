using FluentValidation;
using LibraryManager.Application.Commands.Loans.DeleteLoan;

namespace LibraryManager.Application.Validators.Loans
{
    public class DeleteLoanValidator : AbstractValidator<DeleteLoanCommand>
    {
        public DeleteLoanValidator()
        {
            RuleFor(l => l.LoanId)
                .GreaterThan(0)
                .WithMessage("O ID do empréstimo é inválido");
        }
    }
}
