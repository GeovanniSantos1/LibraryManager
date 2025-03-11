using LibraryManager.Core.Entities;

namespace LibraryManager.Core.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAll();
        Task<Book> GetById(int id);
        Task<int> Add(Book book);
        Task<bool> Update(Book book);
        Task<bool> Delete(int id);
    }
}
