using FluentValidation;
using LibraryManager.Application.Commands.Loans.InsertLoan;

namespace LibraryManager.Application.Validators.Loans
{
    public class InsertLoanValidator : AbstractValidator<InsertLoanCommand>
    {
        public InsertLoanValidator()
        {
            RuleFor(l => l.UserId)
                .GreaterThan(0)
                .WithMessage("O ID do usuário é inválido");

            RuleFor(l => l.BookId)
                .GreaterThan(0)
                .WithMessage("O ID do livro é inválido");
        }
    }
}
