using LibraryManager.Application.Models;
using LibraryManager.Core.Interfaces;
using MediatR;

namespace LibraryManager.Application.Commands.Loans.LoanReturn
{
    public class LoanReturnHandler : IRequestHandler<LoanReturnCommand, ResultViewModel<string>>
    {
        private readonly IloanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;

        public LoanReturnHandler(
            IloanRepository loanRepository,
            IBookRepository bookRepository)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
        }

        public async Task<ResultViewModel<string>> Handle(LoanReturnCommand request, CancellationToken cancellationToken)
        {
            var loan = await _loanRepository.GetById(request.LoanId);
            if (loan == null)
            {
                return ResultViewModel<string>.Error("Empréstimo não encontrado.");
            }

            if (loan.ActualReturnDate.HasValue)
            {
                return ResultViewModel<string>.Error("Este livro já foi devolvido.");
            }

            var book = await _bookRepository.GetById(loan.BookId);
            if (book != null)
            {
                book.IsAvailable = true;
                await _bookRepository.Update(book);
            }

            loan.ActualReturnDate = DateTime.Now;
            await _loanRepository.Update(loan);

            var delayedDays = loan.GetDelayedDays();
            if (delayedDays > 0)
            {
                return ResultViewModel<string>.Sucess($"Livro devolvido com {delayedDays} dias de atraso.");
            }

            return ResultViewModel<string>.Sucess("Livro devolvido no prazo.");
        }
    }
}