using LibraryManager.Application.Models;
using LibraryManager.Infrastructure.Persistence;

namespace LibraryManager.Application.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryManagerDbContext _context;

        public BookService(LibraryManagerDbContext context)
        {
            _context = context;
        }
        public ResultViewModel Delete(int id)
        {
            var book = _context.Books.SingleOrDefault(b => b.Id == id);

            if (book == null)
                return ResultViewModel.Error("Livro não encontrado.");

            book.SetAsDeletedAsync();

            _context.Books.Update(book);
            _context.SaveChanges();

            return ResultViewModel.Sucess();
        }

        public ResultViewModel<List<BookItemViewModel>> GetAll(string search = "")
        {
            var books = _context.Books
                .Where(b => !b.IsDeleted && (search == "" || b.Title.Contains(search) || b.Author.Contains(search) || b.ISBN.Contains(search)))
                //.Skip(page * size)
                //.Take(size)
                .Select(b => BookItemViewModel.FromEntity(b))
                .ToList();

            return ResultViewModel<List<BookItemViewModel>>.Sucess(books);
        }

        public ResultViewModel<BookItemViewModel> GetById(int id)
        {
            var book = _context.Books.SingleOrDefault(b => b.Id == id);

            if (book is null)
                return ResultViewModel<BookItemViewModel>.Error("Livro não existe");

            var bookViewModel = BookItemViewModel.FromEntity(book);

            return ResultViewModel<BookItemViewModel>.Sucess(bookViewModel);
        }

        public ResultViewModel<int> Insert(CreateBookInputModel model)
        {

            // Verifica se já existe um livro com o mesmo ISBN
            if (_context.Books.Any(b => b.ISBN == model.ISBN))
                return (ResultViewModel<int>)ResultViewModel.Error("Já existe um livro cadastrado com este ISBN.");

            var book = model.ToEntity();
            _context.Books.Add(book);
            _context.SaveChanges();

            return ResultViewModel<int>.Sucess(book.Id);
        }

        public ResultViewModel Update(UpdateBookInputModel model)
        {
            var book = _context.Books.SingleOrDefault(b => b.Id == model.IdBook);
            if (book == null)
            {
                return (ResultViewModel<int>)ResultViewModel.Error("Livro não encontrado.");
            }

            if (_context.Books.Any(b => b.ISBN == model.ISBN && b.Id != model.IdBook))
                return (ResultViewModel<int>)ResultViewModel.Error("Já existe outro livro cadastrado com este ISBN.");

            model.UpdateEntity(book);

            _context.Books.Update(book);
            _context.SaveChanges();

            return ResultViewModel.Sucess();
        }
    }
}
