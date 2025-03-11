using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Infrastructure.Persistence.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryManagerDbContext _context;

        public BookRepository(LibraryManagerDbContext context)
        {
            _context = context;
        }

        public async Task<int> Add(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return book.Id;
        }

        public async Task<bool> Delete(int id)
        {
            var book = await GetById(id);

            if (book == null)
                return false;

            book.SetAsDeletedAsync();

            _context.Books.Update(book);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            var books = await _context.Books
                   .Where(b => !b.IsDeleted)
                   .ToListAsync();

            return books;
        }

        public async Task<Book> GetById(int id)
        {
            var book = await _context.Books.SingleOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

            return book;
        }

        public async Task<bool> Update(Book book)
        {
            var existingBook = await GetById(book.Id);

            if (existingBook == null)
                return false;

            _context.Entry(existingBook).CurrentValues.SetValues(book);
            var affectedRows = await _context.SaveChangesAsync();

            return affectedRows > 0;
        }
    }
}
