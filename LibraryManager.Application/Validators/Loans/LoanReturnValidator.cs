using FluentValidation;
using LibraryManager.Application.Commands.Loans.LoanReturn;

namespace LibraryManager.Application.Validators.Loans
{
    public class LoanReturnValidator : AbstractValidator<LoanReturnCommand>
    {
        public LoanReturnValidator()
        {
            RuleFor(l => l.LoanId)
                .GreaterThan(0)
                .WithMessage("O ID do empréstimo é inválido");
        }
    }
}
