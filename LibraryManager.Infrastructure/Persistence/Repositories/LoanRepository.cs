using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Infrastructure.Persistence.Repositories
{
    public class LoanRepository : IloanRepository
    {
        private readonly LibraryManagerDbContext _context;

        public LoanRepository(LibraryManagerDbContext context)
        {
            _context = context;
        }

        public async Task<Loan> Add(Loan loan)
        {
            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();
            return loan;
        }

        public async Task Delete(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan != null)
            {
                _context.Loans.Remove(loan);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Loans.AnyAsync(l => l.Id == id);
        }


        public async Task<IEnumerable<Loan>> GetAll()
        {
            var loans = await _context.Loans
                .Where(l => !l.IsDeleted)
                .ToListAsync();

            return loans;
        }

        public async Task<Loan> GetById(int id)
        {
            var loan = await _context.Loans
                    .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted);

            return loan;
        }

        public async Task<Loan> ReturnBook(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
                return null;

            if (loan.ActualReturnDate.HasValue)
                return null;

            loan.ActualReturnDate = DateTime.Now;

            var book = await _context.Books.FindAsync(loan.BookId);
            if (book != null)
            {
                book.IsAvailable = true;
            }

            await _context.SaveChangesAsync();

            return loan;
        }

        public async Task<Loan> Update(Loan loan)
        {
            var existingLoan = await _context.Loans.FindAsync(loan.Id);

            if (existingLoan == null)
                return null;

            _context.Entry(existingLoan).CurrentValues.SetValues(loan);
            await _context.SaveChangesAsync();

            return existingLoan;
        }
    }
}
