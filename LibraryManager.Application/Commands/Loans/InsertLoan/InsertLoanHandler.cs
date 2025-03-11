using LibraryManager.Application.Models;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using LibraryManager.Infrastructure.Persistence.Repositories;
using MediatR;

namespace LibraryManager.Application.Commands.Loans.InsertLoan
{
    public class InsertLoanHandler : IRequestHandler<InsertLoanCommand, ResultViewModel<int>>
    {
        private readonly IloanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public InsertLoanHandler(
            IloanRepository loanRepository,
            IBookRepository bookRepository,
            IUserRepository userRepository)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }
        public async Task<ResultViewModel<int>> Handle(InsertLoanCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
                return ResultViewModel<int>.Error("Usuário não encontrado.");

            var book = await _bookRepository.GetById(request.BookId);
            if (book == null)
                return ResultViewModel<int>.Error("Livro não encontrado.");

            if (!book.IsAvailable)
                return ResultViewModel<int>.Error("Livro não está disponível para empréstimo.");

            var loan = new Loan(request.UserId, request.BookId);

            book.IsAvailable = false;
            await _bookRepository.Update(book);
            await _loanRepository.Add(loan);

            return ResultViewModel<int>.Sucess(loan.Id);
        }
    }
}
